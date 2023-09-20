using Unity.Entities.SourceGen.SystemGenerator;
using Xunit;

public class ForEachErrorTests
{
    /*
     We no longer look for SystemAPI calls in non-system methods (to save sourcegen time)
     This will throw a runtime error instead
    [Fact]
    public void SGFE001_ForEachIterationThroughAspectQuery_InSystemBaseType()
    {
        SourceGenTests.CheckForError<SystemGenerator>(
            @"
            using Unity.Entities;
            partial class TranslationSystem
            {
                protected void OnUpdate()
                {
                    foreach (var character in SystemAPI.Query<RefRW<Translation>>())
                    {
                    }
                }
            }", "SGFE001");
    }
    */

    [Fact]
    public void SGFE002_ForEachIterationThroughAspectQuery_InPropertyAccessor()
    {
        SourceGenTests.CheckForError<SystemGenerator>(
            @"partial struct TranslationSystem : ISystem
            {
                public int Translation
                {
                    get
                    {
                        foreach (var translation in SystemAPI.Query<RefRW<Translation>>()){}
                        return 3;
                    }
                }

                public void OnCreate(ref SystemState state) { }

                public void OnDestroy(ref SystemState state) { }

                public void OnUpdate(ref SystemState state) { }
            }", "SGFE002");
    }

    [Fact]
    public void SGQC001_QueryingUnsupportedType()
    {
        SourceGenTests.CheckForError<SystemGenerator>( @"
        struct NotAComponent { }

        partial struct TranslationSystem : ISystem
        {
            public void OnCreate(ref SystemState state) { }

            public void OnDestroy(ref SystemState state)
            {
                foreach (var aspect in SystemAPI.Query<RefRW<Translation>>().WithNone<NotAComponent>())
                {
                }
            }

            public void OnUpdate(ref SystemState state) { }
        }", "SGQC001");
    }

    [Fact]
    public void SGQC004_SameTypeSpecifiedInWithNone_AndWithAll()
    {
        SourceGenTests.CheckForError<SystemGenerator>( @"
        partial struct TranslationSystem : ISystem
        {
            public void OnCreate(ref SystemState state) { }

            public void OnDestroy(ref SystemState state)
            {
                foreach (var aspect in SystemAPI.Query<RefRW<Translation>>().WithNone<Translation>().WithAll<Translation>())
                {
                }
            }

            public void OnUpdate(ref SystemState state) { }
        }", "SGQC004");
    }

    [Fact]
    public void SGQC004_SameTypeSpecifiedInWithNone_AndWithAny()
    {
        SourceGenTests.CheckForError<SystemGenerator>( @"
        partial struct TranslationSystem : ISystem
        {
            public void OnCreate(ref SystemState state) { }

            public void OnDestroy(ref SystemState state)
            {
                foreach (var aspect in SystemAPI.Query<RefRW<Translation>>().WithNone<Translation>().WithAny<Translation>())
                {
                }
            }

            public void OnUpdate(ref SystemState state) { }
        }", "SGQC004");
    }

    [Fact]
    public void SGQC004_SameTypeSpecifiedInWithAll_AndWithAny()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial struct TranslationSystem : ISystem
        {
            public void OnCreate(ref SystemState state) { }

            public void OnDestroy(ref SystemState state)
            {
                foreach (var aspect in SystemAPI.Query<RefRW<Translation>>().WithAll<Translation>().WithAny<Translation>())
                {
                }
            }

            public void OnUpdate(ref SystemState state) { }
        }", "SGQC004");
    }

    [Fact]
    public void SGQC004_QueriedAspectTypeSpecifiedInWithNone()
    {
        SourceGenTests.CheckForError<SystemGenerator>( @"
        partial struct TranslationSystem : ISystem
        {
            public void OnCreate(ref SystemState state) { }

            public void OnDestroy(ref SystemState state)
            {
                foreach (var aspect in SystemAPI.Query<RefRW<Translation>>().WithNone<Translation>())
                {
                }
            }

            public void OnUpdate(ref SystemState state) { }
        }", "SGQC004");
    }

    [Fact]
    public void SGQC004_QueriedAspectTypeSpecifiedInWithAny()
    {
        SourceGenTests.CheckForError<SystemGenerator>( @"
        partial struct TranslationSystem : ISystem
        {
            public void OnCreate(ref SystemState state) { }

            public void OnDestroy(ref SystemState state)
            {
                foreach (var translation in SystemAPI.Query<RefRW<Translation>>().WithAny<Translation>())
                {
                }
            }

            public void OnUpdate(ref SystemState state) { }
        }", "SGQC004");
    }

    [Fact]
    public void SGSG0002_MethodWithoutSystemStateParameter()
    {
        SourceGenTests.CheckForError<SystemGenerator>(
            @"using Unity.Entities;
            using Unity.Entities.Tests;
            partial struct CharacterMovementSystem : ISystem
            {
                public void OnCreate(ref SystemState state) { }
                public void OnUpdate(ref SystemState state)
                {
                }
                public void MethodWithoutSystemStateParameter()
                {
                    foreach (var translation in SystemAPI.Query<RefRW<Translation>>())
                    {
                    }
                }
                public void OnDestroy(ref SystemState state) { }
            }",
            nameof(SystemGeneratorErrors.SGSG0002));
    }

    [Fact]
    public void SGFE003_TooManyChangeFilters()
    {
        SourceGenTests.CheckForError<SystemGenerator>(
            @"using Unity.Entities;
            using Unity.Entities.Tests;
            partial struct CharacterMovementSystem : ISystem
            {
                public void OnCreate(ref SystemState state) { }
                public void OnUpdate(ref SystemState state)
                {
                    foreach (var aspect in SystemAPI.Query<RefRO<EcsTestData>>()
                    .WithChangeFilter<EcsTestData2, EcsTestData3>()
                    .WithChangeFilter<EcsTestData4>()) {}
                }
                public void OnDestroy(ref SystemState state) { }
            }",
            "SGFE003");
    }

    [Fact]
    public void SGFE007_TooManySharedComponentChangeFilters()
    {
        SourceGenTests.CheckForError<SystemGenerator>(
            @"using Unity.Entities;
            using Unity.Entities.Tests;
            partial struct CharacterMovementSystem : ISystem
            {
                public void OnCreate(ref SystemState state) { }
                public void OnUpdate(ref SystemState state)
                {
                    foreach (var component in SystemAPI.Query<RefRO<EcsTestData>>()
                        .WithSharedComponentFilter<EcsTestData2, EcsTestData3>()
                        .WithSharedComponentFilter<EcsTestData4>()) {}
                }
                public void OnDestroy(ref SystemState state) { }
            }",
            "SGFE007");
    }

    [Fact]
    public void SGFE008_TooManyEntityQueryOptionsArguments()
    {
        SourceGenTests.CheckForError<SystemGenerator>(
            @"using Unity.Entities;
            using Unity.Entities.Tests;

            partial struct CharacterMovementSystem : ISystem
            {
                public void OnCreate(ref SystemState state) { }

                public void OnUpdate(ref SystemState state)
                {
                    foreach (var component in SystemAPI.Query<RefRO<EcsTestData>>().WithOptions(EntityQueryOptions.Default).WithOptions(EntityQueryOptions.FilterWriteGroup))
                    {}
                }

                public void OnDestroy(ref SystemState state) { }
            }",
            "SGFE008");
    }

    [Fact]
    public void SGFE001_SystemAPIQueryInvoked_WithoutForEach()
    {
        SourceGenTests.CheckForError<SystemGenerator>(
            @"using Unity.Entities;
            using Unity.Entities.Tests;

            partial class CharacterMovementSystem : SystemBase
            {
                protected override void OnUpdate() { }

                public QueryEnumerable<EcsTestData> GetEntitiesWithEcsTestData() => SystemAPI.Query<EcsTestData>();
            }",
            "SGFE001");
    }

    [Fact]
    public void SGFI009_ValueTypeComponentDataInForEachIteration()
    {
        SourceGenTests.CheckForInfo<SystemGenerator>(
            @"using Unity.Entities;
            using Unity.Entities.Tests;

            partial struct CharacterMovementSystem : ISystem
            {
                public void OnCreate(ref SystemState state) { }

                public void OnUpdate(ref SystemState state)
                {
                    foreach (var component in SystemAPI.Query<EcsTestData>())
                    { }
                }

                public void OnDestroy(ref SystemState state) { }
            }",
            "SGFI009");
    }
}
