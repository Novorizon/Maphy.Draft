using Unity.Entities.SourceGen.SystemGenerator;
using Xunit;

public class ForEachNoErrorTests
{
    const string aspectTestSource = @"
            using Unity.Entities;

            public readonly partial struct AspectTest : IAspect
            {
                public readonly RefRW<EcsTestData> Value;
            }";

    [Fact]
    public void ForEachIteration_InSystemBase()
    {
        SourceGenTests.CheckForNoError<SystemGenerator>(@"
            public partial class SomeSystem : SystemBase {
                protected override void OnUpdate() {
                    foreach (var aspect in SystemAPI.Query<AspectTest>()) {}
                }
            }", aspectTestSource);
    }

    [Fact]
    public void DifferentAssemblies_Aspect()
    {
        SourceGenTests.CheckForNoError<SystemGenerator>(@"
            public partial struct SomeSystem : ISystem {
                public void OnUpdate(ref SystemState state) {
                    foreach (var aspect in SystemAPI.Query<AspectTest>()){}
                }
            }", aspectTestSource);
    }

    [Fact]
    public void DifferentAssemblies_ComponentDataRef()
    {
        SourceGenTests.CheckForNoError<SystemGenerator>(@"
            public partial struct SomeSystem : ISystem {
                public void OnUpdate(ref SystemState state) {
                    foreach (var data in SystemAPI.Query<RefRW<EcsTestData>>()){}
                }
            }");
    }

    [Fact]
    public void DifferentAssemblies_Combined()
    {
        SourceGenTests.CheckForNoError<SystemGenerator>(@"
            public partial struct SomeSystem : ISystem {
                public void OnUpdate(ref SystemState state) {
                    foreach (var (aspect, data) in SystemAPI.Query<AspectTest, RefRW<EcsTestData>>()){}
                }
            }", aspectTestSource);
    }

    [Fact]
    public void SystemBasePartialTypes_IdiomaticForEach()
    {
        SourceGenTests.CheckForNoError<SystemGenerator>(@"
            public partial class UserWrittenPartial : SystemBase {
                protected override void OnUpdate() {
                    foreach (var (data, data1) in SystemAPI.Query<RefRO<EcsTestData>, RefRO<EcsTestData2>>())
                    {
                        OnUpdate(unusedParameter: true);
                    }
                }
            }

            public partial class UserWrittenPartial : SystemBase {
                protected void OnUpdate(bool unusedParameter) {
                    foreach (var (data, data1) in SystemAPI.Query<RefRO<EcsTestData>, RefRO<EcsTestData2>>())
                    {
                    }
                }
            }");
    }

    [Fact]
    public void ISystemPartialTypes_IdiomaticForEach()
    {
        SourceGenTests.CheckForNoError<SystemGenerator>(@"
            public partial struct UserWrittenPartial : ISystem {
                public void OnUpdate(ref SystemState state) {
                    foreach (var (data, data1) in SystemAPI.Query<RefRO<EcsTestData>, RefRO<EcsTestData2>>())
                    {
                        OnUpdate(unusedParameter: true, ref state);
                    }
                }
            }

            public partial struct UserWrittenPartial : ISystem {
                public void OnUpdate(bool unusedParameter, ref SystemState state) {
                    foreach (var (data, data1) in SystemAPI.Query<RefRO<EcsTestData>, RefRO<EcsTestData2>>())
                    {
                    }
                }
            }");
    }
}
