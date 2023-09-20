//HintName: Verify.gen__System_977025139.g.cs
#pragma warning disable 0219
#line 1 "Temp\GeneratedCode\SourceGen.VerifyTests"
using Unity.Burst;
using Unity.Entities;
using Unity.Entities.Tests;
using static Unity.Entities.SystemAPI;

[global::System.Runtime.CompilerServices.CompilerGenerated]
public partial struct SomeSystem : Unity.Entities.ISystem, Unity.Entities.ISystemCompilerGenerated
{
    [Unity.Entities.DOTSCompilerPatchedMethod("OnUpdate_ref_Unity.Entities.SystemState")]
    void __OnUpdate_6E994214(ref SystemState state)
    {
        #line 10 "Verify.gen.cs"
        var e = state.EntityManager.CreateEntity();
        #line 11 "Verify.gen.cs"
        state.EntityManager.AddComponentData(e, new EcsTestManagedComponent{value = "cake"});
        #line 12 "Verify.gen.cs"
        switch (access)
        {
            case SystemAPIAccess.SystemAPI:
                #line 15 "Verify.gen.cs"
                Assert.AreEqual(__query_1591414865_0.GetSingleton<EcsTestManagedComponent>().value, "cake");
                #line 16 "Verify.gen.cs"
                break;
            case SystemAPIAccess.Using:
                #line 18 "Verify.gen.cs"
                Assert.AreEqual(__query_1591414865_0.GetSingleton<EcsTestManagedComponent>().value, "cake");
                #line 19 "Verify.gen.cs"
                break;
        }
    }

    Unity.Entities.EntityQuery __query_1591414865_0;
    public void OnCreateForCompiler(ref SystemState state)
    {
        __query_1591414865_0 = state.GetEntityQuery(new Unity.Entities.EntityQueryDesc{All = new Unity.Entities.ComponentType[]{Unity.Entities.ComponentType.ReadOnly<Unity.Entities.Tests.EcsTestManagedComponent>()}, Any = new Unity.Entities.ComponentType[]{}, None = new Unity.Entities.ComponentType[]{}, Options = Unity.Entities.EntityQueryOptions.Default | Unity.Entities.EntityQueryOptions.IncludeSystems});
    }
}