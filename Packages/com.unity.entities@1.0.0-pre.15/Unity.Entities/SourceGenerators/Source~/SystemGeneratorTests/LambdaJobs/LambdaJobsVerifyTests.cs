using System.Threading.Tasks;
using Unity.Entities.SourceGen.SystemGenerator;
using VerifyXunit;
using Xunit;

[UsesVerify]
public class LambdaJobsVerifyTests
{
    [Fact]
    public Task BasicEFE()
    {
        var testSource = @"
using Unity.Burst;
using Unity.Entities;
using Unity.Entities.Tests;

public struct TestData : IComponentData;
{
    public int value;
}

public partial class BasicEFESystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref TestData testData) =>
            {
                testData.value++;
            })
            .ScheduleParallel();
    }
}
";

        return SourceGenTests.Verify<SystemGenerator>(testSource);
    }
}
