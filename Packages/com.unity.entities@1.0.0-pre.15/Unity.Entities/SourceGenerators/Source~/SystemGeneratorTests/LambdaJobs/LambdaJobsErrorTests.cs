using Unity.Entities.SourceGen.SystemGenerator;
using Xunit;

public class LambdaJobsErrorTests
{
    [Fact]
    public void NestedSingletonInGenerated()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
            using Unity.Entities;
            using Unity.Entities.Tests;
            partial class SomeSystem : SystemBase {
                protected override void OnUpdate()
                {
                    Entities.ForEach((Entity e) => {
                            SetComponent(e, GetSingleton<EcsTestData>());
                    }).Schedule();
                }
            }", "DC0004");
    }

    [Fact]
    public void DC0003_WithConflictingName()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
            public partial class WithConflictingName : SystemBase
            {
                protected override void OnUpdate()
                {
                    Entities.WithName(""VeryCommonName"").ForEach((ref Translation t) => {}).Schedule();
                    Entities.WithName(""VeryCommonName"").ForEach((ref Translation t) => {}).Schedule();
                }
            }", "DC0003");
    }

    [Fact]
    public void DC0004_JobWithCodeCapturingFieldInSystem()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class JobWithCodeCapturingFieldInSystem_System : SystemBase
        {
            public int _someField;
            protected override void OnUpdate()
            {
                Job
                    .WithCode(() => { _someField = 123; })
                    .Run();
            }
        }", "DC0004");
    }

    [Fact]
    public void DC0004_WithGetComponentAndCaptureOfThisTest()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class WithGetComponentAndCaptureOfThis : SystemBase
        {
            float someField = 3.0f;

            protected override void OnUpdate()
            {
                Entities.ForEach((ref Translation translation) =>
                    {
                        var vel = GetComponent<Velocity>(default);
                        translation = new Translation() {Value = someField * vel.Value};
                    }).Schedule();
            }
        }", "DC0004");
    }

    [Fact]
    public void DC0004_WithGetComponentAndCaptureOfThisAndVarTest()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class WithGetComponentAndCaptureOfThisAndVar : SystemBase
        {
            float someField = 3.0f;

            protected override void OnUpdate()
            {
                float someVar = 2.0f;
                Entities.ForEach((ref Translation translation) =>
                    {
                        var vel = GetComponent<Velocity>(default);
                        translation = new Translation() {Value = someField * vel.Value * someVar};
                    }).Schedule();
            }
        }", "DC0004");
    }

    [Fact]
    public void DC0004_CaptureFieldInNonLocalCapturingLambdaTest()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class CaptureFieldInNonLocalCapturingLambda : SystemBase
        {
            private int myfield = 123;

            protected override void OnUpdate()
            {
                Entities
                    .ForEach((ref Translation t) => { t.Value = myfield; })
                    .Schedule();
            }
        }", "DC0004");
    }

    [Fact]
    public void DC0004_CaptureFieldByRefTest()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class CaptureFieldByRef : SystemBase
        {
            int m_MyField = 123;

            protected override void OnUpdate()
            {
                Entities
                    .ForEach((ref Translation t) =>{ NotAProblem(ref m_MyField); })
                    .Schedule();
            }

            static void NotAProblem(ref int a) {}
        }", "DC0004");
    }

    [Fact]
    public void DC0004_InvokeInstanceMethodInCapturingLambdaTest()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class InvokeInstanceMethodInCapturingLambda : SystemBase
        {
            public object GetSomething(int i) => default;

            protected override void OnUpdate()
            {
                int also_capture_local = 1;
                Entities
                    .ForEach((ref Translation t) => { GetSomething(also_capture_local); })
                    .Schedule();
            }
        }", "DC0004");
    }

    [Fact]
    public void DC0004_InvokeInstanceMethodInNonCapturingLambdaTest()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class InvokeInstanceMethodInNonCapturingLambda : SystemBase
        {
            public object GetSomething(int i) => default;

            protected override void OnUpdate()
            {
                Entities
                    .ForEach((ref Translation t) => { GetSomething(3); })
                    .Schedule();
            }
        }", "DC0004");
    }


    [Fact]
    public void DC0004_CallsMethodInComponentSystemBaseTest()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class CallsMethodInComponentSystemBase : SystemBase
        {
            protected override void OnUpdate()
            {
                Entities
                    .ForEach((ref Translation t) => { var targetDistance = Time.DeltaTime; })
                    .Schedule();
            }
        }", "DC0004");
    }

    [Fact]
    public void DC0004_WithCapturedReferenceType()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
            public partial class WithCapturedReferenceType : SystemBase
            {
                class CapturedClass
                {
                    public float value;
                }

                protected override void OnUpdate()
                {
                    var capturedClass = new CapturedClass() {value = 3.0f};
                    Entities
                        .ForEach((ref Translation t) => { t.Value = capturedClass.value; })
                        .Schedule();
                }
            }", "DC0004");
    }

    [Fact] // This should use DC0001 but we don't have a good way to do that with source generators yet
    public void DC0004_CaptureFieldInLocalCapturingLambdaTest()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
            public partial class CaptureFieldInLocalCapturingLambda : SystemBase
            {
                private int field = 123;

                protected override void OnUpdate()
                {
                    int also_capture_local = 1;
                    Entities
                        .ForEach((ref Translation t) => { t.Value = field + also_capture_local; })
                        .Schedule();
                }
            }", "DC0004");
    }

    [Fact]
    public void DC0004_InvokeBaseMethodInBurstLambdaTest()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
            public partial class InvokeBaseMethodInBurstLambdaTest : SystemBase
            {
                protected override void OnUpdate()
                {
                    int version = 0;
                    Entities.ForEach((ref Translation t) => { version = EntityManager.EntityOrderVersion; }).Run();
                }
            }", "DC0004");
    }

    [Fact]
    public void DC0004_InvokeThisMethodInForEach()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
            partial class Test : SystemBase
            {
                public void SomeMethod(){}

                protected override void OnUpdate()
                {
                    Entities.ForEach((in Translation t) => SomeMethod()).Run();
                }
            }", "DC0004");
    }

    [Fact]
    public void DC0004_InvokeThisMethodInWithCode()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
            partial class Test : SystemBase
            {
                public void SomeMethod(){}

                protected override void OnUpdate()
                {
                    Job.WithCode(() => SomeMethod()).Run();
                }
            }", "DC0004");
    }

    [Fact]
    public void DC0004_InvokeThisPropertyInForEach()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
            partial class Test : SystemBase
            {
                public bool SomeProp{get;set;}

                protected override void OnUpdate()
                {
                    Entities.ForEach((ref Translation t) =>
                    {
                        var val = !SomeProp;
                    }).Run();
                }
            }", "DC0004");
    }

    [Fact]
    public void DC0004_InvokeThisFieldInWithCode()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
            partial class Test : SystemBase
            {
                public bool SomeProp{get;set;}

                protected override void OnUpdate()
                {
                    Job.WithCode(() =>
                    {
                        var val = !SomeProp;
                    }).Run();
                }
            }", "DC0004");
    }

    [Fact]
    public void DC0004_InvokeThisFieldInForEach()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
            partial class Test : SystemBase
            {
                public bool SomeField = false;

                protected override void OnUpdate()
                {
                    Entities.ForEach((ref Translation t) =>
                    {
                        var val = !SomeField;
                    }).Run();
                }
            }", "DC0004");
    }

    [Fact]
    public void DC0004_InvokeThisPropertyInWithCode()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
            partial class Test : SystemBase
            {
                public bool SomeField = false;

                protected override void OnUpdate()
                {
                    Job.WithCode(() =>
                    {
                        var val = !SomeField;
                    }).Run();
                }
            }", "DC0004");
    }

    [Fact]
    public void DC0005_WithUnsupportedParameter()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
            public partial class WithUnsupportedParameter : SystemBase
            {
                protected override void OnUpdate()
                {
                    Entities
                        .ForEach((string whoKnowsWhatThisMeans, in Translation translation) => {})
                        .Schedule();
                }
            }", "DC0005");
    }

    [Fact]
    public void DC0008_WithBurstWithNonLiteral()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class WithBurstWithNonLiteral : SystemBase
        {
            protected override void OnUpdate()
            {
                var floatMode = Unity.Burst.FloatMode.Deterministic;
                Entities
                    .WithBurst(floatMode)
                    .ForEach((in Translation translation) => {})
                    .Schedule();
            }
        }", "DC0008");
    }

    [Fact]
    public void DC0009_UsingConstructionMultipleTimes()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class UsingConstructionMultipleTimes : SystemBase
        {
            protected override void OnUpdate()
            {
                Entities
                    .WithName(""Cannot"")
                    .WithName(""Make up my mind"")
                    .ForEach((in Translation translation) => {})
                    .Schedule();
            }
        }", "DC0009");
    }

    [Fact]
    public void DC0010_ControlFlowInsideWithChainTest()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class ControlFlowInsideWithChainSystem : SystemBase
        {
            protected override void OnUpdate()
            {
                var maybe = false;
                Entities
                    .WithName(maybe ? ""One"" : ""Two"")
                    .ForEach(
                        (ref Translation translation, in Velocity velocity) =>
                        {
                            translation.Value += velocity.Value;
                        })
                    .Schedule();
            }
        }", "DC0010");
    }

    [Fact]
    public void DC0011_WithoutScheduleInvocationTest()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class WithoutScheduleInvocation : SystemBase
        {
            protected override void OnUpdate()
            {
                Entities.ForEach(
                    (ref Translation translation, in Velocity velocity) =>
                    {
                        translation.Value += velocity.Value;
                    });
            }
        }", "DC0011");
    }

    [Fact]
    public void DC0012_WithReadOnly_IllegalArgument_Test()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class WithReadOnly_IllegalArgument : SystemBase
        {
            protected override void OnUpdate()
            {
                Entities
                    .WithReadOnly(""stringLiteral"")
                    .ForEach((in Translation translation) => {})
                    .Schedule();
            }
        }", "DC0012");
    }

    [Fact]
    public void DC0012_WithReadOnly_NonCapturedVariable_Test()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class WithReadOnly_NonCapturedVariable : SystemBase
        {
            protected override void OnUpdate()
            {
                var myNativeArray = new NativeArray<float>();

                Entities
                    .WithReadOnly(myNativeArray)
                    .ForEach((in Translation translation) => {})
                    .Schedule();
            }
        }", "DC0012");
    }

    [Fact]
    public void DC0012_WithDisposeOnCompletion_WithRun_NonCapturedVariable_Test()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class WithDisposeOnCompletion_WithRun_NonCapturedVariable : SystemBase
        {
            protected override void OnUpdate()
            {
                var myNativeArray = new NativeArray<float>();

                Entities
                    .WithDisposeOnCompletion(myNativeArray)
                    .ForEach((in Translation translation) => {})
                    .Run();
            }
        }", "DC0012");
    }

    [Fact]
    public void DC0013_LocalFunctionThatWritesBackToCapturedLocalTest()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class LocalFunctionThatWritesBackToCapturedLocal : SystemBase
        {
            protected override void OnUpdate()
            {
                int capture_me = 123;
                Entities
                    .ForEach((ref Translation t) =>
                    {
                        void MyLocalFunction()
                        {
                            capture_me++;
                        }

                        MyLocalFunction();
                    }).Schedule();
                var test = capture_me;
            }
        }", "DC0013");
    }

    [Fact]
    public void DC0013_LambdaThatWritesBackToCapturedLocalTest()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class LambdaThatWritesBackToCapturedLocal : SystemBase
        {
            protected override void OnUpdate()
            {
                int capture_me = 123;
                Entities.ForEach((ref Translation t) => { capture_me++; }).Schedule();
                var test = capture_me;
            }
        }", "DC0013");
    }

    [Fact]
    public void DC0014_UseOfUnsupportedIntParamInLambda()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class UseOfUnsupportedIntParamInLambda : SystemBase
        {
            protected override void OnUpdate()
            {
                Entities.ForEach((int NotAValidParam, ref Translation t) => { }).Schedule();
            }
        }", "DC0014");
    }

    [Fact]
    public void DC0223_UseSharedComponentDataUsingSchedule()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class SharedComponentDataUsingSchedule : SystemBase
        {
            struct MySharedComponentData : ISharedComponentData {}

            protected override void OnUpdate()
            {
                Entities
                    .ForEach((MySharedComponentData mydata) => {})
                    .Schedule();
            }
        }", "DC0223");
    }

    [Fact]
    public void DC0020_SharedComponentDataReceivedByRef()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class SharedComponentDataReceivedByRef : SystemBase
        {
            struct MySharedComponentData : ISharedComponentData {}

            protected override void OnUpdate()
            {
                Entities
                    .WithoutBurst()
                    .ForEach((ref MySharedComponentData mydata) => {})
                    .Run();
            }
        }", "DC0020");
    }

    [Fact]
    public void DC0021_CustomStructArgumentThatDoesntImplementSupportedInterfaceTest()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class CustomStructArgumentThatDoesntImplementSupportedInterface : SystemBase
        {
            struct ForgotToAddInterface {}

            protected override void OnUpdate()
            {
                Entities
                    .ForEach((ref ForgotToAddInterface t) => {})
                    .Schedule();
            }
        }", "DC0021");
    }

#if !UNITY_DISABLE_MANAGED_COMPONENTS

    [Fact]
    public void DC0223_ManagedComponentInBurstJobTest()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class ManagedComponentInBurstJobTest : SystemBase
        {
            protected override void OnUpdate()
            {
                Entities.ForEach((ManagedComponent t) => {}).Run();
            }
        }
        class ManagedComponent : IComponentData, IEquatable<ManagedComponent>
        {
            public bool Equals(ManagedComponent other) => false;
            public override bool Equals(object obj) => false;
            public override int GetHashCode() =>  0;
        }", "DC0223");
    }

    [Fact]
    public void DC0223_ManagedComponentInSchedule()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class ManagedComponentInSchedule : SystemBase
        {
            protected override void OnUpdate()
            {
                Entities.ForEach((ManagedComponent t) => {}).Schedule();
            }
        }
        class ManagedComponent : IComponentData, IEquatable<ManagedComponent>
        {
            public bool Equals(ManagedComponent other) => false;
            public override bool Equals(object obj) => false;
            public override int GetHashCode() =>  0;
        }", "DC0223");
    }

    [Fact]
    public void DC0024_ManagedComponentByReference()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class ManagedComponentByReference : SystemBase
        {
            protected override void OnUpdate()
            {
                Entities.WithoutBurst().ForEach((ref ManagedComponent t) => {}).Run();
            }
        }
        class ManagedComponent : IComponentData, IEquatable<ManagedComponent>
        {
            public bool Equals(ManagedComponent other) => false;
            public override bool Equals(object obj) => false;
            public override int GetHashCode() =>  0;
        }", "DC0024");
    }
#endif

    [Fact]
    public void DC0025_SystemWithDefinedOnCreateForCompiler()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class SystemWithDefinedOnCreateForCompiler : SystemBase
        {
            protected override void OnCreateForCompiler() {}

            protected override void OnUpdate() {
                Entities.ForEach((in Translation translation) => {}).Schedule();
            }
        }", "DC0025", "SystemWithDefinedOnCreateForCompiler");
    }

    [Fact]
    public void SGQC002_WithAllWithSharedFilterTest()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class WithAllWithSharedFilter : SystemBase
        {
            struct MySharedComponentData : ISharedComponentData { public int Value; }

            protected override void OnUpdate()
            {
                Entities
                    .WithAll<MySharedComponentData>()
                    .WithSharedComponentFilter(new MySharedComponentData() { Value = 3 })
                    .ForEach((in Translation translation) => {})
                    .Schedule();
            }
        }", "SGQC002");
    }

    [Fact]
    public void SGQC003_WithNoneWithSharedFilterTest()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class WithAllWithSharedFilter : SystemBase
        {
            struct MySharedComponentData : ISharedComponentData { public int Value; }

            protected override void OnUpdate()
            {
                Entities
                    .WithAny<MySharedComponentData>()
                    .WithSharedComponentFilter(new MySharedComponentData() { Value = 3 })
                    .ForEach((in Translation translation) => {})
                    .Schedule();
            }
        }", "SGQC003");
    }

    [Fact]
    public void DC0027_LambdaThatMakesNonExplicitStructuralChangesTest()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class LambdaThatMakesNonExplicitStructuralChanges : SystemBase
        {
            protected override void OnUpdate()
            {
                Entities
                    .WithoutBurst()
                    .ForEach((Entity entity, ref Translation t) =>
                    {
                        EntityManager.RemoveComponent<Translation>(entity);
                    }).Run();
            }
        }", "DC0027");
    }

    [Fact]
    public void DC0027_LambdaThatMakesNonExplicitStructuralChangesTestFromCapturedEntityManager()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class LambdaThatMakesNonExplicitStructuralChangesTestFromCapturedEntityManager : SystemBase
        {
            protected override void OnUpdate()
            {
                var em = EntityManager;
                Entities
                    .WithoutBurst()
                    .ForEach((ref Translation t) =>
                    {
                        em.CreateEntity();
                    }).Run();
            }
        }", "DC0027");
    }


    [Fact]
    public void DC0027_LambdaThatMakesStructuralChangesWithScheduleTest()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class LambdaThatMakesStructuralChangesWithSchedule : SystemBase
        {
            protected override void OnUpdate()
            {
                float delta = 0.0f;
                Entities.WithoutBurst()
                    .WithStructuralChanges()
                    .ForEach((Entity entity, ref Translation t) =>
                    {
                        float blah = delta + 1.0f;
                        EntityManager.RemoveComponent<Translation>(entity);
                    }).Schedule();
            }
        }", "DC0027");
    }

    [Fact]
    public void DC0029_LambdaThatHasNestedLambdaTest()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class LambdaThatHasNestedLambda : SystemBase
        {
            protected override void OnUpdate()
            {
                float delta = 0.0f;
                Entities
                    .WithoutBurst()
                    .ForEach((Entity e1, ref Translation t1) =>
                    {
                        Entities
                            .WithoutBurst()
                            .ForEach((Entity e2, ref Translation t2) => { delta += 1.0f; }).Run();
                    }).Run();
            }
        }", "DC0029");
    }

    [Fact]
    public void DC0031_LambdaThatTriesToStoreNonValidEntityQueryVariableTest()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class LambdaThatTriesToStoreNonValidEntityQueryVariable : SystemBase
        {
            partial class EntityQueryHolder
            {
                public EntityQuery m_Query;
            }

            protected override void OnUpdate()
            {
                EntityQueryHolder entityQueryHolder = new EntityQueryHolder();

                float delta = 0.0f;
                Entities
                    .WithStoreEntityQueryInField(ref entityQueryHolder.m_Query)
                    .ForEach((Entity e2, ref Translation t2) => { delta += 1.0f; }).Run();
            }
        }", "DC0031");
    }

    [Fact]
    public void DC0031_LambdaThatTriesToStoreLocalEntityQueryVariableTest()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class LambdaThatTriesToStoreLocalEntityQueryVariable : SystemBase
        {
            protected override void OnUpdate()
            {
                EntityQuery query = default;

                float delta = 0.0f;
                Entities
                    .WithStoreEntityQueryInField(ref query)
                    .ForEach((Entity e2, ref Translation t2) => { delta += 1.0f; }).Run();
            }
        }", "DC0031");
    }

    [Fact]
    public void DC0033_IncorrectUsageOfBufferIsDetected()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        public struct MyBufferFloat : IBufferElementData
        {
            public float Value;
        }
        partial class IncorrectUsageOfBuffer : SystemBase
        {
            protected override void OnUpdate()
            {
                Entities
                    .ForEach((MyBufferFloat f) => {})
                    .Schedule();
            }
        }", "DC0033");
    }

    [Fact]
    public void DC0034_ReadOnlyWarnsAboutArgumentType_IncorrectReadOnlyUsageWithStruct()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class IncorrectReadOnlyUsageWithStruct : SystemBase
        {
            struct StructWithPrimitiveType { public int field; }
            protected override void OnUpdate()
            {
                StructWithPrimitiveType structWithPrimitiveType = default;
                structWithPrimitiveType.field = default;
                Entities
                    .WithReadOnly(structWithPrimitiveType)
                    .ForEach((ref Translation t) => { t.Value += structWithPrimitiveType.field; })
                    .Schedule();
            }
        }", "DC0034");
    }

    [Fact]
    public void DC0034_ReadOnlyWarnsAboutArgumentType_IncorrectReadOnlyUsageWithPrimitiveType()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class IncorrectReadOnlyUsageWithPrimitiveType : SystemBase
        {
            protected override void OnUpdate()
            {
                var myVar = 0;
                Entities
                    .WithReadOnly(myVar)
                    .ForEach((ref Translation t) => { t.Value += myVar; })
                    .Schedule();
            }
        }", "DC0034");
    }

    [Fact]
    public void DC0036_DisableContainerSafetyRestrictionWarnsAboutArgumentType_IncorrectDisableContainerSafetyRestrictionUsageWithStruct()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class IncorrectDisableContainerSafetyRestrictionUsageWithStruct : SystemBase
        {
            struct StructWithPrimitiveType { public int field; }
            protected override void OnUpdate()
            {
                StructWithPrimitiveType structWithPrimitiveType = default;
                structWithPrimitiveType.field = default;
                Entities
                    .WithNativeDisableContainerSafetyRestriction(structWithPrimitiveType)
                    .ForEach((ref Translation t) =>
                    {
                        t.Value += structWithPrimitiveType.field;
                    }).Schedule();
            }
        }", "DC0036");
    }

    [Fact]
    public void DC0036_DisableContainerSafetyRestrictionWarnsAboutArgumentType_IncorrectDisableContainerSafetyRestrictionUsageWithPrimitiveType()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class IncorrectDisableContainerSafetyRestrictionUsageWithPrimitiveType : SystemBase
        {
            protected override void OnUpdate()
            {
                var myVar = 0;
                Entities
                    .WithNativeDisableContainerSafetyRestriction(myVar)
                    .ForEach((ref Translation t) => { t.Value += myVar; })
                    .Schedule();
            }
        }", "DC0036");
    }

    [Fact]
    public void DC0037_DisableParallelForRestrictionWarnsAboutArgumentType_IncorrectDisableParallelForRestrictionUsageWithStruct()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class IncorrectDisableParallelForRestrictionUsageWithStruct : SystemBase
        {
            struct StructWithPrimitiveType { public int field; }
            protected override void OnUpdate()
            {
                StructWithPrimitiveType structWithPrimitiveType = default;
                structWithPrimitiveType.field = default;
                Entities
                    .WithNativeDisableParallelForRestriction(structWithPrimitiveType)
                    .ForEach((ref Translation t) => { t.Value += structWithPrimitiveType.field; }).Schedule();
            }
        }", "DC0037");
    }

    [Fact]
    public void DC0037_DisableParallelForRestrictionWarnsAboutArgumentType_IncorrectDisableParallelForRestrictionUsageWithPrimitiveType()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class IncorrectDisableParallelForRestrictionUsageWithPrimitiveType : SystemBase
        {
            protected override void OnUpdate()
            {
                var myVar = 0;
                Entities
                    .WithNativeDisableParallelForRestriction(myVar)
                    .ForEach((ref Translation t) => { t.Value += myVar; }).Schedule();
            }
        }", "DC0037");
    }

    [Fact]
    public void DC0043_InvalidJobNamesThrow_InvalidJobNameWithSpaces()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class InvalidJobNameWithSpaces : SystemBase
        {
            protected override void OnUpdate()
            {
                Entities
                .WithName(""This name may not contain spaces"")
                .ForEach(
                    (ref Translation translation, in Velocity velocity) =>
                    {
                        translation.Value += velocity.Value;
                    })
                .Schedule();
            }
        }", "DC0043");
    }

    [Fact]
    public void DC0043_InvalidJobNamesThrow_InvalidJobNameStartsWithDigit()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class InvalidJobNameStartsWithDigit : SystemBase
        {
            protected override void OnUpdate()
            {
                Entities
                .WithName(""1job"")
                .ForEach(
                    (ref Translation translation, in Velocity velocity) =>
                    {
                        translation.Value += velocity.Value;
                    })
                .Schedule();
            }
        }", "DC0043");
    }

    [Fact]
    public void DC0043_InvalidJobNamesThrow_InvalidJobNameCompilerReservedName()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class InvalidJobNameCompilerReservedName : SystemBase
        {
            protected override void OnUpdate()
            {
                Entities
                .WithName(""__job"")
                .ForEach(
                    (ref Translation translation, in Velocity velocity) =>
                    {
                        translation.Value += velocity.Value;
                    })
                .Schedule();
            }
        }", "DC0043");
    }

    [Fact]
    public void DC0044_WithLambdaStoredInFieldTest()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class WithLambdaStoredInFieldSystem : SystemBase
        {
            Unity.Entities.UniversalDelegates.R<Translation> _translationAction;

            protected override void OnUpdate()
            {
                _translationAction = (ref Translation t) => {};
                Entities.ForEach(_translationAction).Schedule();
            }
        }", "DC0044");
    }

    [Fact]
    public void DC0044_WithLambdaStoredInVariableTest()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class WithLambdaStoredInVariableSystem : SystemBase
        {
            protected override void OnUpdate()
            {
                Unity.Entities.UniversalDelegates.R<Translation> translationAction = (ref Translation t) => {};
                Entities.ForEach(translationAction).Schedule();
            }
        }", "DC0044");
    }

    [Fact]
    public void DC0044_WithLambdaStoredInArgTest()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class WithLambdaStoredInArgSystem : SystemBase
        {
            void Test(Unity.Entities.UniversalDelegates.R<Translation> action)
            {
                Entities.ForEach(action).Schedule();
            }

            protected override void OnUpdate()
            {
                Test((ref Translation t) => {});
            }
        }", "DC0044");
    }

    [Fact]
    public void DC0044_WithLambdaReturnedFromMethodTest()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class WithLambdaReturnedFromMethodSystem : SystemBase
        {
            protected override void OnUpdate()
            {
                Entities.ForEach(GetAction()).Schedule();
            }

            static Unity.Entities.UniversalDelegates.R<Translation> GetAction()
            {
                return (ref Translation t) => {};
            }
        }", "DC0044");
    }

    [Fact]
    public void DC0046_SetComponentWithNotPermittedComponentAccessThatAliasesTest()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class SetComponentWithNotPermittedComponentAccessThatAliases : SystemBase
        {
            protected override void OnUpdate()
            {
                Entities.ForEach((Entity e, in Translation data) => {
                    SetComponent(e, new Translation());
                }).Run();
            }
        }", "DC0046");
    }

    [Fact]
    public void DC0047_SetComponentWithNotPermittedParameterThatAliasesTestTest()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class SetComponentWithNotPermittedParameterThatAliasesTest : SystemBase
        {
            protected override void OnUpdate()
            {
                Entities.ForEach((Entity e, ref Translation data) => {
                    var translation = GetComponent<Translation>(e);
                }).Run();
            }
        }", "DC0047");
    }

    [Fact]
    public void DC0050_ParameterWithInvalidGenericParameterTest()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class SomeClass<TValue> where TValue : struct
        {
            partial class ParameterWithInvalidGenericParameter : SystemBase
            {
                protected override void OnUpdate() {}

                void Test()
                {
                    Entities
                        .ForEach((in TValue generic) => {})
                        .Schedule();
                }
            }
        }", "DC0050");
    }

    [Fact]
    public void DC0050_ParameterWithInvalidGenericTypeTest()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class ParameterWithInvalidGenericType : SystemBase
        {
            struct GenericType<TValue> : IComponentData {}

            protected override void OnUpdate()
            {
                Entities
                    .ForEach((in GenericType<int> generic) => {})
                    .Schedule();
            }
        }", "DC0050");
    }

    [Fact]
    public void DC0050_ParameterWithInvalidGenericDynamicBufferTypeTest()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class ParameterWithInvalidGenericDynamicBufferType : SystemBase
        {
            public struct GenericBufferType<T> : IBufferElementData where T : struct, IComponentData {}

            protected override void OnUpdate()
            {
                Entities
                    .ForEach((ref DynamicBuffer<GenericBufferType<EcsTestData>> buffer) => {})
                    .Schedule();
            }
        }", "DC0050");
    }

    [Fact]
    public void DC0051_WithNoneWithInvalidGenericParameterTest()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class SomeClass<TValue> where TValue : struct
        {
            partial class WithNoneWithInvalidGenericParameter : SystemBase
            {
                protected override void OnUpdate() {}

                void Test()
                {
                    Entities
                        .WithNone<TValue>()
                        .ForEach((in Translation translation) => {})
                        .Schedule();
                }
            }
        }", "DC0051");
    }

    [Fact]
    public void DC0051_WithNoneWithInvalidGenericTypeTest()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class WithNoneWithInvalidGenericType : SystemBase
        {
            struct GenericType<TValue> : IComponentData {}
            protected override void OnUpdate()
            {
                Entities
                    .WithNone<GenericType<int>>()
                    .ForEach((in Translation translation) => {})
                    .Schedule();
            }
        }", "DC0051");
    }

    [Fact]
    public void SGQC001_WithNoneWithInvalidTypeTest()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class WithNoneWithInvalidType : SystemBase
        {
            class ANonIComponentDataClass
            {
            }
            protected override void OnUpdate()
            {
                Entities
                    .WithNone<ANonIComponentDataClass>()
                    .ForEach((in Translation translation) => {})
                    .Schedule();
            }
        }", "SGQC001");
    }

    [Fact]
    public void DC0053_InGenericSystemTypeTest()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class InGenericSystemType<T> : SystemBase where T : struct, IComponentData
        {
            protected override void OnUpdate()
            {
                Entities.ForEach((Entity e) => {}).Run();
            }
        }", "DC0053");
    }

    [Fact]
    public void DC0054_InGenericMethodThatCapturesTest()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class InGenericMethodThatCapturesType : SystemBase
        {
            void Test<T>()
            {
                var capture = 3.41f;
                Entities.ForEach((ref Translation translation) =>
                {
                    translation.Value = capture;
                }).Run();
            }

            protected override void OnUpdate()
            {
                Test<int>();
            }
        }", "DC0054");
    }

    [Fact]
    public void DC0055_ComponentPassedByValueGeneratesWarning_Test()
    {
        SourceGenTests.CheckForWarning<SystemGenerator>(@"
        partial class ComponentPassedByValueGeneratesWarning_System : SystemBase
        {
            protected override void OnUpdate()
            {
                Entities.ForEach((Translation translation) => { }).Run();
            }
        }", "DC0055");
    }

    [Fact]
    public void SGQC004_InvalidWithNoneComponentGeneratesError_Test_WithNone_WithAll()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class InvalidWithNoneInWithAllComponentGeneratesError_System : SystemBase
        {
            protected override void OnUpdate()
            {
                Entities.WithNone<Translation>().WithAll<Translation>().ForEach((Entity entity) => { }).Run();
            }
        }", "SGQC004");
    }

    [Fact]
    public void SGQC004_InvalidWithNoneComponentGeneratesError_Test_WithNone_WithAny()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class InvalidWithNoneInWithAnyComponentGeneratesError_System : SystemBase
        {
            protected override void OnUpdate()
            {
                Entities.WithNone<Translation>().WithAny<Translation>().ForEach((Entity entity) => { }).Run();
            }
        }", "SGQC004");
    }

    [Fact]
    public void SGQC004_InvalidWithNoneComponentGeneratesError_Test_WithNone_LambdaParameter()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class InvalidWithNoneInLambdaParamComponentGeneratesError_System : SystemBase
        {
            protected override void OnUpdate()
            {
                Entities.WithNone<Translation>()
                    .ForEach((ref Translation translation) => { }).Run();
            }
        }", "SGQC004");
    }

    [Fact]
    public void SGQC004_InvalidWithAnyComponentGeneratesError_Test_WithAny_WithAll()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class InvalidWithAnyInWithAllComponentGeneratesError_System : SystemBase
        {
            protected override void OnUpdate()
            {
                Entities.WithAny<Translation>().WithAll<Translation>().ForEach((Entity entity) => { }).Run();
            }
        }", "SGQC004");
    }

    [Fact]
    public void SGQC004_InvalidWithAnyComponentGeneratesError_Test_WithAny_LambdaParameter()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class InvalidWithAnyInLambdaParamComponentGeneratesError_System : SystemBase
        {
            protected override void OnUpdate()
            {
                Entities.WithAny<Translation>().ForEach((ref Translation translation) => { }).Run();
            }
        }", "SGQC004");
    }

    [Fact]
    public void DC0057_JobWithCodeAndStructuralChanges_Test()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class JobWithCodeAndStructuralChanges_System : SystemBase
        {
            protected override void OnUpdate()
            {
                Job.WithStructuralChanges().WithCode(() =>{ }).Run();
            }
        }", "DC0057");
    }

    [Fact]
    public void DC0058_EntitiesForEachNotInPartialClass()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        class EntitiesForEachNotInPartialClass : SystemBase
        {
            protected override void OnUpdate()
            {
                Entities.ForEach((ref Translation translation) => {}).Schedule();
            }
        }", "DC0058");
    }

    [Fact]
    public void DC0058_EntitiesForEachNotInPartialStruct()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        struct EntitiesForEachNotInPartialClass : ISystem
        {
            public void OnUpdate(ref SystemState state)
            {
                state.Entities.ForEach((ref Translation translation) => {}).Schedule();
            }
            public void OnCreate(ref SystemState state){}
            public void OnDestroy(ref SystemState state){}
        }", "DC0058");
    }

    [Fact]
    public void DC0059_GetComponentLookupWithMethodAsParam_ProducesError()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class GetComponentLookupWithMethodAsParam : SystemBase
        {
            static bool MethodThatReturnsBool() => false;
            protected override void OnUpdate()
            {
                Entities
                    .ForEach((Entity entity, in Translation tde) =>
                    {
                        GetComponentLookup<Velocity>(MethodThatReturnsBool());
                    }).Run();
            }
        }", "DC0059");
    }

    [Fact]
    public void DC0059_GetComponentLookupWithVarAsParam_ProducesError()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class GetComponentLookupWithVarAsParam : SystemBase
        {
            protected override void OnUpdate()
            {
                var localBool = false;
                Entities.ForEach((Entity entity, in Translation tde) => { GetComponentLookup<Velocity>(localBool); }).Run();
            }
        }", "DC0059");
    }

    [Fact]
    public void DC0059_GetComponentLookupWithArgAsParam_ProducesError()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class GetComponentLookupWithArgAsParam : SystemBase
        {
            protected override void OnUpdate() {}
            void Test(bool argBool)
            {
                Entities.ForEach((Entity entity, in Translation tde) => { GetComponentLookup<Velocity>(argBool); }).Run();
            }
        }", "DC0059");
    }

    [Fact]
    public void DC0060_EntitiesForEachInAssemblyNotReferencingBurst()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class EntitiesForEachInAssemblyNotReferencingBurst : SystemBase
        {
            protected override void OnUpdate()
            {
                Entities.ForEach((Entity entity, in Translation tde) => { }).Run();
            }
        }", "DC0060", null, false);
    }

    [Fact]
    public void DC0063_SetComponentInScheduleParallel()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class SetComponentInScheduleParallel : SystemBase
        {
            protected override void OnUpdate()
            {
                Entities.ForEach((Entity entity) =>
                {
                    SetComponent(entity, new Translation());
                }).ScheduleParallel();
            }
        }", "DC0063");
    }

    [Fact]
    public void DC0063_GetBufferInScheduleParallel()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class GetBufferInScheduleParallel : SystemBase
        {
            protected override void OnUpdate()
            {
                Entities.ForEach((Entity entity) => {
                    var value = GetBuffer<EcsIntElement>(entity)[0].Value;
                }).ScheduleParallel();
            }
        }", "DC0063");
    }

    [Fact]
    public void DC0063_GetComponentLookupInScheduleParallel()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class GetComponentLookupInScheduleParallel : SystemBase
        {
            protected override void OnUpdate()
            {
                Entities.ForEach((Entity entity) => {
                    var lookup = GetComponentLookup<EcsTestData>(false);
                }).ScheduleParallel();
            }
        }", "DC0063");
    }

    [Fact]
    public void DC0070_EntitiesForEach_IllegalDuplicateTypesUsed()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class DuplicateIComponentDataTypes : SystemBase
        {
            protected override void OnUpdate()
            {
                Entities.ForEach((Entity entity, in Translation translation1, in Translation translation2) => { }).Run();
            }
        }", "DC0070");
    }


    [Fact]
    public void DC0073_WithScheduleGranularity_Run()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class TestWithScheduleGranularity : SystemBase
        {

            protected override void OnUpdate()
            {
                Entities
                    .WithScheduleGranularity(ScheduleGranularity.Chunk)
                    .ForEach((ref Translation t) =>
                    {
                    }).Run();
            }
        }", "DC0073");
    }
    [Fact]
    public void DC0073_WithScheduleGranularity_Schedule()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class TestWithScheduleGranularity : SystemBase
        {
            protected override void OnUpdate()
            {
                Entities
                    .WithScheduleGranularity(ScheduleGranularity.Chunk)
                    .ForEach((ref Translation t) =>
                    {
                    }).Schedule();
            }
        }", "DC0073");
    }

    [Fact]
    public void DC0074_EcbParameter_MissingPlaybackInstructions()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        partial class MissingPlaybackInstructions : SystemBase
        {
            protected override void OnUpdate()
            {
                Entities
                    .ForEach((EntityCommandBuffer buffer) =>
                    {
                    }).Schedule();
            }
        }", "DC0074");
    }

    [Fact]
    public void DC0075_EcbParameter_ConflictingPlaybackInstructions()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        public class TestEntityCommandBufferSystem : EntityCommandBufferSystem { }

        partial class ConflictingPlaybackInstructions : SystemBase
        {
            protected override void OnUpdate()
            {
                Entities
                    .WithImmediatePlayback()
                    .WithDeferredPlaybackSystem<TestEntityCommandBufferSystem>()
                    .ForEach((EntityCommandBuffer buffer) =>
                    {
                    }).Run();
            }
        }", "DC0075");
    }

    [Fact]
    public void DC0076_EcbParameter_UsedMoreThanOnce()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        public class TestEntityCommandBufferSystem : EntityCommandBufferSystem { }

        partial class EntityCommandsUsedMoreThanOnce : SystemBase
        {
            protected override void OnUpdate()
            {
                Entities
                    .WithImmediatePlayback()
                    .ForEach((EntityCommandBuffer buffer1, EntityCommandBuffer buffer2) =>
                    {
                    }).Run();
            }
        }", "DC0076");
    }

    [Fact]
    public void DC0077_EcbParameter_ImmediatePlayback_WithScheduling()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        public class TestEntityCommandBufferSystem : EntityCommandBufferSystem { }

        partial class ImmediatePlayback_WithScheduling : SystemBase
        {
            protected override void OnUpdate()
            {
                Entities
                    .WithImmediatePlayback()
                    .ForEach((EntityCommandBuffer buffer) =>
                    {
                    }).Schedule();
            }
        }", "DC0077");
    }

    [Fact]
    public void DC0078_EcbParameter_MoreThanOnePlaybackSystemsSpecified()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        public class MyEntityCommandBufferSystem : EntityCommandBufferSystem { }

        public class YourEntityCommandBufferSystem : EntityCommandBufferSystem { }

        partial class MoreThanOnePlaybackSystemsSpecified : SystemBase
        {
            protected override void OnUpdate()
            {
                Entities
                    .WithDeferredPlaybackSystem<MyEntityCommandBufferSystem>()
                    .WithDeferredPlaybackSystem<YourEntityCommandBufferSystem>()
                    .ForEach((EntityCommandBuffer buffer) =>
                    {
                    }).Run();
            }
        }", "DC0078");
    }

    [Fact]
    public void DC0079_EcbParameter_UnsupportedEcbMethodUsed()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        public class TestEntityCommandBufferSystem : EntityCommandBufferSystem { }

        partial class UnsupportedEcbMethodUsed : SystemBase
        {
            protected override void OnUpdate()
            {
                Entities
                    .WithDeferredPlaybackSystem<TestEntityCommandBufferSystem>()
                    .ForEach((EntityCommandBuffer buffer) =>
                    {
                        buffer.Playback(EntityManager);
                    }).Schedule();
            }
        }", "DC0079");
    }

    [Fact]
    public void DC0080_EcbParameter_MethodExpectingEntityQuery_NotOnMainThread()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        public class TestEntityCommandBufferSystem : EntityCommandBufferSystem { }

        public struct MyTag : IComponentData { }

        partial class MethodExpectingEntityQuery : SystemBase
        {
            protected override void OnUpdate()
            {
                var entityQuery = EntityManager.CreateEntityQuery(typeof(MyTag));

                Entities
                    .WithDeferredPlaybackSystem<TestEntityCommandBufferSystem>()
                    .ForEach((EntityCommandBuffer buffer) =>
                    {
                        buffer.RemoveComponentForEntityQuery<MyTag>(entityQuery);
                    }).Schedule();
            }
        }", "DC0080");
    }

    [Fact]
    public void DC0080_EcbParameter_MethodExpectingComponentDataClass_NotOnMainThread()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        public class TestEntityCommandBufferSystem : EntityCommandBufferSystem { }

        public class MyComponentDataClass : IComponentData { }

        partial class MethodExpectingComponentDataClass : SystemBase
        {
            protected override void OnUpdate()
            {
                Entities
                    .WithDeferredPlaybackSystem<TestEntityCommandBufferSystem>()
                    .ForEach((Entity e, EntityCommandBuffer buffer) =>
                    {
                        buffer.AddComponent<MyComponentDataClass>(e);
                    }).Schedule();
            }
        }", "DC0080");
    }

    [Fact]
    public void DC0081_EcbParallelWriterParameter()
    {
        SourceGenTests.CheckForError<SystemGenerator>(@"
        public class TestEntityCommandBufferSystem : EntityCommandBufferSystem { }

        partial class EcbParallelWriterParameter : SystemBase
        {
            protected override void OnUpdate()
            {
                Entities
                    .WithDeferredPlaybackSystem<TestEntityCommandBufferSystem>()
                    .ForEach((EntityCommandBuffer.ParallelWriter parallelWriter) =>
                    {
                    }).Schedule();
            }
        }", "DC0081");
    }
}
