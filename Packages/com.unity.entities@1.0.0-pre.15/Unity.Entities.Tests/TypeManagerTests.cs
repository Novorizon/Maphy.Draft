using System;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Entities.Tests;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

#pragma warning disable 649

[assembly: RegisterGenericComponentType(typeof(TypeManagerTests.GenericComponent<int>))]
[assembly: RegisterGenericComponentType(typeof(TypeManagerTests.GenericComponent<short>))]
[assembly: RegisterGenericComponentType(typeof(TypeManagerTests.GenericComponent<Entity>))]
[assembly: RegisterGenericComponentType(typeof(TypeManagerTests.GenericComponent<BlobAssetReference<float>>))]

namespace Unity.Entities.Tests
{
    partial class TypeManagerTests : ECSTestsFixture
    {
        struct TestType1 : IComponentData
        {
            int empty;
        }
        struct TestTypeWithEntity : IComponentData
        {
            Entity Entity;
        }
        struct TestTypeWithBlobRef : IComponentData
        {
            BlobAssetReference<float> Blobb;
        }

        struct TestType2 : IComponentData
        {
            int empty;
        }

        struct TestTypeWithBool : IComponentData, IEquatable<TestTypeWithBool>
        {
            bool empty;

            public bool Equals(TestTypeWithBool other)
            {
                return other.empty == empty;
            }

            public override int GetHashCode()
            {
                return empty.GetHashCode();
            }
        }

        struct TestTypeWithChar : IComponentData, IEquatable<TestTypeWithChar>
        {
            char empty;

            public bool Equals(TestTypeWithChar other)
            {
                return empty == other.empty;
            }

            public override int GetHashCode()
            {
                return empty.GetHashCode();
            }
        }

        public struct GenericComponent<T> : IComponentData
        {
            T value;
        }

#if !UNITY_DISABLE_MANAGED_COMPONENTS
        public class TestTypeBaseClass : IComponentData, IEquatable<TestTypeBaseClass>
        {
            int empty;
            public bool Equals(TestTypeBaseClass other)
            {
                return empty == other.empty;
            }

            public override int GetHashCode()
            {
                return empty.GetHashCode();
            }
        }

        public class TestTypeSubClass1 : TestTypeBaseClass, IEquatable<TestTypeSubClass1>
        {
            int empty;
            public bool Equals(TestTypeSubClass1 other)
            {
                return empty == other.empty;
            }

            public override int GetHashCode()
            {
                return empty.GetHashCode();
            }

        }

        public class TestTypeSubClass2 : TestTypeBaseClass, IEquatable<TestTypeSubClass2>
        {
            int empty;
            public bool Equals(TestTypeSubClass2 other)
            {
                return empty == other.empty;
            }

            public override int GetHashCode()
            {
                return empty.GetHashCode();
            }

        }

        public class TestTypeSubSubClass1 : TestTypeSubClass1, IEquatable<TestTypeSubSubClass1>
        {
            int empty;
            public bool Equals(TestTypeSubSubClass1 other)
            {
                return empty == other.empty;
            }

            public override int GetHashCode()
            {
                return empty.GetHashCode();
            }

        }

        public class TestTypeSubSubClass2 : TestTypeSubClass2, IEquatable<TestTypeSubSubClass2>
        {
            int empty;
            public bool Equals(TestTypeSubSubClass2 other)
            {
                return empty == other.empty;
            }

            public override int GetHashCode()
            {
                return empty.GetHashCode();
            }

        }

        public class TestTypeSingleClass : IComponentData, IEquatable<TestTypeSingleClass>
        {
            int empty;

            public bool Equals(TestTypeSingleClass other)
            {
                return empty == other.empty;
            }

            public override int GetHashCode()
            {
                return empty.GetHashCode();
            }
        }

        [Test]
        public void TestDescendents_DescendantsOfDescendants()
        {
            // TestTypeBaseClass
            //     TestTypeSubClass1 : TestTypeBaseClass
            //         TestTypeSubSubClass1 : TestTypeSubClass1
            //
            //     TestTypeSubClass2 : TestTypeBaseClass
            //         TestTypeSubSubClass2 : TestTypeSubClass2


            var baseTypeIndex = TypeManager.GetTypeIndex<TestTypeBaseClass>();

            var subClass1TypeIndex = TypeManager.GetTypeIndex<TestTypeSubClass1>();
            var subClass2TypeIndex = TypeManager.GetTypeIndex<TestTypeSubClass2>();

            var subSubClass1TypeIndex = TypeManager.GetTypeIndex<TestTypeSubSubClass1>();
            var subSubClass2TypeIndex = TypeManager.GetTypeIndex<TestTypeSubSubClass2>();

            Assert.IsTrue(TypeManager.HasDescendants(baseTypeIndex));
            Assert.IsTrue(TypeManager.HasDescendants(subClass1TypeIndex));
            Assert.IsTrue(TypeManager.HasDescendants(subClass2TypeIndex));

            Assert.IsFalse(TypeManager.HasDescendants(subSubClass1TypeIndex));
            Assert.IsFalse(TypeManager.HasDescendants(subSubClass2TypeIndex));

            Assert.AreEqual(4, TypeManager.GetDescendantCount(baseTypeIndex));

            Assert.AreEqual(1, TypeManager.GetDescendantCount(subClass1TypeIndex));
            Assert.AreEqual(1, TypeManager.GetDescendantCount(subClass2TypeIndex));

            Assert.AreEqual(0, TypeManager.GetDescendantCount(subSubClass1TypeIndex));
            Assert.AreEqual(0, TypeManager.GetDescendantCount(subSubClass2TypeIndex));

            // Check every type in this tree if types is a descendant of base type
            Assert.IsTrue(TypeManager.IsDescendantOf(baseTypeIndex, baseTypeIndex));
            Assert.IsTrue(TypeManager.IsDescendantOf(subClass1TypeIndex, baseTypeIndex));
            Assert.IsTrue(TypeManager.IsDescendantOf(subClass2TypeIndex, baseTypeIndex));
            Assert.IsTrue(TypeManager.IsDescendantOf(subSubClass1TypeIndex, baseTypeIndex));
            Assert.IsTrue(TypeManager.IsDescendantOf(subSubClass2TypeIndex, baseTypeIndex));

            // Test that the Sub Sub types are descendants of their immediate parent types
            Assert.IsTrue(TypeManager.IsDescendantOf(subSubClass1TypeIndex, subClass1TypeIndex));
            Assert.IsTrue(TypeManager.IsDescendantOf(subSubClass2TypeIndex, subClass2TypeIndex));
            Assert.IsFalse(TypeManager.IsDescendantOf(subSubClass1TypeIndex, subClass2TypeIndex));
            Assert.IsFalse(TypeManager.IsDescendantOf(subSubClass2TypeIndex, subClass1TypeIndex));
        }

        [Test]
        public void TestDescendants_StructType_NoDescendants()
        {
            var structTypeIndex = TypeManager.GetTypeIndex<TestType1>();
            Assert.IsFalse(TypeManager.HasDescendants(structTypeIndex));
            Assert.AreEqual(0, TypeManager.GetDescendantCount(structTypeIndex));
        }

        [Test]
        public void TestDescendants_IsDescendantOfSelf()
        {
            var baseTypeIndex = TypeManager.GetTypeIndex<TestTypeBaseClass>();
            Assert.IsTrue(TypeManager.IsDescendantOf(baseTypeIndex, baseTypeIndex));
        }

        [Test]
        public void TestDescendants_SingleClassType_NoDescendants()
        {
            var singleClassTypeIndex = TypeManager.GetTypeIndex<TestTypeSingleClass>();
            Assert.IsFalse(TypeManager.HasDescendants(singleClassTypeIndex));
            Assert.AreEqual(0, TypeManager.GetDescendantCount(singleClassTypeIndex));
        }
#endif

        [Test]
        public void CreateArchetypes()
        {
            var archetype1 = m_Manager.CreateArchetype(ComponentType.ReadWrite<TestType1>(), ComponentType.ReadWrite<TestType2>());
            var archetype1Same = m_Manager.CreateArchetype(ComponentType.ReadWrite<TestType1>(), ComponentType.ReadWrite<TestType2>());
            Assert.AreEqual(archetype1, archetype1Same);

            var archetype2 = m_Manager.CreateArchetype(ComponentType.ReadWrite<TestType1>());
            var archetype2Same = m_Manager.CreateArchetype(ComponentType.ReadWrite<TestType1>());
            Assert.AreEqual(archetype2Same, archetype2Same);

            Assert.AreNotEqual(archetype1, archetype2);
        }

        [Test]
        public void TestPrimitiveButNotBlittableTypesAllowed()
        {
            Assert.AreEqual(1, TypeManager.GetTypeInfo<TestTypeWithBool>().SizeInChunk);
            Assert.AreEqual(2, TypeManager.GetTypeInfo<TestTypeWithChar>().SizeInChunk);
        }

        // We need to decide whether this should actually be allowed; for now, add a test to make sure
        // we don't break things more than they already are.


        [Test]
        public void TestGenericComponents()
        {
            var index1 = TypeManager.GetTypeIndex<GenericComponent<int>>();
            var index2 = TypeManager.GetTypeIndex<GenericComponent<short>>();

            Assert.AreNotEqual(index1, index2);
        }

        [Test]
        public void TestEntityRef()
        {
            Assert.IsTrue(TypeManager.HasEntityReferences(TypeManager.GetTypeIndex<TestTypeWithEntity>()));
            Assert.IsTrue(TypeManager.HasEntityReferences(TypeManager.GetTypeIndex<GenericComponent<Entity>>()));
            Assert.IsFalse(TypeManager.HasEntityReferences(TypeManager.GetTypeIndex<GenericComponent<short>>()));
            Assert.IsFalse(TypeManager.HasEntityReferences(TypeManager.GetTypeIndex<TestType1>()));
        }

        [Test]
        public void TestBlobRef()
        {
            Assert.IsTrue(TypeManager.GetTypeInfo<TestTypeWithBlobRef>().HasBlobAssetRefs);
            Assert.IsTrue(TypeManager.GetTypeInfo<GenericComponent<BlobAssetReference<float>>>().HasBlobAssetRefs);
            Assert.IsFalse(TypeManager.GetTypeInfo<GenericComponent<short>>().HasBlobAssetRefs);
            Assert.IsFalse(TypeManager.GetTypeInfo<TestType1>().HasBlobAssetRefs);
        }

        [Test]
        public void TestGenericComponentsThrowsOnUnregisteredGeneric()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                TypeManager.GetTypeIndex<GenericComponent<long>>();
            });
        }

        [Test]
        public unsafe void TestConstructComponentFromBuffer()
        {
            EcsTestData td;
            td.value = 42;

            object o = TypeManager.ConstructComponentFromBuffer(TypeManager.GetTypeIndex<EcsTestData>(), &td);
            EcsTestData tr = (EcsTestData) o;
            Assert.AreEqual(td.value, tr.value);
        }

        [InternalBufferCapacity(99)]
        public struct IntElement : IBufferElementData
        {
            public int Value;
        }

        [Test]
        public void BufferTypeClassificationWorks()
        {
            var t = TypeManager.GetTypeInfo<IntElement>();
            Assert.AreEqual(TypeManager.TypeCategory.BufferData, t.Category);
            Assert.AreEqual(99, t.BufferCapacity);
            Assert.AreEqual(UnsafeUtility.SizeOf<BufferHeader>() + 99 * sizeof(int), t.SizeInChunk);
        }

        [Test]
        public void TestTypeManager()
        {
            var entityType = ComponentType.ReadWrite<Entity>();
            var testDataType = ComponentType.ReadWrite<EcsTestData>();

            Assert.AreEqual(entityType, ComponentType.ReadWrite<Entity>());
            Assert.AreEqual(entityType, new ComponentType(typeof(Entity)));
            Assert.AreEqual(testDataType, ComponentType.ReadWrite<EcsTestData>());
            Assert.AreEqual(testDataType, new ComponentType(typeof(EcsTestData)));
            Assert.AreNotEqual(ComponentType.ReadWrite<Entity>(), ComponentType.ReadWrite<EcsTestData>());

            Assert.AreEqual(ComponentType.AccessMode.ReadOnly, ComponentType.ReadOnly<EcsTestData>().AccessModeType);
            Assert.AreEqual(ComponentType.AccessMode.ReadOnly, ComponentType.ReadOnly(typeof(EcsTestData)).AccessModeType);

            Assert.AreEqual(typeof(Entity), entityType.GetManagedType());
        }

        [Test]
        public void TestAlignUp_Align0ToPow2()
        {
            Assert.AreEqual(0, CollectionHelper.Align(0, 1));
            Assert.AreEqual(0, CollectionHelper.Align(0, 2));
            Assert.AreEqual(0, CollectionHelper.Align(0, 4));
            Assert.AreEqual(0, CollectionHelper.Align(0, 8));
            Assert.AreEqual(0, CollectionHelper.Align(0, 16));
            Assert.AreEqual(0, CollectionHelper.Align(0, 32));
            Assert.AreEqual(0, CollectionHelper.Align(0, 64));
            Assert.AreEqual(0, CollectionHelper.Align(0, 128));
        }

        [Test]
        public void TestAlignUp_AlignMultipleOfAlignment()
        {
            Assert.AreEqual(2, CollectionHelper.Align(2, 1));
            Assert.AreEqual(4, CollectionHelper.Align(4, 2));
            Assert.AreEqual(8, CollectionHelper.Align(8, 4));
            Assert.AreEqual(16, CollectionHelper.Align(16, 8));
            Assert.AreEqual(32, CollectionHelper.Align(32, 16));
            Assert.AreEqual(64, CollectionHelper.Align(64, 32));
            Assert.AreEqual(128, CollectionHelper.Align(128, 64));
            Assert.AreEqual(256, CollectionHelper.Align(256, 128));
        }

        [Test]
        public void TestAlignUp_Align1ToPow2()
        {
            Assert.AreEqual(1, CollectionHelper.Align(1, 1));
            Assert.AreEqual(2, CollectionHelper.Align(1, 2));
            Assert.AreEqual(4, CollectionHelper.Align(1, 4));
            Assert.AreEqual(8, CollectionHelper.Align(1, 8));
            Assert.AreEqual(16, CollectionHelper.Align(1, 16));
            Assert.AreEqual(32, CollectionHelper.Align(1, 32));
            Assert.AreEqual(64, CollectionHelper.Align(1, 64));
            Assert.AreEqual(128, CollectionHelper.Align(1, 128));
        }

        [Test]
        public void TestAlignUp_Align3ToPow2()
        {
            Assert.AreEqual(3, CollectionHelper.Align(3, 1));
            Assert.AreEqual(4, CollectionHelper.Align(3, 2));
            Assert.AreEqual(4, CollectionHelper.Align(3, 4));
            Assert.AreEqual(8, CollectionHelper.Align(3, 8));
            Assert.AreEqual(16, CollectionHelper.Align(3, 16));
            Assert.AreEqual(32, CollectionHelper.Align(3, 32));
            Assert.AreEqual(64, CollectionHelper.Align(3, 64));
            Assert.AreEqual(128, CollectionHelper.Align(3, 128));
        }

        [Test]
        public void TestAlignUp_Align15ToPow2()
        {
            Assert.AreEqual(15, CollectionHelper.Align(15, 1));
            Assert.AreEqual(16, CollectionHelper.Align(15, 2));
            Assert.AreEqual(16, CollectionHelper.Align(15, 4));
            Assert.AreEqual(16, CollectionHelper.Align(15, 8));
            Assert.AreEqual(16, CollectionHelper.Align(15, 16));
            Assert.AreEqual(32, CollectionHelper.Align(15, 32));
            Assert.AreEqual(64, CollectionHelper.Align(15, 64));
            Assert.AreEqual(128, CollectionHelper.Align(15, 128));
        }

        [Test]
        public void TestAlignUp_Align63ToPow2()
        {
            Assert.AreEqual(63, CollectionHelper.Align(63, 1));
            Assert.AreEqual(64, CollectionHelper.Align(63, 2));
            Assert.AreEqual(64, CollectionHelper.Align(63, 4));
            Assert.AreEqual(64, CollectionHelper.Align(63, 8));
            Assert.AreEqual(64, CollectionHelper.Align(63, 16));
            Assert.AreEqual(64, CollectionHelper.Align(63, 32));
            Assert.AreEqual(64, CollectionHelper.Align(63, 64));
            Assert.AreEqual(128, CollectionHelper.Align(63, 128));
        }

        [Test]
        public void TestAlignUp_ZeroAlignment()
        {
            for (int value = 0; value < 512; ++value)
            {
                Assert.AreEqual(value, CollectionHelper.Align(value, 0));
            }
        }

        [Test]
        public void TestAlignUlong_Align0ToPow2()
        {
            Assert.AreEqual(0ul, CollectionHelper.Align(0ul, 1ul));
            Assert.AreEqual(0ul, CollectionHelper.Align(0ul, 2ul));
            Assert.AreEqual(0ul, CollectionHelper.Align(0ul, 4ul));
            Assert.AreEqual(0ul, CollectionHelper.Align(0ul, 8ul));
            Assert.AreEqual(0ul, CollectionHelper.Align(0ul, 16ul));
            Assert.AreEqual(0ul, CollectionHelper.Align(0ul, 32ul));
            Assert.AreEqual(0ul, CollectionHelper.Align(0ul, 64ul));
            Assert.AreEqual(0ul, CollectionHelper.Align(0ul, 128ul));
        }

        [Test]
        public void TestAlignUlong_AlignMultipleOfAlignment()
        {
            Assert.AreEqual(2ul, CollectionHelper.Align(2ul, 1ul));
            Assert.AreEqual(4ul, CollectionHelper.Align(4ul, 2ul));
            Assert.AreEqual(8ul, CollectionHelper.Align(8ul, 4ul));
            Assert.AreEqual(16ul, CollectionHelper.Align(16ul, 8ul));
            Assert.AreEqual(32ul, CollectionHelper.Align(32ul, 16ul));
            Assert.AreEqual(64ul, CollectionHelper.Align(64ul, 32ul));
            Assert.AreEqual(128ul, CollectionHelper.Align(128ul, 64ul));
            Assert.AreEqual(256ul, CollectionHelper.Align(256ul, 128ul));
        }

        [Test]
        public void TestAlignUlong_Align1ToPow2()
        {
            Assert.AreEqual(1ul, CollectionHelper.Align(1ul, 1ul));
            Assert.AreEqual(2ul, CollectionHelper.Align(1ul, 2ul));
            Assert.AreEqual(4ul, CollectionHelper.Align(1ul, 4ul));
            Assert.AreEqual(8ul, CollectionHelper.Align(1ul, 8ul));
            Assert.AreEqual(16ul, CollectionHelper.Align(1ul, 16ul));
            Assert.AreEqual(32ul, CollectionHelper.Align(1ul, 32ul));
            Assert.AreEqual(64ul, CollectionHelper.Align(1ul, 64ul));
            Assert.AreEqual(128ul, CollectionHelper.Align(1ul, 128ul));
        }

        [Test]
        public void TestAlignUlong_Align3ToPow2()
        {
            Assert.AreEqual(3ul, CollectionHelper.Align(3ul, 1ul));
            Assert.AreEqual(4ul, CollectionHelper.Align(3ul, 2ul));
            Assert.AreEqual(4ul, CollectionHelper.Align(3ul, 4ul));
            Assert.AreEqual(8ul, CollectionHelper.Align(3ul, 8ul));
            Assert.AreEqual(16ul, CollectionHelper.Align(3ul, 16ul));
            Assert.AreEqual(32ul, CollectionHelper.Align(3ul, 32ul));
            Assert.AreEqual(64ul, CollectionHelper.Align(3ul, 64ul));
            Assert.AreEqual(128ul, CollectionHelper.Align(3ul, 128ul));
        }

        [Test]
        public void TestAlignUlong_Align15ToPow2()
        {
            Assert.AreEqual(15ul, CollectionHelper.Align(15ul, 1ul));
            Assert.AreEqual(16ul, CollectionHelper.Align(15ul, 2ul));
            Assert.AreEqual(16ul, CollectionHelper.Align(15ul, 4ul));
            Assert.AreEqual(16ul, CollectionHelper.Align(15ul, 8ul));
            Assert.AreEqual(16ul, CollectionHelper.Align(15ul, 16ul));
            Assert.AreEqual(32ul, CollectionHelper.Align(15ul, 32ul));
            Assert.AreEqual(64ul, CollectionHelper.Align(15ul, 64ul));
            Assert.AreEqual(128ul, CollectionHelper.Align(15ul, 128ul));
        }

        [Test]
        public void TestAlignUlong_Align63ToPow2()
        {
            Assert.AreEqual(63ul, CollectionHelper.Align(63ul, 1ul));
            Assert.AreEqual(64ul, CollectionHelper.Align(63ul, 2ul));
            Assert.AreEqual(64ul, CollectionHelper.Align(63ul, 4ul));
            Assert.AreEqual(64ul, CollectionHelper.Align(63ul, 8ul));
            Assert.AreEqual(64ul, CollectionHelper.Align(63ul, 16ul));
            Assert.AreEqual(64ul, CollectionHelper.Align(63ul, 32ul));
            Assert.AreEqual(64ul, CollectionHelper.Align(63ul, 64ul));
            Assert.AreEqual(128ul, CollectionHelper.Align(63ul, 128ul));
        }

        [Test]
        public void TestAlignUlong_ZeroAlignment()
        {
            for (ulong value = 0; value < 512ul; ++value)
            {
                Assert.AreEqual(value, CollectionHelper.Align(value, 0ul));
            }
        }

        [UpdateBefore(typeof(PresentationSystemGroup))]
        [UpdateAfter(typeof(InitializationSystemGroup))]
        partial class TestComponentSystem : SystemBase
        {
            protected override void OnUpdate()
            {
            }
        }

        class TestComponentSystemGroup : ComponentSystemGroup
        {
            protected override void OnUpdate()
            {
            }
        }

        [Test]
        public void TestGetSystems()
        {
            // Remove disabled systems since in Entities.Tests we have systems that are intentionally broken and will throw exceptions
            var allSystemTypes = TypeManager.GetSystems(WorldSystemFilterFlags.All & ~WorldSystemFilterFlags.Disabled);
            bool foundTestSystem = false;
            for(int i = 0; i < allSystemTypes.Count;++i)
            {
                var sys = allSystemTypes[i];
                if(sys == typeof(PresentationSystemGroup)) // A group we know will always exist
                {
                    foundTestSystem = true;
                    break;
                }
            }
            Assert.IsTrue(foundTestSystem);
        }

        [Test]
        [Conditional("DEBUG")]
        public void TestGetSystemName()
        {
            Assert.AreEqual("Unity.Entities.Tests.TypeManagerTests+TestComponentSystem", TypeManager.GetSystemName(typeof(TestComponentSystem)));
        }


        // Warnings will be thrown about redundant attributes since all systems are disabled in this assembly
        // [DisableAutoCreation]
        partial class DisabledSystem : SystemBase
        {
            protected override void OnUpdate()
            {
            }
        }

        partial class ChildOfDisabledSystem : SystemBase
        {
            protected override void OnUpdate()
            {
            }
        }

        [Test]
        public void TestGetSystemAttributes()
        {
            var updateBeforeAttributes = TypeManager.GetSystemAttributes(typeof(TestComponentSystem), typeof(UpdateBeforeAttribute));
            Assert.AreEqual(1, updateBeforeAttributes.Length);
            Assert.AreEqual(typeof(PresentationSystemGroup), ((UpdateBeforeAttribute)updateBeforeAttributes[0]).SystemType);

            var updateAfterAttributes = TypeManager.GetSystemAttributes(typeof(TestComponentSystem), typeof(UpdateAfterAttribute));
            Assert.AreEqual(1, updateAfterAttributes.Length);
            Assert.AreEqual(typeof(InitializationSystemGroup), ((UpdateAfterAttribute)updateAfterAttributes[0]).SystemType);

            var disableAttributes = TypeManager.GetSystemAttributes(typeof(DisabledSystem), typeof(DisableAutoCreationAttribute));
            Assert.AreEqual(1, disableAttributes.Length);

            // Annoyingly we cannot test this without adding a new dependent assembly to this test assembly. This is because all systems are disabled (rightfully so)
            // for this test assembly via [assembly: DisableAutoCreation] so we cannot check that a child system defined in this assembly is _not_ disabled
            //var inheritedDisableAttributes = TypeManager.GetSystemAttributes(typeof(ChildOfDisabledSystem), typeof(DisableAutoCreationAttribute));
            //Assert.AreEqual(0, inheritedDisableAttributes.Length); // we should not inherit DisableAutoCreation attributes
        }

        [Test]
        public void TestIsComponentSystemGroup()
        {
            Assert.IsTrue(!TypeManager.IsSystemAGroup(typeof(TestComponentSystem)));
            Assert.IsTrue(TypeManager.IsSystemAGroup(typeof(TestComponentSystemGroup)));
        }

        [WorldSystemFilter(WorldSystemFilterFlags.Default)]
        partial class DefaultFilteredSystem : SystemBase{ protected override void OnUpdate() { } }
        [WorldSystemFilter(WorldSystemFilterFlags.ProcessAfterLoad)]
        partial class ProcessAfterLoadFilteredSystem : SystemBase { protected override void OnUpdate() { } }

        int ValidateFilterFlags(WorldSystemFilterFlags expectedFilterFlags, WorldSystemFilterFlags requiredFlags = 0)
        {
            Assert.IsTrue(expectedFilterFlags != WorldSystemFilterFlags.All);
            if ((expectedFilterFlags & WorldSystemFilterFlags.Default) != 0)
            {
                expectedFilterFlags &= ~WorldSystemFilterFlags.Default;
                expectedFilterFlags |= WorldSystemFilterFlags.LocalSimulation | WorldSystemFilterFlags.Presentation;
            }

            var filteredTypes = TypeManager.GetSystems(expectedFilterFlags, requiredFlags);
            requiredFlags &= ~WorldSystemFilterFlags.Default;
            foreach(var t in filteredTypes)
            {
                var actualFilterFlags = TypeManager.GetSystemFilterFlags(t);

                Assert.IsTrue((expectedFilterFlags & actualFilterFlags) != 0, $"Flags for system {t} do not match");
                Assert.GreaterOrEqual((int)actualFilterFlags, (int)requiredFlags, $"Actual Flags for system {t} should be greater than or equal to required flags");
            }

            return filteredTypes.Count;
        }

        [Test]
        public void GetSystemsWorldSystemFilterFlags()
        {
            var allTypesCount = TypeManager.GetSystems(WorldSystemFilterFlags.All & ~WorldSystemFilterFlags.Disabled).Count;

            var numDefaultSystems = ValidateFilterFlags(WorldSystemFilterFlags.Default, 0);
            var numProcessAfterLoadSystems = ValidateFilterFlags(WorldSystemFilterFlags.ProcessAfterLoad, WorldSystemFilterFlags.ProcessAfterLoad);
            var numCombinedSystems = ValidateFilterFlags(WorldSystemFilterFlags.Default | WorldSystemFilterFlags.ProcessAfterLoad);
            Assert.AreEqual(numCombinedSystems, numDefaultSystems + numProcessAfterLoadSystems);
            Assert.IsTrue(numCombinedSystems <= allTypesCount);
        }

        [WorldSystemFilter(WorldSystemFilterFlags.Default, WorldSystemFilterFlags.LocalSimulation)]
        partial class Test_FirstLevelGroup : ComponentSystemGroup { }
        [WorldSystemFilter(WorldSystemFilterFlags.Default, WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Presentation)]
        [UpdateInGroup(typeof(Test_FirstLevelGroup))]
        partial class Test_SecondLevelGroup : ComponentSystemGroup { }
        [UpdateInGroup(typeof(Test_FirstLevelGroup))]
        partial class Test_FirstLevelSystem : SystemBase { protected override void OnUpdate() { } }
        [UpdateInGroup(typeof(Test_SecondLevelGroup))]
        partial class Test_SecondLevelSystem : SystemBase { protected override void OnUpdate() { } }
        [WorldSystemFilter(WorldSystemFilterFlags.Default | WorldSystemFilterFlags.ClientSimulation)]
        [UpdateInGroup(typeof(Test_SecondLevelGroup))]
        partial class Test_SecondLevelExtendedSystem : SystemBase { protected override void OnUpdate() { } }
        [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
        [UpdateInGroup(typeof(Test_SecondLevelGroup))]
        partial class Test_SecondLevelExplicitSystem : SystemBase { protected override void OnUpdate() { } }

        [Test]
        public void GetDefaultWorldSystemFilterFlagsFromGroup()
        {
            Assert.AreEqual(WorldSystemFilterFlags.LocalSimulation, TypeManager.GetSystemFilterFlags(typeof(Test_FirstLevelSystem)));
            Assert.AreEqual(WorldSystemFilterFlags.LocalSimulation | WorldSystemFilterFlags.Presentation, TypeManager.GetSystemFilterFlags(typeof(Test_SecondLevelSystem)));
            Assert.AreEqual(WorldSystemFilterFlags.LocalSimulation | WorldSystemFilterFlags.Presentation | WorldSystemFilterFlags.ClientSimulation, TypeManager.GetSystemFilterFlags(typeof(Test_SecondLevelExtendedSystem)));
            Assert.AreEqual(WorldSystemFilterFlags.ClientSimulation, TypeManager.GetSystemFilterFlags(typeof(Test_SecondLevelExplicitSystem)));
        }

        [CreateAfter(typeof(Test_CreateOrder_B))]
        [WorldSystemFilter(WorldSystemFilterFlags.Editor)]
        partial class Test_CreateOrder_C : SystemBase { protected override void OnUpdate() { } }
        [WorldSystemFilter(WorldSystemFilterFlags.Editor)]
        partial class Test_CreateOrder_B : SystemBase { protected override void OnUpdate() { } }
        [CreateBefore(typeof(Test_CreateOrder_B))]
        [WorldSystemFilter(WorldSystemFilterFlags.Editor)]
        partial class Test_CreateOrder_A : SystemBase { protected override void OnUpdate() { } }

        [Test]
        [Ignore("Fix Filter Flags to allow disabled systems to be found outside of All queries - DOTS-5966")]
        public void GetSystemsRespectsCreateBeforeCreateAfter()
        {
            // All systems in the test assembly are disabled by default. If we fetch disabled systems we will trip on intentionally
            // broken systems, so we instead stuff our disabled systems into the Editor world filter which will exclude the broken systems
            var allTypes = new List<Type>(TypeManager.GetSystems(WorldSystemFilterFlags.All, WorldSystemFilterFlags.Editor | WorldSystemFilterFlags.Disabled));

            var indexOfA = allTypes.IndexOf(typeof(Test_CreateOrder_A));
            var indexOfB = allTypes.IndexOf(typeof(Test_CreateOrder_B));
            var indexOfC = allTypes.IndexOf(typeof(Test_CreateOrder_C));

            Assert.Less(indexOfA, indexOfB);
            Assert.Less(indexOfB, indexOfC);
        }
#if !UNITY_DOTSRUNTIME // No reflection support in TypeManager in DOTS Runtime even without TinyBCL; no UnityEngine in DOTS Runtime
        [DisableAutoTypeRegistration]
        struct NonBlittableComponentData : IComponentData
        {
            string empty;
        }

        [DisableAutoTypeRegistration]
        struct NonBlittableComponentData2 : IComponentData
        {
            IComponentData empty;
        }

        interface InterfaceComponentData : IComponentData
        {
        }

        [DisableAutoTypeRegistration]
        struct NonBlittableBuffer : IBufferElementData
        {
            string empty;
        }

        class ClassBuffer : IBufferElementData
        {
        }

        interface InterfaceBuffer : IBufferElementData
        {
        }

        class ClassShared : ISharedComponentData
        {
        }

        interface InterfaceShared : ISharedComponentData
        {
        }

        [DisableAutoTypeRegistration]
        struct Cleanup : ICleanupComponentData, IEnableableComponent
        {

        }

        [DisableAutoTypeRegistration]
        struct Shared : ISharedComponentData, IEnableableComponent
        {
            public int mVal;
        }

        [DisableAutoTypeRegistration]
        struct EmptyBufferComponent : IBufferElementData
        {

        }

        [DisableAutoTypeRegistration]
        struct EmptySharedComponent : ISharedComponentData
        {

        }

        [TestCase(typeof(InterfaceComponentData), @"\binterface\b", TestName = "Interface implementing IComponentData")]
        [TestCase(typeof(NonBlittableComponentData), @"\bblittable\b", TestName = "Non-blittable component data (string)")]
        [TestCase(typeof(NonBlittableComponentData2), @"\bblittable\b", TestName = "Non-blittable component data (interface)")]

        [TestCase(typeof(InterfaceBuffer), @"\binterface\b", TestName = "Interface implementing IBufferElementData")]
        [TestCase(typeof(ClassBuffer), @"\b(struct|class)\b", TestName = "Class implementing IBufferElementData")]
        [TestCase(typeof(NonBlittableBuffer), @"\bblittable\b", TestName = "Non-blittable IBufferElementData")]

        [TestCase(typeof(InterfaceShared), @"\binterface\b", TestName = "Interface implementing ISharedComponentData")]
        [TestCase(typeof(ClassShared), @"\b(struct|class)\b", TestName = "Class implementing ISharedComponentData")]

        [TestCase(typeof(Cleanup), @"\bdisabled\b", TestName = "Implements both ICleanupComponentData and IEnableableComponent")]
        [TestCase(typeof(Shared), @"\bdisabled\b", TestName = "Implements both ISharedComponentData and IEnableableComponent")]

        [TestCase(typeof(float), @"\b(not .*|in)valid\b", TestName = "Not valid component type")]

        [TestCase(typeof(EmptyBufferComponent), @"\b(is .*|in)valid\b", TestName = "IBufferElementData types cannot be empty")]
        [TestCase(typeof(EmptySharedComponent), @"\b(is .*|in)valid\b", TestName = "ISharedComponentData types cannot be empty")]
        [TestRequiresDotsDebugOrCollectionChecks("Test requires component type safety checks")]
        public void BuildComponentType_ThrowsArgumentException_WithExpectedFailures(Type type, string keywordPattern)
        {
            Assert.That(
                () => TypeManager.BuildComponentType(type, new Dictionary<Type, ulong>(), new HashSet<Type>()),
                Throws.ArgumentException.With.Message.Matches(keywordPattern)
            );
        }

        struct UnmanagedSharedComponent : ISharedComponentData
        {
            int a;
        }

        struct ManagedSharedComponent : ISharedComponentData, IEquatable<ManagedSharedComponent>
        {
            private string a;

            public bool Equals(ManagedSharedComponent other)
            {
                return a == other.a;
            }

            public override bool Equals(object obj)
            {
                return obj is ManagedSharedComponent other && Equals(other);
            }

            public override int GetHashCode()
            {
                return (a != null ? a.GetHashCode() : 0);
            }
        }

        [Test]
        public void SharedComponent_ManagedFlagCorrectlySet()
        {
            Assert.IsTrue(TypeManager.IsManagedType(TypeManager.GetTypeIndex<ManagedSharedComponent>()));
            Assert.IsTrue(TypeManager.IsSharedComponentType(TypeManager.GetTypeIndex<ManagedSharedComponent>()));
            Assert.IsTrue(TypeManager.IsManagedSharedComponent(TypeManager.GetTypeIndex<ManagedSharedComponent>()));


            Assert.IsFalse(TypeManager.IsManagedType(TypeManager.GetTypeIndex<UnmanagedSharedComponent>()));
            Assert.IsTrue(TypeManager.IsSharedComponentType(TypeManager.GetTypeIndex<UnmanagedSharedComponent>()));
            Assert.IsFalse(TypeManager.IsManagedSharedComponent(TypeManager.GetTypeIndex<UnmanagedSharedComponent>()));
        }

        struct OptionalEnableableComponent : IComponentData, IEnableableComponent
        {

        }

        [Test]
        public void OptionalComponent_IsEnableableFlagCorrectlySet()
        {
            Assert.IsTrue(TypeManager.IsEnableable(TypeManager.GetTypeIndex<OptionalEnableableComponent>()));
        }

        [TestCase(typeof(UnityEngine.Transform))]
        [TestCase(typeof(TypeManagerTests))]
        [TestRequiresDotsDebugOrCollectionChecks("Test requires component type safety checks")]
        public void BuildComponentType_WithClass_WhenUnityEngineObjectTypeIsNull_ThrowsArgumentException(Type type)
        {
            var componentType = TypeManager.UnityEngineObjectType;
            TypeManager.UnityEngineObjectType = null;
            try
            {
                Assert.That(
                    () => TypeManager.BuildComponentType(type, new Dictionary<Type, ulong>(), new HashSet<Type>()),
                    Throws.ArgumentException.With.Message.Matches($"\\bregister\\b.*\\b{nameof(TypeManager.UnityEngineObjectType)}\\b")
                );
            }
            finally
            {
                TypeManager.UnityEngineObjectType = componentType;
            }
        }

        [Test]
        [TestRequiresDotsDebugOrCollectionChecks("Test requires component type safety checks")]
        public void BuildComponentType_WithNonComponent_WhenUnityEngineObjectTypeIsCorrect_ThrowsArgumentException()
        {
            var componentType = TypeManager.UnityEngineObjectType;
            TypeManager.UnityEngineObjectType = typeof(UnityEngine.Component);
            try
            {
                var type = typeof(TypeManagerTests);
                Assert.That(
                    () => TypeManager.BuildComponentType(type, new Dictionary<Type, ulong>(), new HashSet<Type>()),
                    Throws.ArgumentException.With.Message.Matches($"\\bmust inherit {typeof(UnityEngine.Component)}\\b")
                );
            }
            finally
            {
                TypeManager.UnityEngineObjectType = componentType;
            }
        }

        [Test]
        public void BuildComponentType_WithComponent_WhenUnityEngineObjectTypeIsCorrect_DoesNotThrowException()
        {
            var componentType = TypeManager.UnityEngineObjectType;
            TypeManager.UnityEngineObjectType = typeof(UnityEngine.Component);
            try
            {
                TypeManager.BuildComponentType(typeof(UnityEngine.Transform), new Dictionary<Type, ulong>(), new HashSet<Type>());
            }
            finally
            {
                TypeManager.UnityEngineObjectType = componentType;
            }
        }

        [TestCase(null)]
        [TestCase(typeof(TestType1))]
        [TestCase(typeof(InterfaceShared))]
        [TestCase(typeof(ClassShared))]
        [TestCase(typeof(UnityEngine.Transform))]
        public void RegisterUnityEngineObjectType_WithWrongType_ThrowsArgumentException(Type type)
        {
            Assert.Throws<ArgumentException>(() => TypeManager.RegisterUnityEngineObjectType(type));
        }

        [Test]
        public void IsAssemblyReferencingEntities()
        {
            Assert.IsFalse(TypeManager.IsAssemblyReferencingEntities(typeof(UnityEngine.GameObject).Assembly));
            Assert.IsFalse(TypeManager.IsAssemblyReferencingEntities(typeof(System.Collections.Generic.List<>).Assembly));
            Assert.IsFalse(TypeManager.IsAssemblyReferencingEntities(typeof(Collections.NativeList<>).Assembly));

            Assert.IsTrue(TypeManager.IsAssemblyReferencingEntities(typeof(IComponentData).Assembly));
            Assert.IsTrue(TypeManager.IsAssemblyReferencingEntities(typeof(EcsTestData).Assembly));
        }

        [Test]
        public void IsAssemblyReferencingEntitiesOrUnityEngine()
        {
            TypeManager.IsAssemblyReferencingEntitiesOrUnityEngine(typeof(UnityEngine.GameObject).Assembly, out var gameObjectAsmRefersToEntities, out var gameObjectAsmRefersToUnityEngine);
            Assert.IsFalse(gameObjectAsmRefersToEntities);
            Assert.IsTrue(gameObjectAsmRefersToUnityEngine);

            TypeManager.IsAssemblyReferencingEntitiesOrUnityEngine(typeof(System.Collections.Generic.List<>).Assembly, out var listAsmRefersToEntities, out var listAsmRefersToUnityEngine);
            Assert.IsFalse(listAsmRefersToEntities);
            Assert.IsFalse(listAsmRefersToUnityEngine);

            TypeManager.IsAssemblyReferencingEntitiesOrUnityEngine(typeof(Collections.NativeList<>).Assembly, out var nativeListAsmRefersToEntities, out var nativeListAsmRefersToUnityEngine);
            Assert.IsFalse(nativeListAsmRefersToEntities);
            Assert.IsTrue(nativeListAsmRefersToUnityEngine);

            TypeManager.IsAssemblyReferencingEntitiesOrUnityEngine(typeof(IComponentData).Assembly, out var icomponentAsmRefersToEntities, out var icomponentAsmRefersToUnityEngine);
            Assert.IsTrue(icomponentAsmRefersToEntities);
            Assert.IsTrue(icomponentAsmRefersToUnityEngine);

            TypeManager.IsAssemblyReferencingEntitiesOrUnityEngine(typeof(EcsTestData).Assembly, out var ecsTestDataAsmRefersToEntities, out var ecsTestDataAsmRefersToUnityEngine);
            Assert.IsTrue(ecsTestDataAsmRefersToEntities);
            Assert.IsTrue(ecsTestDataAsmRefersToUnityEngine);
        }

        partial class TestSystem : SystemBase
        {

            protected override void OnCreate()
            {
            }

            protected override void OnUpdate()
            {
#if !ENABLE_TRANSFORM_V1
                Entities.ForEach((Entity e, ref LocalTransform t) =>
#else
                Entities.ForEach((Entity e, ref Translation t) =>
#endif
                {
                }).Run();
            }
        }

        [DisableAutoTypeRegistration]
        public struct UnregisteredComponent : IComponentData
        {
            public int Int;
        }

        [Test]
        public unsafe void TypeInfo_EntityReferenceOffsets_AreSortedAndCorrect()
        {
            var typeInfo = TypeManager.GetTypeInfo<EcsTestDataEntity2>();
            Assert.IsTrue(TypeManager.HasEntityReferences(TypeManager.GetTypeIndex<EcsTestDataEntity2>()));
            Assert.AreEqual(2, typeInfo.EntityOffsetCount);
            var offsets = TypeManager.GetEntityOffsets(typeInfo);
            int offsetA = offsets[0].Offset;
            int offsetB = offsets[1].Offset;
            Assert.Less(offsetA, offsetB, "Entity offsets are assumed to be sorted.");
            Assert.AreEqual(UnsafeUtility.GetFieldOffset(typeof(EcsTestDataEntity2).GetField(nameof(EcsTestDataEntity2.value1))), offsetA);
            Assert.AreEqual(UnsafeUtility.GetFieldOffset(typeof(EcsTestDataEntity2).GetField(nameof(EcsTestDataEntity2.value2))), offsetB);
        }

        [Test]
        public unsafe void TypeInfo_BlobAssetReferenceOffsets_AreSortedAndCorrect()
        {
            var typeInfo = TypeManager.GetTypeInfo<EcsTestDataBlobAssetRef2>();
            Assert.AreEqual(2, typeInfo.BlobAssetRefOffsetCount);
            var offsets = TypeManager.GetBlobAssetRefOffsets(typeInfo);
            int offsetA = offsets[0].Offset;
            int offsetB = offsets[1].Offset;
            Assert.Less(offsetA, offsetB, "BlobAssetOffsets offsets are assumed to be sorted.");
            Assert.AreEqual(UnsafeUtility.GetFieldOffset(typeof(EcsTestDataBlobAssetRef2).GetField(nameof(EcsTestDataBlobAssetRef2.value))), offsetA);
            Assert.AreEqual(UnsafeUtility.GetFieldOffset(typeof(EcsTestDataBlobAssetRef2).GetField(nameof(EcsTestDataBlobAssetRef2.value2))), offsetB);
        }

        [TypeManager.TypeOverrides(hasNoEntityReferences: true, hasNoBlobReferences: true)]
        public struct TypeOverridesBlobEntityUnmanaged : IComponentData
        {
            public Entity entity;
            public BlobAssetReference<int> blob;
        }

        [TypeManager.TypeOverrides(hasNoEntityReferences: true, hasNoBlobReferences: false)]
        public struct TypeOverridesEntityUnmanaged : IComponentData
        {
            public Entity entity;
        }

        [TypeManager.TypeOverrides(hasNoEntityReferences: false, hasNoBlobReferences: true)]
        public struct TypeOverridesBlobUnmanaged : IComponentData
        {
            public BlobAssetReference<int> blob;
        }

        [TypeManager.TypeOverrides(hasNoEntityReferences: false, hasNoBlobReferences: true)]
        public struct TypeOverridesNoBlobUnmanaged : IComponentData
        {
            public Entity entity;
        }

        [TypeManager.TypeOverrides(hasNoEntityReferences: true, hasNoBlobReferences: false)]
        public struct TypeOverridesNoEntityUnmanaged : IComponentData
        {
            public BlobAssetReference<int> blob;
        }

        [TypeManager.TypeOverrides(hasNoEntityReferences: false, hasNoBlobReferences: false)]
        public struct TypeOverridesNoBlobNoEntityUnmanaged : IComponentData
        {
            public float data;
        }

        [Test]
        public void TypeOverrideWorks_Unmanaged()
        {
            var typeOverridesBlobInfo = TypeManager.GetTypeInfo<TypeOverridesBlobUnmanaged>();
            Assert.IsFalse(TypeManager.HasEntityReferences(typeOverridesBlobInfo.TypeIndex));
            Assert.IsFalse(typeOverridesBlobInfo.HasBlobAssetRefs);

            var typeOverridesEntityInfo = TypeManager.GetTypeInfo<TypeOverridesEntityUnmanaged>();
            Assert.IsFalse(TypeManager.HasEntityReferences(typeOverridesEntityInfo.TypeIndex));
            Assert.IsFalse(typeOverridesEntityInfo.HasBlobAssetRefs);

            var typeOverridesBlobEntityInfo = TypeManager.GetTypeInfo<TypeOverridesBlobEntityUnmanaged>();
            Assert.IsFalse(TypeManager.HasEntityReferences(typeOverridesBlobEntityInfo.TypeIndex));
            Assert.IsFalse(typeOverridesBlobEntityInfo.HasBlobAssetRefs);

            var typeOverridesNoBlobInfo = TypeManager.GetTypeInfo<TypeOverridesNoBlobUnmanaged>();
            Assert.IsTrue(TypeManager.HasEntityReferences(typeOverridesNoBlobInfo.TypeIndex));
            Assert.IsFalse(typeOverridesNoBlobInfo.HasBlobAssetRefs);

            var typeOverridesNoEntityInfo = TypeManager.GetTypeInfo<TypeOverridesNoEntityUnmanaged>();
            Assert.IsFalse(TypeManager.HasEntityReferences(typeOverridesNoEntityInfo.TypeIndex));
            Assert.IsTrue(typeOverridesNoEntityInfo.HasBlobAssetRefs);

            var typeOverridesNoBlobNoEntityInfo = TypeManager.GetTypeInfo<TypeOverridesNoBlobNoEntityUnmanaged>();
            Assert.IsFalse(TypeManager.HasEntityReferences(typeOverridesNoBlobNoEntityInfo.TypeIndex));
            Assert.IsFalse(typeOverridesNoBlobNoEntityInfo.HasBlobAssetRefs);
        }

        [DisableAutoTypeRegistration]
        struct NativeContainerComponent : IComponentData
        {
            NativeArray<int> data;
        }
        [DisableAutoTypeRegistration]
        struct NestedNativeContainerComponent : IComponentData
        {
            NativeArray<NativeArray<int>> data;
        }

        [Test]
        public void TestNativeContainersWorks()
        {
            Assert.DoesNotThrow(
                () => TypeManager.BuildComponentType(typeof(NativeContainerComponent), new Dictionary<Type, ulong>(), new HashSet<Type>()));
        }

        [Test]
        [TestRequiresDotsDebugOrCollectionChecks("Test requires component type safety checks")]
        public void TestNestedNativeContainersFails()
        {
            Assert.Throws<ArgumentException>(
                () => TypeManager.BuildComponentType(typeof(NestedNativeContainerComponent), new Dictionary<Type, ulong>(), new HashSet<Type>()));
        }
#endif

#if !UNITY_DISABLE_MANAGED_COMPONENTS
        interface IBaseInterface
        {
        }
        class BaseClass
        {
        }

        sealed class SealedNothing
        {
            private float value;
        }

        sealed class ClassWithBlobAndEntity : BaseClass
        {
            BlobAssetReference<float> blob;
            Entity entity;
        }

        class TestNoRefManaged : IComponentData
        {
            float3 blah;
            SealedNothing nothing;
            float[] floatArray;
            SealedNothing[] nothingArray;
        }

        class TestEmbedInterface : IComponentData
        {
            private IBaseInterface baseInterface;
        }

        class TestTestEmbedBaseClass : IComponentData
        {
            private BaseClass baseClass;
        }

        class TestHasBlobRefAndEntityRef : IComponentData
        {
            ClassWithBlobAndEntity reference;
        }
        class TestEntityArray : IComponentData
        {
            Entity[] entityArray;
        }
        class TestBlobArray : IComponentData
        {
            BlobAssetReference<float>[] entityArray;
        }
        class TestEntityInClass : IComponentData
        {
            Entity entityRef;
        }

        [TypeManager.TypeOverrides(hasNoEntityReferences:true, hasNoBlobReferences:true)]
        public sealed class TypeOverridesBlobEntity: IComponentData
        {
            public Entity entity;
            public BlobAssetReference<int> blob;
        }

        [TypeManager.TypeOverrides(hasNoEntityReferences:true, hasNoBlobReferences:false)]
        public sealed class TypeOverridesEntity : IComponentData
        {
            public Entity entity;
        }

        [TypeManager.TypeOverrides(hasNoEntityReferences:false, hasNoBlobReferences:true)]
        public sealed class TypeOverridesBlob: IComponentData
        {
            public BlobAssetReference<int> blob;
        }

        [TypeManager.TypeOverrides(hasNoEntityReferences:false, hasNoBlobReferences:true)]
        public sealed class TypeOverridesNoBlob: IComponentData
        {
            public Entity entity;
        }

        [TypeManager.TypeOverrides(hasNoEntityReferences:true, hasNoBlobReferences:false)]
        public sealed class TypeOverridesNoEntity : IComponentData
        {
            public BlobAssetReference<int> blob;
        }

        [TypeManager.TypeOverrides(hasNoEntityReferences:false, hasNoBlobReferences:false)]
        public sealed class TypeOverridesNoBlobNoEntity: IComponentData
        {
            public string data;
        }

        [Test]
        public void TypeOverrideWorks_ManagedComponents()
        {
            var typeOverridesBlobInfo = TypeManager.GetTypeInfo<TypeOverridesBlob>();
            Assert.IsFalse(TypeManager.HasEntityReferences(typeOverridesBlobInfo.TypeIndex));
            Assert.IsFalse(typeOverridesBlobInfo.HasBlobAssetRefs);

            var typeOverridesEntityInfo = TypeManager.GetTypeInfo<TypeOverridesEntity>();
            Assert.IsFalse(TypeManager.HasEntityReferences(typeOverridesEntityInfo.TypeIndex));
            Assert.IsFalse(typeOverridesEntityInfo.HasBlobAssetRefs);

            var typeOverridesBlobEntityInfo = TypeManager.GetTypeInfo<TypeOverridesBlobEntity>();
            Assert.IsFalse(TypeManager.HasEntityReferences(typeOverridesBlobEntityInfo.TypeIndex));
            Assert.IsFalse(typeOverridesBlobEntityInfo.HasBlobAssetRefs);

            var typeOverridesNoBlobInfo = TypeManager.GetTypeInfo<TypeOverridesNoBlob>();
            Assert.IsTrue(TypeManager.HasEntityReferences(typeOverridesNoBlobInfo.TypeIndex));
            Assert.IsFalse(typeOverridesNoBlobInfo.HasBlobAssetRefs);

            var typeOverridesNoEntityInfo = TypeManager.GetTypeInfo<TypeOverridesNoEntity>();
            Assert.IsFalse(TypeManager.HasEntityReferences(typeOverridesNoEntityInfo.TypeIndex));
            Assert.IsTrue(typeOverridesNoEntityInfo.HasBlobAssetRefs);

            var typeOverridesNoBlobNoEntityInfo = TypeManager.GetTypeInfo<TypeOverridesNoBlobNoEntity>();
            Assert.IsFalse(TypeManager.HasEntityReferences(typeOverridesNoBlobNoEntityInfo.TypeIndex));
            Assert.IsFalse(typeOverridesNoBlobNoEntityInfo.HasBlobAssetRefs);
        }

        [Test]
        public void TestHasEntityReferencedManaged_Managed()
        {
            Assert.IsFalse(TypeManager.HasEntityReferences(TypeManager.GetTypeIndex<TestNoRefManaged>()));
            Assert.IsFalse(TypeManager.HasEntityReferences(TypeManager.GetTypeIndex<TestBlobArray>()));

            Assert.IsTrue(TypeManager.HasEntityReferences(TypeManager.GetTypeIndex<TestEmbedInterface>()));
            Assert.IsTrue(TypeManager.HasEntityReferences(TypeManager.GetTypeIndex<TestTestEmbedBaseClass>()));
            Assert.IsTrue(TypeManager.HasEntityReferences(TypeManager.GetTypeIndex<TestHasBlobRefAndEntityRef>()));
            Assert.IsTrue(TypeManager.HasEntityReferences(TypeManager.GetTypeIndex<TestEntityInClass>()));
            Assert.IsTrue(TypeManager.HasEntityReferences(TypeManager.GetTypeIndex<TestEntityArray>()));
        }

        [Test]
        public void TestHasBlobReferencesManaged()
        {
            Assert.IsFalse(TypeManager.GetTypeInfo<TestNoRefManaged>().HasBlobAssetRefs);
            Assert.IsFalse(TypeManager.GetTypeInfo<TestEntityInClass>().HasBlobAssetRefs);
            Assert.IsFalse(TypeManager.GetTypeInfo<TestEntityArray>().HasBlobAssetRefs);

            Assert.IsTrue(TypeManager.GetTypeInfo<TestEmbedInterface>().HasBlobAssetRefs);
            Assert.IsTrue(TypeManager.GetTypeInfo<TestTestEmbedBaseClass>().HasBlobAssetRefs);
            Assert.IsTrue(TypeManager.GetTypeInfo<TestHasBlobRefAndEntityRef>().HasBlobAssetRefs);
            Assert.IsTrue(TypeManager.GetTypeInfo<TestHasBlobRefAndEntityRef>().HasBlobAssetRefs);
        }
#endif


        // Tests including Unityengine, or reflection
#if !UNITY_DISABLE_MANAGED_COMPONENTS && !UNITY_DOTSRUNTIME

#pragma warning disable 649
        class TestScriptableObjectWithFields : UnityEngine.ScriptableObject
        {
            public int Value;
        }

        class ComponentWithScriptableObjectReference : IComponentData
        {
            public TestScriptableObjectWithFields Value;
        }
#pragma warning restore 649
        [Test]
        public void TypeManagerGetHashCode_WithNullScriptableObject_DoesNotThrow()
        {
            var component = new ComponentWithScriptableObjectReference();
            Assert.DoesNotThrow(() =>
            {
                TypeManager.GetHashCode(component, TypeManager.GetTypeIndex<ComponentWithScriptableObjectReference>());
            });
        }

        [DisableAutoTypeRegistration]
        class ArrayNativeContainerComponent : IComponentData
        {
            NativeArray<int>[] data;
        }
        [DisableAutoTypeRegistration]
        class NestedArrayNativeContainerComponent : IComponentData
        {
            NativeArray<NativeArray<int>>[] data;
        }

        [Test]
        public void TestArrayNativeContainersWorks()
        {
            Assert.DoesNotThrow(
                () => TypeManager.BuildComponentType(typeof(ArrayNativeContainerComponent), new Dictionary<Type, ulong>(), new HashSet<Type>()));
        }

        [Test]
        [TestRequiresDotsDebugOrCollectionChecks("Test requires component type safety checks")]
        public void TestNestedArrayNativeContainersFails()
        {
            Assert.Throws<ArgumentException>(
                () => TypeManager.BuildComponentType(typeof(NestedArrayNativeContainerComponent), new Dictionary<Type, ulong>(), new HashSet<Type>()));
        }
#endif
    }
}
