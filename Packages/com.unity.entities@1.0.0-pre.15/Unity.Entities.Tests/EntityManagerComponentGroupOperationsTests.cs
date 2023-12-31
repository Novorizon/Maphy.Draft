using System;
using System.Runtime.InteropServices;
using NUnit.Framework;
using Unity.Collections;

namespace Unity.Entities.Tests
{
    class EntityManagerComponentGroupOperationsTests : ECSTestsFixture
    {
        [Test]
        public void AddRemoveChunkComponentWithGroupWorks()
        {
            var metaChunkGroup = m_Manager.CreateEntityQuery(typeof(ChunkHeader));

            var entity1 = m_Manager.CreateEntity(typeof(EcsTestData));
            var entity2 = m_Manager.CreateEntity(typeof(EcsTestData), typeof(EcsTestData2));
            var entity3 = m_Manager.CreateEntity(typeof(EcsTestData2));

            var group1 = m_Manager.CreateEntityQuery(ComponentType.ReadWrite<EcsTestData>());

            m_Manager.AddChunkComponentData(group1, new EcsTestData3(7));

            Assert.IsTrue(m_Manager.HasComponent(entity1, ComponentType.ChunkComponent<EcsTestData3>()));
            var val1 = m_Manager.GetChunkComponentData<EcsTestData3>(entity1).value0;
            Assert.AreEqual(7, val1);

            Assert.IsTrue(m_Manager.HasComponent(entity2, ComponentType.ChunkComponent<EcsTestData3>()));
            var val2 = m_Manager.GetChunkComponentData<EcsTestData3>(entity2).value0;
            Assert.AreEqual(7, val2);

            Assert.IsFalse(m_Manager.HasComponent(entity3, ComponentType.ChunkComponent<EcsTestData3>()));

            Assert.AreEqual(2, metaChunkGroup.CalculateEntityCount());

            m_ManagerDebug.CheckInternalConsistency();

            var group2 = m_Manager.CreateEntityQuery(ComponentType.ReadWrite<EcsTestData2>(), ComponentType.ChunkComponent<EcsTestData3>());

            m_Manager.RemoveChunkComponentData<EcsTestData3>(group2);

            Assert.IsFalse(m_Manager.HasComponent(entity2, ComponentType.ChunkComponent<EcsTestData3>()));

            Assert.AreEqual(1, metaChunkGroup.CalculateEntityCount());

            m_Manager.DestroyEntity(entity1);
            m_Manager.DestroyEntity(entity2);
            m_Manager.DestroyEntity(entity3);
            metaChunkGroup.Dispose();
            group1.Dispose();
            group2.Dispose();
        }

        [Test]
        public void AddRemoveSharedComponentWithGroupWorks()
        {
            var entity1 = m_Manager.CreateEntity(typeof(EcsTestData));
            var entity2 = m_Manager.CreateEntity(typeof(EcsTestData), typeof(EcsTestData2));
            var entity3 = m_Manager.CreateEntity(typeof(EcsTestData2));

            var group1 = m_Manager.CreateEntityQuery(ComponentType.ReadWrite<EcsTestData>());

            m_Manager.AddSharedComponentManaged(group1, new EcsTestSharedComp(7));

            Assert.IsTrue(m_Manager.HasComponent(entity1, ComponentType.ReadWrite<EcsTestSharedComp>()));
            var val1 = m_Manager.GetSharedComponentManaged<EcsTestSharedComp>(entity1).value;
            Assert.AreEqual(7, val1);

            Assert.IsTrue(m_Manager.HasComponent(entity2, ComponentType.ReadWrite<EcsTestSharedComp>()));
            var val2 = m_Manager.GetSharedComponentManaged<EcsTestSharedComp>(entity2).value;
            Assert.AreEqual(7, val2);

            Assert.IsFalse(m_Manager.HasComponent(entity3, ComponentType.ReadWrite<EcsTestSharedComp>()));

            m_ManagerDebug.CheckInternalConsistency();

            var group2 = m_Manager.CreateEntityQuery(ComponentType.ReadWrite<EcsTestData2>(), ComponentType.ReadWrite<EcsTestSharedComp>());

            m_Manager.RemoveComponent(group2, typeof(EcsTestSharedComp));

            Assert.IsFalse(m_Manager.HasComponent(entity2, typeof(EcsTestSharedComp)));
        }

        [Test]
        public void AddRemoveAnyComponentWithGroupWorksWithVariousTypes()
        {
            var componentTypes = new ComponentType[] { typeof(EcsTestTag), typeof(EcsTestData4), ComponentType.ChunkComponent<EcsTestData4>(), typeof(EcsTestSharedComp) };

            foreach (var type in componentTypes)
            {
                // We want a clean slate for the m_manager so teardown and setup before the test
                TearDown();
                Setup();

                var metaChunkGroup = m_Manager.CreateEntityQuery(typeof(ChunkHeader));

                var entity1 = m_Manager.CreateEntity(typeof(EcsTestData));
                var entity2 = m_Manager.CreateEntity(typeof(EcsTestData), typeof(EcsTestData2));
                var entity3 = m_Manager.CreateEntity(typeof(EcsTestData2));

                var group1 = m_Manager.CreateEntityQuery(ComponentType.ReadWrite<EcsTestData>());

                m_Manager.AddComponent(group1, type);


                Assert.IsTrue(m_Manager.HasComponent(entity1, type));
                Assert.IsTrue(m_Manager.HasComponent(entity2, type));
                Assert.IsFalse(m_Manager.HasComponent(entity3, type));

                if (type.IsChunkComponent)
                    Assert.AreEqual(2, metaChunkGroup.CalculateEntityCount());

                if (type == ComponentType.ReadWrite<EcsTestSharedComp>())
                {
                    m_Manager.SetSharedComponentManaged(entity1, new EcsTestSharedComp(1));
                    m_Manager.SetSharedComponentManaged(entity2, new EcsTestSharedComp(2));
                }

                m_ManagerDebug.CheckInternalConsistency();

                var group2 = m_Manager.CreateEntityQuery(ComponentType.ReadWrite<EcsTestData2>(), type);

                m_Manager.RemoveComponent(group2, type);

                Assert.IsFalse(m_Manager.HasComponent(entity2, ComponentType.ChunkComponent<EcsTestData3>()));

                if (type.IsChunkComponent)
                    Assert.AreEqual(1, metaChunkGroup.CalculateEntityCount());
            }
        }

        [Test]
        public void AddMultipleComponentsWithQuery()
        {
            var componentTypes = new ComponentTypeSet(
                typeof(EcsTestTag), typeof(EcsTestData2), ComponentType.ChunkComponent<EcsTestData4>(), typeof(EcsTestSharedComp));

            var entity1 = m_Manager.CreateEntity(typeof(EcsTestData));
            var entity2 = m_Manager.CreateEntity(typeof(EcsTestData), typeof(EcsTestData2));
            var entity3 = m_Manager.CreateEntity(typeof(EcsTestData2));

            var query = m_Manager.CreateEntityQuery(typeof(EcsTestData));

            m_Manager.AddComponent(query, componentTypes);

            m_ManagerDebug.CheckInternalConsistency();

            var archetype = m_Manager.CreateArchetype(typeof(EcsTestData), typeof(EcsTestTag), typeof(EcsTestData2), ComponentType.ChunkComponent<EcsTestData4>(),
                typeof(EcsTestSharedComp));
            Assert.AreEqual(archetype, m_Manager.GetChunk(entity1).Archetype);
            Assert.AreEqual(archetype, m_Manager.GetChunk(entity2).Archetype);

            // verify entity3 is unchanged
            Assert.AreEqual(m_Manager.CreateArchetype(typeof(EcsTestData2)), m_Manager.GetChunk(entity3).Archetype);
        }

        [Test]
        [TestRequiresDotsDebugOrCollectionChecks("Test requires entity data access safety checks")]
        public void AddMultipleComponentsWithQuery_AddingEntityComponentTypeThrows()
        {
            var componentTypes = new ComponentTypeSet(
                typeof(EcsTestTag), typeof(Entity), typeof(EcsTestData2), ComponentType.ChunkComponent<EcsTestData4>(), typeof(EcsTestSharedComp));

            var entity1 = m_Manager.CreateEntity(typeof(EcsTestData));
            var entity2 = m_Manager.CreateEntity(typeof(EcsTestData), typeof(EcsTestData2));
            var entity3 = m_Manager.CreateEntity(typeof(EcsTestData2));
            var archetype1 = m_Manager.GetChunk(entity1).Archetype;
            var archetype2 = m_Manager.GetChunk(entity2).Archetype;
            var archetype3 = m_Manager.GetChunk(entity3).Archetype;

            var query = m_Manager.CreateEntityQuery(typeof(EcsTestData));

            Assert.Throws<ArgumentException>(() => m_Manager.AddComponent(query, componentTypes));

            m_ManagerDebug.CheckInternalConsistency();

            // entities should be unchanged
            Assert.AreEqual(archetype1, m_Manager.GetChunk(entity1).Archetype);
            Assert.AreEqual(archetype2, m_Manager.GetChunk(entity2).Archetype);
            Assert.AreEqual(archetype3, m_Manager.GetChunk(entity3).Archetype);
        }

        [Test]
        [TestRequiresDotsDebugOrCollectionChecks("Test requires entity data access safety checks")]
        public void AddMultipleComponentsWithQuery_ExceedMaxSharedComponentsThrows()
        {
            var componentTypes = new ComponentTypeSet(new ComponentType[] {
                typeof(EcsTestSharedComp2), typeof(EcsTestSharedComp3), typeof(EcsTestSharedComp4),
                typeof(EcsTestSharedComp5), typeof(EcsTestSharedComp6), typeof(EcsTestSharedComp7), typeof(EcsTestSharedComp8),
                typeof(EcsTestSharedComp9), typeof(EcsTestSharedComp10), typeof(EcsTestSharedComp11), typeof(EcsTestSharedComp12),
                typeof(EcsTestSharedComp13), typeof(EcsTestSharedComp14), typeof(EcsTestSharedComp15), typeof(EcsTestSharedComp16)
            });

            Assert.AreEqual(16, EntityComponentStore.kMaxSharedComponentCount);   // if kMaxSharedComponentCount changes, need to update this test

            var entity1 = m_Manager.CreateEntity(typeof(EcsTestSharedComp), typeof(EcsTestData), typeof(EcsTestSharedComp17));
            var entity2 = m_Manager.CreateEntity(typeof(EcsTestSharedComp), typeof(EcsTestData), typeof(EcsTestData2));
            var entity3 = m_Manager.CreateEntity(typeof(EcsTestSharedComp), typeof(EcsTestData2));
            var archetype1 = m_Manager.GetChunk(entity1).Archetype;
            var archetype2 = m_Manager.GetChunk(entity2).Archetype;
            var archetype3 = m_Manager.GetChunk(entity3).Archetype;

            var query = m_Manager.CreateEntityQuery(typeof(EcsTestData));


#if UNITY_DOTSRUNTIME
            Assert.Throws<InvalidOperationException>(() => m_Manager.AddComponent(query, componentTypes));
#else
            Assert.That(() => m_Manager.AddComponent(query, componentTypes), Throws.InvalidOperationException
                  .With.Message.StartsWith($"Cannot add more than {EntityComponentStore.kMaxSharedComponentCount} SharedComponent's to a single Archetype"));
#endif

            m_ManagerDebug.CheckInternalConsistency();

            // entities should be unchanged
            Assert.AreEqual(archetype1, m_Manager.GetChunk(entity1).Archetype);
            Assert.AreEqual(archetype2, m_Manager.GetChunk(entity2).Archetype);
            Assert.AreEqual(archetype3, m_Manager.GetChunk(entity3).Archetype);
        }

        [Test]
        public void AddMultipleComponentsWithQuery_SharedComponentValuesPreserved()
        {
            // test for entities of different chunks getting their correct shared values
            var entity1 = m_Manager.CreateEntity(typeof(EcsTestData), typeof(EcsTestData2), typeof(EcsTestSharedComp));
            var entity2 = m_Manager.CreateEntity(typeof(EcsTestData), typeof(EcsTestData2), typeof(EcsTestSharedComp));

            m_Manager.SetSharedComponentManaged(entity1, new EcsTestSharedComp() {value = 5});
            m_Manager.SetSharedComponentManaged(entity2, new EcsTestSharedComp() {value = 9});

            var query = m_Manager.CreateEntityQuery(typeof(EcsTestData2));
            m_Manager.AddComponent(query, new ComponentTypeSet(typeof(EcsTestData2), typeof(EcsTestData4)));

            m_ManagerDebug.CheckInternalConsistency();

            Assert.AreEqual(5, m_Manager.GetSharedComponentManaged<EcsTestSharedComp>(entity1).value);
            Assert.AreEqual(9, m_Manager.GetSharedComponentManaged<EcsTestSharedComp>(entity2).value);

            var archetype = m_Manager.CreateArchetype(typeof(EcsTestData), typeof(EcsTestData2), typeof(EcsTestData4), typeof(EcsTestSharedComp));
            Assert.AreEqual(archetype, m_Manager.GetChunk(entity1).Archetype);
            Assert.AreEqual(archetype, m_Manager.GetChunk(entity2).Archetype);
        }

        private struct EcsTestDataHuge : IComponentData
        {
            public FixedString4096Bytes value0;
            public FixedString4096Bytes value1;
            public FixedString4096Bytes value2;
            public FixedString4096Bytes value3;
            public FixedString4096Bytes value4;

            public EcsTestDataHuge(FixedString4096Bytes inValue)
            {
                value4 = value3 = value2 = value1 = value0 = inValue;
            }
        }

        [Test]
        [TestRequiresDotsDebugOrCollectionChecks("Test requires entity data access safety checks")]
        public void AddMultipleComponentsWithQuery_ExceedChunkCapacityThrows()
        {
            var componentTypes = new ComponentTypeSet(typeof(EcsTestDataHuge)); // add really big component(s)

            Assert.AreEqual(16320, Chunk.GetChunkBufferSize());   // if chunk size changes, need to update this test

            var entity1 = m_Manager.CreateEntity(typeof(EcsTestData));
            var entity2 = m_Manager.CreateEntity(typeof(EcsTestData), typeof(EcsTestData2));
            var entity3 = m_Manager.CreateEntity(typeof(EcsTestData2));
            var archetype1 = m_Manager.GetChunk(entity1).Archetype;
            var archetype2 = m_Manager.GetChunk(entity2).Archetype;
            var archetype3 = m_Manager.GetChunk(entity3).Archetype;

            var query = m_Manager.CreateEntityQuery(typeof(EcsTestData));

#if UNITY_DOTSRUNTIME
            Assert.Throws<InvalidOperationException>(() => m_Manager.AddComponent(query, componentTypes));
#else
            Assert.That(() => m_Manager.AddComponent(query, componentTypes), Throws.InvalidOperationException
                            .With.Message.Contains("Entity archetype component data is too large."));
#endif

            m_ManagerDebug.CheckInternalConsistency();

            // entities should be unchanged
            Assert.AreEqual(archetype1, m_Manager.GetChunk(entity1).Archetype);
            Assert.AreEqual(archetype2, m_Manager.GetChunk(entity2).Archetype);
            Assert.AreEqual(archetype3, m_Manager.GetChunk(entity3).Archetype);
        }

        [Test]
        public void AddMultipleComponentsWithNativeArray()
        {
            var componentTypes = new ComponentTypeSet(typeof(EcsTestData),
                typeof(EcsTestTag), typeof(EcsTestData2), ComponentType.ChunkComponent<EcsTestData4>(), typeof(EcsTestSharedComp));

            var entity1 = m_Manager.CreateEntity(typeof(EcsTestData));
            var entity2 = m_Manager.CreateEntity(typeof(EcsTestData), typeof(EcsTestData2));
            var entity3 = m_Manager.CreateEntity(typeof(EcsTestData2));

            using(var entities = CollectionHelper.CreateNativeArray<Entity>( new[] { entity1, entity2, entity3 },World.UpdateAllocator.ToAllocator))
            {
                m_Manager.AddComponent(entities, componentTypes);

                m_ManagerDebug.CheckInternalConsistency();

                var archetype = m_Manager.CreateArchetype(typeof(EcsTestData), typeof(EcsTestTag), typeof(EcsTestData2), ComponentType.ChunkComponent<EcsTestData4>(),
                    typeof(EcsTestSharedComp));
                Assert.AreEqual(archetype, m_Manager.GetChunk(entity1).Archetype);
                Assert.AreEqual(archetype, m_Manager.GetChunk(entity2).Archetype);
                Assert.AreEqual(archetype, m_Manager.GetChunk(entity3).Archetype);
            }

        }



        [Test]
        public void RemoveMultipleComponentsWithQuery()
        {
            var entity1 = m_Manager.CreateEntity(typeof(EcsTestTag), typeof(EcsTestData), typeof(EcsTestData4), ComponentType.ChunkComponent<EcsTestData4>(), typeof(EcsTestSharedComp) );
            var entity2 = m_Manager.CreateEntity(typeof(EcsTestData), typeof(EcsTestData2), typeof(EcsTestData4), ComponentType.ChunkComponent<EcsTestData4>(), typeof(EcsTestSharedComp) );
            var entity3 = m_Manager.CreateEntity(typeof(EcsTestData2));

            var query = m_Manager.CreateEntityQuery(typeof(EcsTestData2));

            m_Manager.RemoveComponent(query, new ComponentTypeSet(typeof(EcsTestData2), typeof(EcsTestData4), ComponentType.ChunkComponent<EcsTestData4>(), typeof(EcsTestSharedComp)));

            m_ManagerDebug.CheckInternalConsistency();

            var archetype1 = m_Manager.CreateArchetype(typeof(EcsTestTag), typeof(EcsTestData), typeof(EcsTestData4), ComponentType.ChunkComponent<EcsTestData4>(), typeof(EcsTestSharedComp) );
            var archetype2 = m_Manager.CreateArchetype(typeof(EcsTestData));
            var archetype3 = m_Manager.CreateArchetype();
            Assert.AreEqual(archetype1, m_Manager.GetChunk(entity1).Archetype);
            Assert.AreEqual(archetype2, m_Manager.GetChunk(entity2).Archetype);
            Assert.AreEqual(archetype3, m_Manager.GetChunk(entity3).Archetype);
        }

        [Test]
        public void RemoveMultipleComponentsWithQuery_SharedComponentValuesPreserved()
        {
            // test for entities of different chunks getting their correct shared values
            var entity1 = m_Manager.CreateEntity(typeof(EcsTestData), typeof(EcsTestData2), typeof(EcsTestSharedComp));
            var entity2 = m_Manager.CreateEntity(typeof(EcsTestData), typeof(EcsTestData2), typeof(EcsTestSharedComp));

            m_Manager.SetSharedComponentManaged(entity1, new EcsTestSharedComp() {value = 5});
            m_Manager.SetSharedComponentManaged(entity2, new EcsTestSharedComp() {value = 9});

            var query = m_Manager.CreateEntityQuery(typeof(EcsTestData2));
            m_Manager.RemoveComponent(query, new ComponentTypeSet(typeof(EcsTestData2), typeof(EcsTestData4)));

            m_ManagerDebug.CheckInternalConsistency();

            Assert.AreEqual(5, m_Manager.GetSharedComponentManaged<EcsTestSharedComp>(entity1).value);
            Assert.AreEqual(9, m_Manager.GetSharedComponentManaged<EcsTestSharedComp>(entity2).value);

            var archetype = m_Manager.CreateArchetype(typeof(EcsTestData), typeof(EcsTestSharedComp));
            Assert.AreEqual(archetype, m_Manager.GetChunk(entity1).Archetype);
            Assert.AreEqual(archetype, m_Manager.GetChunk(entity2).Archetype);
        }

        [Test]
        [IgnoreInPortableTests("intermittent crash (likely race condition)")]
        public void RemoveAnyComponentWithGroupIgnoresChunksThatDontHaveTheComponent()
        {
            var componentTypes = new ComponentType[]
            {
                typeof(EcsTestTag), typeof(EcsTestData4), ComponentType.ChunkComponent<EcsTestData4>(), typeof(EcsTestSharedComp)
            };

            foreach (var type in componentTypes)
            {
                // We want a clean slate for the m_manager so teardown and setup before the test
                TearDown();
                Setup();

                var metaChunkGroup = m_Manager.CreateEntityQuery(typeof(ChunkHeader));

                var entity1 = m_Manager.CreateEntity(typeof(EcsTestData));
                var entity2 = m_Manager.CreateEntity(typeof(EcsTestData), typeof(EcsTestData2));
                var entity3 = m_Manager.CreateEntity(typeof(EcsTestData2));

                var group1 = m_Manager.CreateEntityQuery(ComponentType.ReadWrite<EcsTestData>());

                m_Manager.AddComponent(group1, type);

                Assert.IsTrue(m_Manager.HasComponent(entity1, type));
                Assert.IsTrue(m_Manager.HasComponent(entity2, type));
                Assert.IsFalse(m_Manager.HasComponent(entity3, type));

                if (type.IsChunkComponent)
                    Assert.AreEqual(2, metaChunkGroup.CalculateEntityCount());

                if (type == ComponentType.ReadWrite<EcsTestSharedComp>())
                {
                    m_Manager.SetSharedComponentManaged(entity1, new EcsTestSharedComp(1));
                    m_Manager.SetSharedComponentManaged(entity2, new EcsTestSharedComp(2));
                }

                m_ManagerDebug.CheckInternalConsistency();

                m_Manager.RemoveComponent(m_Manager.UniversalQuery, type);

                Assert.AreEqual(0, m_Manager.CreateEntityQuery(type).CalculateEntityCount());
            }
        }

        [Test]
        public void RemoveMultipleComponentsWithNativeArray()
        {
            var entity1 = m_Manager.CreateEntity(typeof(EcsTestTag), typeof(EcsTestData), typeof(EcsTestData4), ComponentType.ChunkComponent<EcsTestData4>(), typeof(EcsTestSharedComp) );
            var entity2 = m_Manager.CreateEntity(typeof(EcsTestData), typeof(EcsTestData2), typeof(EcsTestData4), ComponentType.ChunkComponent<EcsTestData4>(), typeof(EcsTestSharedComp) );
            var entity3 = m_Manager.CreateEntity(typeof(EcsTestData2));

            using(var entities = CollectionHelper.CreateNativeArray<Entity>( new[] { entity1, entity2, entity3 },World.UpdateAllocator.ToAllocator))
            {

                m_Manager.RemoveComponent(entities, new ComponentTypeSet(typeof(EcsTestData2), typeof(EcsTestData4), ComponentType.ChunkComponent<EcsTestData4>(), typeof(EcsTestSharedComp)));

                m_ManagerDebug.CheckInternalConsistency();

                var archetype1 = m_Manager.CreateArchetype(typeof(EcsTestTag), typeof(EcsTestData));
                var archetype2 = m_Manager.CreateArchetype(typeof(EcsTestData));
                var archetype3 = m_Manager.CreateArchetype();
                Assert.AreEqual(archetype1, m_Manager.GetChunk(entity1).Archetype);
                Assert.AreEqual(archetype2, m_Manager.GetChunk(entity2).Archetype);
                Assert.AreEqual(archetype3, m_Manager.GetChunk(entity3).Archetype);
            }

        }

        uint GetComponentDataVersion<T>(Entity e) where T :
#if UNITY_DISABLE_MANAGED_COMPONENTS
        struct,
#endif
        IComponentData
        {
            var typeHandle = m_Manager.GetComponentTypeHandle<T>(true);
            return m_Manager.GetChunk(e).GetChangeVersion(ref typeHandle);
        }

        uint GetSharedComponentDataVersion<T>(Entity e) where T : struct, ISharedComponentData
        {
            return m_Manager.GetChunk(e).GetChangeVersion(m_Manager.GetSharedComponentTypeHandle<T>());
        }

        [Test]
        public void AddRemoveComponentWithGroupPreservesChangeVersions()
        {
            m_ManagerDebug.SetGlobalSystemVersion(10);
            var entity1 = m_Manager.CreateEntity(typeof(EcsTestData));
            var entity2 = m_Manager.CreateEntity(typeof(EcsTestData), typeof(EcsTestData2));
            var entity3 = m_Manager.CreateEntity(typeof(EcsTestData2));

            m_ManagerDebug.SetGlobalSystemVersion(20);

            m_Manager.SetComponentData(entity2, new EcsTestData2(7));
            m_Manager.SetComponentData(entity3, new EcsTestData2(8));

            Assert.AreEqual(10, GetComponentDataVersion<EcsTestData>(entity1));
            Assert.AreEqual(10, GetComponentDataVersion<EcsTestData>(entity2));
            Assert.AreEqual(20, GetComponentDataVersion<EcsTestData2>(entity2));
            Assert.AreEqual(20, GetComponentDataVersion<EcsTestData2>(entity3));

            m_ManagerDebug.SetGlobalSystemVersion(30);

            m_Manager.AddSharedComponentManaged(m_Manager.UniversalQuery, new EcsTestSharedComp(1));
            m_ManagerDebug.SetGlobalSystemVersion(40);
            m_Manager.AddComponent(m_Manager.UniversalQuery, typeof(EcsTestTag));
            Assert.AreEqual(30, GetSharedComponentDataVersion<EcsTestSharedComp>(entity1));
            Assert.AreEqual(30, GetSharedComponentDataVersion<EcsTestSharedComp>(entity2));
            Assert.AreEqual(30, GetSharedComponentDataVersion<EcsTestSharedComp>(entity3));

            Assert.AreEqual(40, GetComponentDataVersion<EcsTestTag>(entity1));
            Assert.AreEqual(40, GetComponentDataVersion<EcsTestTag>(entity2));
            Assert.AreEqual(40, GetComponentDataVersion<EcsTestTag>(entity3));

            m_ManagerDebug.SetGlobalSystemVersion(50);

            m_Manager.RemoveComponent(m_Manager.UniversalQuery, typeof(EcsTestSharedComp2));
            Assert.AreEqual(30, GetSharedComponentDataVersion<EcsTestSharedComp>(entity1));
            Assert.AreEqual(30, GetSharedComponentDataVersion<EcsTestSharedComp>(entity2));
            Assert.AreEqual(30, GetSharedComponentDataVersion<EcsTestSharedComp>(entity3));

            m_ManagerDebug.SetGlobalSystemVersion(60);

            m_Manager.RemoveComponent(m_Manager.UniversalQuery, typeof(EcsTestSharedComp));
            Assert.AreEqual(10, GetComponentDataVersion<EcsTestData>(entity1));
            Assert.AreEqual(10, GetComponentDataVersion<EcsTestData>(entity2));
            Assert.AreEqual(20, GetComponentDataVersion<EcsTestData2>(entity2));
            Assert.AreEqual(20, GetComponentDataVersion<EcsTestData2>(entity3));
        }

#if !UNITY_DISABLE_MANAGED_COMPONENTS
        [Test]
        public void AddRemoveChunkComponentWithGroupWorks_ManagedComponents()
        {
            var metaChunkGroup = m_Manager.CreateEntityQuery(typeof(ChunkHeader));

            var entity1 = m_Manager.CreateEntity(typeof(EcsTestData));
            var entity2 = m_Manager.CreateEntity(typeof(EcsTestData), typeof(EcsTestData2));
            var entity3 = m_Manager.CreateEntity(typeof(EcsTestData2));

            var group1 = m_Manager.CreateEntityQuery(ComponentType.ReadWrite<EcsTestData>());

            m_ManagerDebug.CheckInternalConsistency();
            m_Manager.AddChunkComponentData(group1, new EcsTestManagedComponent() { value = "SomeString" });
            m_ManagerDebug.CheckInternalConsistency();

            Assert.IsTrue(m_Manager.HasComponent(entity1, ComponentType.ChunkComponent<EcsTestManagedComponent>()));
            var val1 = m_Manager.GetChunkComponentData<EcsTestManagedComponent>(entity1).value;
            Assert.AreEqual("SomeString", val1);

            Assert.IsTrue(m_Manager.HasComponent(entity2, ComponentType.ChunkComponent<EcsTestManagedComponent>()));
            var val2 = m_Manager.GetChunkComponentData<EcsTestManagedComponent>(entity2).value;
            Assert.AreEqual("SomeString", val2);

            Assert.IsFalse(m_Manager.HasComponent(entity3, ComponentType.ChunkComponent<EcsTestManagedComponent>()));

            Assert.AreEqual(2, metaChunkGroup.CalculateEntityCount());

            m_ManagerDebug.CheckInternalConsistency();

            var group2 = m_Manager.CreateEntityQuery(ComponentType.ReadWrite<EcsTestData2>(), ComponentType.ChunkComponent<EcsTestManagedComponent>());

            m_Manager.RemoveChunkComponentData<EcsTestManagedComponent>(group2);

            Assert.IsFalse(m_Manager.HasComponent(entity2, ComponentType.ChunkComponent<EcsTestManagedComponent>()));

            Assert.AreEqual(1, metaChunkGroup.CalculateEntityCount());

            m_Manager.DestroyEntity(entity1);
            m_Manager.DestroyEntity(entity2);
            m_Manager.DestroyEntity(entity3);
            metaChunkGroup.Dispose();
            group1.Dispose();
            group2.Dispose();
        }

        [Test]
        public void AddRemoveAnyComponentWithGroupWorksWithVariousTypes_ManagedComponents()
        {
            var componentTypes = new ComponentType[]
            {
                typeof(EcsTestTag), typeof(EcsTestData4), ComponentType.ChunkComponent<EcsTestData4>(), typeof(EcsTestSharedComp),
                typeof(EcsTestManagedComponent), ComponentType.ChunkComponent<EcsTestManagedComponent>()
            };

            foreach (var type in componentTypes)
            {
                // We want a clean slate for the m_manager so teardown and setup before the test
                TearDown();
                Setup();

                var metaChunkGroup = m_Manager.CreateEntityQuery(typeof(ChunkHeader));

                var entity1 = m_Manager.CreateEntity(typeof(EcsTestData));
                var entity2 = m_Manager.CreateEntity(typeof(EcsTestData), typeof(EcsTestData2));
                var entity3 = m_Manager.CreateEntity(typeof(EcsTestData2));

                var group1 = m_Manager.CreateEntityQuery(ComponentType.ReadWrite<EcsTestData>());

                m_Manager.AddComponent(group1, type);

                Assert.IsTrue(m_Manager.HasComponent(entity1, type));
                Assert.IsTrue(m_Manager.HasComponent(entity2, type));
                Assert.IsFalse(m_Manager.HasComponent(entity3, type));

                if (type.IsChunkComponent)
                    Assert.AreEqual(2, metaChunkGroup.CalculateEntityCount());

                if (type == ComponentType.ReadWrite<EcsTestSharedComp>())
                {
                    m_Manager.SetSharedComponentManaged(entity1, new EcsTestSharedComp(1));
                    m_Manager.SetSharedComponentManaged(entity2, new EcsTestSharedComp(2));
                }

                m_ManagerDebug.CheckInternalConsistency();

                var group2 = m_Manager.CreateEntityQuery(ComponentType.ReadWrite<EcsTestData2>(), type);

                m_Manager.RemoveComponent(group2, type);

                Assert.IsFalse(m_Manager.HasComponent(entity2, ComponentType.ChunkComponent<EcsTestData3>()));

                if (type.IsChunkComponent)
                    Assert.AreEqual(1, metaChunkGroup.CalculateEntityCount());
            }
        }

        [Test]
        [IgnoreInPortableTests("intermittent crash (likely race condition)")]
        public void RemoveAnyComponentWithGroupIgnoresChunksThatDontHaveTheComponent_ManagedComponents()
        {
            var componentTypes = new ComponentType[]
            {
                typeof(EcsTestTag), typeof(EcsTestData4), ComponentType.ChunkComponent<EcsTestData4>(), typeof(EcsTestSharedComp),
                typeof(EcsTestManagedComponent), ComponentType.ChunkComponent<EcsTestManagedComponent>()
            };

            foreach (var type in componentTypes)
            {
                // We want a clean slate for the m_manager so teardown and setup before the test
                TearDown();
                Setup();

                var metaChunkGroup = m_Manager.CreateEntityQuery(typeof(ChunkHeader));

                var entity1 = m_Manager.CreateEntity(typeof(EcsTestData));
                var entity2 = m_Manager.CreateEntity(typeof(EcsTestData), typeof(EcsTestData2));
                var entity3 = m_Manager.CreateEntity(typeof(EcsTestData2));

                var group1 = m_Manager.CreateEntityQuery(ComponentType.ReadWrite<EcsTestData>());

                m_Manager.AddComponent(group1, type);

                Assert.IsTrue(m_Manager.HasComponent(entity1, type));
                Assert.IsTrue(m_Manager.HasComponent(entity2, type));
                Assert.IsFalse(m_Manager.HasComponent(entity3, type));

                if (type.IsChunkComponent)
                    Assert.AreEqual(2, metaChunkGroup.CalculateEntityCount());

                if (type == ComponentType.ReadWrite<EcsTestSharedComp>())
                {
                    m_Manager.SetSharedComponentManaged(entity1, new EcsTestSharedComp(1));
                    m_Manager.SetSharedComponentManaged(entity2, new EcsTestSharedComp(2));
                }

                m_ManagerDebug.CheckInternalConsistency();

                m_Manager.RemoveComponent(m_Manager.UniversalQuery, type);

                Assert.AreEqual(0, m_Manager.CreateEntityQuery(type).CalculateEntityCount());
            }
        }

        [Test]
        public void AddRemoveComponentWithGroupPreservesChangeVersions_ManagedComponents()
        {
            m_ManagerDebug.SetGlobalSystemVersion(10);
            var entity1 = m_Manager.CreateEntity(typeof(EcsTestData));
            var entity2 = m_Manager.CreateEntity(typeof(EcsTestData), typeof(EcsTestData2));
            var entity3 = m_Manager.CreateEntity(typeof(EcsTestData2));
            var entity4 = m_Manager.CreateEntity(typeof(EcsTestData2), typeof(EcsTestManagedComponent));
            var entity5 = m_Manager.CreateEntity(typeof(EcsTestManagedComponent));

            m_ManagerDebug.SetGlobalSystemVersion(20);

            m_Manager.SetComponentData(entity2, new EcsTestData2(7));
            m_Manager.SetComponentData(entity3, new EcsTestData2(8));
            m_Manager.SetComponentData(entity4, new EcsTestData2(9));

            Assert.AreEqual(10, GetComponentDataVersion<EcsTestData>(entity1));
            Assert.AreEqual(10, GetComponentDataVersion<EcsTestData>(entity2));
            Assert.AreEqual(20, GetComponentDataVersion<EcsTestData2>(entity2));
            Assert.AreEqual(20, GetComponentDataVersion<EcsTestData2>(entity3));
            Assert.AreEqual(20, GetComponentDataVersion<EcsTestData2>(entity4));
            Assert.AreEqual(10, GetComponentDataVersion<EcsTestManagedComponent>(entity4));
            Assert.AreEqual(10, GetComponentDataVersion<EcsTestManagedComponent>(entity5));

            m_ManagerDebug.SetGlobalSystemVersion(30);

            m_Manager.AddSharedComponentManaged(m_Manager.UniversalQuery, new EcsTestSharedComp(1));

            m_ManagerDebug.SetGlobalSystemVersion(40);

            m_Manager.AddComponent(m_Manager.UniversalQuery, typeof(EcsTestTag));

            Assert.AreEqual(30, GetSharedComponentDataVersion<EcsTestSharedComp>(entity1));
            Assert.AreEqual(30, GetSharedComponentDataVersion<EcsTestSharedComp>(entity2));
            Assert.AreEqual(30, GetSharedComponentDataVersion<EcsTestSharedComp>(entity3));
            Assert.AreEqual(30, GetSharedComponentDataVersion<EcsTestSharedComp>(entity4));
            Assert.AreEqual(30, GetSharedComponentDataVersion<EcsTestSharedComp>(entity5));

            Assert.AreEqual(40, GetComponentDataVersion<EcsTestTag>(entity1));
            Assert.AreEqual(40, GetComponentDataVersion<EcsTestTag>(entity2));
            Assert.AreEqual(40, GetComponentDataVersion<EcsTestTag>(entity3));
            Assert.AreEqual(40, GetComponentDataVersion<EcsTestTag>(entity4));
            Assert.AreEqual(40, GetComponentDataVersion<EcsTestTag>(entity5));

            m_ManagerDebug.SetGlobalSystemVersion(50);

            m_Manager.RemoveComponent(m_Manager.UniversalQuery, typeof(EcsTestSharedComp2));

            Assert.AreEqual(30, GetSharedComponentDataVersion<EcsTestSharedComp>(entity1));
            Assert.AreEqual(30, GetSharedComponentDataVersion<EcsTestSharedComp>(entity2));
            Assert.AreEqual(30, GetSharedComponentDataVersion<EcsTestSharedComp>(entity3));
            Assert.AreEqual(30, GetSharedComponentDataVersion<EcsTestSharedComp>(entity4));
            Assert.AreEqual(30, GetSharedComponentDataVersion<EcsTestSharedComp>(entity5));

            m_ManagerDebug.SetGlobalSystemVersion(60);

            m_Manager.RemoveComponent(m_Manager.UniversalQuery, typeof(EcsTestSharedComp));

            Assert.AreEqual(10, GetComponentDataVersion<EcsTestData>(entity1));
            Assert.AreEqual(10, GetComponentDataVersion<EcsTestData>(entity2));
            Assert.AreEqual(20, GetComponentDataVersion<EcsTestData2>(entity2));
            Assert.AreEqual(20, GetComponentDataVersion<EcsTestData2>(entity3));
            Assert.AreEqual(20, GetComponentDataVersion<EcsTestData2>(entity4));
            Assert.AreEqual(10, GetComponentDataVersion<EcsTestManagedComponent>(entity4));
            Assert.AreEqual(10, GetComponentDataVersion<EcsTestManagedComponent>(entity5));
        }

#endif
    }
}
