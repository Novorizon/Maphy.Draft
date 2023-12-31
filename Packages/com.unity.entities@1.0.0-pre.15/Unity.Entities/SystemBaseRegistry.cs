using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Unity.Assertions;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
    internal unsafe struct UnmanagedComponentSystemDelegates
    {
        // The function to call from a burst context to create/update/destroy.
        internal fixed ulong BurstFunctions[6];

        // The function to call from a managed context to create/update/destroy.
        internal fixed ulong ManagedFunctions[6];

        // Maintain a reference to any burst->managed delegate wrapper so they are not collected
        internal fixed ulong GCDefeat1[6];

        internal ushort PresentFunctionBits;
        internal ushort BurstFunctionBits;



        internal unsafe void Dispose()
        {
            for (int i = 5; i >= 0; --i)
            {
                if (ManagedFunctions[i] != 0)
                {
                    GCHandle.FromIntPtr((IntPtr)ManagedFunctions[i]).Free();
                }

                if (GCDefeat1[i] != 0)
                {
                    GCHandle.FromIntPtr((IntPtr)GCDefeat1[i]).Free();
                }
            }
        }
    }

    internal unsafe struct UnmanagedSystemTypeRegistryData
    {
        private UnsafeParallelHashMap<long, int> m_TypeHashToIndex;
        private UnsafeList<UnmanagedComponentSystemDelegates> m_Delegates;

        public bool Constructed => m_Delegates.Ptr != null;

        internal void Construct()
        {
            m_TypeHashToIndex = new UnsafeParallelHashMap<long, int>(64, Allocator.Persistent);
            m_Delegates = new UnsafeList<UnmanagedComponentSystemDelegates>(64, Allocator.Persistent);
        }

        internal void Dispose()
        {
            if (Constructed)
            {
                for (int i = 0; i < m_Delegates.Length; ++i)
                {
                    m_Delegates[i].Dispose();
                }

                m_Delegates.Dispose();
                m_TypeHashToIndex.Dispose();
            }

            this = default;
        }

        internal int AddSystemType(long typeHash, UnmanagedComponentSystemDelegates delegates)
        {
            if (m_TypeHashToIndex.TryGetValue(typeHash, out int index))
            {
                m_Delegates[index] = delegates;
                return index;
            }
            else
            {
                int newIndex = m_Delegates.Length;
                m_TypeHashToIndex.Add(typeHash, newIndex);
                m_Delegates.Add(delegates);
                return newIndex;
            }
        }

        internal bool FindSystemMetaIndex(long typeHash, out int index)
        {
            return m_TypeHashToIndex.TryGetValue(typeHash, out index);
        }

        internal ref readonly UnmanagedComponentSystemDelegates GetSystemDelegates(int index)
        {
            return ref m_Delegates.Ptr[index];
        }
    }

    /// <summary>
    /// Internal class used by codegen (as such it is necessary to be public). For registering unmanaged systems
    /// prefer <seealso cref="World.AddSystem"/>
    /// </summary>
    [GenerateTestsForBurstCompatibility]    
    public static class SystemBaseRegistry
    {
        class Managed
        {
            public static List<Type> s_StructTypes = null;
            public static List<RegistrationEntry> s_PendingRegistrations;
    #if !UNITY_DOTSRUNTIME
            public static bool s_DisposeRegistered = false;
    #endif
        }

        struct Dummy
        {
        }

        internal readonly static SharedStatic<UnmanagedSystemTypeRegistryData> s_Data = SharedStatic<UnmanagedSystemTypeRegistryData>.GetOrCreate<Dummy>();

        // TODO: Need to dispose this thing when domain reload happens.
        public delegate void ForwardingFunc(IntPtr systemPtr, IntPtr state);

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS"), Conditional("UNITY_DOTS_DEBUG")]
        private static void AssertTypeRegistryInitialized(in UnmanagedSystemTypeRegistryData data)
        {
            if (!data.Constructed)
                throw new InvalidOperationException("type registry is not initialized");
        }

        [GenerateTestsForBurstCompatibility]
        internal static int GetSystemTypeMetaIndex(long typeHash)
        {
            ref var data = ref s_Data.Data;
            AssertTypeRegistryInitialized(in data);

            if (!data.FindSystemMetaIndex(typeHash, out int index))
                index = -1;
            return index;
        }

        private struct RegistrationEntry
        {
            public Type m_Type;
            public long m_TypeHash;
            public ForwardingFunc[] m_Functions;
            public string m_DebugName;
            public int m_BurstCompileBits;
        }

        [ExcludeFromBurstCompatTesting("Takes managed Type")]
        public static unsafe void AddUnmanagedSystemType(Type type, long typeHash, ForwardingFunc onCreate, ForwardingFunc onUpdate,
            ForwardingFunc onDestroy, ForwardingFunc onStartRunning, ForwardingFunc onStopRunning, ForwardingFunc onCreateForCompiler,
            string debugName, int burstCompileBits)
        {
            //Debug.Log($"Adding unmanaged system type {debugName}, bcb={burstCompileBits}");

            // Lazily create list to hold pending work items as needed. This will be called from early initialization code and we need to defer burst compilation.
            if (Managed.s_PendingRegistrations == null)
            {
                Managed.s_PendingRegistrations = new List<RegistrationEntry>();
                Managed.s_StructTypes = new List<Type>();

                ref var data = ref s_Data.Data;
                data.Construct();

                // Arrange for domain unloads to wipe the pending registration list, which works around multiple domain reloads in sequence
#if !UNITY_DOTSRUNTIME
                if (!Managed.s_DisposeRegistered)
                {
                    Managed.s_DisposeRegistered = true;
                    AppDomain.CurrentDomain.DomainUnload += (_, __) =>
                    {
                        s_Data.Data.Dispose();
                        Managed.s_PendingRegistrations = null;
                    };
                }
#endif
            }

            // Buffer the data
            Managed.s_PendingRegistrations.Add(new RegistrationEntry
            {
                m_Type = type,
                m_TypeHash = typeHash,
                m_Functions = new ForwardingFunc[] { onCreate, onUpdate, onDestroy, onStartRunning, onStopRunning, onCreateForCompiler },
                m_DebugName = debugName,
                m_BurstCompileBits = burstCompileBits
            });
        }

        [ExcludeFromBurstCompatTesting("Uses managed delegate")]
        static void SelectManagedFn(out ulong result, ulong burstFn, ForwardingFunc managedFn)
        {
            if (burstFn != 0)
            {
                result = (ulong)GCHandle.ToIntPtr(GCHandle.Alloc(new FunctionPointer<ForwardingFunc>((IntPtr)burstFn).Invoke));
            }
            else
            {
                result = (ulong)GCHandle.ToIntPtr(GCHandle.Alloc(managedFn));
            }
        }

        [ExcludeFromBurstCompatTesting("Uses managed delegate")]
        static void SelectBurstFn(out ulong result, out ulong defeatGc, ulong burstFn, ForwardingFunc managedFn)
        {
            if (burstFn != default)
            {
                result = burstFn;
                defeatGc = default;
            }
            else
            {
#if UNITY_DOTSRUNTIME_IL2CPP || ENABLE_IL2CPP
                // Tiny IL2CPP does not handle reverse pinvoke wrapping for lambda functions
                // and since try/catch isn't supported in Tiny IL2CPP either, there is no need
                // to wrap the managedFn here.
                defeatGc = (ulong)GCHandle.ToIntPtr(GCHandle.Alloc(managedFn));
                result = (ulong)Marshal.GetFunctionPointerForDelegate(managedFn);
#else
                ForwardingFunc wrapper = (IntPtr system, IntPtr state) =>
                {
                    try
                    {
                        managedFn(system, state);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogException(ex);
                    }
                };

                defeatGc = (ulong)GCHandle.ToIntPtr(GCHandle.Alloc(wrapper));
                result = (ulong)Marshal.GetFunctionPointerForDelegate(wrapper);
#endif
            }
        }

        [ExcludeFromBurstCompatTesting("Uses managed delegates")]
        public unsafe static void InitializePendingTypes()
        {
            if (Managed.s_PendingRegistrations == null)
                return;

            foreach (var r in Managed.s_PendingRegistrations)
            {
                var burstCompileBits = r.m_BurstCompileBits;
                var delegates = default(UnmanagedComponentSystemDelegates);

                ushort functionBit = 1;
                delegates.BurstFunctionBits = 0;
                for (int i = 0; i < r.m_Functions.Length; ++i, functionBit <<= 1)
                {
                    var dlg = r.m_Functions[i];

                    if (dlg != null)
                    {
                        var useBurstFunction = (burstCompileBits & functionBit) != 0;
                        ulong burstFunc = useBurstFunction ? (ulong)BurstCompiler.CompileFunctionPointer(dlg).Value : 0;

                        // Select what to call when calling into a system from managed code.
                        SelectManagedFn(out delegates.ManagedFunctions[i], burstFunc, dlg);

                        // Select what to call when calling into a system from Burst code.
                        SelectBurstFn(out delegates.BurstFunctions[i], out delegates.GCDefeat1[i], burstFunc, dlg);

                        delegates.PresentFunctionBits |= functionBit;
                        if (useBurstFunction)
                            delegates.BurstFunctionBits |= functionBit;
                    }
                }

                Managed.s_StructTypes.Add(r.m_Type);
                s_Data.Data.AddSystemType(r.m_TypeHash, delegates);
            }

            Managed.s_PendingRegistrations = null;
        }

        [BurstDiscard]
        internal static void CheckBurst(ref bool status)
        {
            status = false;
        }

        [Burst.CompilerServices.IgnoreWarning(1371)]
        private static unsafe void CallForwardingFunction(SystemState* systemState, int functionIndex)
        {
            var metaIndex = systemState->UnmanagedMetaIndex;
            var systemPointer = systemState->m_SystemPtr;
            var delegates = s_Data.Data.GetSystemDelegates(metaIndex);
            bool isBurst = true;
            CheckBurst(ref isBurst);

            if (0 != (delegates.PresentFunctionBits & (1 << functionIndex)))
            {
                if (isBurst)
                {
                    // Burst: we're calling either directly into Burst code, or we are calling into a managed wrapper.
                    // In any case, creating the function pointer from the IntPtr is free.
                    new FunctionPointer<ForwardingFunc>((IntPtr)delegates.BurstFunctions[functionIndex]).Invoke((IntPtr)systemPointer, (IntPtr)systemState);
                }
                else
                {
                    // We're in managed land. We may be calling into either a managed routine, or into Burst code.
                    // We have a managed delegate GCHandle ready to go.
                    var delegatePtr = (IntPtr)delegates.ManagedFunctions[functionIndex];
                    ForwardToManaged(delegatePtr, systemState, systemPointer);
                }
            }
        }

        internal static unsafe ref readonly UnmanagedComponentSystemDelegates GetDelegates(SystemState* systemState)
        {
            var metaIndex = systemState->UnmanagedMetaIndex;
            return ref s_Data.Data.GetSystemDelegates(metaIndex);
        }

        // Returns if OnUpdate is going to use burst.
        // This method is unfortunately only an approximation.
        // It doesn't work if:
        // * Burst failed to compile
        // * Burst has completed compilation of the method yet
        // Unfortunately there is no way to get this data from burst yet.
        // BUR-1651
        internal static bool IsOnUpdateUsingBurst(in UnmanagedComponentSystemDelegates delegates)
        {
            bool isBurst = true;
            CheckBurst(ref isBurst);

            return isBurst & (delegates.BurstFunctionBits & 2) != 0;
        }

        [GenerateTestsForBurstCompatibility]
        [Burst.CompilerServices.IgnoreWarning(1371)]
        internal static unsafe void CallOnUpdate(in UnmanagedComponentSystemDelegates delegates, SystemState* systemState)
        {
            bool isBurst = true;
            CheckBurst(ref isBurst);

            if (isBurst)
            {
                // Burst: we're calling either directly into Burst code, or we are calling into a managed wrapper.
                // In any case, creating the function pointer from the IntPtr is free.
                new FunctionPointer<ForwardingFunc>((IntPtr)delegates.BurstFunctions[1]).Invoke((IntPtr)systemState->m_SystemPtr, (IntPtr)systemState);
            }
            else
            {
                // We're in managed land. We may be calling into either a managed routine, or into Burst code.
                // We have a managed delegate GCHandle ready to go.
                var delegatePtr = (IntPtr)delegates.ManagedFunctions[1];
                ForwardToManaged(delegatePtr, systemState, systemState->m_SystemPtr);
            }
        }

        [BurstDiscard]
        private static unsafe void ForwardToManaged(IntPtr delegateIntPtr, SystemState* systemState, void* systemPointer)
        {
            GCHandle h = GCHandle.FromIntPtr(delegateIntPtr);
            ((ForwardingFunc)h.Target)((IntPtr)systemPointer, (IntPtr)systemState);
        }

        [GenerateTestsForBurstCompatibility]
        internal static unsafe void CallOnCreate(SystemState* systemState)
        {
            CallForwardingFunction(systemState, 0);
        }

        [GenerateTestsForBurstCompatibility]
        internal static unsafe void CallOnUpdate(SystemState* systemState)
        {
            CallForwardingFunction(systemState, 1);
        }

        [GenerateTestsForBurstCompatibility]
        internal static unsafe void CallOnDestroy(SystemState* systemState)
        {
            CallForwardingFunction(systemState, 2);
        }

        [GenerateTestsForBurstCompatibility]
        internal static unsafe void CallOnStartRunning(SystemState* systemState)
        {
            CallForwardingFunction(systemState, 3);
        }

        [GenerateTestsForBurstCompatibility]
        internal static unsafe void CallOnStopRunning(SystemState* systemState)
        {
            CallForwardingFunction(systemState, 4);
        }

        [GenerateTestsForBurstCompatibility]
        internal static unsafe void CallOnCreateForCompiler(SystemState* systemState)
        {
            CallForwardingFunction(systemState, 5);
        }

        [ExcludeFromBurstCompatTesting("returns managed Type")]
        internal static Type GetStructType(int metaIndex)
        {
            return Managed.s_StructTypes[metaIndex];
        }
    }
}
