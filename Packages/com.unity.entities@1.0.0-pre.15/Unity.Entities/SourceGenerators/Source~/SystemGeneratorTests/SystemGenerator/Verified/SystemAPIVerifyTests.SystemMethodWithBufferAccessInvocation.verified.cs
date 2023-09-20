//HintName: Verify.gen__System_977025139.g.cs
#pragma warning disable 0219
#line 1 "Temp\GeneratedCode\SourceGen.VerifyTests"
using Unity.Burst;
using Unity.Entities;
using static Unity.Entities.SystemAPI;

[global::System.Runtime.CompilerServices.CompilerGenerated]
public partial struct RotationSpeedSystemForEachISystem : Unity.Entities.ISystem, Unity.Entities.ISystemCompilerGenerated
{
    [Unity.Entities.DOTSCompilerPatchedMethod("OnUpdate_ref_Unity.Entities.SystemState")]
    void __OnUpdate_6E994214(ref SystemState state)
    {
        #line 20 "Verify.gen.cs"
        foreach (var entity in Query<Entity>().WithAll<Rotation>())
        {
                #line 22 "Verify.gen.cs"
                __BufferData_RW_BufferLookup.Update(ref state);
                #line hidden
                var lookup_rw = __BufferData_RW_BufferLookup;
                #line 23 "Verify.gen.cs"
                __BufferData_RO_BufferLookup.Update(ref state);
                #line hidden
                var lookup_ro = __BufferData_RO_BufferLookup;
                #line 27 "Verify.gen.cs"
                state.EntityManager.CompleteDependencyBeforeRO<BufferData>();
                #line hidden
                __BufferData_RO_BufferLookup.Update(ref state);
                #line hidden
                if (__BufferData_RO_BufferLookup.HasBuffer(entity)
                {
                        #line 27 "Verify.gen.cs"
                        state.EntityManager.CompleteDependencyBeforeRW<BufferData>();
                        #line hidden
                        __BufferData_RW_BufferLookup.Update(ref state);
                        #line hidden
                        var rotation = __BufferData_RW_BufferLookup[entity];
                }
        }
    }

    Unity.Entities.BufferLookup<BufferData> __BufferData_RW_BufferLookup;
    Unity.Entities.BufferLookup<BufferData> __BufferData_RO_BufferLookup;
    public void OnCreateForCompiler(ref SystemState state)
    {
        __BufferData_RW_BufferLookup = state.GetBufferLookup<BufferData>(false);
        __BufferData_RO_BufferLookup = state.GetBufferLookup<BufferData>(true);
    }
}