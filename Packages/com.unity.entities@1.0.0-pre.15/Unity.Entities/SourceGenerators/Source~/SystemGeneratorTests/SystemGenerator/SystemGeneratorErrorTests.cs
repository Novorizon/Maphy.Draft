using Unity.Entities.SourceGen.SystemGenerator;
using Xunit;

public class SystemGeneratorErrorTests
{
    [Fact]
    public void DC0058_NonPartialClassBase()
    {
        SourceGenTests.CheckForError<SystemGenerator>(
            @"using Unity.Entities;
            using Unity.Entities.Tests;
            using Unity.Collections;

            class NonPartialClassBase : SystemBase
            {
                protected override void OnUpdate() { }
            }",
            nameof(SystemGeneratorErrors.DC0058));
    }

    [Fact]
    public void DC0065_ClassBasedISystem()
    {
        SourceGenTests.CheckForError<SystemGenerator>(
            @"using Unity.Entities;
            using Unity.Entities.Tests;
            using Unity.Collections;

            partial class ClassBasedISystem : ISystem
            {
                public void OnCreate(ref SystemState state) { }

                public void OnUpdate(ref SystemState state) { }

                public void OnDestroy(ref SystemState state) { }
            }",
            nameof(SystemGeneratorErrors.DC0065));
    }

    [Fact]
    public void PropertyWithNeedForSystemState()
    {
        SourceGenTests.CheckForError<SystemGenerator>(
            @"using Unity.Entities;
            using Unity.Entities.Tests;

            partial struct SomeSystem : ISystem
            {
                public void SomeProperty => SystemAPI.GetComponent<EcsTestData>(new Entity());

                public void OnCreate(ref SystemState state) { }

                public void OnUpdate(ref SystemState state) { }

                public void OnDestroy(ref SystemState state) { }
            }",
            nameof(SystemGeneratorErrors.SGSG0001));
    }
}
