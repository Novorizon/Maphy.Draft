//HintName: Verify.gen__Aspect_977025139.g.cs
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Tests;


    public readonly partial struct AspectSimple : global::Unity.Entities.IAspect, global::Unity.Entities.IAspectCreate<AspectSimple>
    {
        AspectSimple(global::Unity.Entities.RefRW<global::Unity.Entities.Tests.EcsTestData> data)
        {
            this.Data = data;


        }
        public AspectSimple CreateAspect(global::Unity.Entities.Entity entity, ref global::Unity.Entities.SystemState systemState, bool isReadOnly)
        {
            var lookup = new Lookup(ref systemState, isReadOnly);
            return lookup[entity];
        }

        public static global::Unity.Entities.ComponentType[] ExcludeComponents => global::System.Array.Empty<Unity.Entities.ComponentType>();
        static global::Unity.Entities.ComponentType[] s_RequiredComponents => new [] {  global::Unity.Entities.ComponentType.ReadWrite<global::Unity.Entities.Tests.EcsTestData>() };
        static global::Unity.Entities.ComponentType[] s_RequiredComponentsRO => new [] {  global::Unity.Entities.ComponentType.ReadOnly<global::Unity.Entities.Tests.EcsTestData>() };
        public static global::Unity.Entities.ComponentType[] RequiredComponents => s_RequiredComponents;
        public static global::Unity.Entities.ComponentType[] RequiredComponentsRO => s_RequiredComponentsRO;
        public struct Lookup
        {
            bool _IsReadOnly
            {
                get { return __IsReadOnly == 1; }
                set { __IsReadOnly = value ? (byte) 1 : (byte) 0; }
            }
            private byte __IsReadOnly;

            global::Unity.Entities.ComponentLookup<global::Unity.Entities.Tests.EcsTestData> DataComponentLookup;



            public Lookup(ref global::Unity.Entities.SystemState state, bool isReadOnly)
            {
                __IsReadOnly = isReadOnly ? (byte) 1u : (byte) 0u;
                this.DataComponentLookup = state.GetComponentLookup<global::Unity.Entities.Tests.EcsTestData>(isReadOnly);



            }
            public void Update(ref global::Unity.Entities.SystemState state)
            {
                this.DataComponentLookup.Update(ref state);


            }
            public AspectSimple this[global::Unity.Entities.Entity entity]
            {
                get
                {
                    return new AspectSimple(this.DataComponentLookup.GetRefRW(entity, _IsReadOnly));
                }
            }
        }
        public struct ResolvedChunk
        {

            internal global::Unity.Collections.NativeArray<global::Unity.Entities.Tests.EcsTestData> Data;


            public AspectSimple this[int index]
            {
                get
                {
                    return new AspectSimple(                        new global::Unity.Entities.RefRW<Unity.Entities.Tests.EcsTestData>(this.Data, index));
                }
            }
            public int Length;
        }
        public struct TypeHandle
        {
            global::Unity.Entities.ComponentTypeHandle<global::Unity.Entities.Tests.EcsTestData> DataCth;




            public TypeHandle(ref global::Unity.Entities.SystemState state, bool isReadOnly)
            {
                this.DataCth = state.GetComponentTypeHandle<global::Unity.Entities.Tests.EcsTestData>(isReadOnly);




            }
            public void Update(ref global::Unity.Entities.SystemState state)
            {
                this.DataCth.Update(ref state);



            }
            public ResolvedChunk Resolve(global::Unity.Entities.ArchetypeChunk chunk)
            {
                ResolvedChunk resolved;


                resolved.Data = chunk.GetNativeArray(ref this.DataCth);

                resolved.Length = chunk.Count;
                return resolved;
            }
        }
        public static Enumerator Query(global::Unity.Entities.EntityQuery query, TypeHandle typeHandle) { return new Enumerator(query, typeHandle); }
        public struct Enumerator : global::System.Collections.Generic.IEnumerator<AspectSimple>, global::System.Collections.Generic.IEnumerable<AspectSimple>
        {
            ResolvedChunk                                _Resolved;
            global::Unity.Entities.EntityQueryEnumerator _QueryEnumerator;
            TypeHandle                                   _Handle;
            internal Enumerator(global::Unity.Entities.EntityQuery query, TypeHandle typeHandle)
            {
                _QueryEnumerator = new global::Unity.Entities.EntityQueryEnumerator(query);
                _Handle = typeHandle;
                _Resolved = default;
            }
            public void Dispose() { _QueryEnumerator.Dispose(); }
            public bool MoveNext()
            {
                if (_QueryEnumerator.MoveNextHotLoop())
                    return true;
                return MoveNextCold();
            }
            [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
            bool MoveNextCold()
            {
                var didMove = _QueryEnumerator.MoveNextColdLoop(out var chunk);
                if (didMove)
                    _Resolved = _Handle.Resolve(chunk);
                return didMove;
            }
            public AspectSimple Current {
                get {
                    #if ENABLE_UNITY_COLLECTIONS_CHECKS || UNITY_DOTS_DEBUG
                        _QueryEnumerator.CheckDisposed();
                    #endif
                        return _Resolved[_QueryEnumerator.IndexInChunk];
                    }
            }
            public Enumerator GetEnumerator()  { return this; }
            void global::System.Collections.IEnumerator.Reset() => throw new global::System.NotImplementedException();
            object global::System.Collections.IEnumerator.Current => throw new global::System.NotImplementedException();
            global::System.Collections.Generic.IEnumerator<AspectSimple> global::System.Collections.Generic.IEnumerable<AspectSimple>.GetEnumerator() => throw new global::System.NotImplementedException();
            global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()=> throw new global::System.NotImplementedException();
        }

        /// <summary>
        /// Completes the dependency chain required for this aspect to have read access.
        /// So it completes all write dependencies of the components, buffers, etc. to allow for reading.
        /// </summary>
        /// <param name="state">The <see cref="SystemState"/> containing an <see cref="EntityManager"/> storing all dependencies.</param>
        public static void CompleteDependencyBeforeRO(ref global::Unity.Entities.SystemState state){
           state.EntityManager.CompleteDependencyBeforeRO<global::Unity.Entities.Tests.EcsTestData>();
        }

        /// <summary>
        /// Completes the dependency chain required for this component to have read and write access.
        /// So it completes all write dependencies of the components, buffers, etc. to allow for reading,
        /// and it completes all read dependencies, so we can write to it.
        /// </summary>
        /// <param name="state">The <see cref="SystemState"/> containing an <see cref="EntityManager"/> storing all dependencies.</param>
        public static void CompleteDependencyBeforeRW(ref global::Unity.Entities.SystemState state){
           state.EntityManager.CompleteDependencyBeforeRW<global::Unity.Entities.Tests.EcsTestData>();
        }
    }

namespace AspectTests
{

    public readonly partial struct Aspect2 : global::Unity.Entities.IAspect, global::Unity.Entities.IAspectCreate<Aspect2>
    {
        Aspect2(global::Unity.Entities.Entity entity,global::Unity.Entities.RefRW<global::Unity.Entities.Tests.EcsTestData> data,global::Unity.Entities.RefRW<global::Unity.Entities.Tests.EcsTestData2> data2,global::Unity.Entities.RefRW<global::Unity.Entities.Tests.EcsTestData3> data3,global::Unity.Entities.RefRO<global::Unity.Entities.Tests.EcsTestData4> dataro,global::Unity.Entities.RefRW<global::Unity.Entities.Tests.EcsTestData5> dataoptional,global::Unity.Entities.DynamicBuffer<global::Unity.Entities.Tests.EcsIntElement> dynamicbuffer,global::AspectSimple nestedaspectsimple,global::Unity.Entities.EnabledRefRW<global::Unity.Entities.Tests.EcsTestDataEnableable> ecstestdataenableable)
        {
            this.Data = data;
            this.Data2 = data2;
            this.Data3 = data3;
            this.DataRO = dataro;
            this.DataOptional = dataoptional;
            this.DynamicBuffer = dynamicbuffer;
            this.NestedAspectSimple = nestedaspectsimple;
            this.EcsTestDataEnableable = ecstestdataenableable;

            this.Self = entity;

        }
        public Aspect2 CreateAspect(global::Unity.Entities.Entity entity, ref global::Unity.Entities.SystemState systemState, bool isReadOnly)
        {
            var lookup = new Lookup(ref systemState, isReadOnly);
            return lookup[entity];
        }

        public static global::Unity.Entities.ComponentType[] ExcludeComponents => global::System.Array.Empty<Unity.Entities.ComponentType>();
        static global::Unity.Entities.ComponentType[] s_RequiredComponents => global::Unity.Entities.ComponentType.Combine(new [] {  global::Unity.Entities.ComponentType.ReadWrite<global::Unity.Entities.Tests.EcsTestData>(), global::Unity.Entities.ComponentType.ReadWrite<global::Unity.Entities.Tests.EcsTestData2>(), global::Unity.Entities.ComponentType.ReadWrite<global::Unity.Entities.Tests.EcsTestData3>(), global::Unity.Entities.ComponentType.ReadOnly<global::Unity.Entities.Tests.EcsTestData4>(), global::Unity.Entities.ComponentType.ReadWrite<global::Unity.Entities.Tests.EcsIntElement>(), global::Unity.Entities.ComponentType.ReadWrite<global::Unity.Entities.Tests.EcsTestDataEnableable>() },  AspectSimple.RequiredComponentsRO);
        static global::Unity.Entities.ComponentType[] s_RequiredComponentsRO => global::Unity.Entities.ComponentType.Combine(new [] {  global::Unity.Entities.ComponentType.ReadOnly<global::Unity.Entities.Tests.EcsTestData>(), global::Unity.Entities.ComponentType.ReadOnly<global::Unity.Entities.Tests.EcsTestData2>(), global::Unity.Entities.ComponentType.ReadOnly<global::Unity.Entities.Tests.EcsTestData3>(), global::Unity.Entities.ComponentType.ReadOnly<global::Unity.Entities.Tests.EcsTestData4>(), global::Unity.Entities.ComponentType.ReadOnly<global::Unity.Entities.Tests.EcsIntElement>(), global::Unity.Entities.ComponentType.ReadOnly<global::Unity.Entities.Tests.EcsTestDataEnableable>() },  AspectSimple.RequiredComponentsRO);
        public static global::Unity.Entities.ComponentType[] RequiredComponents => s_RequiredComponents;
        public static global::Unity.Entities.ComponentType[] RequiredComponentsRO => s_RequiredComponentsRO;
        public struct Lookup
        {
            bool _IsReadOnly
            {
                get { return __IsReadOnly == 1; }
                set { __IsReadOnly = value ? (byte) 1 : (byte) 0; }
            }
            private byte __IsReadOnly;

            global::Unity.Entities.ComponentLookup<global::Unity.Entities.Tests.EcsTestData> DataComponentLookup;
            global::Unity.Entities.ComponentLookup<global::Unity.Entities.Tests.EcsTestData2> Data2ComponentLookup;
            global::Unity.Entities.ComponentLookup<global::Unity.Entities.Tests.EcsTestData3> Data3ComponentLookup;
            [global::Unity.Collections.ReadOnly]
            global::Unity.Entities.ComponentLookup<global::Unity.Entities.Tests.EcsTestData4> DataROComponentLookup;
            global::Unity.Entities.ComponentLookup<global::Unity.Entities.Tests.EcsTestData5> DataOptionalComponentLookup;
            global::Unity.Entities.ComponentLookup<global::Unity.Entities.Tests.EcsTestDataEnableable> EcsTestDataEnableableComponentLookup;

            global::Unity.Entities.BufferLookup<Unity.Entities.Tests.EcsIntElement> DynamicBufferDBuff;
            [global::Unity.Collections.ReadOnly]
            global::AspectSimple.Lookup NestedAspectSimple;
            public Lookup(ref global::Unity.Entities.SystemState state, bool isReadOnly)
            {
                __IsReadOnly = isReadOnly ? (byte) 1u : (byte) 0u;
                this.DataComponentLookup = state.GetComponentLookup<global::Unity.Entities.Tests.EcsTestData>(isReadOnly);
                this.Data2ComponentLookup = state.GetComponentLookup<global::Unity.Entities.Tests.EcsTestData2>(isReadOnly);
                this.Data3ComponentLookup = state.GetComponentLookup<global::Unity.Entities.Tests.EcsTestData3>(isReadOnly);
                this.DataROComponentLookup = state.GetComponentLookup<global::Unity.Entities.Tests.EcsTestData4>(true);
                this.DataOptionalComponentLookup = state.GetComponentLookup<global::Unity.Entities.Tests.EcsTestData5>(isReadOnly);
                this.EcsTestDataEnableableComponentLookup = state.GetComponentLookup<global::Unity.Entities.Tests.EcsTestDataEnableable>(isReadOnly);

                this.DynamicBufferDBuff = state.GetBufferLookup<Unity.Entities.Tests.EcsIntElement>(isReadOnly);
                this.NestedAspectSimple = new global::AspectSimple.Lookup(ref state, true);
            }
            public void Update(ref global::Unity.Entities.SystemState state)
            {
                this.DataComponentLookup.Update(ref state);
                this.Data2ComponentLookup.Update(ref state);
                this.Data3ComponentLookup.Update(ref state);
                this.DataROComponentLookup.Update(ref state);
                this.DataOptionalComponentLookup.Update(ref state);
                this.EcsTestDataEnableableComponentLookup.Update(ref state);

                this.DynamicBufferDBuff.Update(ref state);
                this.NestedAspectSimple.Update(ref state);
            }
            public Aspect2 this[global::Unity.Entities.Entity entity]
            {
                get
                {
                    return new Aspect2(entity,this.DataComponentLookup.GetRefRW(entity, _IsReadOnly),this.Data2ComponentLookup.GetRefRW(entity, _IsReadOnly),this.Data3ComponentLookup.GetRefRW(entity, _IsReadOnly),this.DataROComponentLookup.GetRefRO(entity),this.DataOptionalComponentLookup.GetRefRWOptional(entity, _IsReadOnly),this.DynamicBufferDBuff[entity],this.NestedAspectSimple[entity],this.EcsTestDataEnableableComponentLookup.GetEnabledRefRW<global::Unity.Entities.Tests.EcsTestDataEnableable>(entity, _IsReadOnly));
                }
            }
        }
        public struct ResolvedChunk
        {
            internal global::Unity.Collections.NativeArray<global::Unity.Entities.Entity> m_Entities;
            internal global::Unity.Entities.BufferAccessor<global::Unity.Entities.Tests.EcsIntElement> DynamicBufferDBuff;
            internal global::Unity.Collections.NativeArray<global::Unity.Entities.Tests.EcsTestData> Data;
            internal global::Unity.Collections.NativeArray<global::Unity.Entities.Tests.EcsTestData2> Data2;
            internal global::Unity.Collections.NativeArray<global::Unity.Entities.Tests.EcsTestData3> Data3;
            internal global::Unity.Collections.NativeArray<global::Unity.Entities.Tests.EcsTestData4> DataRO;
            internal global::Unity.Collections.NativeArray<global::Unity.Entities.Tests.EcsTestData5> DataOptional;
            internal global::Unity.Entities.EnabledMask EcsTestDataEnableable;

            internal global::AspectSimple.ResolvedChunk NestedAspectSimple;
            public Aspect2 this[int index]
            {
                get
                {
                    return new Aspect2(m_Entities[index],
                        new global::Unity.Entities.RefRW<Unity.Entities.Tests.EcsTestData>(this.Data, index),
                        new global::Unity.Entities.RefRW<Unity.Entities.Tests.EcsTestData2>(this.Data2, index),
                        new global::Unity.Entities.RefRW<Unity.Entities.Tests.EcsTestData3>(this.Data3, index),
                        new global::Unity.Entities.RefRO<Unity.Entities.Tests.EcsTestData4>(this.DataRO, index),
                        global::Unity.Entities.RefRW<Unity.Entities.Tests.EcsTestData5>.Optional(this.DataOptional, index),
this.DynamicBufferDBuff[index],
this.NestedAspectSimple[index],
this.EcsTestDataEnableable.GetEnabledRefRW<Unity.Entities.Tests.EcsTestDataEnableable>(index));
                }
            }
            public int Length;
        }
        public struct TypeHandle
        {
            global::Unity.Entities.ComponentTypeHandle<global::Unity.Entities.Tests.EcsTestData> DataCth;
            global::Unity.Entities.ComponentTypeHandle<global::Unity.Entities.Tests.EcsTestData2> Data2Cth;
            global::Unity.Entities.ComponentTypeHandle<global::Unity.Entities.Tests.EcsTestData3> Data3Cth;
            [global::Unity.Collections.ReadOnly]
            global::Unity.Entities.ComponentTypeHandle<global::Unity.Entities.Tests.EcsTestData4> DataROCth;
            global::Unity.Entities.ComponentTypeHandle<global::Unity.Entities.Tests.EcsTestData5> DataOptionalCth;
            global::Unity.Entities.ComponentTypeHandle<global::Unity.Entities.Tests.EcsTestDataEnableable> EcsTestDataEnableableCth;

            global::Unity.Entities.EntityTypeHandle m_Entities;
            global::Unity.Entities.BufferTypeHandle<global::Unity.Entities.Tests.EcsIntElement> DynamicBufferDBuff;
            [global::Unity.Collections.ReadOnly]
            global::AspectSimple.TypeHandle NestedAspectSimple;
            public TypeHandle(ref global::Unity.Entities.SystemState state, bool isReadOnly)
            {
                this.DataCth = state.GetComponentTypeHandle<global::Unity.Entities.Tests.EcsTestData>(isReadOnly);
                this.Data2Cth = state.GetComponentTypeHandle<global::Unity.Entities.Tests.EcsTestData2>(isReadOnly);
                this.Data3Cth = state.GetComponentTypeHandle<global::Unity.Entities.Tests.EcsTestData3>(isReadOnly);
                this.DataROCth = state.GetComponentTypeHandle<global::Unity.Entities.Tests.EcsTestData4>(true);
                this.DataOptionalCth = state.GetComponentTypeHandle<global::Unity.Entities.Tests.EcsTestData5>(isReadOnly);
                this.EcsTestDataEnableableCth = state.GetComponentTypeHandle<global::Unity.Entities.Tests.EcsTestDataEnableable>(isReadOnly);

                this.m_Entities = state.GetEntityTypeHandle();
                this.DynamicBufferDBuff = state.GetBufferTypeHandle<global::Unity.Entities.Tests.EcsIntElement>(isReadOnly);
                this.NestedAspectSimple = new global::AspectSimple.TypeHandle(ref state, true);
            }
            public void Update(ref global::Unity.Entities.SystemState state)
            {
                this.DataCth.Update(ref state);
                this.Data2Cth.Update(ref state);
                this.Data3Cth.Update(ref state);
                this.DataROCth.Update(ref state);
                this.DataOptionalCth.Update(ref state);
                this.EcsTestDataEnableableCth.Update(ref state);

                this.m_Entities.Update(ref state);
                this.DynamicBufferDBuff.Update(ref state);
                this.NestedAspectSimple.Update(ref state);
            }
            public ResolvedChunk Resolve(global::Unity.Entities.ArchetypeChunk chunk)
            {
                ResolvedChunk resolved;
                resolved.m_Entities = chunk.GetNativeArray(this.m_Entities);
                resolved.NestedAspectSimple = this.NestedAspectSimple.Resolve(chunk);
                resolved.DynamicBufferDBuff = chunk.GetBufferAccessor(ref this.DynamicBufferDBuff);
                resolved.Data = chunk.GetNativeArray(ref this.DataCth);
                resolved.Data2 = chunk.GetNativeArray(ref this.Data2Cth);
                resolved.Data3 = chunk.GetNativeArray(ref this.Data3Cth);
                resolved.DataRO = chunk.GetNativeArray(ref this.DataROCth);
                resolved.DataOptional = chunk.GetNativeArray(ref this.DataOptionalCth);
                resolved.EcsTestDataEnableable = chunk.GetEnabledMask(ref this.EcsTestDataEnableableCth);

                resolved.Length = chunk.Count;
                return resolved;
            }
        }
        public static Enumerator Query(global::Unity.Entities.EntityQuery query, TypeHandle typeHandle) { return new Enumerator(query, typeHandle); }
        public struct Enumerator : global::System.Collections.Generic.IEnumerator<Aspect2>, global::System.Collections.Generic.IEnumerable<Aspect2>
        {
            ResolvedChunk                                _Resolved;
            global::Unity.Entities.EntityQueryEnumerator _QueryEnumerator;
            TypeHandle                                   _Handle;
            internal Enumerator(global::Unity.Entities.EntityQuery query, TypeHandle typeHandle)
            {
                _QueryEnumerator = new global::Unity.Entities.EntityQueryEnumerator(query);
                _Handle = typeHandle;
                _Resolved = default;
            }
            public void Dispose() { _QueryEnumerator.Dispose(); }
            public bool MoveNext()
            {
                if (_QueryEnumerator.MoveNextHotLoop())
                    return true;
                return MoveNextCold();
            }
            [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
            bool MoveNextCold()
            {
                var didMove = _QueryEnumerator.MoveNextColdLoop(out var chunk);
                if (didMove)
                    _Resolved = _Handle.Resolve(chunk);
                return didMove;
            }
            public Aspect2 Current {
                get {
                    #if ENABLE_UNITY_COLLECTIONS_CHECKS || UNITY_DOTS_DEBUG
                        _QueryEnumerator.CheckDisposed();
                    #endif
                        return _Resolved[_QueryEnumerator.IndexInChunk];
                    }
            }
            public Enumerator GetEnumerator()  { return this; }
            void global::System.Collections.IEnumerator.Reset() => throw new global::System.NotImplementedException();
            object global::System.Collections.IEnumerator.Current => throw new global::System.NotImplementedException();
            global::System.Collections.Generic.IEnumerator<Aspect2> global::System.Collections.Generic.IEnumerable<Aspect2>.GetEnumerator() => throw new global::System.NotImplementedException();
            global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()=> throw new global::System.NotImplementedException();
        }

        /// <summary>
        /// Completes the dependency chain required for this aspect to have read access.
        /// So it completes all write dependencies of the components, buffers, etc. to allow for reading.
        /// </summary>
        /// <param name="state">The <see cref="SystemState"/> containing an <see cref="EntityManager"/> storing all dependencies.</param>
        public static void CompleteDependencyBeforeRO(ref global::Unity.Entities.SystemState state){
           state.EntityManager.CompleteDependencyBeforeRO<global::Unity.Entities.Tests.EcsTestData>();
           state.EntityManager.CompleteDependencyBeforeRO<global::Unity.Entities.Tests.EcsTestData2>();
           state.EntityManager.CompleteDependencyBeforeRO<global::Unity.Entities.Tests.EcsTestData3>();
           state.EntityManager.CompleteDependencyBeforeRO<global::Unity.Entities.Tests.EcsTestData4>();
           state.EntityManager.CompleteDependencyBeforeRO<global::Unity.Entities.Tests.EcsTestData5>();
           state.EntityManager.CompleteDependencyBeforeRO<global::Unity.Entities.Tests.EcsIntElement>();
           AspectSimple.CompleteDependencyBeforeRW(ref state);
           state.EntityManager.CompleteDependencyBeforeRO<global::Unity.Entities.Tests.EcsTestDataEnableable>();
        }

        /// <summary>
        /// Completes the dependency chain required for this component to have read and write access.
        /// So it completes all write dependencies of the components, buffers, etc. to allow for reading,
        /// and it completes all read dependencies, so we can write to it.
        /// </summary>
        /// <param name="state">The <see cref="SystemState"/> containing an <see cref="EntityManager"/> storing all dependencies.</param>
        public static void CompleteDependencyBeforeRW(ref global::Unity.Entities.SystemState state){
           state.EntityManager.CompleteDependencyBeforeRW<global::Unity.Entities.Tests.EcsTestData>();
           state.EntityManager.CompleteDependencyBeforeRW<global::Unity.Entities.Tests.EcsTestData2>();
           state.EntityManager.CompleteDependencyBeforeRW<global::Unity.Entities.Tests.EcsTestData3>();
           state.EntityManager.CompleteDependencyBeforeRO<global::Unity.Entities.Tests.EcsTestData4>();
           state.EntityManager.CompleteDependencyBeforeRW<global::Unity.Entities.Tests.EcsTestData5>();
           state.EntityManager.CompleteDependencyBeforeRW<global::Unity.Entities.Tests.EcsIntElement>();
           AspectSimple.CompleteDependencyBeforeRW(ref state);
           state.EntityManager.CompleteDependencyBeforeRW<global::Unity.Entities.Tests.EcsTestDataEnableable>();
        }
    }
}

namespace AspectTests
{

    public readonly partial struct AspectNestedAliasing : global::Unity.Entities.IAspect, global::Unity.Entities.IAspectCreate<AspectNestedAliasing>
    {
        AspectNestedAliasing(global::Unity.Entities.Entity entity,global::AspectTests.Aspect2 aspect2,global::Unity.Entities.RefRW<global::Unity.Entities.Tests.EcsTestData> data,global::Unity.Entities.RefRW<global::Unity.Entities.Tests.EcsTestData2> data2,global::Unity.Entities.RefRW<global::Unity.Entities.Tests.EcsTestData3> data3,global::Unity.Entities.RefRO<global::Unity.Entities.Tests.EcsTestData4> dataro,global::Unity.Entities.RefRW<global::Unity.Entities.Tests.EcsTestData5> dataoptional,global::Unity.Entities.DynamicBuffer<global::Unity.Entities.Tests.EcsIntElement> dynamicbuffer,global::AspectSimple nestedaspectsimple,global::Unity.Entities.EnabledRefRW<global::Unity.Entities.Tests.EcsTestDataEnableable> ecstestdataenableable)
        {
            this.Aspect2 = aspect2;
            this.Data = data;
            this.Data2 = data2;
            this.Data3 = data3;
            this.DataRO = dataro;
            this.DataOptional = dataoptional;
            this.DynamicBuffer = dynamicbuffer;
            this.NestedAspectSimple = nestedaspectsimple;
            this.EcsTestDataEnableable = ecstestdataenableable;

            this.Self = entity;

        }
        public AspectNestedAliasing CreateAspect(global::Unity.Entities.Entity entity, ref global::Unity.Entities.SystemState systemState, bool isReadOnly)
        {
            var lookup = new Lookup(ref systemState, isReadOnly);
            return lookup[entity];
        }

        public static global::Unity.Entities.ComponentType[] ExcludeComponents => global::System.Array.Empty<Unity.Entities.ComponentType>();
        static global::Unity.Entities.ComponentType[] s_RequiredComponents => global::Unity.Entities.ComponentType.Combine(new [] {  global::Unity.Entities.ComponentType.ReadWrite<global::Unity.Entities.Tests.EcsTestData>(), global::Unity.Entities.ComponentType.ReadWrite<global::Unity.Entities.Tests.EcsTestData2>(), global::Unity.Entities.ComponentType.ReadWrite<global::Unity.Entities.Tests.EcsTestData3>(), global::Unity.Entities.ComponentType.ReadOnly<global::Unity.Entities.Tests.EcsTestData4>(), global::Unity.Entities.ComponentType.ReadWrite<global::Unity.Entities.Tests.EcsIntElement>(), global::Unity.Entities.ComponentType.ReadWrite<global::Unity.Entities.Tests.EcsTestDataEnableable>() },  AspectTests.Aspect2.RequiredComponents, AspectSimple.RequiredComponentsRO);
        static global::Unity.Entities.ComponentType[] s_RequiredComponentsRO => global::Unity.Entities.ComponentType.Combine(new [] {  global::Unity.Entities.ComponentType.ReadOnly<global::Unity.Entities.Tests.EcsTestData>(), global::Unity.Entities.ComponentType.ReadOnly<global::Unity.Entities.Tests.EcsTestData2>(), global::Unity.Entities.ComponentType.ReadOnly<global::Unity.Entities.Tests.EcsTestData3>(), global::Unity.Entities.ComponentType.ReadOnly<global::Unity.Entities.Tests.EcsTestData4>(), global::Unity.Entities.ComponentType.ReadOnly<global::Unity.Entities.Tests.EcsIntElement>(), global::Unity.Entities.ComponentType.ReadOnly<global::Unity.Entities.Tests.EcsTestDataEnableable>() },  AspectTests.Aspect2.RequiredComponentsRO, AspectSimple.RequiredComponentsRO);
        public static global::Unity.Entities.ComponentType[] RequiredComponents => s_RequiredComponents;
        public static global::Unity.Entities.ComponentType[] RequiredComponentsRO => s_RequiredComponentsRO;
        public struct Lookup
        {
            bool _IsReadOnly
            {
                get { return __IsReadOnly == 1; }
                set { __IsReadOnly = value ? (byte) 1 : (byte) 0; }
            }
            private byte __IsReadOnly;

            global::Unity.Entities.ComponentLookup<global::Unity.Entities.Tests.EcsTestData> DataComponentLookup;
            global::Unity.Entities.ComponentLookup<global::Unity.Entities.Tests.EcsTestData2> Data2ComponentLookup;
            global::Unity.Entities.ComponentLookup<global::Unity.Entities.Tests.EcsTestData3> Data3ComponentLookup;
            [global::Unity.Collections.ReadOnly]
            global::Unity.Entities.ComponentLookup<global::Unity.Entities.Tests.EcsTestData4> DataROComponentLookup;
            global::Unity.Entities.ComponentLookup<global::Unity.Entities.Tests.EcsTestData5> DataOptionalComponentLookup;
            global::Unity.Entities.ComponentLookup<global::Unity.Entities.Tests.EcsTestDataEnableable> EcsTestDataEnableableComponentLookup;

            global::Unity.Entities.BufferLookup<Unity.Entities.Tests.EcsIntElement> DynamicBufferDBuff;
            global::AspectTests.Aspect2.Lookup Aspect2;
            [global::Unity.Collections.ReadOnly]
            global::AspectSimple.Lookup NestedAspectSimple;
            public Lookup(ref global::Unity.Entities.SystemState state, bool isReadOnly)
            {
                __IsReadOnly = isReadOnly ? (byte) 1u : (byte) 0u;
                this.DataComponentLookup = state.GetComponentLookup<global::Unity.Entities.Tests.EcsTestData>(isReadOnly);
                this.Data2ComponentLookup = state.GetComponentLookup<global::Unity.Entities.Tests.EcsTestData2>(isReadOnly);
                this.Data3ComponentLookup = state.GetComponentLookup<global::Unity.Entities.Tests.EcsTestData3>(isReadOnly);
                this.DataROComponentLookup = state.GetComponentLookup<global::Unity.Entities.Tests.EcsTestData4>(true);
                this.DataOptionalComponentLookup = state.GetComponentLookup<global::Unity.Entities.Tests.EcsTestData5>(isReadOnly);
                this.EcsTestDataEnableableComponentLookup = state.GetComponentLookup<global::Unity.Entities.Tests.EcsTestDataEnableable>(isReadOnly);

                this.DynamicBufferDBuff = state.GetBufferLookup<Unity.Entities.Tests.EcsIntElement>(isReadOnly);
                this.Aspect2 = new global::AspectTests.Aspect2.Lookup(ref state, isReadOnly);
                this.NestedAspectSimple = new global::AspectSimple.Lookup(ref state, true);
            }
            public void Update(ref global::Unity.Entities.SystemState state)
            {
                this.DataComponentLookup.Update(ref state);
                this.Data2ComponentLookup.Update(ref state);
                this.Data3ComponentLookup.Update(ref state);
                this.DataROComponentLookup.Update(ref state);
                this.DataOptionalComponentLookup.Update(ref state);
                this.EcsTestDataEnableableComponentLookup.Update(ref state);

                this.Aspect2.Update(ref state);
                this.DynamicBufferDBuff.Update(ref state);
                this.NestedAspectSimple.Update(ref state);
            }
            public AspectNestedAliasing this[global::Unity.Entities.Entity entity]
            {
                get
                {
                    return new AspectNestedAliasing(entity,this.Aspect2[entity],this.DataComponentLookup.GetRefRW(entity, _IsReadOnly),this.Data2ComponentLookup.GetRefRW(entity, _IsReadOnly),this.Data3ComponentLookup.GetRefRW(entity, _IsReadOnly),this.DataROComponentLookup.GetRefRO(entity),this.DataOptionalComponentLookup.GetRefRWOptional(entity, _IsReadOnly),this.DynamicBufferDBuff[entity],this.NestedAspectSimple[entity],this.EcsTestDataEnableableComponentLookup.GetEnabledRefRW<global::Unity.Entities.Tests.EcsTestDataEnableable>(entity, _IsReadOnly));
                }
            }
        }
        public struct ResolvedChunk
        {
            internal global::Unity.Collections.NativeArray<global::Unity.Entities.Entity> m_Entities;
            internal global::Unity.Entities.BufferAccessor<global::Unity.Entities.Tests.EcsIntElement> DynamicBufferDBuff;
            internal global::Unity.Collections.NativeArray<global::Unity.Entities.Tests.EcsTestData> Data;
            internal global::Unity.Collections.NativeArray<global::Unity.Entities.Tests.EcsTestData2> Data2;
            internal global::Unity.Collections.NativeArray<global::Unity.Entities.Tests.EcsTestData3> Data3;
            internal global::Unity.Collections.NativeArray<global::Unity.Entities.Tests.EcsTestData4> DataRO;
            internal global::Unity.Collections.NativeArray<global::Unity.Entities.Tests.EcsTestData5> DataOptional;
            internal global::Unity.Entities.EnabledMask EcsTestDataEnableable;

            internal global::AspectTests.Aspect2.ResolvedChunk Aspect2;
            internal global::AspectSimple.ResolvedChunk NestedAspectSimple;
            public AspectNestedAliasing this[int index]
            {
                get
                {
                    return new AspectNestedAliasing(m_Entities[index],
this.Aspect2[index],
                        new global::Unity.Entities.RefRW<Unity.Entities.Tests.EcsTestData>(this.Data, index),
                        new global::Unity.Entities.RefRW<Unity.Entities.Tests.EcsTestData2>(this.Data2, index),
                        new global::Unity.Entities.RefRW<Unity.Entities.Tests.EcsTestData3>(this.Data3, index),
                        new global::Unity.Entities.RefRO<Unity.Entities.Tests.EcsTestData4>(this.DataRO, index),
                        global::Unity.Entities.RefRW<Unity.Entities.Tests.EcsTestData5>.Optional(this.DataOptional, index),
this.DynamicBufferDBuff[index],
this.NestedAspectSimple[index],
this.EcsTestDataEnableable.GetEnabledRefRW<Unity.Entities.Tests.EcsTestDataEnableable>(index));
                }
            }
            public int Length;
        }
        public struct TypeHandle
        {
            global::Unity.Entities.ComponentTypeHandle<global::Unity.Entities.Tests.EcsTestData> DataCth;
            global::Unity.Entities.ComponentTypeHandle<global::Unity.Entities.Tests.EcsTestData2> Data2Cth;
            global::Unity.Entities.ComponentTypeHandle<global::Unity.Entities.Tests.EcsTestData3> Data3Cth;
            [global::Unity.Collections.ReadOnly]
            global::Unity.Entities.ComponentTypeHandle<global::Unity.Entities.Tests.EcsTestData4> DataROCth;
            global::Unity.Entities.ComponentTypeHandle<global::Unity.Entities.Tests.EcsTestData5> DataOptionalCth;
            global::Unity.Entities.ComponentTypeHandle<global::Unity.Entities.Tests.EcsTestDataEnableable> EcsTestDataEnableableCth;

            global::Unity.Entities.EntityTypeHandle m_Entities;
            global::Unity.Entities.BufferTypeHandle<global::Unity.Entities.Tests.EcsIntElement> DynamicBufferDBuff;
            global::AspectTests.Aspect2.TypeHandle Aspect2;
            [global::Unity.Collections.ReadOnly]
            global::AspectSimple.TypeHandle NestedAspectSimple;
            public TypeHandle(ref global::Unity.Entities.SystemState state, bool isReadOnly)
            {
                this.DataCth = state.GetComponentTypeHandle<global::Unity.Entities.Tests.EcsTestData>(isReadOnly);
                this.Data2Cth = state.GetComponentTypeHandle<global::Unity.Entities.Tests.EcsTestData2>(isReadOnly);
                this.Data3Cth = state.GetComponentTypeHandle<global::Unity.Entities.Tests.EcsTestData3>(isReadOnly);
                this.DataROCth = state.GetComponentTypeHandle<global::Unity.Entities.Tests.EcsTestData4>(true);
                this.DataOptionalCth = state.GetComponentTypeHandle<global::Unity.Entities.Tests.EcsTestData5>(isReadOnly);
                this.EcsTestDataEnableableCth = state.GetComponentTypeHandle<global::Unity.Entities.Tests.EcsTestDataEnableable>(isReadOnly);

                this.m_Entities = state.GetEntityTypeHandle();
                this.DynamicBufferDBuff = state.GetBufferTypeHandle<global::Unity.Entities.Tests.EcsIntElement>(isReadOnly);
                this.Aspect2 = new global::AspectTests.Aspect2.TypeHandle(ref state, isReadOnly);
                this.NestedAspectSimple = new global::AspectSimple.TypeHandle(ref state, true);
            }
            public void Update(ref global::Unity.Entities.SystemState state)
            {
                this.DataCth.Update(ref state);
                this.Data2Cth.Update(ref state);
                this.Data3Cth.Update(ref state);
                this.DataROCth.Update(ref state);
                this.DataOptionalCth.Update(ref state);
                this.EcsTestDataEnableableCth.Update(ref state);

                this.m_Entities.Update(ref state);
                this.Aspect2.Update(ref state);
                this.DynamicBufferDBuff.Update(ref state);
                this.NestedAspectSimple.Update(ref state);
            }
            public ResolvedChunk Resolve(global::Unity.Entities.ArchetypeChunk chunk)
            {
                ResolvedChunk resolved;
                resolved.m_Entities = chunk.GetNativeArray(this.m_Entities);
                resolved.Aspect2 = this.Aspect2.Resolve(chunk);
                resolved.NestedAspectSimple = this.NestedAspectSimple.Resolve(chunk);
                resolved.DynamicBufferDBuff = chunk.GetBufferAccessor(ref this.DynamicBufferDBuff);
                resolved.Data = chunk.GetNativeArray(ref this.DataCth);
                resolved.Data2 = chunk.GetNativeArray(ref this.Data2Cth);
                resolved.Data3 = chunk.GetNativeArray(ref this.Data3Cth);
                resolved.DataRO = chunk.GetNativeArray(ref this.DataROCth);
                resolved.DataOptional = chunk.GetNativeArray(ref this.DataOptionalCth);
                resolved.EcsTestDataEnableable = chunk.GetEnabledMask(ref this.EcsTestDataEnableableCth);

                resolved.Length = chunk.Count;
                return resolved;
            }
        }
        public static Enumerator Query(global::Unity.Entities.EntityQuery query, TypeHandle typeHandle) { return new Enumerator(query, typeHandle); }
        public struct Enumerator : global::System.Collections.Generic.IEnumerator<AspectNestedAliasing>, global::System.Collections.Generic.IEnumerable<AspectNestedAliasing>
        {
            ResolvedChunk                                _Resolved;
            global::Unity.Entities.EntityQueryEnumerator _QueryEnumerator;
            TypeHandle                                   _Handle;
            internal Enumerator(global::Unity.Entities.EntityQuery query, TypeHandle typeHandle)
            {
                _QueryEnumerator = new global::Unity.Entities.EntityQueryEnumerator(query);
                _Handle = typeHandle;
                _Resolved = default;
            }
            public void Dispose() { _QueryEnumerator.Dispose(); }
            public bool MoveNext()
            {
                if (_QueryEnumerator.MoveNextHotLoop())
                    return true;
                return MoveNextCold();
            }
            [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
            bool MoveNextCold()
            {
                var didMove = _QueryEnumerator.MoveNextColdLoop(out var chunk);
                if (didMove)
                    _Resolved = _Handle.Resolve(chunk);
                return didMove;
            }
            public AspectNestedAliasing Current {
                get {
                    #if ENABLE_UNITY_COLLECTIONS_CHECKS || UNITY_DOTS_DEBUG
                        _QueryEnumerator.CheckDisposed();
                    #endif
                        return _Resolved[_QueryEnumerator.IndexInChunk];
                    }
            }
            public Enumerator GetEnumerator()  { return this; }
            void global::System.Collections.IEnumerator.Reset() => throw new global::System.NotImplementedException();
            object global::System.Collections.IEnumerator.Current => throw new global::System.NotImplementedException();
            global::System.Collections.Generic.IEnumerator<AspectNestedAliasing> global::System.Collections.Generic.IEnumerable<AspectNestedAliasing>.GetEnumerator() => throw new global::System.NotImplementedException();
            global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()=> throw new global::System.NotImplementedException();
        }

        /// <summary>
        /// Completes the dependency chain required for this aspect to have read access.
        /// So it completes all write dependencies of the components, buffers, etc. to allow for reading.
        /// </summary>
        /// <param name="state">The <see cref="SystemState"/> containing an <see cref="EntityManager"/> storing all dependencies.</param>
        public static void CompleteDependencyBeforeRO(ref global::Unity.Entities.SystemState state){
           AspectTests.Aspect2.CompleteDependencyBeforeRW(ref state);
           state.EntityManager.CompleteDependencyBeforeRO<global::Unity.Entities.Tests.EcsTestData>();
           state.EntityManager.CompleteDependencyBeforeRO<global::Unity.Entities.Tests.EcsTestData2>();
           state.EntityManager.CompleteDependencyBeforeRO<global::Unity.Entities.Tests.EcsTestData3>();
           state.EntityManager.CompleteDependencyBeforeRO<global::Unity.Entities.Tests.EcsTestData4>();
           state.EntityManager.CompleteDependencyBeforeRO<global::Unity.Entities.Tests.EcsTestData5>();
           state.EntityManager.CompleteDependencyBeforeRO<global::Unity.Entities.Tests.EcsIntElement>();
           AspectSimple.CompleteDependencyBeforeRW(ref state);
           state.EntityManager.CompleteDependencyBeforeRO<global::Unity.Entities.Tests.EcsTestDataEnableable>();
        }

        /// <summary>
        /// Completes the dependency chain required for this component to have read and write access.
        /// So it completes all write dependencies of the components, buffers, etc. to allow for reading,
        /// and it completes all read dependencies, so we can write to it.
        /// </summary>
        /// <param name="state">The <see cref="SystemState"/> containing an <see cref="EntityManager"/> storing all dependencies.</param>
        public static void CompleteDependencyBeforeRW(ref global::Unity.Entities.SystemState state){
           AspectTests.Aspect2.CompleteDependencyBeforeRO(ref state);
           state.EntityManager.CompleteDependencyBeforeRW<global::Unity.Entities.Tests.EcsTestData>();
           state.EntityManager.CompleteDependencyBeforeRW<global::Unity.Entities.Tests.EcsTestData2>();
           state.EntityManager.CompleteDependencyBeforeRW<global::Unity.Entities.Tests.EcsTestData3>();
           state.EntityManager.CompleteDependencyBeforeRO<global::Unity.Entities.Tests.EcsTestData4>();
           state.EntityManager.CompleteDependencyBeforeRW<global::Unity.Entities.Tests.EcsTestData5>();
           state.EntityManager.CompleteDependencyBeforeRW<global::Unity.Entities.Tests.EcsIntElement>();
           AspectSimple.CompleteDependencyBeforeRW(ref state);
           state.EntityManager.CompleteDependencyBeforeRW<global::Unity.Entities.Tests.EcsTestDataEnableable>();
        }
    }
}
