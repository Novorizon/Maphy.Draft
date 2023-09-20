using Unity.Entities.SourceGen.Aspect;
using Xunit;

public class AspectErrorTests
{
    [Fact]
    public void SGA0001_MultipleFieldsOfSameFieldType()
    {
        SourceGenTests.CheckForError<AspectGenerator>(@"
            using Unity.Entities;
            public readonly partial struct MyAspect : IAspect
            {
                public readonly RefRW<Unity.Entities.Tests.EcsTestData> Data;
                public readonly RefRW<Unity.Entities.Tests.EcsTestData> Data1;
            }",
            "SGA0001");
    }

    [Fact]
    public void SGA0001_MultipleRefFieldsToSameIComponentDataType()
    {
        SourceGenTests.CheckForError<AspectGenerator>(@"
            using Unity.Entities;
            public readonly partial struct MyAspect : IAspect
            {
                public readonly RefRO<Unity.Entities.Tests.EcsTestData> RO_Data;
                public readonly RefRW<Unity.Entities.Tests.EcsTestData> RW_Data;
            }",
            "SGA0001");
    }

    [Fact]
    public void SGA0002_ImplementedIAspectCreateOfDifferentType()
    {
        SourceGenTests.CheckForError<AspectGenerator>(@"
            using Unity.Entities;
            public readonly partial struct MyStruct
            {
            }
            public readonly partial struct MyAspect1 : IAspect, IAspectCreate<MyStruct>
            {
                public readonly RefRW<Unity.Entities.Tests.EcsTestData2> Data;
            }",
            "SGA0002");
    }
    [Fact]
    public void SGA0003_AspectMustBePartial()
    {
        SourceGenTests.CheckForError<AspectGenerator>(@"
            using Unity.Entities;
            public readonly struct MyAspect0 : IAspect
            {
                public readonly RefRW<Unity.Entities.Tests.EcsTestData> Data;
            }",
            "SGA0003");
    }

    [Fact]
    public void SGA0004_Empty()
    {
        SourceGenTests.CheckForError<AspectGenerator>(@"
            using Unity.Entities;
            public readonly partial struct MyAspect : IAspect
            {
            }",
            "SGA0004");
    }

    [Fact]
    public void SGA0005_NotReadonly()
    {
        SourceGenTests.CheckForError<AspectGenerator>(@"
            using Unity.Entities;
            public partial struct MyAspect : IAspect
            {
            }",
            "SGA0005");
    }

    [Fact]
    public void SGA0006_EntityFieldDuplicate()
    {
        SourceGenTests.CheckForError<AspectGenerator>(@"
            using Unity.Entities;
            public partial struct MyAspect : IAspect
            {
                Entity e0;
                Entity e1;
            }",
            "SGA0006");
    }

    [Fact]
    public void SGA0007_DataField()
    {
        SourceGenTests.CheckForError<AspectGenerator>(@"
            using Unity.Entities;
            public partial struct MyAspect : IAspect
            {
                public int i;
            }",
            "SGA0007");
    }

    [Fact]
    public void SGA0009_GenericAspect_Not_Supported()
    {
        SourceGenTests.CheckForError<AspectGenerator>(@"
            using Unity.Entities;

            public partial struct GenericAspect<T> : IAspect
            {
                public readonly RefRW<Unity.Entities.Tests.EcsTestData> Data;
            }",
            "SGA0009");
    }

    [Fact]
    public void SGA0010_NestingAspectsInClass_Not_Supported()
    {
        SourceGenTests.CheckForError<AspectGenerator>(@"
            using Unity.Entities;

            public class Parent
            {
                public readonly partial struct TestAspect : IAspect
                {
                    readonly internal RefRW<EcsTestData> Data;
                }
            }",
            "SGA0010");
    }

    [Fact]
    public void SGA0010_NestingAspectsInStruct_Not_Supported()
    {
        SourceGenTests.CheckForError<AspectGenerator>(@"
            using Unity.Entities;

            public struct Parent
            {
                public readonly partial struct TestAspect : IAspect
                {
                    readonly internal RefRW<EcsTestData> Data;
                }
            }",
            "SGA0010");
    }

    [Fact]
    public void SGA0010_NestingAspectsInGenericClass_Not_Supported()
    {
        SourceGenTests.CheckForError<AspectGenerator>(@"
            using Unity.Entities;

            public class Parent<T>
            {
                public readonly partial struct TestAspect : IAspect
                {
                    readonly internal RefRW<EcsTestData> Data;
                }
            }",
            "SGA0010");
    }

    [Fact]
    public void SGA0010_NestingAspectsInGenericStruct_Not_Supported()
    {
        SourceGenTests.CheckForError<AspectGenerator>(@"
            using Unity.Entities;

            public struct Parent<T>
            {
                public readonly partial struct TestAspect : IAspect
                {
                    readonly internal RefRW<EcsTestData> Data;
                }
            }",
            "SGA0010");
    }

    [Fact]
    public void SGA0010_NestingAspectsInAspect_Not_Supported()
    {
        SourceGenTests.CheckForError<AspectGenerator>(@"
            using Unity.Entities;

            public readonly partial struct ParentAspect : IAspect
            {
                readonly internal RefRW<EcsTestData> ParentData;

                public readonly partial struct TestAspect : IAspect
                {
                    readonly internal ComponentDataRef<EcsTestData> Data;
                }
            }",
            "SGA0010");
    }

    [Fact]
    public void SGA0011_ReadOnlyRefRW_NotSupported()
    {
        SourceGenTests.CheckForError<AspectGenerator>(@"
            using Unity.Entities;
            using Unity.Entities.Tests;
            using Unity.Collections;

            public readonly partial struct TestAspect : IAspect
            {
                [ReadOnly] readonly internal RefRW<EcsTestData> Data;
            }",
            "SGA0011");
    }
}
