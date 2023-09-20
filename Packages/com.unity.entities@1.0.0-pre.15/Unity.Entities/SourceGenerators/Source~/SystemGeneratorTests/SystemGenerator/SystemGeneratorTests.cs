using System.Threading.Tasks;
using Unity.Entities.SourceGen.SystemGenerator;
using VerifyXunit;
using Xunit;

[UsesVerify]
public class SystemGeneratorTests
{
    [Fact]
    public Task EntitiesForEachNonCapturing()
    {
        var testSource = @"
using Unity.Entities;
using Unity.Mathematics;
using Unity.Burst;

struct Translation : IComponentData { public float Value; }
struct TagComponent1 : IComponentData {}
struct TagComponent2 : IComponentData {}

partial class EntitiesForEachNonCapturing : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref Translation translation, ref TagComponent1 tag1, in TagComponent2 tag2) => { translation.Value += 5; }).Run();
    }
}";

        return SourceGenTests.Verify<SystemGenerator>(testSource);
    }

    [Fact]
    public Task EntitiesForEachCachesBufferTypeHandle()
    {
        var testSource = @"
using Unity.Entities;

struct BufferData : IBufferElementData { public float Value; }

partial class EntitiesForEachDynamicBuffer : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((DynamicBuffer<BufferData> buf) => { }).Run();
    }
}";

        return SourceGenTests.Verify<SystemGenerator>(testSource);
    }
}
