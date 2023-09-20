//HintName: Verify.gen__System_977025139.g.cs
#pragma warning disable 0219
#line 1 "Temp\GeneratedCode\SourceGen.VerifyTests"
using Unity.Burst;
using Unity.Entities;
using Unity.Entities.Tests;
using static Unity.Entities.SystemAPI;

[global::System.Runtime.CompilerServices.CompilerGenerated]
public partial struct RotationSpeedSystemForEachISystem : Unity.Entities.ISystem, Unity.Entities.ISystemCompilerGenerated
{
    [Unity.Entities.DOTSCompilerPatchedMethod("OnUpdate_ref_Unity.Entities.SystemState")]
    void __OnUpdate_6E994214(ref SystemState state)
    {
        #line 16 "Verify.gen.cs"
        Entity entity = default;
            #line 17 "Verify.gen.cs"
            Unity.Entities.Tests.EcsTestAspect.CompleteDependencyBeforeRO(ref state);
            #line hidden
            __Unity_Entities_Tests_EcsTestAspect_RO_AspectLookup.Update(ref state);
            #line hidden
            var testAspectRO = __Unity_Entities_Tests_EcsTestAspect_RO_AspectLookup[entity];
            #line 18 "Verify.gen.cs"
            Unity.Entities.Tests.EcsTestAspect.CompleteDependencyBeforeRW(ref state);
            #line hidden
            __Unity_Entities_Tests_EcsTestAspect_RW_AspectLookup.Update(ref state);
            #line hidden
            var testAspectRW = __Unity_Entities_Tests_EcsTestAspect_RW_AspectLookup[entity];
    }

    Unity.Entities.Tests.EcsTestAspect.Lookup __Unity_Entities_Tests_EcsTestAspect_RO_AspectLookup;
    Unity.Entities.Tests.EcsTestAspect.Lookup __Unity_Entities_Tests_EcsTestAspect_RW_AspectLookup;
    public void OnCreateForCompiler(ref SystemState state)
    {
        __Unity_Entities_Tests_EcsTestAspect_RO_AspectLookup = new Unity.Entities.Tests.EcsTestAspect.Lookup(ref state, true);
        __Unity_Entities_Tests_EcsTestAspect_RW_AspectLookup = new Unity.Entities.Tests.EcsTestAspect.Lookup(ref state, false);
    }
}