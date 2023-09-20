//HintName: Verify.gen__System_977025139.g.cs
#pragma warning disable 0219
#line 1 "Temp\GeneratedCode\SourceGen.VerifyTests"
using Unity.Entities;

[global::System.Runtime.CompilerServices.CompilerGenerated]
partial class EntitiesForEachDynamicBuffer : Unity.Entities.SystemBase
{
    [Unity.Entities.DOTSCompilerPatchedMethod("OnUpdate")]
    void __OnUpdate_1817F1CB()
    {
        #line 10 "Verify.gen.cs"
        EntitiesForEachDynamicBuffer_67594103_LambdaJob_0_Execute();
    }

    #line 16 "Temp\GeneratedCode\SourceGen.VerifyTests"
    [Unity.Burst.NoAlias]
    [Unity.Burst.BurstCompile(FloatMode = Unity.Burst.FloatMode.Default, FloatPrecision = Unity.Burst.FloatPrecision.Standard, CompileSynchronously = false)]
    struct EntitiesForEachDynamicBuffer_67594103_LambdaJob_0_Job : Unity.Entities.IJobChunk
    {
        internal static Unity.Entities.InternalCompilerInterface.JobChunkRunWithoutJobSystemDelegate FunctionPtrFieldNoBurst;
        internal static Unity.Entities.InternalCompilerInterface.JobChunkRunWithoutJobSystemDelegate FunctionPtrFieldBurst;
        public BufferTypeHandle<BufferData> __bufTypeHandle;
        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        void OriginalLambdaBody(DynamicBuffer<BufferData> buf)
        {
        }

        #line 29 "Temp\GeneratedCode\SourceGen.VerifyTests"
        [global::System.Runtime.CompilerServices.CompilerGenerated]
        public void Execute(in ArchetypeChunk chunk, int batchIndex, bool useEnabledMask, in Unity.Burst.Intrinsics.v128 chunkEnabledMask)
        {
            var bufAccessor = chunk.GetBufferAccessor(ref __bufTypeHandle);
            int chunkEntityCount = chunk.Count;
            if (!useEnabledMask)
            {
                for (var entityIndex = 0; entityIndex < chunkEntityCount; ++entityIndex)
                {
                    OriginalLambdaBody(bufAccessor[entityIndex]);
                }
            }
            else
            {
                int edgeCount = Unity.Mathematics.math.countbits(chunkEnabledMask.ULong0 ^ (chunkEnabledMask.ULong0 << 1)) + Unity.Mathematics.math.countbits(chunkEnabledMask.ULong1 ^ (chunkEnabledMask.ULong1 << 1)) - 1;
                bool useRanges = edgeCount <= 4;
                if (useRanges)
                {
                    var enabledMask = chunkEnabledMask;
                    int entityIndex = 0;
                    int batchEndIndex = 0;
                    while (EnabledBitUtility.GetNextRange(ref enabledMask, ref entityIndex, ref batchEndIndex))
                    {
                        while (entityIndex < batchEndIndex)
                        {
                            OriginalLambdaBody(bufAccessor[entityIndex]);
                            entityIndex++;
                        }
                    }
                }
                else
                {
                    ulong mask64 = chunkEnabledMask.ULong0;
                    int count = Unity.Mathematics.math.min(64, chunkEntityCount);
                    for (var entityIndex = 0; entityIndex < count; ++entityIndex)
                    {
                        if ((mask64 & 1) != 0)
                        {
                            OriginalLambdaBody(bufAccessor[entityIndex]);
                        }

                        mask64 >>= 1;
                    }

                    mask64 = chunkEnabledMask.ULong1;
                    for (var entityIndex = 64; entityIndex < chunkEntityCount; ++entityIndex)
                    {
                        if ((mask64 & 1) != 0)
                        {
                            OriginalLambdaBody(bufAccessor[entityIndex]);
                        }

                        mask64 >>= 1;
                    }
                }
            }
        }

        [Unity.Burst.BurstCompile(FloatMode = Unity.Burst.FloatMode.Default, FloatPrecision = Unity.Burst.FloatPrecision.Standard, CompileSynchronously = false)]
        [AOT.MonoPInvokeCallback(typeof(Unity.Entities.InternalCompilerInterface.JobChunkRunWithoutJobSystemDelegate))]
        public static void RunWithoutJobSystem(ref Unity.Entities.EntityQuery query, global::System.IntPtr jobPtr)
        {
            try
            {
                Unity.Entities.InternalCompilerInterface.JobChunkInterface.RunWithoutJobsInternal(ref Unity.Entities.InternalCompilerInterface.UnsafeAsRef<EntitiesForEachDynamicBuffer_67594103_LambdaJob_0_Job>(jobPtr), ref query);
            }
            finally
            {
            }
        }
    }

    void EntitiesForEachDynamicBuffer_67594103_LambdaJob_0_Execute()
    {
        __BufferData_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
        var __job = new EntitiesForEachDynamicBuffer_67594103_LambdaJob_0_Job{__bufTypeHandle = __BufferData_RW_BufferTypeHandle};
        CompleteDependency();
        var __functionPointer = Unity.Jobs.LowLevel.Unsafe.JobsUtility.JobCompilerEnabled ? EntitiesForEachDynamicBuffer_67594103_LambdaJob_0_Job.FunctionPtrFieldBurst : EntitiesForEachDynamicBuffer_67594103_LambdaJob_0_Job.FunctionPtrFieldNoBurst;
        Unity.Entities.InternalCompilerInterface.UnsafeRunJobChunk(ref __job, __query_172016070_0, __functionPointer);
    }

    Unity.Entities.EntityQuery __query_172016070_0;
    Unity.Entities.BufferTypeHandle<BufferData> __BufferData_RW_BufferTypeHandle;
    protected override void OnCreateForCompiler()
    {
        base.OnCreateForCompiler();
        __query_172016070_0 = this.CheckedStateRef.GetEntityQuery(new Unity.Entities.EntityQueryDesc{All = new Unity.Entities.ComponentType[]{Unity.Entities.ComponentType.ReadWrite<BufferData>()}, Any = new Unity.Entities.ComponentType[]{}, None = new Unity.Entities.ComponentType[]{}, Options = Unity.Entities.EntityQueryOptions.Default});
        __BufferData_RW_BufferTypeHandle = this.CheckedStateRef.GetBufferTypeHandle<BufferData>(false);
        EntitiesForEachDynamicBuffer_67594103_LambdaJob_0_Job.FunctionPtrFieldNoBurst = EntitiesForEachDynamicBuffer_67594103_LambdaJob_0_Job.RunWithoutJobSystem;
        EntitiesForEachDynamicBuffer_67594103_LambdaJob_0_Job.FunctionPtrFieldBurst = Unity.Entities.InternalCompilerInterface.BurstCompile(EntitiesForEachDynamicBuffer_67594103_LambdaJob_0_Job.FunctionPtrFieldNoBurst);
    }
}