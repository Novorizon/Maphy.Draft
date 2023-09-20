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
            __EntityStorageInfoLookup.Update(ref state);
            #line hidden
            var storageInfo = __EntityStorageInfoLookup;
        #line 21 "Verify.gen.cs"
        foreach (var entity in Query<Entity>().WithAll<Rotation>())
        {
                #line 23 "Verify.gen.cs"
                __EntityStorageInfoLookup.Update(ref state);
                #line hidden
                var check1 = __EntityStorageInfoLookup.Exists(entity);
            #line 24 "Verify.gen.cs"
            var check2 = storageInfo.Exists(entity);
        }
    }

    Unity.Entities.EntityStorageInfoLookup __EntityStorageInfoLookup;
    public void OnCreateForCompiler(ref SystemState state)
    {
        __EntityStorageInfoLookup = state.GetEntityStorageInfoLookup();
    }
}