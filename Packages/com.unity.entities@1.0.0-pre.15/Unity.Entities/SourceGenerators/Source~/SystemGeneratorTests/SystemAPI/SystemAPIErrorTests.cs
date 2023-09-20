using Unity.Entities.SourceGen.SystemCodegenContext;
using Xunit;

namespace Unity.Entities.SourceGen.Tests.SystemAPI;

public class SystemAPIErrorTests
{
    [Fact]
    void SGSA0001()
    {
        SourceGenTests.CheckForError<SystemGenerator.SystemGenerator>(@"
            using Unity.Entities;
            using Unity.Entities.Tests;
            using static Unity.Entities.SystemAPI;

            partial struct System : ISystem {
                public void OnUpdate(ref SystemState state) {
                    Idk<EcsTestData>();
                }

                public void Idk<T>() where T:struct,IComponentData{
                    var hadComp = HasComponent<T>(default);
                }
            }
", nameof(SystemAPIErrors.SGSA0001));
    }

    [Fact]
    void SGSA0002()
    {
        SourceGenTests.CheckForError<SystemGenerator.SystemGenerator>(@"
            using Unity.Entities;
            using Unity.Entities.Tests;
            using static Unity.Entities.SystemAPI;

            partial struct System : ISystem {
                public void OnUpdate(ref SystemState state) {
                    var ro = false;
                    var lookup = GetComponentLookup<EcsTestData>(ro);
                }
            }
", nameof(SystemAPIErrors.SGSA0002));
    }
}
