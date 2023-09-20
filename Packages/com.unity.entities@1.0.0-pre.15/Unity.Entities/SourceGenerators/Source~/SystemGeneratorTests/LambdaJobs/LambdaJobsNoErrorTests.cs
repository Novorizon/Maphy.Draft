using Xunit;
using Unity.Entities.SourceGen.SystemGenerator;

public class LambdaJobsNoErrorTests
{
    [Fact]
    public void PartialTypes_ThreeParts()
    {
        SourceGenTests.CheckForNoError<SystemGenerator>(@"
            public partial class UserWrittenPartial : SystemBase {
                protected override void OnUpdate() {
                    Entities.ForEach((ref EcsTestData data, ref EcsTestData2 data2) => { }).ScheduleParallel();
                    OnUpdate(unusedParameter: true); // We just want to test that having multiple methods with identical names doesn't lead to compile-time errors
                }
            }

            public partial class UserWrittenPartial : SystemBase {
                protected void OnUpdate(bool unusedParameter) {
                    Entities.ForEach((ref EcsTestData data, ref EcsTestData2 data2) => { }).ScheduleParallel();
                    OnUpdate2();
                }
            }

            public partial class UserWrittenPartial : SystemBase {
                protected void OnUpdate2() {
                    Entities.ForEach((ref EcsTestData data, ref EcsTestData2 data2) => { }).ScheduleParallel();
                }
            }");
    }

    [Fact]
    public void PartialTypes_TwoParts_EntitiesForEachWithExactSameComponentDataSet()
    {
        SourceGenTests.CheckForNoError<SystemGenerator>(@"
            public partial class UserWrittenPartial : SystemBase {
                protected override void OnUpdate() {
                    Entities.ForEach((ref EcsTestData data, ref EcsTestData2 data2) => { }).ScheduleParallel();
                    OnUpdate(unusedParameter: true); // We just want to test that having multiple methods with identical names doesn't lead to compile-time errors
                }
            }

            public partial class UserWrittenPartial : SystemBase {
                protected void OnUpdate(bool unusedParameter) {
                    Entities.ForEach((ref EcsTestData data, ref EcsTestData2 data2) => { }).ScheduleParallel();
                }
            }");
    }

    [Fact]
    public void PartialTypes_TwoParts_EntitiesForEachWithOverlappingComponentDataSets()
    {
        // Only EcsTestData2 is used in both Entities.ForEach() invocations
        SourceGenTests.CheckForNoError<SystemGenerator>(@"
            public partial class UserWrittenPartial : SystemBase {
                protected override void OnUpdate() {
                    Entities.ForEach((ref EcsTestData data, ref EcsTestData2 data2) => { }).ScheduleParallel();
                    OnUpdate(unusedParameter: true); // We just want to test that having multiple methods with identical names doesn't lead to compile-time errors
                }
            }

            public partial class UserWrittenPartial : SystemBase {
                protected void OnUpdate(bool unusedParameter) {
                    Entities.ForEach((ref EcsTestData2 data2, ref EcsTestData3 data3) => { }).ScheduleParallel();
                }
            }");
    }

    [Fact]
    public void PartialTypes_TwoParts_EntitiesForEachWithUniqueComponentDataSets()
    {
        SourceGenTests.CheckForNoError<SystemGenerator>(@"
            public partial class UserWrittenPartial : SystemBase {
                protected override void OnUpdate() {
                    Entities.ForEach((ref EcsTestData data, ref EcsTestData2 data2) => { }).ScheduleParallel();
                    OnUpdate(unusedParameter: true); // We just want to test that having multiple methods with identical names doesn't lead to compile-time errors
                }
            }

            public partial class UserWrittenPartial : SystemBase {
                protected void OnUpdate(bool unusedParameter) {
                    Entities.ForEach((ref EcsTestData3 data3, ref EcsTestData4 data4) => { }).ScheduleParallel();
                }
            }");
    }

    [Fact]
    public void PartialTypes_TwoParts_JobWithCode()
    {
        SourceGenTests.CheckForNoError<SystemGenerator>(@"
            public partial class UserWrittenPartial : SystemBase {
                protected override void OnUpdate() {
                    Job.WithCode(() => { }).Schedule();
                    OnUpdate(unusedParameter: true); // We just want to test that having multiple methods with identical names doesn't lead to compile-time errors
                }
            }

            public partial class UserWrittenPartial : SystemBase {
                protected void OnUpdate(bool unusedParameter) {
                    Job.WithCode(() => { }).Schedule();
                }
            }");
    }

    [Fact]
    public void SealedSystem_With_EntitiesForEach()
    {
        SourceGenTests.CheckForNoError<SystemGenerator>(@"
            public sealed partial class SealedSystem : SystemBase {
                protected override void OnUpdate() {
                    Entities.ForEach((ref EcsTestData data, ref EcsTestData2 data2) => { }).ScheduleParallel();
                }
            }");
    }

    [Fact]
    public void EntitiesForEach_WithXXX_AndUnqueryableParams()
    {
        SourceGenTests.CheckForNoError<SystemGenerator>(@"
            class TestEntityCommandBufferSystem : EntityCommandBufferSystem
            {
                protected override void OnUpdate()
                {
                }
            }

            public partial class TestSystem : SystemBase
            {
                protected override void OnUpdate()
                {
                }

                public void AddSetSharedComponentToEntity_WithDeferredPlayback(int n)
                {
                    Entities
                        .WithDeferredPlaybackSystem<TestEntityCommandBufferSystem>()
                        .WithAny<EcsTestData>()
                        .ForEach(
                            (Entity e, EntityCommandBuffer ecb) =>
                            {
                            })
                        .ScheduleParallel();
                }
            }");
    }
}
