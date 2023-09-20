using System.Threading.Tasks;
using Unity.Entities.SourceGen.Aspect;
using VerifyXunit;
using Xunit;

[UsesVerify]
public class AspectIntegrationTests
{
    [Fact]
    public Task AspectSimple()
    {
        var testSource = @"
            using Unity.Entities;
            using Unity.Collections;
            using Unity.Entities.Tests;

            // Test: Aspect generation must work when the aspect is declared in global scope
            public readonly partial struct AspectSimple : IAspect
            {
                public readonly RefRW<EcsTestData> Data;
            }";

        return SourceGenTests.Verify<AspectGenerator>(testSource);
    }

    [Fact]
    public Task AspectComplex()
    {
        var testSource = @"
            using Unity.Entities;
            using Unity.Collections;
            using Unity.Entities.Tests;

            public readonly partial struct AspectSimple : IAspect
            {
                public readonly RefRW<EcsTestData> Data;
            }

            namespace AspectTests
            {
                // Aspect generation must work when the aspect is within a namespace
                public readonly partial struct Aspect2 : IAspect
                {
                    // Test: component type should be correctly without any qualifiers
                    public readonly RefRW<EcsTestData> Data;

                    // Test: component type should be correctly handled with qualifiers
                    public readonly RefRW<Unity.Entities.Tests.EcsTestData2> Data2;

                    // Test: component type should be correctly handled with the 'global' qualifier
                    public readonly RefRW<global::Unity.Entities.Tests.EcsTestData3> Data3;

                    // Test: const data field must *not* be initialized in any generated constructors
                    public const int constI = 10;

                    // Test: RefRO fields must be read-only in the constructed entity query
                    public readonly RefRO<EcsTestData4> DataRO;

                    // Test: RefRO fields must be read-only in the constructed entity query
                    [Optional]
                    public readonly RefRW<EcsTestData5> DataOptional;

                    public readonly DynamicBuffer<EcsIntElement> DynamicBuffer;

                    [ReadOnly] readonly AspectSimple NestedAspectSimple;

                    // Test: Entity field
                    public readonly Entity Self;

                    // Test: EnabledRef
                    public readonly EnabledRefRO<EcsTestDataEnableable> EcsTestDataEnableable;
                }

                public readonly partial struct AspectNestedAliasing : IAspect
                {
                    // Nest Aspect2
                    public readonly Aspect2 Aspect2;

                    // Alias all fields of Aspect2, copy paste follows:

                    // Test: component type should be correctly without any qualifiers
                    public readonly RefRW<EcsTestData> Data;

                    // Test: component type should be correctly handled with qualifiers
                    public readonly RefRW<Unity.Entities.Tests.EcsTestData2> Data2;

                    // Test: component type should be correctly handled with the 'global' qualifier
                    public readonly RefRW<global::Unity.Entities.Tests.EcsTestData3> Data3;

                    // Test: const data field must *not* be initialized in any generated constructors
                    public const int constI = 10;

                    // Test: RefRO fields must be read-only in the constructed entity query
                    public readonly RefRO<EcsTestData4> DataRO;

                    // Test: RefRO fields must be read-only in the constructed entity query
                    [Optional]
                    public readonly RefRW<EcsTestData5> DataOptional;

                    public readonly DynamicBuffer<EcsIntElement> DynamicBuffer;

                    [ReadOnly] readonly AspectSimple NestedAspectSimple;

                    // Test: Entity field
                    public readonly Entity Self;

                    // Test: EnabledRef
                    public readonly EnabledRefRO<EcsTestDataEnableable> EcsTestDataEnableable;
                }

                // Test: the aspect generator must support multiple partial declaration of the same aspect.
                public readonly partial struct Aspect2 : global::Unity.Entities.IAspect
                {
                    public int ReadSum()
                    {
                        var v = Data.ValueRO.value +
                            Data2.ValueRO.value0 +
                            Data3.ValueRO.value0 +
                            DataRO.ValueRO.value0;

                        if (DataOptional.IsValid)
                        {
                            v += DataOptional.ValueRO.value0;
                        }
                        return v;
                    }
                    public void WriteAll(int v)
                    {
                        Data.ValueRW.value = v;
                        Data2.ValueRW.value0 = v;
                        Data3.ValueRW.value0 = v;
                        if (DataOptional.IsValid)
                        {
                            DataOptional.ValueRW.value0 = v;
                        }
                    }
                }

                // Test: an aspect declared with the [DisableGeneration] attribute must not be generated.
                [DisableGeneration]
                public partial struct AspectDisableGeneration : IAspect
                {
                    public AspectDisableGeneration CreateAspect(Entity entity, ref SystemState system, bool isReadOnly)
                    {
                        return default;
                    }
                    public RefRW<EcsTestData> Data;
                }
            }";

        return SourceGenTests.Verify<AspectGenerator>(testSource);
    }

    [Fact]
    public Task AspectEFE()
    {
        var testSource = @"
            using Unity.Entities;
            using Unity.Collections;
            using Unity.Entities.Tests;

            public readonly partial struct MyAspectEFE : IAspect
            {
                public readonly RefRW<Unity.Entities.Tests.EcsTestData> Data;
            }
            public readonly partial struct MyAspectEFE2 : IAspect
            {
                public readonly RefRW<Unity.Entities.Tests.EcsTestData2> Data;
            }
            public partial class AspectTestEFESystem : SystemBase
            {
                protected override void OnUpdate()
                {

                    int count = 0;
                    Entities.ForEach((MyAspectEFE myAspect) => { ++count; }).Run();
                    Entities.ForEach((in MyAspectEFE myAspect) => { ++count; }).Run();
                    Entities.ForEach((ref MyAspectEFE myAspect) => { ++count; }).Run();
                    Entities.ForEach((MyAspectEFE myAspect, MyAspectEFE2 myAspect2) => { ++count; }).Run();
                    Entities.ForEach((in MyAspectEFE myAspect, in Unity.Entities.Tests.EcsTestData2 data2) => { ++count; }).Run();
                    Entities.ForEach((Entity e, in EcsTestData data) =>
                    {
                        var a = GetAspect<MyAspectEFE>(e);
                        ++count;
                    }).Run();
                    Entities.ForEach((Entity e, in EcsTestData2 data2) =>
                    {
                        var a = GetAspectRO<MyAspectEFE2>(e);
                        ++count;
                    }).Run();
                }
            }";

        return SourceGenTests.Verify<AspectGenerator>(testSource);
    }
}
