using NUnit.Framework;
#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.TestTools;
using Unity.Scenes.Editor.Tests;
#endif
using Unity.Entities;
using Unity.Entities.Tests;

namespace Unity.Scenes.Hybrid.Tests
{
    public class SubSceneSectionTestsConversion : SubSceneSectionTests
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            m_Settings.Setup(false);
            base.SetUpOnce();
        }

        [OneTimeTearDown]
        public void OneTimeTeardown()
        {
            base.TearDownOnce();
            m_Settings.TearDown();
        }
    }

    public class SubSceneSectionTestsBaking : SubSceneSectionTests
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            m_Settings.Setup(true);
            base.SetUpOnce();
        }

        [OneTimeTearDown]
        public void OneTimeTeardown()
        {
            base.TearDownOnce();
            m_Settings.TearDown();
        }
    }

    public abstract class SubSceneSectionTests : SubSceneTestFixture
    {
        public TestLiveConversionSettings m_Settings;
        public SubSceneSectionTests() : base("Packages/com.unity.entities/Unity.Scenes.Hybrid.Tests/TestSceneWithSubScene/SubSceneSectionTestScene.unity")
        {
        }

        // Only works in Editor for now until we can support SubScene building with new build settings in a test
        [Test]
        [UnityPlatform(RuntimePlatform.WindowsEditor, RuntimePlatform.OSXEditor, RuntimePlatform.LinuxEditor)]
        public void LoadSceneAsync_LoadsAllSections()
        {
            using (var world = TestWorldSetup.CreateEntityWorld("World", false))
            {
                var manager = world.EntityManager;

                var resolveParams = new SceneSystem.LoadParameters
                {
                    Flags = SceneLoadFlags.BlockOnImport | SceneLoadFlags.BlockOnStreamIn
                };

                SceneSystem.LoadSceneAsync(world.Unmanaged, SceneGUID, resolveParams);
                world.Update();

                EntitiesAssert.Contains(manager,
                    EntityMatch.Partial(new SubSceneSectionTestData(42)),
                    EntityMatch.Partial(new SubSceneSectionTestData(43)),
                    EntityMatch.Partial(new SubSceneSectionTestData(44))
                );
            }
        }

        // Only works in Editor for now until we can support SubScene building with new build settings in a test
        [Test]
        [UnityPlatform(RuntimePlatform.WindowsEditor, RuntimePlatform.OSXEditor, RuntimePlatform.LinuxEditor)]
        public void LoadSceneAsync_DeleteSceneEntityUnloadsAllSections()
        {
            using (var world = TestWorldSetup.CreateEntityWorld("World", false))
            {
                var manager = world.EntityManager;

                var resolveParams = new SceneSystem.LoadParameters
                {
                    Flags = SceneLoadFlags.BlockOnImport | SceneLoadFlags.BlockOnStreamIn
                };

                var subSceneSectionTestDataQuery = manager.CreateEntityQuery(typeof(SubSceneSectionTestData));
                
                var sceneEntity = SceneSystem.LoadSceneAsync(world.Unmanaged, SceneGUID, resolveParams);
                world.Update();

                Assert.AreEqual(3, subSceneSectionTestDataQuery.CalculateEntityCount());

                manager.DestroyEntity(sceneEntity);
                world.Update();

                Assert.AreEqual(0, subSceneSectionTestDataQuery.CalculateEntityCount());
            }
        }

        // Only works in Editor for now until we can support SubScene building with new build settings in a test
        [Test]
        [UnityPlatform(RuntimePlatform.WindowsEditor, RuntimePlatform.OSXEditor, RuntimePlatform.LinuxEditor)]
        public void CanLoadSectionsIndividually()
        {
            using (var world = TestWorldSetup.CreateEntityWorld("World", false))
            {
                var manager = world.EntityManager;

                var resolveParams = new SceneSystem.LoadParameters
                {
                    Flags = SceneLoadFlags.BlockOnImport | SceneLoadFlags.BlockOnStreamIn | SceneLoadFlags.DisableAutoLoad
                };

                var subSceneSectionTestDataQuery = manager.CreateEntityQuery(typeof(SubSceneSectionTestData));

                var sceneEntity = SceneSystem.LoadSceneAsync(world.Unmanaged, SceneGUID, resolveParams);
                world.Update();

                Assert.AreEqual(0, subSceneSectionTestDataQuery.CalculateEntityCount());

                var section0Entity = FindSectionEntity(manager, sceneEntity, 0);
                var section10Entity = FindSectionEntity(manager, sceneEntity, 10);
                var section20Entity = FindSectionEntity(manager, sceneEntity, 20);
                Assert.AreNotEqual(Entity.Null, section0Entity);
                Assert.AreNotEqual(Entity.Null, section10Entity);
                Assert.AreNotEqual(Entity.Null, section20Entity);

                Assert.IsFalse(SceneSystem.IsSectionLoaded(world.Unmanaged, section0Entity));
                Assert.IsFalse(SceneSystem.IsSectionLoaded(world.Unmanaged, section10Entity));
                Assert.IsFalse(SceneSystem.IsSectionLoaded(world.Unmanaged, section20Entity));

                manager.AddComponentData(section0Entity,
                    new RequestSceneLoaded {LoadFlags = SceneLoadFlags.BlockOnImport | SceneLoadFlags.BlockOnStreamIn});

                world.Update();

                Assert.IsTrue(SceneSystem.IsSectionLoaded(world.Unmanaged, section0Entity));
                Assert.IsFalse(SceneSystem.IsSectionLoaded(world.Unmanaged, section10Entity));
                Assert.IsFalse(SceneSystem.IsSectionLoaded(world.Unmanaged, section20Entity));

                Assert.AreEqual(1, subSceneSectionTestDataQuery.CalculateEntityCount());
                Assert.AreEqual(42, subSceneSectionTestDataQuery.GetSingleton<SubSceneSectionTestData>().Value);

                manager.AddComponentData(section20Entity,
                    new RequestSceneLoaded {LoadFlags = SceneLoadFlags.BlockOnImport | SceneLoadFlags.BlockOnStreamIn});
                world.Update();

                Assert.IsTrue(SceneSystem.IsSectionLoaded(world.Unmanaged, section0Entity));
                Assert.IsFalse(SceneSystem.IsSectionLoaded(world.Unmanaged, section10Entity));
                Assert.IsTrue(SceneSystem.IsSectionLoaded(world.Unmanaged, section20Entity));

                Assert.AreEqual(2, subSceneSectionTestDataQuery.CalculateEntityCount());
                EntitiesAssert.Contains(manager,
                    EntityMatch.Partial(new SubSceneSectionTestData(42)),
                    EntityMatch.Partial(new SubSceneSectionTestData(44)));


                manager.AddComponentData(section10Entity,
                    new RequestSceneLoaded {LoadFlags = SceneLoadFlags.BlockOnImport | SceneLoadFlags.BlockOnStreamIn});
                world.Update();

                Assert.IsTrue(SceneSystem.IsSectionLoaded(world.Unmanaged, section0Entity));
                Assert.IsTrue(SceneSystem.IsSectionLoaded(world.Unmanaged, section10Entity));
                Assert.IsTrue(SceneSystem.IsSectionLoaded(world.Unmanaged, section20Entity));

                Assert.AreEqual(3, subSceneSectionTestDataQuery.CalculateEntityCount());
                EntitiesAssert.Contains(manager,
                    EntityMatch.Partial(new SubSceneSectionTestData(42)),
                    EntityMatch.Partial(new SubSceneSectionTestData(43)),
                    EntityMatch.Partial(new SubSceneSectionTestData(44)));
            }
        }

        static Entity FindSectionEntity(EntityManager manager, Entity sceneEntity, int sectionIndex)
        {
            var sections = manager.GetBuffer<ResolvedSectionEntity>(sceneEntity);
            for (int i = 0; i < sections.Length; ++i)
            {
                var sectionEntity = sections[i].SectionEntity;
                var sectionData = manager.GetComponentData<SceneSectionData>(sectionEntity);
                if (sectionData.SubSectionIndex == sectionIndex)
                    return sectionEntity;
            }
            return Entity.Null;
        }
    }
}
