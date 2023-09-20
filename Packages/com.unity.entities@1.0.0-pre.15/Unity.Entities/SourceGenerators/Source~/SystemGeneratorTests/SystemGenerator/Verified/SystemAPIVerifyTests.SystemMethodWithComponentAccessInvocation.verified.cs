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
                state.EntityManager.CompleteDependencyBeforeRO<Rotation>();
                #line hidden
                __Rotation_RO_ComponentLookup.Update(ref state);
                #line hidden
                var rotation = __Rotation_RO_ComponentLookup[entity];
        }
    }

    Unity.Entities.ComponentLookup<Rotation> __Rotation_RO_ComponentLookup;
    public void OnCreateForCompiler(ref SystemState state)
    {
        __Rotation_RO_ComponentLookup = state.GetComponentLookup<Rotation>(true);
    }
}