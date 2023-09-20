//HintName: Verify.gen__Aspect_977025139.g.cs
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Tests;


    public readonly partial struct MyAspectEFE : global::Unity.Entities.IAspect, global::Unity.Entities.IAspectCreate<MyAspectEFE>
    {
        MyAspectEFE(global::Unity.Entities.RefRW<global::Unity.Entities.Tests.EcsTestData> data)
        {
            this.Data = data;


        }
        public MyAspectEFE CreateAspect(global::Unity.Entities.Entity entity, ref global::Unity.Entities.SystemState systemState, bool isReadOnly)
        {
            var lookup = new Lookup(ref systemState, isReadOnly);
            return lookup[entity];
        }

        public static global::Unity.Entities.ComponentType[] ExcludeComponents => global::System.Array.Empty<Unity.Entities.ComponentType>();
        static global::Unity.Entities.ComponentType[] s_RequiredComponents => new [] {  global::Unity.Entities.ComponentType.ReadWrite<global::Unity.Entities.Tests.EcsTestData>() };
        static global::Unity.Entities.ComponentType[] s_RequiredComponentsRO => new [] {  global::Unity.Entities.ComponentType.ReadOnly<global::Unity.Entities.Tests.EcsTestData>() };
        public static global::Unity.Entities.ComponentType[] RequiredComponents => s_RequiredComponents;
        public static global::Unity.Entities.ComponentType[] RequiredComponentsRO => s_RequiredComponentsRO;
        public struct Lookup
        {
            bool _IsReadOnly
            {
                get { return __IsReadOnly == 1; }
                set { __IsReadOnly = value ? (byte) 1 : (byte) 0; }
            }
            private byte __IsReadOnly;

            global::Unity.Entities.ComponentLookup<global::Unity.Entities.Tests.EcsTestData> DataComponentLookup;



            public Lookup(ref global::Unity.Entities.SystemState state, bool isReadOnly)
            {
                __IsReadOnly = isReadOnly ? (byte) 1u : (byte) 0u;
                this.DataComponentLookup = state.GetComponentLookup<global::Unity.Entities.Tests.EcsTestData>(isReadOnly);



            }
            public void Update(ref global::Unity.Entities.SystemState state)
            {
                this.DataComponentLookup.Update(ref state);


            }
            public MyAspectEFE this[global::Unity.Entities.Entity entity]
            {
                get
                {
                    return new MyAspectEFE(this.DataComponentLookup.GetRefRW(entity, _IsReadOnly));
                }
            }
        }
        public struct ResolvedChunk
        {

            internal global::Unity.Collections.NativeArray<global::Unity.Entities.Tests.EcsTestData> Data;


            public MyAspectEFE this[int index]
            {
                get
                {
                    return new MyAspectEFE(                        new global::Unity.Entities.RefRW<Unity.Entities.Tests.EcsTestData>(this.Data, index));
                }
            }
            public int Length;
        }
        public struct TypeHandle
        {
            global::Unity.Entities.ComponentTypeHandle<global::Unity.Entities.Tests.EcsTestData> DataCth;




            public TypeHandle(ref global::Unity.Entities.SystemState state, bool isReadOnly)
            {
                this.DataCth = state.GetComponentTypeHandle<global::Unity.Entities.Tests.EcsTestData>(isReadOnly);




            }
            public void Update(ref global::Unity.Entities.SystemState state)
            {
                this.DataCth.Update(ref state);



            }
            public ResolvedChunk Resolve(global::Unity.Entities.ArchetypeChunk chunk)
            {
                ResolvedChunk resolved;


                resolved.Data = chunk.GetNativeArray(ref this.DataCth);

                resolved.Length = chunk.Count;
                return resolved;
            }
        }
        public static Enumerator Query(global::Unity.Entities.EntityQuery query, TypeHandle typeHandle) { return new Enumerator(query, typeHandle); }
        public struct Enumerator : global::System.Collections.Generic.IEnumerator<MyAspectEFE>, global::System.Collections.Generic.IEnumerable<MyAspectEFE>
        {
            ResolvedChunk                                _Resolved;
            global::Unity.Entities.EntityQueryEnumerator _QueryEnumerator;
            TypeHandle                                   _Handle;
            internal Enumerator(global::Unity.Entities.EntityQuery query, TypeHandle typeHandle)
            {
                _QueryEnumerator = new global::Unity.Entities.EntityQueryEnumerator(query);
                _Handle = typeHandle;
                _Resolved = default;
            }
            public void Dispose() { _QueryEnumerator.Dispose(); }
            public bool MoveNext()
            {
                if (_QueryEnumerator.MoveNextHotLoop())
                    return true;
                return MoveNextCold();
            }
            [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
            bool MoveNextCold()
            {
                var didMove = _QueryEnumerator.MoveNextColdLoop(out var chunk);
                if (didMove)
                    _Resolved = _Handle.Resolve(chunk);
                return didMove;
            }
            public MyAspectEFE Current {
                get {
                    #if ENABLE_UNITY_COLLECTIONS_CHECKS || UNITY_DOTS_DEBUG
                        _QueryEnumerator.CheckDisposed();
                    #endif
                        return _Resolved[_QueryEnumerator.IndexInChunk];
                    }
            }
            public Enumerator GetEnumerator()  { return this; }
            void global::System.Collections.IEnumerator.Reset() => throw new global::System.NotImplementedException();
            object global::System.Collections.IEnumerator.Current => throw new global::System.NotImplementedException();
            global::System.Collections.Generic.IEnumerator<MyAspectEFE> global::System.Collections.Generic.IEnumerable<MyAspectEFE>.GetEnumerator() => throw new global::System.NotImplementedException();
            global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()=> throw new global::System.NotImplementedException();
        }

        /// <summary>
        /// Completes the dependency chain required for this aspect to have read access.
        /// So it completes all write dependencies of the components, buffers, etc. to allow for reading.
        /// </summary>
        /// <param name="state">The <see cref="SystemState"/> containing an <see cref="EntityManager"/> storing all dependencies.</param>
        public static void CompleteDependencyBeforeRO(ref global::Unity.Entities.SystemState state){
           state.EntityManager.CompleteDependencyBeforeRO<global::Unity.Entities.Tests.EcsTestData>();
        }

        /// <summary>
        /// Completes the dependency chain required for this component to have read and write access.
        /// So it completes all write dependencies of the components, buffers, etc. to allow for reading,
        /// and it completes all read dependencies, so we can write to it.
        /// </summary>
        /// <param name="state">The <see cref="SystemState"/> containing an <see cref="EntityManager"/> storing all dependencies.</param>
        public static void CompleteDependencyBeforeRW(ref global::Unity.Entities.SystemState state){
           state.EntityManager.CompleteDependencyBeforeRW<global::Unity.Entities.Tests.EcsTestData>();
        }
    }


    public readonly partial struct MyAspectEFE2 : global::Unity.Entities.IAspect, global::Unity.Entities.IAspectCreate<MyAspectEFE2>
    {
        MyAspectEFE2(global::Unity.Entities.RefRW<global::Unity.Entities.Tests.EcsTestData2> data)
        {
            this.Data = data;


        }
        public MyAspectEFE2 CreateAspect(global::Unity.Entities.Entity entity, ref global::Unity.Entities.SystemState systemState, bool isReadOnly)
        {
            var lookup = new Lookup(ref systemState, isReadOnly);
            return lookup[entity];
        }

        public static global::Unity.Entities.ComponentType[] ExcludeComponents => global::System.Array.Empty<Unity.Entities.ComponentType>();
        static global::Unity.Entities.ComponentType[] s_RequiredComponents => new [] {  global::Unity.Entities.ComponentType.ReadWrite<global::Unity.Entities.Tests.EcsTestData2>() };
        static global::Unity.Entities.ComponentType[] s_RequiredComponentsRO => new [] {  global::Unity.Entities.ComponentType.ReadOnly<global::Unity.Entities.Tests.EcsTestData2>() };
        public static global::Unity.Entities.ComponentType[] RequiredComponents => s_RequiredComponents;
        public static global::Unity.Entities.ComponentType[] RequiredComponentsRO => s_RequiredComponentsRO;
        public struct Lookup
        {
            bool _IsReadOnly
            {
                get { return __IsReadOnly == 1; }
                set { __IsReadOnly = value ? (byte) 1 : (byte) 0; }
            }
            private byte __IsReadOnly;

            global::Unity.Entities.ComponentLookup<global::Unity.Entities.Tests.EcsTestData2> DataComponentLookup;



            public Lookup(ref global::Unity.Entities.SystemState state, bool isReadOnly)
            {
                __IsReadOnly = isReadOnly ? (byte) 1u : (byte) 0u;
                this.DataComponentLookup = state.GetComponentLookup<global::Unity.Entities.Tests.EcsTestData2>(isReadOnly);



            }
            public void Update(ref global::Unity.Entities.SystemState state)
            {
                this.DataComponentLookup.Update(ref state);


            }
            public MyAspectEFE2 this[global::Unity.Entities.Entity entity]
            {
                get
                {
                    return new MyAspectEFE2(this.DataComponentLookup.GetRefRW(entity, _IsReadOnly));
                }
            }
        }
        public struct ResolvedChunk
        {

            internal global::Unity.Collections.NativeArray<global::Unity.Entities.Tests.EcsTestData2> Data;


            public MyAspectEFE2 this[int index]
            {
                get
                {
                    return new MyAspectEFE2(                        new global::Unity.Entities.RefRW<Unity.Entities.Tests.EcsTestData2>(this.Data, index));
                }
            }
            public int Length;
        }
        public struct TypeHandle
        {
            global::Unity.Entities.ComponentTypeHandle<global::Unity.Entities.Tests.EcsTestData2> DataCth;




            public TypeHandle(ref global::Unity.Entities.SystemState state, bool isReadOnly)
            {
                this.DataCth = state.GetComponentTypeHandle<global::Unity.Entities.Tests.EcsTestData2>(isReadOnly);




            }
            public void Update(ref global::Unity.Entities.SystemState state)
            {
                this.DataCth.Update(ref state);



            }
            public ResolvedChunk Resolve(global::Unity.Entities.ArchetypeChunk chunk)
            {
                ResolvedChunk resolved;


                resolved.Data = chunk.GetNativeArray(ref this.DataCth);

                resolved.Length = chunk.Count;
                return resolved;
            }
        }
        public static Enumerator Query(global::Unity.Entities.EntityQuery query, TypeHandle typeHandle) { return new Enumerator(query, typeHandle); }
        public struct Enumerator : global::System.Collections.Generic.IEnumerator<MyAspectEFE2>, global::System.Collections.Generic.IEnumerable<MyAspectEFE2>
        {
            ResolvedChunk                                _Resolved;
            global::Unity.Entities.EntityQueryEnumerator _QueryEnumerator;
            TypeHandle                                   _Handle;
            internal Enumerator(global::Unity.Entities.EntityQuery query, TypeHandle typeHandle)
            {
                _QueryEnumerator = new global::Unity.Entities.EntityQueryEnumerator(query);
                _Handle = typeHandle;
                _Resolved = default;
            }
            public void Dispose() { _QueryEnumerator.Dispose(); }
            public bool MoveNext()
            {
                if (_QueryEnumerator.MoveNextHotLoop())
                    return true;
                return MoveNextCold();
            }
            [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
            bool MoveNextCold()
            {
                var didMove = _QueryEnumerator.MoveNextColdLoop(out var chunk);
                if (didMove)
                    _Resolved = _Handle.Resolve(chunk);
                return didMove;
            }
            public MyAspectEFE2 Current {
                get {
                    #if ENABLE_UNITY_COLLECTIONS_CHECKS || UNITY_DOTS_DEBUG
                        _QueryEnumerator.CheckDisposed();
                    #endif
                        return _Resolved[_QueryEnumerator.IndexInChunk];
                    }
            }
            public Enumerator GetEnumerator()  { return this; }
            void global::System.Collections.IEnumerator.Reset() => throw new global::System.NotImplementedException();
            object global::System.Collections.IEnumerator.Current => throw new global::System.NotImplementedException();
            global::System.Collections.Generic.IEnumerator<MyAspectEFE2> global::System.Collections.Generic.IEnumerable<MyAspectEFE2>.GetEnumerator() => throw new global::System.NotImplementedException();
            global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()=> throw new global::System.NotImplementedException();
        }

        /// <summary>
        /// Completes the dependency chain required for this aspect to have read access.
        /// So it completes all write dependencies of the components, buffers, etc. to allow for reading.
        /// </summary>
        /// <param name="state">The <see cref="SystemState"/> containing an <see cref="EntityManager"/> storing all dependencies.</param>
        public static void CompleteDependencyBeforeRO(ref global::Unity.Entities.SystemState state){
           state.EntityManager.CompleteDependencyBeforeRO<global::Unity.Entities.Tests.EcsTestData2>();
        }

        /// <summary>
        /// Completes the dependency chain required for this component to have read and write access.
        /// So it completes all write dependencies of the components, buffers, etc. to allow for reading,
        /// and it completes all read dependencies, so we can write to it.
        /// </summary>
        /// <param name="state">The <see cref="SystemState"/> containing an <see cref="EntityManager"/> storing all dependencies.</param>
        public static void CompleteDependencyBeforeRW(ref global::Unity.Entities.SystemState state){
           state.EntityManager.CompleteDependencyBeforeRW<global::Unity.Entities.Tests.EcsTestData2>();
        }
    }
