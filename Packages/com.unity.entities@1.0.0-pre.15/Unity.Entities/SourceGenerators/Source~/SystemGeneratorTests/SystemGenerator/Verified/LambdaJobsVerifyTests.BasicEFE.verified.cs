//HintName: Verify.gen__System_977025139.g.cs
#pragma warning disable 0219
#line 1 "Temp\GeneratedCode\SourceGen.VerifyTests"
using Unity.Burst;
using Unity.Entities;
using Unity.Entities.Tests;

[global::System.Runtime.CompilerServices.CompilerGenerated]
public partial class BasicEFESystem : Unity.Entities.SystemBase
{
    [Unity.Entities.DOTSCompilerPatchedMethod("OnUpdate")]
    void __OnUpdate_1817F1CB()
    {
        #line 15 "Verify.gen.cs"
        BasicEFESystem_53F24DB4_LambdaJob_0_Execute();
    }

    #line 18 "Temp\GeneratedCode\SourceGen.VerifyTests"
    [Unity.Burst.NoAlias]
    [Unity.Burst.BurstCompile(FloatMode = Unity.Burst.FloatMode.Default, FloatPrecision = Unity.Burst.FloatPrecision.Standard, CompileSynchronously = false)]
    struct BasicEFESystem_53F24DB4_LambdaJob_0_Job : Unity.Entities.IJobChunk
    {
        public Unity.Entities.ComponentTypeHandle<TestData> __testDataTypeHandle;
        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        void OriginalLambdaBody([Unity.Burst.NoAlias] ref TestData testData)
        {
            #line 17 "Verify.gen.cs"
            testData.value++;
        }

        #line 31 "Temp\GeneratedCode\SourceGen.VerifyTests"
        [global::System.Runtime.CompilerServices.CompilerGenerated]
        public void Execute(in ArchetypeChunk chunk, int batchIndex, bool useEnabledMask, in Unity.Burst.Intrinsics.v128 chunkEnabledMask)
        {
            var testDataArrayPtr = Unity.Entities.InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<TestData>(chunk, ref __testDataTypeHandle);
            int chunkEntityCount = chunk.Count;
            if (!useEnabledMask)
            {
                for (var entityIndex = 0; entityIndex < chunkEntityCount; ++entityIndex)
                {
                    OriginalLambdaBody(ref Unity.Entities.InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TestData>(testDataArrayPtr, entityIndex));
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
                            OriginalLambdaBody(ref Unity.Entities.InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TestData>(testDataArrayPtr, entityIndex));
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
                            OriginalLambdaBody(ref Unity.Entities.InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TestData>(testDataArrayPtr, entityIndex));
                        }

                        mask64 >>= 1;
                    }

                    mask64 = chunkEnabledMask.ULong1;
                    for (var entityIndex = 64; entityIndex < chunkEntityCount; ++entityIndex)
                    {
                        if ((mask64 & 1) != 0)
                        {
                            OriginalLambdaBody(ref Unity.Entities.InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<TestData>(testDataArrayPtr, entityIndex));
                        }

                        mask64 >>= 1;
                    }
                }
            }
        }
    }

    void BasicEFESystem_53F24DB4_LambdaJob_0_Execute()
    {
        __TestData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        var __job = new BasicEFESystem_53F24DB4_LambdaJob_0_Job{__testDataTypeHandle = __TestData_RW_ComponentTypeHandle};
        Dependency = Unity.Entities.InternalCompilerInterface.JobChunkInterface.ScheduleParallel(__job, __query_965306704_0, Dependency);
    }

    Unity.Entities.EntityQuery __query_965306704_0;
    Unity.Entities.ComponentTypeHandle<TestData> __TestData_RW_ComponentTypeHandle;
    protected override void OnCreateForCompiler()
    {
        base.OnCreateForCompiler();
        __query_965306704_0 = this.CheckedStateRef.GetEntityQuery(new Unity.Entities.EntityQueryDesc{All = new Unity.Entities.ComponentType[]{Unity.Entities.ComponentType.ReadWrite<TestData>()}, Any = new Unity.Entities.ComponentType[]{}, None = new Unity.Entities.ComponentType[]{}, Options = Unity.Entities.EntityQueryOptions.Default});
        __TestData_RW_ComponentTypeHandle = this.CheckedStateRef.GetComponentTypeHandle<TestData>(false);
    }
}