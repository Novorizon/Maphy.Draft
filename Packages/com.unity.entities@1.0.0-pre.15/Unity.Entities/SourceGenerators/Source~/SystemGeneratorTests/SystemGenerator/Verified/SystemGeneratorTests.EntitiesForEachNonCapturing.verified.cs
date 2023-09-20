//HintName: Verify.gen__System_977025139.g.cs
#pragma warning disable 0219
#line 1 "Temp\GeneratedCode\SourceGen.VerifyTests"
using Unity.Entities;
using Unity.Mathematics;
using Unity.Burst;

[global::System.Runtime.CompilerServices.CompilerGenerated]
partial class EntitiesForEachNonCapturing : Unity.Entities.SystemBase
{
    [Unity.Entities.DOTSCompilerPatchedMethod("OnUpdate")]
    void __OnUpdate_1817F1CB()
    {
        #line 14 "Verify.gen.cs"
        EntitiesForEachNonCapturing_FEFD826_LambdaJob_0_Execute();
    }

    #line 18 "Temp\GeneratedCode\SourceGen.VerifyTests"
    [Unity.Burst.NoAlias]
    [Unity.Burst.BurstCompile(FloatMode = Unity.Burst.FloatMode.Default, FloatPrecision = Unity.Burst.FloatPrecision.Standard, CompileSynchronously = false)]
    struct EntitiesForEachNonCapturing_FEFD826_LambdaJob_0_Job : Unity.Entities.IJobChunk
    {
        internal static Unity.Entities.InternalCompilerInterface.JobChunkRunWithoutJobSystemDelegate FunctionPtrFieldNoBurst;
        internal static Unity.Entities.InternalCompilerInterface.JobChunkRunWithoutJobSystemDelegate FunctionPtrFieldBurst;
        public Unity.Entities.ComponentTypeHandle<Translation> __translationTypeHandle;
        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        void OriginalLambdaBody([Unity.Burst.NoAlias] ref Translation translation, [Unity.Burst.NoAlias] ref TagComponent1 tag1, [Unity.Burst.NoAlias] in TagComponent2 tag2)
        {
            #line 14 "Verify.gen.cs"
            translation.Value += 5;
        }

        #line 33 "Temp\GeneratedCode\SourceGen.VerifyTests"
        [global::System.Runtime.CompilerServices.CompilerGenerated]
        public void Execute(in ArchetypeChunk chunk, int batchIndex, bool useEnabledMask, in Unity.Burst.Intrinsics.v128 chunkEnabledMask)
        {
            var translationArrayPtr = Unity.Entities.InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<Translation>(chunk, ref __translationTypeHandle);
            int chunkEntityCount = chunk.Count;
            if (!useEnabledMask)
            {
                for (var entityIndex = 0; entityIndex < chunkEntityCount; ++entityIndex)
                {
                    TagComponent1 tag1Local = default;
                    ;
                    TagComponent2 tag2Local = default;
                    OriginalLambdaBody(ref Unity.Entities.InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Translation>(translationArrayPtr, entityIndex), ref tag1Local, in tag2Local);
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
                            TagComponent1 tag1Local = default;
                            ;
                            TagComponent2 tag2Local = default;
                            OriginalLambdaBody(ref Unity.Entities.InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Translation>(translationArrayPtr, entityIndex), ref tag1Local, in tag2Local);
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
                            TagComponent1 tag1Local = default;
                            ;
                            TagComponent2 tag2Local = default;
                            OriginalLambdaBody(ref Unity.Entities.InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Translation>(translationArrayPtr, entityIndex), ref tag1Local, in tag2Local);
                        }

                        mask64 >>= 1;
                    }

                    mask64 = chunkEnabledMask.ULong1;
                    for (var entityIndex = 64; entityIndex < chunkEntityCount; ++entityIndex)
                    {
                        if ((mask64 & 1) != 0)
                        {
                            TagComponent1 tag1Local = default;
                            ;
                            TagComponent2 tag2Local = default;
                            OriginalLambdaBody(ref Unity.Entities.InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Translation>(translationArrayPtr, entityIndex), ref tag1Local, in tag2Local);
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
                Unity.Entities.InternalCompilerInterface.JobChunkInterface.RunWithoutJobsInternal(ref Unity.Entities.InternalCompilerInterface.UnsafeAsRef<EntitiesForEachNonCapturing_FEFD826_LambdaJob_0_Job>(jobPtr), ref query);
            }
            finally
            {
            }
        }
    }

    void EntitiesForEachNonCapturing_FEFD826_LambdaJob_0_Execute()
    {
        __Translation_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        var __job = new EntitiesForEachNonCapturing_FEFD826_LambdaJob_0_Job{__translationTypeHandle = __Translation_RW_ComponentTypeHandle};
        CompleteDependency();
        var __functionPointer = Unity.Jobs.LowLevel.Unsafe.JobsUtility.JobCompilerEnabled ? EntitiesForEachNonCapturing_FEFD826_LambdaJob_0_Job.FunctionPtrFieldBurst : EntitiesForEachNonCapturing_FEFD826_LambdaJob_0_Job.FunctionPtrFieldNoBurst;
        Unity.Entities.InternalCompilerInterface.UnsafeRunJobChunk(ref __job, __query_1046172136_0, __functionPointer);
    }

    Unity.Entities.EntityQuery __query_1046172136_0;
    Unity.Entities.ComponentTypeHandle<Translation> __Translation_RW_ComponentTypeHandle;
    protected override void OnCreateForCompiler()
    {
        base.OnCreateForCompiler();
        __query_1046172136_0 = this.CheckedStateRef.GetEntityQuery(new Unity.Entities.EntityQueryDesc{All = new Unity.Entities.ComponentType[]{Unity.Entities.ComponentType.ReadOnly<TagComponent1>(), Unity.Entities.ComponentType.ReadOnly<TagComponent2>(), Unity.Entities.ComponentType.ReadWrite<Translation>()}, Any = new Unity.Entities.ComponentType[]{}, None = new Unity.Entities.ComponentType[]{}, Options = Unity.Entities.EntityQueryOptions.Default});
        __Translation_RW_ComponentTypeHandle = this.CheckedStateRef.GetComponentTypeHandle<Translation>(false);
        EntitiesForEachNonCapturing_FEFD826_LambdaJob_0_Job.FunctionPtrFieldNoBurst = EntitiesForEachNonCapturing_FEFD826_LambdaJob_0_Job.RunWithoutJobSystem;
        EntitiesForEachNonCapturing_FEFD826_LambdaJob_0_Job.FunctionPtrFieldBurst = Unity.Entities.InternalCompilerInterface.BurstCompile(EntitiesForEachNonCapturing_FEFD826_LambdaJob_0_Job.FunctionPtrFieldNoBurst);
    }
}