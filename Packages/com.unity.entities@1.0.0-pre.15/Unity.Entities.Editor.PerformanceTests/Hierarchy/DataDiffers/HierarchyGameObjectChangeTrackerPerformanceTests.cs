using NUnit.Framework;
using Unity.Collections;
using Unity.Editor.Bridge;
using Unity.Jobs;
using Unity.PerformanceTesting;

namespace Unity.Entities.Editor.PerformanceTests
{
    [TestFixture]
    [Category(Categories.Performance)]
    class HierarchyGameObjectChangeTrackerPerformanceTests
    {
        [Test, Performance]
        public void HierarchyGameObjectChangeTracker_MergeEvents_AddNewEvents([Values(10_000, 100_000)] int existingEventCount, [Values(500, 4000)] int eventsToAddCount)
        {
            NativeList<GameObjectChangeTrackerEvent> events = default;
            NativeArray<GameObjectChangeTrackerEvent> eventsToAdd = default;
            NativeHashMap<int, int> eventsIndex = default;

            Measure.Method(() =>
                {
                    var mergeJob = new HierarchyGameObjectChangeTracker.MergeEventsJob { Events = events, EventsToAdd = eventsToAdd, EventsIndex = eventsIndex };
                    mergeJob.Run();
                })
                .SetUp(() =>
                {
                    events = new NativeList<GameObjectChangeTrackerEvent>(existingEventCount, Allocator.TempJob);
                    eventsToAdd = new NativeArray<GameObjectChangeTrackerEvent>(eventsToAddCount, Allocator.TempJob);
                    eventsIndex = new NativeHashMap<int, int>(existingEventCount, Allocator.TempJob);

                    for (var i = 0; i < existingEventCount; i++)
                    {
                        eventsIndex.Add(i, events.Length);
                        events.Add(new GameObjectChangeTrackerEvent(i, GameObjectChangeTrackerEventType.CreatedOrChanged));
                    }

                    for (var i = 0; i < eventsToAddCount; i++)
                    {
                        eventsToAdd[i] = new GameObjectChangeTrackerEvent(existingEventCount + i, GameObjectChangeTrackerEventType.CreatedOrChanged);
                    }
                })
                .CleanUp(() =>
                {
                    eventsIndex.Dispose();
                    events.Dispose();
                    eventsToAdd.Dispose();
                })

                .WarmupCount(5)
                .MeasurementCount(50)
                .Run();
        }

        [Test, Performance]
        public void HierarchyGameObjectChangeTracker_MergeEvents_AddExistingEvents([Values(10_000, 100_000)] int existingEventCount, [Values(500, 4000)] int eventsToAddCount)
        {
            NativeList<GameObjectChangeTrackerEvent> events = default;
            NativeArray<GameObjectChangeTrackerEvent> eventsToAdd = default;
            NativeHashMap<int, int> eventsIndex = default;

            Measure.Method(() =>
                {
                    var mergeJob = new HierarchyGameObjectChangeTracker.MergeEventsJob { Events = events, EventsToAdd = eventsToAdd, EventsIndex = eventsIndex };
                    mergeJob.Run();
                })
                .SetUp(() =>
                {
                    events = new NativeList<GameObjectChangeTrackerEvent>(existingEventCount, Allocator.TempJob);
                    eventsToAdd = new NativeArray<GameObjectChangeTrackerEvent>(eventsToAddCount, Allocator.TempJob);
                    eventsIndex = new NativeHashMap<int, int>(existingEventCount, Allocator.TempJob);

                    for (var i = 0; i < existingEventCount; i++)
                    {
                        eventsIndex.Add(i, events.Length);
                        events.Add(new GameObjectChangeTrackerEvent(i, GameObjectChangeTrackerEventType.CreatedOrChanged));
                    }

                    for (var i = 0; i < eventsToAddCount; i++)
                    {
                        eventsToAdd[i] = new GameObjectChangeTrackerEvent(i, GameObjectChangeTrackerEventType.OrderChanged);
                    }
                })
                .CleanUp(() =>
                {
                    eventsIndex.Dispose();
                    events.Dispose();
                    eventsToAdd.Dispose();
                })
                .WarmupCount(5)
                .MeasurementCount(50)
                .Run();
        }

        [Test, Performance]
        public void HierarchyGameObjectChangeTracker_MergeEvents_AddDuplicateEvents([Values(10_000, 100_000)] int existingEventCount, [Values(500, 4000)] int eventsToAddCount)
        {
            NativeList<GameObjectChangeTrackerEvent> events = default;
            NativeArray<GameObjectChangeTrackerEvent> eventsToAdd = default;
            NativeHashMap<int, int> eventsIndex = default;

            Measure.Method(() =>
                {
                    var mergeJob = new HierarchyGameObjectChangeTracker.MergeEventsJob { Events = events, EventsToAdd = eventsToAdd, EventsIndex = eventsIndex };
                    mergeJob.Run();
                })
                .SetUp(() =>
                {
                    events = new NativeList<GameObjectChangeTrackerEvent>(existingEventCount, Allocator.TempJob);
                    eventsToAdd = new NativeArray<GameObjectChangeTrackerEvent>(eventsToAddCount, Allocator.TempJob);
                    eventsIndex = new NativeHashMap<int, int>(existingEventCount, Allocator.TempJob);

                    for (var i = 0; i < existingEventCount; i++)
                    {
                        eventsIndex.Add(i, events.Length);
                        events.Add(new GameObjectChangeTrackerEvent(i, GameObjectChangeTrackerEventType.CreatedOrChanged));
                    }

                    for (var i = 0; i < eventsToAddCount; i++)
                    {
                        eventsToAdd[i] = new GameObjectChangeTrackerEvent(i, GameObjectChangeTrackerEventType.CreatedOrChanged);
                    }
                })
                .CleanUp(() =>
                {
                    eventsIndex.Dispose();
                    events.Dispose();
                    eventsToAdd.Dispose();
                })
                .WarmupCount(5)
                .MeasurementCount(50)
                .Run();
        }

        [Test, Performance]
        public void HierarchyGameObjectChangeTracker_MergeEvents_AddNewEvents_Continuously(
            [Values(10_000, 100_000)] int existingEventCount,
            [Values(500, 4000)] int eventsToAddCount)
        {
            const int repeats = 25;

            NativeList<GameObjectChangeTrackerEvent> events = default;
            NativeArray<GameObjectChangeTrackerEvent> eventsToAdd = default;
            NativeArray<GameObjectChangeTrackerEvent> eventsBatchToAdd = default;
            NativeHashMap<int, int> eventsIndex = default;

            Measure.Method(() =>
            {
                for (var i = 0; i < repeats; i++)
                {
                    NativeArray<GameObjectChangeTrackerEvent>.Copy(eventsToAdd, i * eventsToAddCount, eventsBatchToAdd, 0, eventsToAddCount);
                    new HierarchyGameObjectChangeTracker.MergeEventsJob
                    {
                        Events = events,
                        EventsToAdd = eventsBatchToAdd,
                        EventsIndex = eventsIndex
                    }.Run();
                }
            })
            .SetUp(() =>
            {
                events = new NativeList<GameObjectChangeTrackerEvent>(existingEventCount, Allocator.TempJob);
                eventsToAdd = new NativeArray<GameObjectChangeTrackerEvent>(eventsToAddCount * repeats, Allocator.TempJob);
                eventsBatchToAdd = new NativeArray<GameObjectChangeTrackerEvent>(eventsToAddCount, Allocator.TempJob);
                eventsIndex = new NativeHashMap<int, int>(existingEventCount, Allocator.TempJob);

                for (var i = 0; i < existingEventCount; i++)
                {
                    eventsIndex.Add(i, events.Length);
                    events.Add(new GameObjectChangeTrackerEvent(i, GameObjectChangeTrackerEventType.CreatedOrChanged));
                }

                for (var i = 0; i < eventsToAddCount * repeats; i++)
                {
                    eventsToAdd[i] = new GameObjectChangeTrackerEvent(existingEventCount + i, GameObjectChangeTrackerEventType.CreatedOrChanged);
                }
            })
            .CleanUp(() =>
            {
                eventsIndex.Dispose();
                events.Dispose();
                eventsToAdd.Dispose();
                eventsBatchToAdd.Dispose();
            })
            .WarmupCount(5)
            .MeasurementCount(50)
            .Run();
        }
    }
}
