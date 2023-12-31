﻿using Unity.Assertions;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;

namespace Unity.Entities.Hybrid.Baking
{
    [DisableAutoCreation]
    [RequireMatchingQueriesForUpdate]
    internal partial class LinkedEntityGroupBaking : SystemBase
    {
        private EntityQuery _AdditionalEntityquery;
        private EntityQuery _LinkedEntityGroupBakingDataQuery;
        private EntityQuery _NoBakeOnlyQuery;
        private EntityQueryMask _HasAdditionalEntityMask;
        private EntityQueryMask _NoBakeOnlyMask;

        protected override void OnCreate()
        {
            EntityQueryDesc desc = new EntityQueryDesc()
            {
                All = new[] {ComponentType.FromTypeIndex(TypeManager.GetTypeIndex<AdditionalEntitiesBakingData>())},
                Options = EntityQueryOptions.IncludeDisabledEntities | EntityQueryOptions.IncludePrefab
            };

            EntityQueryDesc descNoBakingOnly = new EntityQueryDesc()
            {
                None = new[] {ComponentType.FromTypeIndex(TypeManager.GetTypeIndex<BakingOnlyEntity>())},
                Options = EntityQueryOptions.IncludeDisabledEntities | EntityQueryOptions.IncludePrefab
            };

            // Only add LinkedEntityGroups to Entities that are not marked BakingOnlyEntity
            EntityQueryDesc legTempDesc = new EntityQueryDesc()
            {
                All = new[] {ComponentType.FromTypeIndex(TypeManager.GetTypeIndex<LinkedEntityGroupBakingData>())},
                None = new[] {ComponentType.FromTypeIndex(TypeManager.GetTypeIndex<BakingOnlyEntity>())},
                Options = EntityQueryOptions.IncludeDisabledEntities | EntityQueryOptions.IncludePrefab
            };

            _AdditionalEntityquery = GetEntityQuery(desc);
            _LinkedEntityGroupBakingDataQuery = GetEntityQuery(legTempDesc);
            Assert.IsFalse(_LinkedEntityGroupBakingDataQuery.HasFilter(), "The use of EntityQueryMask in this job will not respect the query's active filter settings.");
            _HasAdditionalEntityMask = _AdditionalEntityquery.GetEntityQueryMask();
            _NoBakeOnlyQuery = GetEntityQuery(descNoBakingOnly);
            Assert.IsFalse(_NoBakeOnlyQuery.HasFilter(), "The use of EntityQueryMask in this job will not respect the query's active filter settings.");
            _NoBakeOnlyMask = _NoBakeOnlyQuery.GetEntityQueryMask();
        }

        protected override void OnUpdate()
        {
            var cmd = new EntityCommandBuffer(Allocator.TempJob);
            var additionalEntityJob = new AddLinkedEntityGroupBakingJob
            {
                AdditionalEntities = EntityManager.GetBufferLookup<AdditionalEntitiesBakingData>(),
                LinkedEntityGroupBakingDataHandle = EntityManager.GetBufferTypeHandle<LinkedEntityGroupBakingData>(true),
                Entities = EntityManager.GetEntityTypeHandle(),
                Commands = cmd.AsParallelWriter(),
                HasAdditionalEntityMask = _HasAdditionalEntityMask,
                NoBakeOnlyMask = _NoBakeOnlyMask
            };
            var jobHandle = additionalEntityJob.ScheduleParallelByRef(_LinkedEntityGroupBakingDataQuery, Dependency);

            jobHandle.Complete();
            cmd.Playback(EntityManager);
            cmd.Dispose();
        }
    }

    [BurstCompile]
    unsafe struct AddLinkedEntityGroupBakingJob : IJobChunk
    {
        public EntityTypeHandle                              Entities;
        public EntityQueryMask                               HasAdditionalEntityMask;
        public EntityQueryMask                               NoBakeOnlyMask;
        [ReadOnly]
        public BufferLookup<AdditionalEntitiesBakingData>      AdditionalEntities;
        public BufferTypeHandle<LinkedEntityGroupBakingData>       LinkedEntityGroupBakingDataHandle;
        public EntityCommandBuffer.ParallelWriter            Commands;

        public void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
        {
            Assert.IsFalse(useEnabledMask);
            var legBuffer = chunk.GetBufferAccessor(ref LinkedEntityGroupBakingDataHandle);
            var entities = chunk.GetNativeArray(Entities);

            for (int i = 0, chunkEntityCount = chunk.Count; i < chunkEntityCount; i++)
            {
                var leg = legBuffer[i];
                Commands.AddBuffer<LinkedEntityGroup>(unfilteredChunkIndex, entities[i]);

                for (var j = 0; j < leg.Length; j++)
                {
                    var entity = leg[j].Value;
                    // Only append if not BakeOnlyEntity
                    if (NoBakeOnlyMask.MatchesIgnoreFilter(entity))
                    {
                        Commands.AppendToBuffer(unfilteredChunkIndex, entities[i], new LinkedEntityGroup() {Value = entity});
                    }

                    if (HasAdditionalEntityMask.MatchesIgnoreFilter(entity))
                    {

                        var additionalEntities = AdditionalEntities[entity];
                        for (var k = 0; k < additionalEntities.Length; k++)
                        {
                            if (NoBakeOnlyMask.MatchesIgnoreFilter(additionalEntities[k].Value))
                            {
                                Commands.AppendToBuffer(unfilteredChunkIndex, entities[i], new LinkedEntityGroup() {Value = additionalEntities[k].Value});
                            }
                        }
                    }
                }
            }
        }
    }

    [UpdateInGroup(typeof(PreBakingSystemGroup))]
    [WorldSystemFilter(WorldSystemFilterFlags.BakingSystem)]
    partial class LinkedEntityGroupBakingCleanUp : SystemBase
    {
        private EntityQuery query;

        protected override void OnCreate()
        {
            query = GetEntityQuery(typeof(LinkedEntityGroup));
        }

        protected override void OnUpdate()
        {
            EntityManager.RemoveComponent<LinkedEntityGroup>(query);
        }
    }
}
