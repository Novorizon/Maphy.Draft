using Unity.Entities.SourceGen.SystemGenerator;
using Xunit;

public class SystemGeneratorNoErrorTests
{
    [Fact]
    public void MultipleUserWrittenSystemPartsWithGeneratedQueriesSystem()
    {
        SourceGenTests.CheckForNoError<SystemGenerator>(
            @"
            partial class MultipleUserWrittenSystemPartsWithGeneratedQueriesSystem : SystemBase
            {
                // Query 1
                public EntityQuery UseSystemAPIQueryBuilder() =>
                    SystemAPI.QueryBuilder().WithAll<EcsTestData>().WithNone<EcsTestData2>().Build();

                protected override void OnUpdate()
                {
                }
            }

            partial class MultipleUserWrittenSystemPartsWithGeneratedQueriesSystem : SystemBase
            {
                public void UseBulkOperations()
                {
                    Entities.WithAll<EcsTestData3>().DestroyEntity(); // Query 2
                    Entities.WithNone<EcsTestData2>().WithAll<EcsTestData>().DestroyEntity(); // Same as Query 1
                }
            }

            partial class MultipleUserWrittenSystemPartsWithGeneratedQueriesSystem : SystemBase
            {
                public void UseIdiomaticCSharpForEachs()
                {
                    // Query 3
                    foreach (var _ in SystemAPI.Query<EcsTestData4>())
                    {
                    }
                    // Same as Query 2
                    foreach (var _ in SystemAPI.Query<EcsTestData3>())
                    {
                    }
                }
            }");
    }

    [Fact]
    public void ExpressionBody()
    {
        SourceGenTests.CheckForNoError<SystemGenerator>(@"
            public partial struct SomeSystem : ISystem {
                public void OnUpdate(ref SystemState state) => SystemAPI.GetSingletonRW<EcsTestData>();
            }");
    }

    [Fact]
    public void ExpressionBodyWithReturn()
    {
        SourceGenTests.CheckForNoError<SystemGenerator>(@"
            public partial struct SomeSystem : ISystem {
                RefRW<EcsTestData> GetData(ref SystemState state) => SystemAPI.GetSingletonRW<EcsTestData>();
                public void OnUpdate(ref SystemState state){}
            }");
    }
}
