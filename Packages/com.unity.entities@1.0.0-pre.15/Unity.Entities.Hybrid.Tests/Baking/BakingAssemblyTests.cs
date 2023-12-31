using NUnit.Framework;
#if USING_PLATFORMS_PACKAGE
using Unity.Build;
#endif
using Unity.Entities.Build;
using Unity.Entities.Conversion;
using Unity.Scenes.Editor.Tests;
using UnityEngine;
using Unity.Entities.Hybrid.Tests.Baking.SeparateAssembly;

namespace Unity.Entities.Hybrid.Tests.Baking
{
    internal class BakingAssemblyTests : BakingSystemFixtureBase
    {
        private BakingSystem m_BakingSystem;
        private TestLiveConversionSettings m_Settings;

        [SetUp]
        public override void Setup()
        {
            m_Settings.Setup(true);
            base.Setup();

            m_BakingSystem = World.GetOrCreateSystemManaged<BakingSystem>();
            var settings = MakeDefaultSettings();
            m_BakingSystem.PrepareForBaking(settings, default);

            m_Manager = World.EntityManager;
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            m_BakingSystem = null;
            m_Settings.TearDown();
        }

#if USING_PLATFORMS_PACKAGE
        [Test]
        public void BakingExcludeAssemblyTests()
        {
            m_BakingSystem = World.GetOrCreateSystemManaged<BakingSystem>();
            var manager = World.EntityManager;

            // Create the gameobject with the authoring component
            var go = CreateGameObject();
            go.AddComponent<ComponentInAssemblyAuthoring>();

            // Create the a setting with a filter and another one without a filter
            var normalSettings = MakeDefaultSettings();
            var exludeSettings = MakeDefaultSettings();
            var config = BuildConfiguration.CreateInstance((bs) =>
            {
                bs.hideFlags = HideFlags.HideAndDontSave;
                bs.SetComponent(new ConversionSystemFilterSettings("Unity.Entities.Hybrid.Tests.SeparateAssembly"));
            });
            exludeSettings.BuildConfiguration = config;

            // Bake the gameobject
            BakingUtility.BakeGameObjects(World, new[] {go}, normalSettings);

            // With regular baking it is expected that the baker runs and the component is added
            var entity = m_BakingSystem.GetEntity(go);
            Assert.AreEqual(true, manager.HasComponent<ComponentInAssemblyComponent>(entity));

            // With the filter we expect that the baker hasn't run and the component is not in the entity
            BakingUtility.BakeGameObjects(World, new[] {go}, exludeSettings);
            entity = m_BakingSystem.GetEntity(go);
            Assert.AreEqual(false, manager.HasComponent<ComponentInAssemblyComponent>(entity));

            // We repeat the same two checks to make sure nothing is left that affects this
            BakingUtility.BakeGameObjects(World, new[] {go}, normalSettings);
            entity = m_BakingSystem.GetEntity(go);
            Assert.AreEqual(true, manager.HasComponent<ComponentInAssemblyComponent>(entity));

            BakingUtility.BakeGameObjects(World, new[] {go}, exludeSettings);
            entity = m_BakingSystem.GetEntity(go);
            Assert.AreEqual(false, manager.HasComponent<ComponentInAssemblyComponent>(entity));
        }
#endif

        [Test]
        public void BakingExcludeAssemblyTests_BuiltInBuildsPath()
        {
            m_BakingSystem = World.GetOrCreateSystemManaged<BakingSystem>();
            var manager = World.EntityManager;

            // Create the gameobject with the authoring component
            var go = CreateGameObject();
            go.AddComponent<ComponentInAssemblyAuthoring>();

            // Create the a setting with a filter and another one without a filter
            var originalFilter = EntitiesClientSettings.instance.FilterSettings;
            try
            {
                var clearFilter = new BakingSystemFilterSettings();
                var testFilter = new BakingSystemFilterSettings("Unity.Entities.Hybrid.Tests.SeparateAssembly");

                EntitiesClientSettings.instance.FilterSettings = clearFilter;

                var bakingSettingsNormal = MakeDefaultSettings();
                bakingSettingsNormal.DotsSettings = EntitiesClientSettings.instance;
                var bakingSettingsExclude = MakeDefaultSettings();
                bakingSettingsExclude.DotsSettings = EntitiesClientSettings.instance;

                // Bake the gameobject
                BakingUtility.BakeGameObjects(World, new[] {go}, bakingSettingsNormal);

                // With regular baking it is expected that the baker runs and the component is added
                var entity = m_BakingSystem.GetEntity(go);
                Assert.AreEqual(true, manager.HasComponent<ComponentInAssemblyComponent>(entity));

                // With the filter we expect that the baker hasn't run and the component is not in the entity
                EntitiesClientSettings.instance.FilterSettings = testFilter;
                BakingUtility.BakeGameObjects(World, new[] {go}, bakingSettingsExclude);
                entity = m_BakingSystem.GetEntity(go);
                Assert.AreEqual(false, manager.HasComponent<ComponentInAssemblyComponent>(entity));

                // We repeat the same two checks to make sure nothing is left that affects this
                EntitiesClientSettings.instance.FilterSettings = clearFilter;
                BakingUtility.BakeGameObjects(World, new[] {go}, bakingSettingsNormal);
                entity = m_BakingSystem.GetEntity(go);
                Assert.AreEqual(true, manager.HasComponent<ComponentInAssemblyComponent>(entity));

                EntitiesClientSettings.instance.FilterSettings = testFilter;
                BakingUtility.BakeGameObjects(World, new[] {go}, bakingSettingsExclude);
                entity = m_BakingSystem.GetEntity(go);
                Assert.AreEqual(false, manager.HasComponent<ComponentInAssemblyComponent>(entity));
            }
            finally
            {
                EntitiesClientSettings.instance.FilterSettings = originalFilter;
            }
        }
    }
}
