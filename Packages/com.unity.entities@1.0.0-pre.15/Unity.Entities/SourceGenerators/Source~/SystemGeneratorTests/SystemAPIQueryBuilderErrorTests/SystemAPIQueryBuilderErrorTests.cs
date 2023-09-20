using Unity.Entities.SourceGen.SystemGenerator;
using Xunit;

public class SystemAPIQueryBuilderErrorTests
{
    [Fact]
    public void SGQB001_MultipleWithOptionsInvocations()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
            using Unity.Entities;
            using Unity.Entities.Tests;
            partial class SomeSystem : SystemBase
            {
                protected override void OnUpdate()
                {
                    var query = SystemAPI.QueryBuilder().WithAll<EcsTestData>().WithOptions(EntityQueryOptions.IncludePrefab).WithOptions(EntityQueryOptions.IncludeDisabledEntities).Build();
                    EntityManager.AddComponent<EcsTestTag>(query);
                }
            }", "SGQB001");
    }
}
