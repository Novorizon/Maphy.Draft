using Unity.Entities.SourceGen.JobEntity;
using Unity.Entities.SourceGen.SystemGenerator;
using Xunit;

public class JobEntityErrorTests
{
    [Fact]
    public void SGJE0003_InvalidValueTypesInExecuteMethod1()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
            public partial struct WithInvalidValueTypeParameters : IJobEntity
            {
                void Execute(Entity entity, float invalidFloat)
                {
                }
            }

            public partial class TestSystem : SystemBase
            {
                protected override void OnUpdate()
                {
                    Dependency = new WithInvalidValueTypeParameters().Schedule(Dependency);
                }
            }", "SGJE0003");
    }

    [Fact]
    public void SGJE0003_InvalidValueTypesInExecuteMethod2()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
            public partial class PlayerVehicleControlSystem : SystemBase
            {
                protected override void OnUpdate()
                {
                    var job = new IllegalThrustJob { DeltaTime = Time.DeltaTime };
                    job.ScheduleParallel();
                }
            }

            partial struct IllegalThrustJob : IJobEntity
            {
                public float DeltaTime;

                public void Execute(int NotAValidParam, ref Translation translation)
                {
                    translation.Value *= DeltaTime;
                }
            }", "SGJE0003");
    }

    [Fact]
    public void SGJE0004_NonPartialType()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
            public struct NonPartialJobEntity : IJobEntity
            {
                void Execute(Entity entity)
                {
                }
            }

            public partial class TestSystem : SystemBase
            {
                protected override void OnUpdate()
                {
                    Dependency = new NonPartialJobEntity().Schedule(Dependency);
                }
            }", "SGJE0004");
    }

    [Fact]
    public void SGJE0006_NonIntegerEntityInQueryParameter()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
            public partial struct NonIntegerEntityInQueryParameter : IJobEntity
            {
                void Execute(Entity entity, [EntityIndexInQuery] bool notInteger)
                {
                }
            }

            public partial class TestSystem : SystemBase
            {
                protected override void OnUpdate()
                {
                    Dependency = new NonIntegerEntityInQueryParameter().Schedule(Dependency);
                }
            }", "SGJE0006");
    }

    [Fact]
    public void SGJE0006()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
            using Unity.Entities;
            using Unity.Entities.Tests;

            partial struct ErrorJob : IJobEntity
            {
                void Execute([Unity.Entities.ChunkIndexInQuery] float val){}
            }

            partial struct SomeSystem : ISystem {
                public void OnUpdate(ref SystemState state)
                {
                    new ErrorJob().Run();
                }
            }", "SGJE0006");
    }

    [Fact]
    public void SGJE0007()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
            using Unity.Entities;
            using Unity.Entities.Tests;

            partial struct ErrorJob : IJobEntity
            {
                void Execute([Unity.Entities.ChunkIndexInQuery] int val,[Unity.Entities.EntityIndexInChunk] int val,[Unity.Entities.ChunkIndexInQuery] int val){}
            }

            partial struct SomeSystem : ISystem {
                public void OnUpdate(ref SystemState state)
                {
                    new ErrorJob().Run();
                }
            }", "SGJE0007");
    }

    [Fact]
    public void SGJE0007_TooManyIntegerEntityInQueryParameters()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
            public partial struct TooManyIntegerEntityInQueryParameters : IJobEntity
            {
                void Execute(Entity entity, [EntityIndexInQuery] int first, [EntityIndexInQuery] int second) {}
            }

            public partial class TestSystem : SystemBase
            {
                protected override void OnUpdate()
                {
                    Dependency = new TooManyIntegerEntityInQueryParameters().Schedule(Dependency);
                }
            }", "SGJE0007");
    }

    [Fact]
    public void SGJE0008_NoExecuteMethod()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
            public partial struct NoExecuteMethod : IJobEntity
            {
                void NotExecuting(Entity entity) {}
            }

            public partial class TestSystem : SystemBase
            {
                protected override void OnUpdate()
                {
                    new NoExecuteMethod().Schedule();
                }
            }", "SGJE0008");
    }

    [Fact]
    public void SGJE0008_TooManyExecuteMethods()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
            public partial struct TooManyExecuteMethods : IJobEntity
            {
                void Execute(Entity entity){}
                void Execute([EntityIndexInQuery] int index){}
                void Execute(){}
            }

            public partial class TestSystem : SystemBase
            {
                protected override void OnUpdate()
                {
                    var instance = new TooManyExecuteMethods();
                    Dependency = instance.Schedule(Dependency);
                }
            }", "SGJE0008");
    }

    [Fact]
    public void SGJE0008_MoreThanOneUserDefinedExecuteMethods()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
            public partial class TooManyUserDefinedExecuteMethods : SystemBase
            {
                protected override void OnUpdate()
                {
                    new ThrustJob().ScheduleParallel();
                }

                struct NonIJobEntityStruct
                {
                    public void Execute() {}
                    public void Execute(int someVal) {}
                }

                partial struct ThrustJob : IJobEntity
                {
                    public void Execute(ref Translation translation) {}
                    public void Execute(int someVal) {}
                }
            }", "SGJE0008");
    }

    [Fact]
    public void SGJE0009_ContainsReferenceTypeFields()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
            public partial class PlayerVehicleControlSystem : SystemBase
            {
                protected override void OnUpdate()
                {
                    var job = new ThrustJobWithReferenceTypeField { DeltaTime = Time.DeltaTime, JobName = ""MyThrustJob"" };
                    Dependency = job.ScheduleParallel(Dependency);
                }
            }

            partial struct ThrustJobWithReferenceTypeField : IJobEntity
            {
                public float DeltaTime;
                public string JobName;

                public void Execute(ref Translation translation)
                {
                    translation.Value *= (5f + DeltaTime);
                }
            }", "SGJE0009");
    }

    [Fact]
    public void SGJE0010_UnsupportedParameterTypeUsed()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
            public partial class PlayerVehicleControlSystem : SystemBase
            {
                protected override void OnUpdate()
                {
                    var job = new IllegalThrustJob { DeltaTime = Time.DeltaTime };
                    job.ScheduleParallel();
                }
            }

            partial struct IllegalThrustJob : IJobEntity
            {
                public float DeltaTime;

                public void Execute(ref Translation translation, IllegalClass illegalClass)
                {
                    translation.Value *= illegalClass.IllegalValue * DeltaTime;
                }
            }

            class IllegalClass
            {
                public int IllegalValue { get; private set; } = 42;
            }", "SGJE0010");
    }

    [Fact]
    public void SGJE0013()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
            using Unity.Entities;
            using Unity.Entities.Tests;

            partial struct SharedComponentJob : IJobEntity
            {
                void Execute(ref EcsTestSharedComp e1) => e1.value += 1;
            }

            partial struct SomeSystem : ISystem {
                public void OnUpdate(ref SystemState state)
                {
                    new SharedComponentJob().Run();
                }
            }", "SGJE0013");
    }

    [Fact]
    public void SGJE0016_RefTagComponent()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
            using Unity.Entities;
            using Unity.Entities.Tests;

            partial struct SomeSystem : ISystem {
                public void OnUpdate(ref SystemState state)
                {
                    new MyJob().ScheduleParallel();
                }
            }
            public partial struct MyJob : IJobEntity
            {
                void Execute(ref EcsTestTag empty)
                {
                }
            }
            ", "SGJE0016");
    }

    [Fact]
    public void SGJE0017_DuplicateComponent()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
            using Unity.Entities;
            using Unity.Entities.Tests;

            partial struct SomeSystem : ISystem {
                public void OnUpdate(ref SystemState state)
                {
                    new MyJob().ScheduleParallel();
                }
            }

            public partial struct MyJob : IJobEntity
            {
                void Execute(ref EcsTestData data1, ref EcsTestData data2)
                {
                }
            }
            ", "SGJE0017");
    }

    [Fact]
    public void SGJE0016_TagComponentWithRefWrapper()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
            using Unity.Entities;
            using Unity.Entities.Tests;
            partial struct SomeSystem : ISystem {
                public void OnUpdate(ref SystemState state)
                {
                    new MyJob().ScheduleParallel();
                }
            }
            public partial struct MyJob : IJobEntity
            {
                void Execute(RefRO<EcsTestTag> empty)
                {
                }
            }
            ", "SGJE0016");
    }
    [Fact]
    public void SGJE0018_RefWrapperWithKeyword()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
            using Unity.Entities;
            using Unity.Entities.Tests;
            partial struct SomeSystem : ISystem {
                public void OnUpdate(ref SystemState state)
                {
                    new MyJob().ScheduleParallel();
                }
            }
            public partial struct MyJob : IJobEntity
            {
                void Execute(ref RefRO<EcsTestData> e)
                {
                }
            }
            ", "SGJE0018");
    }

    [Fact]
    public void SGJE0019_RefWrapperWithNonComponent()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
            using Unity.Entities;
            using Unity.Entities.Tests;
            partial struct SomeSystem : ISystem {
                public void OnUpdate(ref SystemState state)
                {
                    new MyJob().ScheduleParallel();
                }
            }
            public partial struct MyJob : IJobEntity
            {
                void Execute(ref RefRO<EcsIntElement> e)
                {
                }
            }
            ", "SGJE0019");
    }
}
