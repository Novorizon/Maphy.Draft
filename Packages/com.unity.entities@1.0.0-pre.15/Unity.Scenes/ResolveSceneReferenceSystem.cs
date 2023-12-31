//#define LOG_RESOLVING
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Serialization;
#if UNITY_EDITOR
using Unity.Assertions;
using Unity.Entities.Conversion;
using UnityEditor;
#endif
using Hash128 = Unity.Entities.Hash128;

namespace Unity.Scenes
{
    /// <summary>
    /// The ResolvedSectionEntity buffer registers all the scene section entities in an scene entity.
    /// </summary>
    public struct ResolvedSectionEntity : IBufferElementData
    {
        /// <summary>
        /// Section Entity
        /// </summary>
        public Entity SectionEntity;
    }


    /// <summary>
    /// This component references the scene entity that contains this scene section entity.
    /// </summary>
    public struct SceneEntityReference : IComponentData
    {
        /// <summary>
        /// Scene Entity
        /// </summary>
        public Entity SceneEntity;
    }

    struct ResolvedSceneHash : IComponentData
    {
        public Hash128 ArtifactHash;
    }

    struct ResolvedSectionPath : IComponentData
    {
        public FixedString512Bytes ScenePath;
        public UntypedWeakReferenceId HybridReferenceId;
    }

    struct SceneSectionCustomMetadata
    {
        public ulong StableTypeHash;
        public BlobArray<byte> Data;
    }

    // Size of SceneMetaData should be 16 byte aligned since this is stored as a blob with additional data following it
    [StructLayout(LayoutKind.Sequential, Size = 48)]
    struct SceneMetaData
    {
        public BlobArray<SceneSectionData> Sections;
        public BlobString                  SceneName;
        public BlobArray<BlobArray<SceneSectionCustomMetadata>> SceneSectionCustomMetadata;
        public int HeaderBlobAssetBatchSize;
    }

    /// <summary>
    /// Component to indicate that the scene or section loading should be temporarily disabled.
    /// </summary>
    /// <remarks>This component is applied during the live conversion patching.</remarks>
    public struct DisableSceneResolveAndLoad : IComponentData
    {
    }

    static class SceneMetaDataSerializeUtility
    {
        public static readonly int CurrentFileFormatVersion = 3;
    }

#if UNITY_EDITOR
    /// <summary>
    /// Scenes are made out of sections, but to find out how many sections there are and extract their data like bounding volume or file size.
    /// The meta data for the scene has to be loaded first.
    /// ResolveSceneReferenceSystem creates section entities for each scene by loading the scenesection's metadata from disk.
    /// </summary>
    [WorldSystemFilter(WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor)]
    [UpdateInGroup(typeof(SceneSystemGroup))]
    [UpdateAfter(typeof(SceneSystem))]
    partial class ResolveSceneReferenceSystem : SystemBase
    {
        struct AssetDependencyTrackerState : ICleanupComponentData
        {
            public UnityEditor.GUID SceneAndBuildConfigGUID;
        }

        EntityQuery m_AddScenes;
        EntityQuery m_RemoveScenes;
        EntityQuery m_ValidSceneQuery;
        EntityQueryMask m_ValidSceneMask;
        ResolveSceneSectionArchetypes m_ResolveSceneSectionArchetypes;

        AssetDependencyTracker<Entity> _AssetDependencyTracker;
        NativeList<AssetDependencyTracker<Entity>.Completed> _Changed;

        SystemHandle _sceneSystem;

        SceneHeaderUtility _SceneHeaderUtility;

        [Conditional("LOG_RESOLVING")]
        void LogResolving(string type, Hash128 sceneGUID)
        {
            Debug.Log(type + ": " + sceneGUID);
        }

        [Conditional("LOG_RESOLVING")]
        void LogResolving(string log)
        {
            Debug.Log(log);
        }

        protected override unsafe void OnUpdate()
        {
            SceneWithBuildConfigurationGUIDs.ValidateBuildSettingsCache();

            var buildConfigurationGUID = EntityManager.GetComponentData<SceneSystemData>(_sceneSystem).BuildConfigurationGUID;

            // Add scene entities that haven't been encountered yet
            if (!m_AddScenes.IsEmptyIgnoreFilter)
            {
                //@TODO: Should use Entities.ForEach but we are missing Entities.ForEach support for explicit queries

                using (var addScenes = m_AddScenes.ToEntityArray(Allocator.TempJob))
                {
                    var trackerStates = new NativeArray<AssetDependencyTrackerState>(addScenes.Length, Allocator.Temp);
                    for (int i = 0; i != addScenes.Length; i++)
                    {
                        var sceneEntity = addScenes[i];
                        var scene = EntityManager.GetComponentData<SceneReference>(sceneEntity);
                        var requestSceneLoaded = EntityManager.GetComponentData<RequestSceneLoaded>(sceneEntity);

                        var guid = SceneWithBuildConfigurationGUIDs.EnsureExistsFor(scene.SceneGUID,
                            buildConfigurationGUID, true, out var requireRefresh);
                        var async = (requestSceneLoaded.LoadFlags & SceneLoadFlags.BlockOnImport) == 0;

                        LogResolving(async ? "Adding Async" : "Adding Sync", guid);

                        _AssetDependencyTracker.Add(guid, sceneEntity, async);
                        if (requireRefresh)
                            _AssetDependencyTracker.RequestRefresh();

                        trackerStates[i] = new AssetDependencyTrackerState {SceneAndBuildConfigGUID = guid};
                    }

                    EntityManager.AddComponentData(m_AddScenes, trackerStates);
                    trackerStates.Dispose();
                }
            }

            // Remove scene entities that were added and should no longer be tracked
            if (!m_RemoveScenes.IsEmptyIgnoreFilter)
            {
                using (var removeEntities = m_RemoveScenes.ToEntityArray(Allocator.TempJob))
                using (var removeGuids =
                    m_RemoveScenes.ToComponentDataArray<AssetDependencyTrackerState>(Allocator.TempJob))
                {
                    for (int i = 0; i != removeEntities.Length; i++)
                    {
                        LogResolving("Removing", removeGuids[i].SceneAndBuildConfigGUID);
                        _AssetDependencyTracker.Remove(removeGuids[i].SceneAndBuildConfigGUID, removeEntities[i]);
                    }
                }

                EntityManager.RemoveComponent<AssetDependencyTrackerState>(m_RemoveScenes);
            }

            // Process any scenes that have completed their asset import
            var isDone = _AssetDependencyTracker.GetCompleted(_Changed);
            foreach (var change in _Changed)
            {
                var sceneEntity = change.UserKey;
                LogResolving($"Resolving: {change.Asset} -> {change.ArtifactID}");

                // This happens when instantiating an already fully resolved scene entity,
                // AssetDependencyTrackerState will be added and results in a completed change request,
                // but since this scene is already fully resolved with the same hash we can simply skip it.
                if (SystemAPI.HasComponent<ResolvedSceneHash>(sceneEntity) &&
                    SystemAPI.GetComponent<ResolvedSceneHash>(sceneEntity).ArtifactHash == (Hash128)change.ArtifactID)
                {
                    if (!SystemAPI.HasBuffer<ResolvedSectionEntity>(sceneEntity))
                        throw new InvalidOperationException($"Entity {sceneEntity} used for a scene load has a {nameof(ResolvedSceneHash)} component but no {nameof(ResolvedSectionEntity)} buffer. " +
                                                            "This suggests that you copied a scene entity after loading it, but before its scene data had been fully resolved. "+
                                                            $"Please only copy it after resolving has finished, which will add {nameof(ResolvedSectionEntity)} to the entity.");
                    continue;
                }

                if (!m_ValidSceneMask.MatchesIgnoreFilter(sceneEntity))
                    throw new InvalidOperationException("entity should have been removed from tracker already");

                // Unload any previous state
                var unloadFlags = SceneSystem.UnloadParameters.DestroySectionProxyEntities |
                                  SceneSystem.UnloadParameters.DontRemoveRequestSceneLoaded;
                SceneSystem.UnloadScene(World.Unmanaged, sceneEntity, unloadFlags);

                // Resolve new state
                var scene = EntityManager.GetComponentData<SceneReference>(change.UserKey);
                var request = EntityManager.GetComponentData<RequestSceneLoaded>(change.UserKey);

                // TODO: Remove updating the AssetDependencyTrackerState.SceneAndBuildConfigGUID on the changed scene entity when getting rid of the bakingEnabled toggle
                // Deal with Baking possibly changing by removing and adding new actual asset to tracker
                var guid = SceneWithBuildConfigurationGUIDs.EnsureExistsFor(scene.SceneGUID,
                    buildConfigurationGUID, true, out var requireRefresh);
                var async = (request.LoadFlags & SceneLoadFlags.BlockOnImport) == 0;

                if (change.Asset != (GUID)guid)
                {
                    LogResolving("Removing", change.Asset);
                    _AssetDependencyTracker.Remove(change.Asset, change.UserKey);
                    EntityManager.RemoveComponent<AssetDependencyTrackerState>(change.UserKey);

                    using (var currentAddedScenes = m_AddScenes.ToEntityArray(Allocator.TempJob))
                    {
                        var trackerStates = new NativeArray<AssetDependencyTrackerState>(currentAddedScenes.Length, Allocator.Temp);
                        for (int i = 0; i != currentAddedScenes.Length; i++)
                        {
                            //Update AssetDependencyTrackerState.SceneAndBuildConfigGUID with new guid
                            if (currentAddedScenes[i].Equals(change.UserKey))
                            {
                                LogResolving(async ? "Adding Async" : "Adding Sync", guid);
                                _AssetDependencyTracker.Add(guid, change.UserKey, async);
                                trackerStates[i] = new AssetDependencyTrackerState {SceneAndBuildConfigGUID = guid};
                            }
                        }
                        EntityManager.AddComponentData(m_AddScenes, trackerStates);
                        trackerStates.Dispose();
                    }
                }
                else
                {
                    if (change.ArtifactID != default)
                    {
                        LogResolving($"Schedule header load: {change.ArtifactID}");
                        SceneHeaderUtility.ScheduleHeaderLoadOnEntity(EntityManager, change.UserKey, scene.SceneGUID, request, change.ArtifactID, SceneSystem.SceneLoadDir);
                    }
                    else
                        Debug.LogError(
                            $"Failed to import entity scene because the automatically generated SceneAndBuildConfigGUID asset was not present: '{AssetDatabaseCompatibility.GuidToPath(scene.SceneGUID)}' -> '{AssetDatabaseCompatibility.GuidToPath(change.Asset)}'");
                }

                if (requireRefresh)
                    _AssetDependencyTracker.RequestRefresh();
            }

            _SceneHeaderUtility.CleanupHeaders(EntityManager);

            bool headerLoadInProgress = false;
            Entities.WithStructuralChanges().WithNone<DisableSceneResolveAndLoad, ResolvedSectionEntity>().ForEach(
                (Entity sceneEntity, ref RequestSceneHeader requestHeader, ref SceneReference scene,
                    ref ResolvedSceneHash resolvedSceneHash, ref RequestSceneLoaded requestSceneLoaded) =>
                {
                        if (!requestHeader.IsCompleted)
                        {
                            if ((requestSceneLoaded.LoadFlags & SceneLoadFlags.BlockOnImport) == 0)
                            {
                                headerLoadInProgress = true;
                                return;
                            }
                            requestHeader.Complete();
                        }

                        using (var headerLoadResult = SceneHeaderUtility.FinishHeaderLoad(requestHeader,
                                   scene.SceneGUID,
                                   SceneSystem.SceneLoadDir))
                        {
                            LogResolving($"Finished header load: {scene.SceneGUID}");
                            if (!headerLoadResult.Success)
                            {
                                requestHeader.Dispose();
                                EntityManager.AddBuffer<ResolvedSectionEntity>(sceneEntity);
                                EntityManager.RemoveComponent<RequestSceneHeader>(sceneEntity);
                                return;
                            }

                            ResolveSceneSectionUtility.ResolveSceneSections(EntityManager, sceneEntity, requestSceneLoaded, ref headerLoadResult.SceneMetaData.Value,
                                m_ResolveSceneSectionArchetypes, headerLoadResult.SectionPaths, headerLoadResult.HeaderBlobOwner);
                            requestHeader.Dispose();
                            EntityManager.RemoveComponent<RequestSceneHeader>(sceneEntity);
                        }
#if UNITY_EDITOR
                        if (EntityManager.HasComponent<SubScene>(sceneEntity))
                        {
                            var subScene = EntityManager.GetComponentObject<SubScene>(sceneEntity);
                            // Add SubScene component to section entities
                            using (var sectionEntities = EntityManager.GetBuffer<ResolvedSectionEntity>(sceneEntity).ToNativeArray(Allocator.Temp))
                            {
                                for (int iSection = 0; iSection < sectionEntities.Length;++iSection)
                                    EntityManager.AddComponentObject(sectionEntities[iSection].SectionEntity, subScene);
                            }
                        }
#endif
                }).Run();

            if(headerLoadInProgress)
                EditorUpdateUtility.EditModeQueuePlayerLoopUpdate();

            if (!isDone)
                EditorUpdateUtility.EditModeQueuePlayerLoopUpdate();
        }

        protected override void OnCreate()
        {
            _sceneSystem = World.GetExistingSystem<SceneSystem>();
            _AssetDependencyTracker =
                new AssetDependencyTracker<Entity>(EntityScenesPaths.SubSceneImporterType, "Import EntityScene");
            _Changed = new NativeList<AssetDependencyTracker<Entity>.Completed>(32, Allocator.Persistent);
            _SceneHeaderUtility = new SceneHeaderUtility(this);
            m_ValidSceneQuery = GetEntityQuery(new EntityQueryDesc
                {
                    All = new[]
                    {
                        ComponentType.ReadOnly<SceneReference>(), ComponentType.ReadOnly<RequestSceneLoaded>(), ComponentType.ReadOnly<AssetDependencyTrackerState>()
                    },
                    None = new[]
                    {
                        ComponentType.ReadOnly<DisableSceneResolveAndLoad>(),
                    }
                });
            Assert.IsFalse(m_ValidSceneQuery.HasFilter(), "The use of EntityQueryMask in this system will not respect the query's active filter settings.");
            m_ValidSceneMask = m_ValidSceneQuery.GetEntityQueryMask();

            m_AddScenes = GetEntityQuery(
                new EntityQueryDesc
                {
                    All = new[]
                    {
                        ComponentType.ReadOnly<SceneReference>(), ComponentType.ReadOnly<RequestSceneLoaded>()
                    },
                    None = new[]
                    {
                        ComponentType.ReadOnly<DisableSceneResolveAndLoad>(),
                        ComponentType.ReadOnly<AssetDependencyTrackerState>(),
                    }
                });

            m_RemoveScenes = GetEntityQuery(
                new EntityQueryDesc
                {
                    All = new[] {ComponentType.ReadOnly<AssetDependencyTrackerState>()},
                    None = new[]
                    {
                        ComponentType.ReadOnly<SceneReference>(),
                        ComponentType.ReadOnly<RequestSceneLoaded>(),
                    }
                },
                new EntityQueryDesc
                {
                    All = new[]
                    {
                        ComponentType.ReadOnly<AssetDependencyTrackerState>(),
                        ComponentType.ReadOnly<DisableSceneResolveAndLoad>()
                    }
                },
                new EntityQueryDesc
                {
                    All = new[]
                    {
                        ComponentType.ReadOnly<AssetDependencyTrackerState>(), ComponentType.ReadOnly<Disabled>()
                    }
                }
            );

            m_ResolveSceneSectionArchetypes = ResolveSceneSectionUtility.CreateResolveSceneSectionArchetypes(EntityManager);
        }

        protected override void OnDestroy()
        {
            _AssetDependencyTracker.Dispose();
            _Changed.Dispose();
            _SceneHeaderUtility.Dispose(EntityManager);
        }
    }

#else
    /// <summary>
    /// Scenes are made out of sections, but to find out how many sections there are and extract their data like bounding volume or file size.
    /// The meta data for the scene has to be loaded first.
    /// ResolveSceneReferenceSystem creates section entities for each scene by loading the scenesection's metadata from disk.
    /// </summary>
    [UpdateInGroup(typeof(SceneSystemGroup))]
    [UpdateAfter(typeof(SceneSystem))]
    partial class ResolveSceneReferenceSystem : SystemBase
    {
        private SceneHeaderUtility _SceneHeaderUtility;
        private ResolveSceneSectionArchetypes m_ResolveSceneSectionArchetypes;

        protected override void OnCreate()
        {
            _SceneHeaderUtility = new SceneHeaderUtility(this);
            m_ResolveSceneSectionArchetypes = ResolveSceneSectionUtility.CreateResolveSceneSectionArchetypes(EntityManager);
        }

        protected override void OnDestroy()
        {
            _SceneHeaderUtility.Dispose(EntityManager);
        }

        protected unsafe override void OnUpdate()
        {
            Entities.WithStructuralChanges().WithNone<DisableSceneResolveAndLoad, ResolvedSectionEntity, RequestSceneHeader>().
                ForEach((Entity sceneEntity, ref SceneReference scene, ref RequestSceneLoaded requestSceneLoaded) =>
                {
                    SceneHeaderUtility.ScheduleHeaderLoadOnEntity(EntityManager, sceneEntity, scene.SceneGUID, requestSceneLoaded, default, SceneSystem.SceneLoadDir);
                }).Run();

            _SceneHeaderUtility.CleanupHeaders(EntityManager);

            Entities.WithStructuralChanges().WithNone<DisableSceneResolveAndLoad, ResolvedSectionEntity>().ForEach(
                (Entity sceneEntity, ref RequestSceneHeader requestHeader, ref SceneReference scene, ref ResolvedSceneHash resolvedSceneHash, ref RequestSceneLoaded requestSceneLoaded) =>
                {
                    if (!requestHeader.IsCompleted)
                    {
#if !UNITY_DOTSRUNTIME // DOTS Runtime does not support blocking on IO
                        if((requestSceneLoaded.LoadFlags & SceneLoadFlags.BlockOnImport) == 0)
                            return;
                        requestHeader.Complete();
#else
                        return;
#endif
                    }

                    using(var headerLoadResult = SceneHeaderUtility.FinishHeaderLoad(requestHeader, scene.SceneGUID, SceneSystem.SceneLoadDir))
                    {
                        if (!headerLoadResult.Success)
                        {
                            requestHeader.Dispose();
                            EntityManager.AddBuffer<ResolvedSectionEntity>(sceneEntity);
                            EntityManager.RemoveComponent<RequestSceneHeader>(sceneEntity);
                            return;
                        }

                        ResolveSceneSectionUtility.ResolveSceneSections(EntityManager, sceneEntity, requestSceneLoaded, ref headerLoadResult.SceneMetaData.Value,
                            m_ResolveSceneSectionArchetypes, headerLoadResult.SectionPaths, headerLoadResult.HeaderBlobOwner);
                    }
                    requestHeader.Dispose();
                    EntityManager.RemoveComponent<RequestSceneHeader>(sceneEntity);
                }).Run();
        }
    }
#endif

}

