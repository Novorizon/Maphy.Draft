using System.Threading.Tasks;
using Unity.Entities.SourceGen.SystemGenerator;
using VerifyXunit;
using Xunit;

[UsesVerify]
public class SystemAPIVerifyTests
{
    [Fact]
    public Task SystemMethodWithComponentAccessInvocation()
    {
        var testSource = @"
using Unity.Burst;
using Unity.Entities;
using static Unity.Entities.SystemAPI;

public struct RotationSpeed : IComponentData
{
    public float RadiansPerSecond;
}

[BurstCompile]
public partial struct RotationSpeedSystemForEachISystem : ISystem
{
    public void OnCreate(ref SystemState state) {}
    public void OnDestroy(ref SystemState state) {}

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var entity in Query<Entity>().WithAll<Rotation>())
        {
            var rotation = GetComponent<Rotation>(entity);
        }
    }
}
";

        return SourceGenTests.Verify<SystemGenerator>(testSource);
    }

    [Fact]
    public Task SystemMethodWithBufferAccessInvocation()
    {
        var testSource = @"
using Unity.Burst;
using Unity.Entities;
using static Unity.Entities.SystemAPI;

public struct BufferData : IBufferElement
{
    public float RadiansPerSecond;
}

[BurstCompile]
public partial struct RotationSpeedSystemForEachISystem : ISystem
{
    public void OnCreate(ref SystemState state) {}
    public void OnDestroy(ref SystemState state) {}

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var entity in Query<Entity>().WithAll<Rotation>())
        {
            var lookup_rw = GetBufferLookup<BufferData>(false);
            var lookup_ro = GetBufferLookup<BufferData>(true);

            if (HasBuffer<BufferData>(entity)
            {
                var rotation = GetBuffer<BufferData>(entity);
            }
        }
    }
}
";

        return SourceGenTests.Verify<SystemGenerator>(testSource);
    }

    [Fact]
    public Task SystemMethodWithStorageInfoInvocation()
    {
        var testSource = @"
using Unity.Burst;
using Unity.Entities;
using static Unity.Entities.SystemAPI;

public struct BufferData : IBufferElement
{
    public float RadiansPerSecond;
}

[BurstCompile]
public partial struct RotationSpeedSystemForEachISystem : ISystem
{
    public void OnCreate(ref SystemState state) {}
    public void OnDestroy(ref SystemState state) {}

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var storageInfo = GetEntityStorageInfoLookup();
        foreach (var entity in Query<Entity>().WithAll<Rotation>())
        {
            var check1 = Exists(entity);
            var check2 = storageInfo.Exists(entity);
        }
    }
}
";

        return SourceGenTests.Verify<SystemGenerator>(testSource);
    }

    [Fact]
    public Task SystemMethodWithAspectInvocation()
    {
        var testSource = @"
using Unity.Burst;
using Unity.Entities;
using Unity.Entities.Tests;
using static Unity.Entities.SystemAPI;

[BurstCompile]
public partial struct RotationSpeedSystemForEachISystem : ISystem
{
    public void OnCreate(ref SystemState state) {}
    public void OnDestroy(ref SystemState state) {}

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        Entity entity = default;
        var testAspectRO = GetAspectRO<EcsTestAspect>(entity);
        var testAspectRW = GetAspectRW<EcsTestAspect>(entity);
    }
}
";

        return SourceGenTests.Verify<SystemGenerator>(testSource);
    }

    [Fact]
    public Task SystemMethodWithManagedComponent()
    {
        var testSource = @"
    using Unity.Burst;
    using Unity.Entities;
    using Unity.Entities.Tests;
    using static Unity.Entities.SystemAPI;
    public partial struct SomeSystem : ISystem {
        public void OnCreate(ref SystemState state){}
        public void OnDestroy(ref SystemState state){}
        public void OnUpdate(ref SystemState state){
            var e = state.EntityManager.CreateEntity();
            state.EntityManager.AddComponentData(e, new EcsTestManagedComponent{value = ""cake""});
            switch (access)
            {
                case SystemAPIAccess.SystemAPI:
                    Assert.AreEqual(SystemAPI.ManagedAPI.GetSingleton<EcsTestManagedComponent>().value, ""cake"");
                    break;
                case SystemAPIAccess.Using:
                    Assert.AreEqual(ManagedAPI.GetSingleton<EcsTestManagedComponent>().value, ""cake"");
                    break;
            }
        }
    }
";

        return SourceGenTests.Verify<SystemGenerator>(testSource);
    }
}
