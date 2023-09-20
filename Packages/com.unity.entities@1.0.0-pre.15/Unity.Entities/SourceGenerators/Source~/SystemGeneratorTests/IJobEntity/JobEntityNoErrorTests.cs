using Unity.Entities.SourceGen.JobEntity;
using Unity.Entities.SourceGen.SystemGenerator;
using Xunit;

public class JobEntityNoErrorTests
{
    [Fact]
    public void InnerNamespaceUsing()
    {
        SourceGenTests.CheckForNoError<JobEntityGenerator>(@"
            namespace SomeNameSpace {
                public partial struct SomeJob : IJobEntity {
                    public void Execute() {}
                }
            }");
    }

    [Fact]
    public void JobInStruct()
    {
        SourceGenTests.CheckForNoError<JobEntityGenerator>(@"
            public partial struct SomeOuter {
                public partial struct SomeJob : IJobEntity {
                    public void Execute() {}
                }
            }");
    }

    [Fact]
    public void JobInClass()
    {
        SourceGenTests.CheckForNoError<JobEntityGenerator>(@"
            public partial class SomeOuter {
                public partial struct SomeJob : IJobEntity {
                    public void Execute() {}
                }
            }");
    }

    [Fact]
    public void TwoJobs()
    {
        SourceGenTests.CheckForNoError<JobEntityGenerator>(@"
            public partial struct SomeOuter {
                public partial struct SomeJobA : IJobEntity {
                    public void Execute() {}
                }
                public partial struct SomeJobB : IJobEntity {
                    public void Execute() {}
                }
            }");
    }

    #region System

    [Fact]
    public void SystemInnerNamespaceUsing()
    {
        SourceGenTests.CheckForNoError<SystemGenerator>(@"
            namespace SomeNameSpace {
                public partial struct SomeSystem : ISystem {
                    public void OnUpdate(ref SystemState state){
                        SystemAPI.GetSingletonRW<EcsTestData>();
                    }
                }
            }");
    }

    [Fact]
    public void SystemInStruct()
    {
        SourceGenTests.CheckForNoError<SystemGenerator>(@"
            public partial struct SomeOuter {
                public partial struct SomeSystemInner : ISystem {
                    public void OnUpdate(ref SystemState state){
                        SystemAPI.GetSingletonRW<EcsTestData>();
                    }
                }
            }");
    }

    [Fact]
    public void SystemInClass()
    {
        SourceGenTests.CheckForNoError<SystemGenerator>(@"
            public partial class SomeOuter {
                public partial struct SomeSystemInner : ISystem {
                    public void OnUpdate(ref SystemState state){
                        SystemAPI.GetSingletonRW<EcsTestData>();
                    }
                }
            }");
    }

    #endregion
}
