using System.Collections;
using System.Collections.Generic;

namespace Unity.Entities
{
    /// <summary>An enumeration interface for the entities that match an <see cref="EntityQuery"/>.</summary>
    /// <remarks>
    /// This feature is primarily intended as a backend implementation for <see cref="SystemAPI.Query"/>. Application code
    /// should prefer to use that interface rather than using this type directly.
    /// </remarks>
    /// <typeparam name="T1">A component type</typeparam>
    public struct QueryEnumerable<T1> : IEnumerable<T1>
    {
        /// <summary>
        /// Specify all read-only component types that must be present.
        /// </summary>
        /// <typeparam name="TComponent1">A component type</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1> WithAll<TComponent1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify all read-only component types that must be present.
        /// </summary>
        /// <typeparam name="TComponent1">A component type</typeparam>
        /// <typeparam name="TComponent2">A component type</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1> WithAll<TComponent1, TComponent2>()  => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify all read-only component types that must be present.
        /// </summary>
        /// <typeparam name="TComponent1">A component type</typeparam>
        /// <typeparam name="TComponent2">A component type</typeparam>
        /// <typeparam name="TComponent3">A component type</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1> WithAll<TComponent1, TComponent2, TComponent3>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify optional read-only component types.
        /// </summary>
        /// <typeparam name="TComponent1">Optional component</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1> WithAny<TComponent1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify optional read-only component types.
        /// </summary>
        /// <typeparam name="TComponent1">Optional component</typeparam>
        /// <typeparam name="TComponent2">Optional component</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1> WithAny<TComponent1, TComponent2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify optional read-only component types.
        /// </summary>
        /// <typeparam name="TComponent1">Optional component</typeparam>
        /// <typeparam name="TComponent2">Optional component</typeparam>
        /// <typeparam name="TComponent3">Optional component</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1> WithAny<TComponent1, TComponent2, TComponent3>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify component types that must be absent.
        /// </summary>
        /// <typeparam name="TComponent1">Absent component</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1> WithNone<TComponent1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify component types that must be absent.
        /// </summary>
        /// <typeparam name="TComponent1">Absent component</typeparam>
        /// <typeparam name="TComponent2">Absent component</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1> WithNone<TComponent1, TComponent2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify component types that must be absent.
        /// </summary>
        /// <typeparam name="TComponent1">Absent component</typeparam>
        /// <typeparam name="TComponent2">Absent component</typeparam>
        /// <typeparam name="TComponent3">Absent component</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1> WithNone<TComponent1, TComponent2, TComponent3>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select components in chunks in which the specified component might have changed since the last time the system updated.
        /// </summary>
        /// <typeparam name="TChangeFilter1">A component type</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1> WithChangeFilter<TChangeFilter1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select components in chunks in which the specified component might have changed since the last time the system updated.
        /// </summary>
        /// <typeparam name="TChangeFilter1">A component type</typeparam>
        /// <typeparam name="TChangeFilter2">A component type</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1> WithChangeFilter<TChangeFilter1, TChangeFilter2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify your own `EntityQueryOptions`.
        /// </summary>
        /// <remarks>
        /// This method may not be invoked more than once for each query description. Subsequent calls will override
        /// previous options, rather than adding to them. Use the bitwise OR operator '|' to combine multiple options.
        /// </remarks>
        /// <param name="options">The options for this query</param>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1> WithOptions(EntityQueryOptions options) => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select chunks that have a specified value for a shared component.
        /// </summary>
        /// <typeparam name="TSharedComponent1">The shared component type</typeparam>
        /// <param name="sharedComponent">The value of <typeparamref name="TSharedComponent1"/> which an entity must have in order to match this query</param>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1> WithSharedComponentFilter<TSharedComponent1>(TSharedComponent1 sharedComponent)
            where TSharedComponent1 : struct, ISharedComponentData
                => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select chunks that have the specified values for two shared components.
        /// </summary>
        /// <typeparam name="TSharedComponent1">The first shared component type</typeparam>
        /// <typeparam name="TSharedComponent2">The second shared component type</typeparam>
        /// <param name="sharedComponent1">The value of <typeparamref name="TSharedComponent1"/> which an entity must have in order to match this query</param>
        /// <param name="sharedComponent2">The value of <typeparamref name="TSharedComponent2"/> which an entity must have in order to match this query</param>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1> WithSharedComponentFilter<TSharedComponent1, TSharedComponent2>(TSharedComponent1 sharedComponent1, TSharedComponent2 sharedComponent2)
            where TSharedComponent1 : struct, ISharedComponentData
            where TSharedComponent2 : struct, ISharedComponentData
                => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Invoke this method if you wish to retrieve a tuple with an `Entity` parameter, thus giving you direct access to an entity.
        /// </summary>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1> WithEntityAccess() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Returns an enumerator over the entities in this query.
        /// </summary>
        /// <returns>An IEnumerator interface into the entities matched by this query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public IEnumerator<T1> GetEnumerator() => throw InternalCompilerInterface.ThrowCodeGenException();
        IEnumerator IEnumerable.GetEnumerator() => throw InternalCompilerInterface.ThrowCodeGenException();
    }

    /// <summary>An enumeration interface for the entities that match an <see cref="EntityQuery"/>. This variant includes access the the entity and its component values.</summary>
    /// <remarks>
    /// This feature is primarily intended as a backend implementation for <see cref="SystemAPI.Query"/>. Application code
    /// should prefer to use that interface rather than using this type directly.
    /// </remarks>
    /// <typeparam name="T1">A component type</typeparam>
    public readonly struct QueryEnumerableWithEntity<T1> : IEnumerable<(T1, Entity)>
    {
        /// <summary>A component value for the current entity.</summary>
        public readonly T1 Item1;
        /// <summary>A component value for the current entity.</summary>
        public readonly Entity Entity;

        /// <summary>
        /// Construct a new object.
        /// </summary>
        /// <remarks>Objects of this type are typically created and destroyed automatically by the source generators.</remarks>
        /// <seealso cref="Deconstruct"/>
        /// <param name="item1">The value for <typeparamref name="T1"/>.</param>
        /// <param name="entity">The entity</param>
        public QueryEnumerableWithEntity(T1 item1, Entity entity)
        {
            Item1 = item1;
            Entity = entity;
        }

        /// <summary>
        /// Clean up an existing object.
        /// </summary>
        /// <remarks>Objects of this type are typically created and destroyed automatically by the source generators.</remarks>
        /// <seealso cref="QueryEnumerableWithEntity(T1, Entity)"/>
        /// <param name="item1">The value for <typeparamref name="T1"/>.</param>
        /// <param name="entity">The entity</param>
        public void Deconstruct(out T1 item1, out Entity entity)
        {
            item1 = Item1;
            entity = Entity;
        }

        /// <summary>
        /// Only select chunks that have a specified value for a shared component.
        /// </summary>
        /// <typeparam name="TSharedComponent1">The shared component type</typeparam>
        /// <param name="sharedComponent">The value of <typeparamref name="TSharedComponent1"/> which an entity must have in order to match this query</param>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1> WithSharedComponentFilter<TSharedComponent1>(TSharedComponent1 sharedComponent)
            where TSharedComponent1 : struct, ISharedComponentData
                => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select chunks that have a specified value for a shared component.
        /// </summary>
        /// <summary>
        /// Only select chunks that have the specified values for two shared components.
        /// </summary>
        /// <typeparam name="TSharedComponent1">The first shared component type</typeparam>
        /// <typeparam name="TSharedComponent2">The second shared component type</typeparam>
        /// <param name="sharedComponent1">The value of <typeparamref name="TSharedComponent1"/> which an entity must have in order to match this query</param>
        /// <param name="sharedComponent2">The value of <typeparamref name="TSharedComponent2"/> which an entity must have in order to match this query</param>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1> WithSharedComponentFilter<TSharedComponent1, TSharedComponent2>(TSharedComponent1 sharedComponent1, TSharedComponent2 sharedComponent2)
            where TSharedComponent1 : struct, ISharedComponentData
            where TSharedComponent2 : struct, ISharedComponentData
                => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select components in chunks in which the specified component might have changed since the last time the system updated.
        /// </summary>
        /// <typeparam name="TChangeFilter1">A component type</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1> WithChangeFilter<TChangeFilter1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select components in chunks in which the specified component might have changed since the last time the system updated.
        /// </summary>
        /// <typeparam name="TChangeFilter1">A component type</typeparam>
        /// <typeparam name="TChangeFilter2">A component type</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1> WithChangeFilter<TChangeFilter1, TChangeFilter2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify all read-only component types that must be present.
        /// </summary>
        /// <typeparam name="TComponent1">A component type</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1> WithAll<TComponent1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify all read-only component types that must be present.
        /// </summary>
        /// <typeparam name="TComponent1">A component type</typeparam>
        /// <typeparam name="TComponent2">A component type</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1> WithAll<TComponent1, TComponent2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify all read-only component types that must be present.
        /// </summary>
        /// <typeparam name="TComponent1">A component type</typeparam>
        /// <typeparam name="TComponent2">A component type</typeparam>
        /// <typeparam name="TComponent3">A component type</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1> WithAll<TComponent1, TComponent2, TComponent3>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify optional read-only component types.
        /// </summary>
        /// <typeparam name="TComponent1">Optional component</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1> WithAny<TComponent1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify optional read-only component types.
        /// </summary>
        /// <typeparam name="TComponent1">Optional component</typeparam>
        /// <typeparam name="TComponent2">Optional component</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1> WithAny<TComponent1, TComponent2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify optional read-only component types.
        /// </summary>
        /// <typeparam name="TComponent1">Optional component</typeparam>
        /// <typeparam name="TComponent2">Optional component</typeparam>
        /// <typeparam name="TComponent3">Optional component</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1> WithAny<TComponent1, TComponent2, TComponent3>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify component types that must be absent.
        /// </summary>
        /// <typeparam name="TComponent1">Absent component</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1> WithNone<TComponent1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify component types that must be absent.
        /// </summary>
        /// <typeparam name="TComponent1">Absent component</typeparam>
        /// <typeparam name="TComponent2">Absent component</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1> WithNone<TComponent1, TComponent2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify component types that must be absent.
        /// </summary>
        /// <typeparam name="TComponent1">Absent component</typeparam>
        /// <typeparam name="TComponent2">Absent component</typeparam>
        /// <typeparam name="TComponent3">Absent component</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1> WithNone<TComponent1, TComponent2, TComponent3>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify your own `EntityQueryOptions`.
        /// </summary>
        /// <remarks>
        /// This method may not be invoked more than once for each query description. Subsequent calls will override
        /// previous options, rather than adding to them. Use the bitwise OR operator '|' to combine multiple options.
        /// </remarks>
        /// <param name="options">The options for this query</param>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1> WithOptions(EntityQueryOptions options) => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Returns an enumerator over the entities in this query.
        /// </summary>
        /// <returns>An IEnumerator interface into the entities matched by this query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public IEnumerator<(T1, Entity)> GetEnumerator() => throw InternalCompilerInterface.ThrowCodeGenException();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    /// <summary>An enumeration interface for the entities that match an <see cref="EntityQuery"/>.</summary>
    /// <remarks>
    /// This feature is primarily intended as a backend implementation for <see cref="SystemAPI.Query"/>. Application code
    /// should prefer to use that interface rather than using this type directly.
    /// </remarks>
    /// <typeparam name="T1">A component type</typeparam>
    /// <typeparam name="T2">A component type</typeparam>
    public struct QueryEnumerable<T1, T2> : IEnumerable<(T1, T2)>
    {
        /// <summary>
        /// Specify all read-only component types that must be present.
        /// </summary>
        /// <typeparam name="TComponent1">A component type</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2> WithAll<TComponent1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify all read-only component types that must be present.
        /// </summary>
        /// <typeparam name="TComponent1">A component type</typeparam>
        /// <typeparam name="TComponent2">A component type</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2> WithAll<TComponent1, TComponent2>()  => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify all read-only component types that must be present.
        /// </summary>
        /// <typeparam name="TComponent1">A component type</typeparam>
        /// <typeparam name="TComponent2">A component type</typeparam>
        /// <typeparam name="TComponent3">A component type</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2> WithAll<TComponent1, TComponent2, TComponent3>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify optional read-only component types.
        /// </summary>
        /// <typeparam name="TComponent1">Optional component</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2> WithAny<TComponent1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify optional read-only component types.
        /// </summary>
        /// <typeparam name="TComponent1">Optional component</typeparam>
        /// <typeparam name="TComponent2">Optional component</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2> WithAny<TComponent1, TComponent2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify optional read-only component types.
        /// </summary>
        /// <typeparam name="TComponent1">Optional component</typeparam>
        /// <typeparam name="TComponent2">Optional component</typeparam>
        /// <typeparam name="TComponent3">Optional component</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2> WithAny<TComponent1, TComponent2, TComponent3>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify component types that must be absent.
        /// </summary>
        /// <typeparam name="TComponent1">Absent component</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2> WithNone<TComponent1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify component types that must be absent.
        /// </summary>
        /// <typeparam name="TComponent1">Absent component</typeparam>
        /// <typeparam name="TComponent2">Absent component</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2> WithNone<TComponent1, TComponent2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify component types that must be absent.
        /// </summary>
        /// <typeparam name="TComponent1">Absent component</typeparam>
        /// <typeparam name="TComponent2">Absent component</typeparam>
        /// <typeparam name="TComponent3">Absent component</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2> WithNone<TComponent1, TComponent2, TComponent3>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select components in chunks in which the specified component might have changed since the last time the system updated.
        /// </summary>
        /// <typeparam name="TChangeFilter1">A component type</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2> WithChangeFilter<TChangeFilter1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select components in chunks in which the specified component might have changed since the last time the system updated.
        /// </summary>
        /// <typeparam name="TChangeFilter1">A component type</typeparam>
        /// <typeparam name="TChangeFilter2">A component type</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2> WithChangeFilter<TChangeFilter1, TChangeFilter2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify your own `EntityQueryOptions`.
        /// </summary>
        /// <remarks>
        /// This method may not be invoked more than once for each query description. Subsequent calls will override
        /// previous options, rather than adding to them. Use the bitwise OR operator '|' to combine multiple options.
        /// </remarks>
        /// <param name="options">The options for this query</param>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2> WithOptions(EntityQueryOptions options) => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select chunks that have a specified value for a shared component.
        /// </summary>
        /// <typeparam name="TSharedComponent1">The shared component type</typeparam>
        /// <param name="sharedComponent">The value of <typeparamref name="TSharedComponent1"/> which an entity must have in order to match this query</param>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2> WithSharedComponentFilter<TSharedComponent1>(TSharedComponent1 sharedComponent)
            where TSharedComponent1 : struct, ISharedComponentData
                => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select chunks that have the specified values for two shared components.
        /// </summary>
        /// <typeparam name="TSharedComponent1">The first shared component type</typeparam>
        /// <typeparam name="TSharedComponent2">The second shared component type</typeparam>
        /// <param name="sharedComponent1">The value of <typeparamref name="TSharedComponent1"/> which an entity must have in order to match this query</param>
        /// <param name="sharedComponent2">The value of <typeparamref name="TSharedComponent2"/> which an entity must have in order to match this query</param>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2> WithSharedComponentFilter<TSharedComponent1, TSharedComponent2>(TSharedComponent1 sharedComponent1, TSharedComponent2 sharedComponent2)
            where TSharedComponent1 : struct, ISharedComponentData
            where TSharedComponent2 : struct, ISharedComponentData
                => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Invoke this method if you wish to retrieve a tuple with an `Entity` parameter, thus giving you direct access to an entity.
        /// </summary>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2> WithEntityAccess() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Returns an enumerator over the entities in this query.
        /// </summary>
        /// <returns>An IEnumerator interface into the entities matched by this query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public IEnumerator<(T1, T2)> GetEnumerator() => throw InternalCompilerInterface.ThrowCodeGenException();
        IEnumerator IEnumerable.GetEnumerator() => throw InternalCompilerInterface.ThrowCodeGenException();
    }

    /// <summary>An enumeration interface for the entities that match an <see cref="EntityQuery"/>. This variant includes access the the entity and its component values.</summary>
    /// <remarks>
    /// This feature is primarily intended as a backend implementation for <see cref="SystemAPI.Query"/>. Application code
    /// should prefer to use that interface rather than using this type directly.
    /// </remarks>
    /// <typeparam name="T1">A component type</typeparam>
    /// <typeparam name="T2">A component type</typeparam>
    public readonly struct QueryEnumerableWithEntity<T1, T2> : IEnumerable<(T1, T2, Entity)>
    {
        /// <summary>A component value for the current entity.</summary>
        public readonly T1 Item1;
        /// <summary>A component value for the current entity.</summary>
        public readonly T2 Item2;
        /// <summary>A component value for the current entity.</summary>
        public readonly Entity Entity;

        /// <summary>
        /// Construct a new object.
        /// </summary>
        /// <remarks>Objects of this type are typically created and destroyed automatically by the source generators.</remarks>
        /// <seealso cref="Deconstruct"/>
        /// <param name="item1">The value for <typeparamref name="T1"/>.</param>
        /// <param name="item2">The value for <typeparamref name="T2"/>.</param>
        /// <param name="entity">The entity</param>
        public QueryEnumerableWithEntity(T1 item1, T2 item2, Entity entity)
        {
            Item1 = item1;
            Item2 = item2;
            Entity = entity;
        }

        /// <summary>
        /// Clean up an existing object.
        /// </summary>
        /// <remarks>Objects of this type are typically created and destroyed automatically by the source generators.</remarks>
        /// <seealso cref="QueryEnumerableWithEntity(T1, T2, Entity)"/>
        /// <param name="item1">The value for <typeparamref name="T1"/>.</param>
        /// <param name="item2">The value for <typeparamref name="T2"/>.</param>
        /// <param name="entity">The entity</param>
        public void Deconstruct(out T1 item1, out T2 item2, out Entity entity)
        {
            item1 = Item1;
            item2 = Item2;
            entity = Entity;
        }

        /// <summary>
        /// Only select chunks that have a specified value for a shared component.
        /// </summary>
        /// <typeparam name="TSharedComponent1">The shared component type</typeparam>
        /// <param name="sharedComponent">The value of <typeparamref name="TSharedComponent1"/> which an entity must have in order to match this query</param>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2> WithSharedComponentFilter<TSharedComponent1>(TSharedComponent1 sharedComponent)
            where TSharedComponent1 : struct, ISharedComponentData
                => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select chunks that have a specified value for a shared component.
        /// </summary>
        /// <summary>
        /// Only select chunks that have the specified values for two shared components.
        /// </summary>
        /// <typeparam name="TSharedComponent1">The first shared component type</typeparam>
        /// <typeparam name="TSharedComponent2">The second shared component type</typeparam>
        /// <param name="sharedComponent1">The value of <typeparamref name="TSharedComponent1"/> which an entity must have in order to match this query</param>
        /// <param name="sharedComponent2">The value of <typeparamref name="TSharedComponent2"/> which an entity must have in order to match this query</param>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2> WithSharedComponentFilter<TSharedComponent1, TSharedComponent2>(TSharedComponent1 sharedComponent1, TSharedComponent2 sharedComponent2)
            where TSharedComponent1 : struct, ISharedComponentData
            where TSharedComponent2 : struct, ISharedComponentData
                => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select components in chunks in which the specified component might have changed since the last time the system updated.
        /// </summary>
        /// <typeparam name="TChangeFilter1">A component type</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2> WithChangeFilter<TChangeFilter1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select components in chunks in which the specified component might have changed since the last time the system updated.
        /// </summary>
        /// <typeparam name="TChangeFilter1">A component type</typeparam>
        /// <typeparam name="TChangeFilter2">A component type</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2> WithChangeFilter<TChangeFilter1, TChangeFilter2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify all read-only component types that must be present.
        /// </summary>
        /// <typeparam name="TComponent1">A component type</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2> WithAll<TComponent1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify all read-only component types that must be present.
        /// </summary>
        /// <typeparam name="TComponent1">A component type</typeparam>
        /// <typeparam name="TComponent2">A component type</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2> WithAll<TComponent1, TComponent2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify all read-only component types that must be present.
        /// </summary>
        /// <typeparam name="TComponent1">A component type</typeparam>
        /// <typeparam name="TComponent2">A component type</typeparam>
        /// <typeparam name="TComponent3">A component type</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2> WithAll<TComponent1, TComponent2, TComponent3>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify optional read-only component types.
        /// </summary>
        /// <typeparam name="TComponent1">Optional component</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2> WithAny<TComponent1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify optional read-only component types.
        /// </summary>
        /// <typeparam name="TComponent1">Optional component</typeparam>
        /// <typeparam name="TComponent2">Optional component</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2> WithAny<TComponent1, TComponent2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify optional read-only component types.
        /// </summary>
        /// <typeparam name="TComponent1">Optional component</typeparam>
        /// <typeparam name="TComponent2">Optional component</typeparam>
        /// <typeparam name="TComponent3">Optional component</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2> WithAny<TComponent1, TComponent2, TComponent3>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify component types that must be absent.
        /// </summary>
        /// <typeparam name="TComponent1">Absent component</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2> WithNone<TComponent1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify component types that must be absent.
        /// </summary>
        /// <typeparam name="TComponent1">Absent component</typeparam>
        /// <typeparam name="TComponent2">Absent component</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2> WithNone<TComponent1, TComponent2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify component types that must be absent.
        /// </summary>
        /// <typeparam name="TComponent1">Absent component</typeparam>
        /// <typeparam name="TComponent2">Absent component</typeparam>
        /// <typeparam name="TComponent3">Absent component</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2> WithNone<TComponent1, TComponent2, TComponent3>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify your own `EntityQueryOptions`.
        /// </summary>
        /// <remarks>
        /// This method may not be invoked more than once for each query description. Subsequent calls will override
        /// previous options, rather than adding to them. Use the bitwise OR operator '|' to combine multiple options.
        /// </remarks>
        /// <param name="options">The options for this query</param>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2> WithOptions(EntityQueryOptions options) => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Returns an enumerator over the entities in this query.
        /// </summary>
        /// <returns>An IEnumerator interface into the entities matched by this query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public IEnumerator<(T1, T2, Entity)> GetEnumerator() => throw InternalCompilerInterface.ThrowCodeGenException();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    /// <summary>An enumeration interface for the entities that match an <see cref="EntityQuery"/>.</summary>
    /// <remarks>
    /// This feature is primarily intended as a backend implementation for <see cref="SystemAPI.Query"/>. Application code
    /// should prefer to use that interface rather than using this type directly.
    /// </remarks>
    /// <typeparam name="T1">A component type</typeparam>
    /// <typeparam name="T2">A component type</typeparam>
    /// <typeparam name="T3">A component type</typeparam>
    public struct QueryEnumerable<T1, T2, T3> : IEnumerable<(T1, T2, T3)>
    {
        /// <summary>
        /// Specify all read-only component types that must be present.
        /// </summary>
        /// <typeparam name="TComponent1">A component type</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3> WithAll<TComponent1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify all read-only component types that must be present.
        /// </summary>
        /// <typeparam name="TComponent1">A component type</typeparam>
        /// <typeparam name="TComponent2">A component type</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3> WithAll<TComponent1, TComponent2>()  => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify all read-only component types that must be present.
        /// </summary>
        /// <typeparam name="TComponent1">A component type</typeparam>
        /// <typeparam name="TComponent2">A component type</typeparam>
        /// <typeparam name="TComponent3">A component type</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3> WithAll<TComponent1, TComponent2, TComponent3>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify optional read-only component types.
        /// </summary>
        /// <typeparam name="TComponent1">Optional component</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3> WithAny<TComponent1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify optional read-only component types.
        /// </summary>
        /// <typeparam name="TComponent1">Optional component</typeparam>
        /// <typeparam name="TComponent2">Optional component</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3> WithAny<TComponent1, TComponent2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify optional read-only component types.
        /// </summary>
        /// <typeparam name="TComponent1">Optional component</typeparam>
        /// <typeparam name="TComponent2">Optional component</typeparam>
        /// <typeparam name="TComponent3">Optional component</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3> WithAny<TComponent1, TComponent2, TComponent3>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify component types that must be absent.
        /// </summary>
        /// <typeparam name="TComponent1">Absent component</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3> WithNone<TComponent1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify component types that must be absent.
        /// </summary>
        /// <typeparam name="TComponent1">Absent component</typeparam>
        /// <typeparam name="TComponent2">Absent component</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3> WithNone<TComponent1, TComponent2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify component types that must be absent.
        /// </summary>
        /// <typeparam name="TComponent1">Absent component</typeparam>
        /// <typeparam name="TComponent2">Absent component</typeparam>
        /// <typeparam name="TComponent3">Absent component</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3> WithNone<TComponent1, TComponent2, TComponent3>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select components in chunks in which the specified component might have changed since the last time the system updated.
        /// </summary>
        /// <typeparam name="TChangeFilter1">A component type</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3> WithChangeFilter<TChangeFilter1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select components in chunks in which the specified component might have changed since the last time the system updated.
        /// </summary>
        /// <typeparam name="TChangeFilter1">A component type</typeparam>
        /// <typeparam name="TChangeFilter2">A component type</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3> WithChangeFilter<TChangeFilter1, TChangeFilter2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify your own `EntityQueryOptions`.
        /// </summary>
        /// <remarks>
        /// This method may not be invoked more than once for each query description. Subsequent calls will override
        /// previous options, rather than adding to them. Use the bitwise OR operator '|' to combine multiple options.
        /// </remarks>
        /// <param name="options">The options for this query</param>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3> WithOptions(EntityQueryOptions options) => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select chunks that have a specified value for a shared component.
        /// </summary>
        /// <typeparam name="TSharedComponent1">The shared component type</typeparam>
        /// <param name="sharedComponent">The value of <typeparamref name="TSharedComponent1"/> which an entity must have in order to match this query</param>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3> WithSharedComponentFilter<TSharedComponent1>(TSharedComponent1 sharedComponent)
            where TSharedComponent1 : struct, ISharedComponentData
                => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select chunks that have the specified values for two shared components.
        /// </summary>
        /// <typeparam name="TSharedComponent1">The first shared component type</typeparam>
        /// <typeparam name="TSharedComponent2">The second shared component type</typeparam>
        /// <param name="sharedComponent1">The value of <typeparamref name="TSharedComponent1"/> which an entity must have in order to match this query</param>
        /// <param name="sharedComponent2">The value of <typeparamref name="TSharedComponent2"/> which an entity must have in order to match this query</param>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3> WithSharedComponentFilter<TSharedComponent1, TSharedComponent2>(TSharedComponent1 sharedComponent1, TSharedComponent2 sharedComponent2)
            where TSharedComponent1 : struct, ISharedComponentData
            where TSharedComponent2 : struct, ISharedComponentData
                => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Invoke this method if you wish to retrieve a tuple with an `Entity` parameter, thus giving you direct access to an entity.
        /// </summary>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3> WithEntityAccess() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Returns an enumerator over the entities in this query.
        /// </summary>
        /// <returns>An IEnumerator interface into the entities matched by this query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public IEnumerator<(T1, T2, T3)> GetEnumerator() => throw InternalCompilerInterface.ThrowCodeGenException();
        IEnumerator IEnumerable.GetEnumerator() => throw InternalCompilerInterface.ThrowCodeGenException();
    }

    /// <summary>An enumeration interface for the entities that match an <see cref="EntityQuery"/>. This variant includes access the the entity and its component values.</summary>
    /// <remarks>
    /// This feature is primarily intended as a backend implementation for <see cref="SystemAPI.Query"/>. Application code
    /// should prefer to use that interface rather than using this type directly.
    /// </remarks>
    /// <typeparam name="T1">A component type</typeparam>
    /// <typeparam name="T2">A component type</typeparam>
    /// <typeparam name="T3">A component type</typeparam>
    public readonly struct QueryEnumerableWithEntity<T1, T2, T3> : IEnumerable<(T1, T2, T3, Entity)>
    {
        /// <summary>A component value for the current entity.</summary>
        public readonly T1 Item1;
        /// <summary>A component value for the current entity.</summary>
        public readonly T2 Item2;
        /// <summary>A component value for the current entity.</summary>
        public readonly T3 Item3;
        /// <summary>A component value for the current entity.</summary>
        public readonly Entity Entity;

        /// <summary>
        /// Construct a new object.
        /// </summary>
        /// <remarks>Objects of this type are typically created and destroyed automatically by the source generators.</remarks>
        /// <seealso cref="Deconstruct"/>
        /// <param name="item1">The value for <typeparamref name="T1"/>.</param>
        /// <param name="item2">The value for <typeparamref name="T2"/>.</param>
        /// <param name="item3">The value for <typeparamref name="T3"/>.</param>
        /// <param name="entity">The entity</param>
        public QueryEnumerableWithEntity(T1 item1, T2 item2, T3 item3, Entity entity)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Entity = entity;
        }

        /// <summary>
        /// Clean up an existing object.
        /// </summary>
        /// <remarks>Objects of this type are typically created and destroyed automatically by the source generators.</remarks>
        /// <seealso cref="QueryEnumerableWithEntity(T1, T2, T3, Entity)"/>
        /// <param name="item1">The value for <typeparamref name="T1"/>.</param>
        /// <param name="item2">The value for <typeparamref name="T2"/>.</param>
        /// <param name="item3">The value for <typeparamref name="T3"/>.</param>
        /// <param name="entity">The entity</param>
        public void Deconstruct(out T1 item1, out T2 item2, out T3 item3, out Entity entity)
        {
            item1 = Item1;
            item2 = Item2;
            item3 = Item3;
            entity = Entity;
        }

        /// <summary>
        /// Only select chunks that have a specified value for a shared component.
        /// </summary>
        /// <typeparam name="TSharedComponent1">The shared component type</typeparam>
        /// <param name="sharedComponent">The value of <typeparamref name="TSharedComponent1"/> which an entity must have in order to match this query</param>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3> WithSharedComponentFilter<TSharedComponent1>(TSharedComponent1 sharedComponent)
            where TSharedComponent1 : struct, ISharedComponentData
                => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select chunks that have a specified value for a shared component.
        /// </summary>
        /// <summary>
        /// Only select chunks that have the specified values for two shared components.
        /// </summary>
        /// <typeparam name="TSharedComponent1">The first shared component type</typeparam>
        /// <typeparam name="TSharedComponent2">The second shared component type</typeparam>
        /// <param name="sharedComponent1">The value of <typeparamref name="TSharedComponent1"/> which an entity must have in order to match this query</param>
        /// <param name="sharedComponent2">The value of <typeparamref name="TSharedComponent2"/> which an entity must have in order to match this query</param>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3> WithSharedComponentFilter<TSharedComponent1, TSharedComponent2>(TSharedComponent1 sharedComponent1, TSharedComponent2 sharedComponent2)
            where TSharedComponent1 : struct, ISharedComponentData
            where TSharedComponent2 : struct, ISharedComponentData
                => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select components in chunks in which the specified component might have changed since the last time the system updated.
        /// </summary>
        /// <typeparam name="TChangeFilter1">A component type</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3> WithChangeFilter<TChangeFilter1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select components in chunks in which the specified component might have changed since the last time the system updated.
        /// </summary>
        /// <typeparam name="TChangeFilter1">A component type</typeparam>
        /// <typeparam name="TChangeFilter2">A component type</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3> WithChangeFilter<TChangeFilter1, TChangeFilter2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify all read-only component types that must be present.
        /// </summary>
        /// <typeparam name="TComponent1">A component type</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3> WithAll<TComponent1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify all read-only component types that must be present.
        /// </summary>
        /// <typeparam name="TComponent1">A component type</typeparam>
        /// <typeparam name="TComponent2">A component type</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3> WithAll<TComponent1, TComponent2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify all read-only component types that must be present.
        /// </summary>
        /// <typeparam name="TComponent1">A component type</typeparam>
        /// <typeparam name="TComponent2">A component type</typeparam>
        /// <typeparam name="TComponent3">A component type</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3> WithAll<TComponent1, TComponent2, TComponent3>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify optional read-only component types.
        /// </summary>
        /// <typeparam name="TComponent1">Optional component</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3> WithAny<TComponent1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify optional read-only component types.
        /// </summary>
        /// <typeparam name="TComponent1">Optional component</typeparam>
        /// <typeparam name="TComponent2">Optional component</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3> WithAny<TComponent1, TComponent2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify optional read-only component types.
        /// </summary>
        /// <typeparam name="TComponent1">Optional component</typeparam>
        /// <typeparam name="TComponent2">Optional component</typeparam>
        /// <typeparam name="TComponent3">Optional component</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3> WithAny<TComponent1, TComponent2, TComponent3>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify component types that must be absent.
        /// </summary>
        /// <typeparam name="TComponent1">Absent component</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3> WithNone<TComponent1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify component types that must be absent.
        /// </summary>
        /// <typeparam name="TComponent1">Absent component</typeparam>
        /// <typeparam name="TComponent2">Absent component</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3> WithNone<TComponent1, TComponent2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify component types that must be absent.
        /// </summary>
        /// <typeparam name="TComponent1">Absent component</typeparam>
        /// <typeparam name="TComponent2">Absent component</typeparam>
        /// <typeparam name="TComponent3">Absent component</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3> WithNone<TComponent1, TComponent2, TComponent3>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify your own `EntityQueryOptions`.
        /// </summary>
        /// <remarks>
        /// This method may not be invoked more than once for each query description. Subsequent calls will override
        /// previous options, rather than adding to them. Use the bitwise OR operator '|' to combine multiple options.
        /// </remarks>
        /// <param name="options">The options for this query</param>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3> WithOptions(EntityQueryOptions options) => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Returns an enumerator over the entities in this query.
        /// </summary>
        /// <returns>An IEnumerator interface into the entities matched by this query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public IEnumerator<(T1, T2, T3, Entity)> GetEnumerator() => throw InternalCompilerInterface.ThrowCodeGenException();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    /// <summary>An enumeration interface for the entities that match an <see cref="EntityQuery"/>.</summary>
    /// <remarks>
    /// This feature is primarily intended as a backend implementation for <see cref="SystemAPI.Query"/>. Application code
    /// should prefer to use that interface rather than using this type directly.
    /// </remarks>
    /// <typeparam name="T1">A component type</typeparam>
    /// <typeparam name="T2">A component type</typeparam>
    /// <typeparam name="T3">A component type</typeparam>
    /// <typeparam name="T4">A component type</typeparam>
    public struct QueryEnumerable<T1, T2, T3, T4> : IEnumerable<(T1, T2, T3, T4)>
    {
        /// <summary>
        /// Specify all read-only component types that must be present.
        /// </summary>
        /// <typeparam name="TComponent1">A component type</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4> WithAll<TComponent1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify all read-only component types that must be present.
        /// </summary>
        /// <typeparam name="TComponent1">A component type</typeparam>
        /// <typeparam name="TComponent2">A component type</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4> WithAll<TComponent1, TComponent2>()  => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify all read-only component types that must be present.
        /// </summary>
        /// <typeparam name="TComponent1">A component type</typeparam>
        /// <typeparam name="TComponent2">A component type</typeparam>
        /// <typeparam name="TComponent3">A component type</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4> WithAll<TComponent1, TComponent2, TComponent3>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify optional read-only component types.
        /// </summary>
        /// <typeparam name="TComponent1">Optional component</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4> WithAny<TComponent1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify optional read-only component types.
        /// </summary>
        /// <typeparam name="TComponent1">Optional component</typeparam>
        /// <typeparam name="TComponent2">Optional component</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4> WithAny<TComponent1, TComponent2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify optional read-only component types.
        /// </summary>
        /// <typeparam name="TComponent1">Optional component</typeparam>
        /// <typeparam name="TComponent2">Optional component</typeparam>
        /// <typeparam name="TComponent3">Optional component</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4> WithAny<TComponent1, TComponent2, TComponent3>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify component types that must be absent.
        /// </summary>
        /// <typeparam name="TComponent1">Absent component</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4> WithNone<TComponent1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify component types that must be absent.
        /// </summary>
        /// <typeparam name="TComponent1">Absent component</typeparam>
        /// <typeparam name="TComponent2">Absent component</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4> WithNone<TComponent1, TComponent2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify component types that must be absent.
        /// </summary>
        /// <typeparam name="TComponent1">Absent component</typeparam>
        /// <typeparam name="TComponent2">Absent component</typeparam>
        /// <typeparam name="TComponent3">Absent component</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4> WithNone<TComponent1, TComponent2, TComponent3>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select components in chunks in which the specified component might have changed since the last time the system updated.
        /// </summary>
        /// <typeparam name="TChangeFilter1">A component type</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4> WithChangeFilter<TChangeFilter1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select components in chunks in which the specified component might have changed since the last time the system updated.
        /// </summary>
        /// <typeparam name="TChangeFilter1">A component type</typeparam>
        /// <typeparam name="TChangeFilter2">A component type</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4> WithChangeFilter<TChangeFilter1, TChangeFilter2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify your own `EntityQueryOptions`.
        /// </summary>
        /// <remarks>
        /// This method may not be invoked more than once for each query description. Subsequent calls will override
        /// previous options, rather than adding to them. Use the bitwise OR operator '|' to combine multiple options.
        /// </remarks>
        /// <param name="options">The options for this query</param>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4> WithOptions(EntityQueryOptions options) => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select chunks that have a specified value for a shared component.
        /// </summary>
        /// <typeparam name="TSharedComponent1">The shared component type</typeparam>
        /// <param name="sharedComponent">The value of <typeparamref name="TSharedComponent1"/> which an entity must have in order to match this query</param>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4> WithSharedComponentFilter<TSharedComponent1>(TSharedComponent1 sharedComponent)
            where TSharedComponent1 : struct, ISharedComponentData
                => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select chunks that have the specified values for two shared components.
        /// </summary>
        /// <typeparam name="TSharedComponent1">The first shared component type</typeparam>
        /// <typeparam name="TSharedComponent2">The second shared component type</typeparam>
        /// <param name="sharedComponent1">The value of <typeparamref name="TSharedComponent1"/> which an entity must have in order to match this query</param>
        /// <param name="sharedComponent2">The value of <typeparamref name="TSharedComponent2"/> which an entity must have in order to match this query</param>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4> WithSharedComponentFilter<TSharedComponent1, TSharedComponent2>(TSharedComponent1 sharedComponent1, TSharedComponent2 sharedComponent2)
            where TSharedComponent1 : struct, ISharedComponentData
            where TSharedComponent2 : struct, ISharedComponentData
                => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Invoke this method if you wish to retrieve a tuple with an `Entity` parameter, thus giving you direct access to an entity.
        /// </summary>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4> WithEntityAccess() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Returns an enumerator over the entities in this query.
        /// </summary>
        /// <returns>An IEnumerator interface into the entities matched by this query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public IEnumerator<(T1, T2, T3, T4)> GetEnumerator() => throw InternalCompilerInterface.ThrowCodeGenException();
        IEnumerator IEnumerable.GetEnumerator() => throw InternalCompilerInterface.ThrowCodeGenException();
    }

    /// <summary>An enumeration interface for the entities that match an <see cref="EntityQuery"/>. This variant includes access the the entity and its component values.</summary>
    /// <remarks>
    /// This feature is primarily intended as a backend implementation for <see cref="SystemAPI.Query"/>. Application code
    /// should prefer to use that interface rather than using this type directly.
    /// </remarks>
    /// <typeparam name="T1">A component type</typeparam>
    /// <typeparam name="T2">A component type</typeparam>
    /// <typeparam name="T3">A component type</typeparam>
    /// <typeparam name="T4">A component type</typeparam>
    public readonly struct QueryEnumerableWithEntity<T1, T2, T3, T4> : IEnumerable<(T1, T2, T3, T4, Entity)>
    {
        /// <summary>A component value for the current entity.</summary>
        public readonly T1 Item1;
        /// <summary>A component value for the current entity.</summary>
        public readonly T2 Item2;
        /// <summary>A component value for the current entity.</summary>
        public readonly T3 Item3;
        /// <summary>A component value for the current entity.</summary>
        public readonly T4 Item4;
        /// <summary>A component value for the current entity.</summary>
        public readonly Entity Entity;

        /// <summary>
        /// Construct a new object.
        /// </summary>
        /// <remarks>Objects of this type are typically created and destroyed automatically by the source generators.</remarks>
        /// <seealso cref="Deconstruct"/>
        /// <param name="item1">The value for <typeparamref name="T1"/>.</param>
        /// <param name="item2">The value for <typeparamref name="T2"/>.</param>
        /// <param name="item3">The value for <typeparamref name="T3"/>.</param>
        /// <param name="item4">The value for <typeparamref name="T4"/>.</param>
        /// <param name="entity">The entity</param>
        public QueryEnumerableWithEntity(T1 item1, T2 item2, T3 item3, T4 item4, Entity entity)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Entity = entity;
        }

        /// <summary>
        /// Clean up an existing object.
        /// </summary>
        /// <remarks>Objects of this type are typically created and destroyed automatically by the source generators.</remarks>
        /// <seealso cref="QueryEnumerableWithEntity(T1, T2, T3, T4, Entity)"/>
        /// <param name="item1">The value for <typeparamref name="T1"/>.</param>
        /// <param name="item2">The value for <typeparamref name="T2"/>.</param>
        /// <param name="item3">The value for <typeparamref name="T3"/>.</param>
        /// <param name="item4">The value for <typeparamref name="T4"/>.</param>
        /// <param name="entity">The entity</param>
        public void Deconstruct(out T1 item1, out T2 item2, out T3 item3, out T4 item4, out Entity entity)
        {
            item1 = Item1;
            item2 = Item2;
            item3 = Item3;
            item4 = Item4;
            entity = Entity;
        }

        /// <summary>
        /// Only select chunks that have a specified value for a shared component.
        /// </summary>
        /// <typeparam name="TSharedComponent1">The shared component type</typeparam>
        /// <param name="sharedComponent">The value of <typeparamref name="TSharedComponent1"/> which an entity must have in order to match this query</param>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4> WithSharedComponentFilter<TSharedComponent1>(TSharedComponent1 sharedComponent)
            where TSharedComponent1 : struct, ISharedComponentData
                => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select chunks that have a specified value for a shared component.
        /// </summary>
        /// <summary>
        /// Only select chunks that have the specified values for two shared components.
        /// </summary>
        /// <typeparam name="TSharedComponent1">The first shared component type</typeparam>
        /// <typeparam name="TSharedComponent2">The second shared component type</typeparam>
        /// <param name="sharedComponent1">The value of <typeparamref name="TSharedComponent1"/> which an entity must have in order to match this query</param>
        /// <param name="sharedComponent2">The value of <typeparamref name="TSharedComponent2"/> which an entity must have in order to match this query</param>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4> WithSharedComponentFilter<TSharedComponent1, TSharedComponent2>(TSharedComponent1 sharedComponent1, TSharedComponent2 sharedComponent2)
            where TSharedComponent1 : struct, ISharedComponentData
            where TSharedComponent2 : struct, ISharedComponentData
                => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select components in chunks in which the specified component might have changed since the last time the system updated.
        /// </summary>
        /// <typeparam name="TChangeFilter1">A component type</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4> WithChangeFilter<TChangeFilter1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select components in chunks in which the specified component might have changed since the last time the system updated.
        /// </summary>
        /// <typeparam name="TChangeFilter1">A component type</typeparam>
        /// <typeparam name="TChangeFilter2">A component type</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4> WithChangeFilter<TChangeFilter1, TChangeFilter2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify all read-only component types that must be present.
        /// </summary>
        /// <typeparam name="TComponent1">A component type</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4> WithAll<TComponent1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify all read-only component types that must be present.
        /// </summary>
        /// <typeparam name="TComponent1">A component type</typeparam>
        /// <typeparam name="TComponent2">A component type</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4> WithAll<TComponent1, TComponent2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify all read-only component types that must be present.
        /// </summary>
        /// <typeparam name="TComponent1">A component type</typeparam>
        /// <typeparam name="TComponent2">A component type</typeparam>
        /// <typeparam name="TComponent3">A component type</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4> WithAll<TComponent1, TComponent2, TComponent3>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify optional read-only component types.
        /// </summary>
        /// <typeparam name="TComponent1">Optional component</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4> WithAny<TComponent1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify optional read-only component types.
        /// </summary>
        /// <typeparam name="TComponent1">Optional component</typeparam>
        /// <typeparam name="TComponent2">Optional component</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4> WithAny<TComponent1, TComponent2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify optional read-only component types.
        /// </summary>
        /// <typeparam name="TComponent1">Optional component</typeparam>
        /// <typeparam name="TComponent2">Optional component</typeparam>
        /// <typeparam name="TComponent3">Optional component</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4> WithAny<TComponent1, TComponent2, TComponent3>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify component types that must be absent.
        /// </summary>
        /// <typeparam name="TComponent1">Absent component</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4> WithNone<TComponent1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify component types that must be absent.
        /// </summary>
        /// <typeparam name="TComponent1">Absent component</typeparam>
        /// <typeparam name="TComponent2">Absent component</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4> WithNone<TComponent1, TComponent2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify component types that must be absent.
        /// </summary>
        /// <typeparam name="TComponent1">Absent component</typeparam>
        /// <typeparam name="TComponent2">Absent component</typeparam>
        /// <typeparam name="TComponent3">Absent component</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4> WithNone<TComponent1, TComponent2, TComponent3>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify your own `EntityQueryOptions`.
        /// </summary>
        /// <remarks>
        /// This method may not be invoked more than once for each query description. Subsequent calls will override
        /// previous options, rather than adding to them. Use the bitwise OR operator '|' to combine multiple options.
        /// </remarks>
        /// <param name="options">The options for this query</param>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4> WithOptions(EntityQueryOptions options) => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Returns an enumerator over the entities in this query.
        /// </summary>
        /// <returns>An IEnumerator interface into the entities matched by this query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public IEnumerator<(T1, T2, T3, T4, Entity)> GetEnumerator() => throw InternalCompilerInterface.ThrowCodeGenException();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    /// <summary>An enumeration interface for the entities that match an <see cref="EntityQuery"/>.</summary>
    /// <remarks>
    /// This feature is primarily intended as a backend implementation for <see cref="SystemAPI.Query"/>. Application code
    /// should prefer to use that interface rather than using this type directly.
    /// </remarks>
    /// <typeparam name="T1">A component type</typeparam>
    /// <typeparam name="T2">A component type</typeparam>
    /// <typeparam name="T3">A component type</typeparam>
    /// <typeparam name="T4">A component type</typeparam>
    /// <typeparam name="T5">A component type</typeparam>
    public struct QueryEnumerable<T1, T2, T3, T4, T5> : IEnumerable<(T1, T2, T3, T4, T5)>
    {
        /// <summary>
        /// Specify all read-only component types that must be present.
        /// </summary>
        /// <typeparam name="TComponent1">A component type</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4, T5> WithAll<TComponent1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify all read-only component types that must be present.
        /// </summary>
        /// <typeparam name="TComponent1">A component type</typeparam>
        /// <typeparam name="TComponent2">A component type</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4, T5> WithAll<TComponent1, TComponent2>()  => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify all read-only component types that must be present.
        /// </summary>
        /// <typeparam name="TComponent1">A component type</typeparam>
        /// <typeparam name="TComponent2">A component type</typeparam>
        /// <typeparam name="TComponent3">A component type</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4, T5> WithAll<TComponent1, TComponent2, TComponent3>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify optional read-only component types.
        /// </summary>
        /// <typeparam name="TComponent1">Optional component</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4, T5> WithAny<TComponent1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify optional read-only component types.
        /// </summary>
        /// <typeparam name="TComponent1">Optional component</typeparam>
        /// <typeparam name="TComponent2">Optional component</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4, T5> WithAny<TComponent1, TComponent2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify optional read-only component types.
        /// </summary>
        /// <typeparam name="TComponent1">Optional component</typeparam>
        /// <typeparam name="TComponent2">Optional component</typeparam>
        /// <typeparam name="TComponent3">Optional component</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4, T5> WithAny<TComponent1, TComponent2, TComponent3>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify component types that must be absent.
        /// </summary>
        /// <typeparam name="TComponent1">Absent component</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4, T5> WithNone<TComponent1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify component types that must be absent.
        /// </summary>
        /// <typeparam name="TComponent1">Absent component</typeparam>
        /// <typeparam name="TComponent2">Absent component</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4, T5> WithNone<TComponent1, TComponent2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify component types that must be absent.
        /// </summary>
        /// <typeparam name="TComponent1">Absent component</typeparam>
        /// <typeparam name="TComponent2">Absent component</typeparam>
        /// <typeparam name="TComponent3">Absent component</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4, T5> WithNone<TComponent1, TComponent2, TComponent3>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select components in chunks in which the specified component might have changed since the last time the system updated.
        /// </summary>
        /// <typeparam name="TChangeFilter1">A component type</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4, T5> WithChangeFilter<TChangeFilter1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select components in chunks in which the specified component might have changed since the last time the system updated.
        /// </summary>
        /// <typeparam name="TChangeFilter1">A component type</typeparam>
        /// <typeparam name="TChangeFilter2">A component type</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4, T5> WithChangeFilter<TChangeFilter1, TChangeFilter2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify your own `EntityQueryOptions`.
        /// </summary>
        /// <remarks>
        /// This method may not be invoked more than once for each query description. Subsequent calls will override
        /// previous options, rather than adding to them. Use the bitwise OR operator '|' to combine multiple options.
        /// </remarks>
        /// <param name="options">The options for this query</param>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4, T5> WithOptions(EntityQueryOptions options) => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select chunks that have a specified value for a shared component.
        /// </summary>
        /// <typeparam name="TSharedComponent1">The shared component type</typeparam>
        /// <param name="sharedComponent">The value of <typeparamref name="TSharedComponent1"/> which an entity must have in order to match this query</param>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4, T5> WithSharedComponentFilter<TSharedComponent1>(TSharedComponent1 sharedComponent)
            where TSharedComponent1 : struct, ISharedComponentData
                => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select chunks that have the specified values for two shared components.
        /// </summary>
        /// <typeparam name="TSharedComponent1">The first shared component type</typeparam>
        /// <typeparam name="TSharedComponent2">The second shared component type</typeparam>
        /// <param name="sharedComponent1">The value of <typeparamref name="TSharedComponent1"/> which an entity must have in order to match this query</param>
        /// <param name="sharedComponent2">The value of <typeparamref name="TSharedComponent2"/> which an entity must have in order to match this query</param>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4, T5> WithSharedComponentFilter<TSharedComponent1, TSharedComponent2>(TSharedComponent1 sharedComponent1, TSharedComponent2 sharedComponent2)
            where TSharedComponent1 : struct, ISharedComponentData
            where TSharedComponent2 : struct, ISharedComponentData
                => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Invoke this method if you wish to retrieve a tuple with an `Entity` parameter, thus giving you direct access to an entity.
        /// </summary>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4, T5> WithEntityAccess() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Returns an enumerator over the entities in this query.
        /// </summary>
        /// <returns>An IEnumerator interface into the entities matched by this query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public IEnumerator<(T1, T2, T3, T4, T5)> GetEnumerator() => throw InternalCompilerInterface.ThrowCodeGenException();
        IEnumerator IEnumerable.GetEnumerator() => throw InternalCompilerInterface.ThrowCodeGenException();
    }

    /// <summary>An enumeration interface for the entities that match an <see cref="EntityQuery"/>. This variant includes access the the entity and its component values.</summary>
    /// <remarks>
    /// This feature is primarily intended as a backend implementation for <see cref="SystemAPI.Query"/>. Application code
    /// should prefer to use that interface rather than using this type directly.
    /// </remarks>
    /// <typeparam name="T1">A component type</typeparam>
    /// <typeparam name="T2">A component type</typeparam>
    /// <typeparam name="T3">A component type</typeparam>
    /// <typeparam name="T4">A component type</typeparam>
    /// <typeparam name="T5">A component type</typeparam>
    public readonly struct QueryEnumerableWithEntity<T1, T2, T3, T4, T5> : IEnumerable<(T1, T2, T3, T4, T5, Entity)>
    {
        /// <summary>A component value for the current entity.</summary>
        public readonly T1 Item1;
        /// <summary>A component value for the current entity.</summary>
        public readonly T2 Item2;
        /// <summary>A component value for the current entity.</summary>
        public readonly T3 Item3;
        /// <summary>A component value for the current entity.</summary>
        public readonly T4 Item4;
        /// <summary>A component value for the current entity.</summary>
        public readonly T5 Item5;
        /// <summary>A component value for the current entity.</summary>
        public readonly Entity Entity;

        /// <summary>
        /// Construct a new object.
        /// </summary>
        /// <remarks>Objects of this type are typically created and destroyed automatically by the source generators.</remarks>
        /// <seealso cref="Deconstruct"/>
        /// <param name="item1">The value for <typeparamref name="T1"/>.</param>
        /// <param name="item2">The value for <typeparamref name="T2"/>.</param>
        /// <param name="item3">The value for <typeparamref name="T3"/>.</param>
        /// <param name="item4">The value for <typeparamref name="T4"/>.</param>
        /// <param name="item5">The value for <typeparamref name="T5"/>.</param>
        /// <param name="entity">The entity</param>
        public QueryEnumerableWithEntity(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, Entity entity)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
            Entity = entity;
        }

        /// <summary>
        /// Clean up an existing object.
        /// </summary>
        /// <remarks>Objects of this type are typically created and destroyed automatically by the source generators.</remarks>
        /// <seealso cref="QueryEnumerableWithEntity(T1, T2, T3, T4, T5, Entity)"/>
        /// <param name="item1">The value for <typeparamref name="T1"/>.</param>
        /// <param name="item2">The value for <typeparamref name="T2"/>.</param>
        /// <param name="item3">The value for <typeparamref name="T3"/>.</param>
        /// <param name="item4">The value for <typeparamref name="T4"/>.</param>
        /// <param name="item5">The value for <typeparamref name="T5"/>.</param>
        /// <param name="entity">The entity</param>
        public void Deconstruct(out T1 item1, out T2 item2, out T3 item3, out T4 item4, out T5 item5, out Entity entity)
        {
            item1 = Item1;
            item2 = Item2;
            item3 = Item3;
            item4 = Item4;
            item5 = Item5;
            entity = Entity;
        }

        /// <summary>
        /// Only select chunks that have a specified value for a shared component.
        /// </summary>
        /// <typeparam name="TSharedComponent1">The shared component type</typeparam>
        /// <param name="sharedComponent">The value of <typeparamref name="TSharedComponent1"/> which an entity must have in order to match this query</param>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4, T5> WithSharedComponentFilter<TSharedComponent1>(TSharedComponent1 sharedComponent)
            where TSharedComponent1 : struct, ISharedComponentData
                => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select chunks that have a specified value for a shared component.
        /// </summary>
        /// <summary>
        /// Only select chunks that have the specified values for two shared components.
        /// </summary>
        /// <typeparam name="TSharedComponent1">The first shared component type</typeparam>
        /// <typeparam name="TSharedComponent2">The second shared component type</typeparam>
        /// <param name="sharedComponent1">The value of <typeparamref name="TSharedComponent1"/> which an entity must have in order to match this query</param>
        /// <param name="sharedComponent2">The value of <typeparamref name="TSharedComponent2"/> which an entity must have in order to match this query</param>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4, T5> WithSharedComponentFilter<TSharedComponent1, TSharedComponent2>(TSharedComponent1 sharedComponent1, TSharedComponent2 sharedComponent2)
            where TSharedComponent1 : struct, ISharedComponentData
            where TSharedComponent2 : struct, ISharedComponentData
                => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select components in chunks in which the specified component might have changed since the last time the system updated.
        /// </summary>
        /// <typeparam name="TChangeFilter1">A component type</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4, T5> WithChangeFilter<TChangeFilter1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select components in chunks in which the specified component might have changed since the last time the system updated.
        /// </summary>
        /// <typeparam name="TChangeFilter1">A component type</typeparam>
        /// <typeparam name="TChangeFilter2">A component type</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4, T5> WithChangeFilter<TChangeFilter1, TChangeFilter2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify all read-only component types that must be present.
        /// </summary>
        /// <typeparam name="TComponent1">A component type</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4, T5> WithAll<TComponent1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify all read-only component types that must be present.
        /// </summary>
        /// <typeparam name="TComponent1">A component type</typeparam>
        /// <typeparam name="TComponent2">A component type</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4, T5> WithAll<TComponent1, TComponent2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify all read-only component types that must be present.
        /// </summary>
        /// <typeparam name="TComponent1">A component type</typeparam>
        /// <typeparam name="TComponent2">A component type</typeparam>
        /// <typeparam name="TComponent3">A component type</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4, T5> WithAll<TComponent1, TComponent2, TComponent3>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify optional read-only component types.
        /// </summary>
        /// <typeparam name="TComponent1">Optional component</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4, T5> WithAny<TComponent1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify optional read-only component types.
        /// </summary>
        /// <typeparam name="TComponent1">Optional component</typeparam>
        /// <typeparam name="TComponent2">Optional component</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4, T5> WithAny<TComponent1, TComponent2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify optional read-only component types.
        /// </summary>
        /// <typeparam name="TComponent1">Optional component</typeparam>
        /// <typeparam name="TComponent2">Optional component</typeparam>
        /// <typeparam name="TComponent3">Optional component</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4, T5> WithAny<TComponent1, TComponent2, TComponent3>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify component types that must be absent.
        /// </summary>
        /// <typeparam name="TComponent1">Absent component</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4, T5> WithNone<TComponent1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify component types that must be absent.
        /// </summary>
        /// <typeparam name="TComponent1">Absent component</typeparam>
        /// <typeparam name="TComponent2">Absent component</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4, T5> WithNone<TComponent1, TComponent2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify component types that must be absent.
        /// </summary>
        /// <typeparam name="TComponent1">Absent component</typeparam>
        /// <typeparam name="TComponent2">Absent component</typeparam>
        /// <typeparam name="TComponent3">Absent component</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4, T5> WithNone<TComponent1, TComponent2, TComponent3>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify your own `EntityQueryOptions`.
        /// </summary>
        /// <remarks>
        /// This method may not be invoked more than once for each query description. Subsequent calls will override
        /// previous options, rather than adding to them. Use the bitwise OR operator '|' to combine multiple options.
        /// </remarks>
        /// <param name="options">The options for this query</param>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4, T5> WithOptions(EntityQueryOptions options) => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Returns an enumerator over the entities in this query.
        /// </summary>
        /// <returns>An IEnumerator interface into the entities matched by this query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public IEnumerator<(T1, T2, T3, T4, T5, Entity)> GetEnumerator() => throw InternalCompilerInterface.ThrowCodeGenException();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    /// <summary>An enumeration interface for the entities that match an <see cref="EntityQuery"/>.</summary>
    /// <remarks>
    /// This feature is primarily intended as a backend implementation for <see cref="SystemAPI.Query"/>. Application code
    /// should prefer to use that interface rather than using this type directly.
    /// </remarks>
    /// <typeparam name="T1">A component type</typeparam>
    /// <typeparam name="T2">A component type</typeparam>
    /// <typeparam name="T3">A component type</typeparam>
    /// <typeparam name="T4">A component type</typeparam>
    /// <typeparam name="T5">A component type</typeparam>
    /// <typeparam name="T6">A component type</typeparam>
    public struct QueryEnumerable<T1, T2, T3, T4, T5, T6> : IEnumerable<(T1, T2, T3, T4, T5, T6)>
    {
        /// <summary>
        /// Specify all read-only component types that must be present.
        /// </summary>
        /// <typeparam name="TComponent1">A component type</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4, T5, T6> WithAll<TComponent1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify all read-only component types that must be present.
        /// </summary>
        /// <typeparam name="TComponent1">A component type</typeparam>
        /// <typeparam name="TComponent2">A component type</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4, T5, T6> WithAll<TComponent1, TComponent2>()  => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify all read-only component types that must be present.
        /// </summary>
        /// <typeparam name="TComponent1">A component type</typeparam>
        /// <typeparam name="TComponent2">A component type</typeparam>
        /// <typeparam name="TComponent3">A component type</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4, T5, T6> WithAll<TComponent1, TComponent2, TComponent3>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify optional read-only component types.
        /// </summary>
        /// <typeparam name="TComponent1">Optional component</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4, T5, T6> WithAny<TComponent1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify optional read-only component types.
        /// </summary>
        /// <typeparam name="TComponent1">Optional component</typeparam>
        /// <typeparam name="TComponent2">Optional component</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4, T5, T6> WithAny<TComponent1, TComponent2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify optional read-only component types.
        /// </summary>
        /// <typeparam name="TComponent1">Optional component</typeparam>
        /// <typeparam name="TComponent2">Optional component</typeparam>
        /// <typeparam name="TComponent3">Optional component</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4, T5, T6> WithAny<TComponent1, TComponent2, TComponent3>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify component types that must be absent.
        /// </summary>
        /// <typeparam name="TComponent1">Absent component</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4, T5, T6> WithNone<TComponent1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify component types that must be absent.
        /// </summary>
        /// <typeparam name="TComponent1">Absent component</typeparam>
        /// <typeparam name="TComponent2">Absent component</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4, T5, T6> WithNone<TComponent1, TComponent2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify component types that must be absent.
        /// </summary>
        /// <typeparam name="TComponent1">Absent component</typeparam>
        /// <typeparam name="TComponent2">Absent component</typeparam>
        /// <typeparam name="TComponent3">Absent component</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4, T5, T6> WithNone<TComponent1, TComponent2, TComponent3>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select components in chunks in which the specified component might have changed since the last time the system updated.
        /// </summary>
        /// <typeparam name="TChangeFilter1">A component type</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4, T5, T6> WithChangeFilter<TChangeFilter1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select components in chunks in which the specified component might have changed since the last time the system updated.
        /// </summary>
        /// <typeparam name="TChangeFilter1">A component type</typeparam>
        /// <typeparam name="TChangeFilter2">A component type</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4, T5, T6> WithChangeFilter<TChangeFilter1, TChangeFilter2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify your own `EntityQueryOptions`.
        /// </summary>
        /// <remarks>
        /// This method may not be invoked more than once for each query description. Subsequent calls will override
        /// previous options, rather than adding to them. Use the bitwise OR operator '|' to combine multiple options.
        /// </remarks>
        /// <param name="options">The options for this query</param>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4, T5, T6> WithOptions(EntityQueryOptions options) => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select chunks that have a specified value for a shared component.
        /// </summary>
        /// <typeparam name="TSharedComponent1">The shared component type</typeparam>
        /// <param name="sharedComponent">The value of <typeparamref name="TSharedComponent1"/> which an entity must have in order to match this query</param>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4, T5, T6> WithSharedComponentFilter<TSharedComponent1>(TSharedComponent1 sharedComponent)
            where TSharedComponent1 : struct, ISharedComponentData
                => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select chunks that have the specified values for two shared components.
        /// </summary>
        /// <typeparam name="TSharedComponent1">The first shared component type</typeparam>
        /// <typeparam name="TSharedComponent2">The second shared component type</typeparam>
        /// <param name="sharedComponent1">The value of <typeparamref name="TSharedComponent1"/> which an entity must have in order to match this query</param>
        /// <param name="sharedComponent2">The value of <typeparamref name="TSharedComponent2"/> which an entity must have in order to match this query</param>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4, T5, T6> WithSharedComponentFilter<TSharedComponent1, TSharedComponent2>(TSharedComponent1 sharedComponent1, TSharedComponent2 sharedComponent2)
            where TSharedComponent1 : struct, ISharedComponentData
            where TSharedComponent2 : struct, ISharedComponentData
                => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Invoke this method if you wish to retrieve a tuple with an `Entity` parameter, thus giving you direct access to an entity.
        /// </summary>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6> WithEntityAccess() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Returns an enumerator over the entities in this query.
        /// </summary>
        /// <returns>An IEnumerator interface into the entities matched by this query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public IEnumerator<(T1, T2, T3, T4, T5, T6)> GetEnumerator() => throw InternalCompilerInterface.ThrowCodeGenException();
        IEnumerator IEnumerable.GetEnumerator() => throw InternalCompilerInterface.ThrowCodeGenException();
    }

    /// <summary>An enumeration interface for the entities that match an <see cref="EntityQuery"/>. This variant includes access the the entity and its component values.</summary>
    /// <remarks>
    /// This feature is primarily intended as a backend implementation for <see cref="SystemAPI.Query"/>. Application code
    /// should prefer to use that interface rather than using this type directly.
    /// </remarks>
    /// <typeparam name="T1">A component type</typeparam>
    /// <typeparam name="T2">A component type</typeparam>
    /// <typeparam name="T3">A component type</typeparam>
    /// <typeparam name="T4">A component type</typeparam>
    /// <typeparam name="T5">A component type</typeparam>
    /// <typeparam name="T6">A component type</typeparam>
    public readonly struct QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6> : IEnumerable<(T1, T2, T3, T4, T5, T6, Entity)>
    {
        /// <summary>A component value for the current entity.</summary>
        public readonly T1 Item1;
        /// <summary>A component value for the current entity.</summary>
        public readonly T2 Item2;
        /// <summary>A component value for the current entity.</summary>
        public readonly T3 Item3;
        /// <summary>A component value for the current entity.</summary>
        public readonly T4 Item4;
        /// <summary>A component value for the current entity.</summary>
        public readonly T5 Item5;
        /// <summary>A component value for the current entity.</summary>
        public readonly T6 Item6;
        /// <summary>A component value for the current entity.</summary>
        public readonly Entity Entity;

        /// <summary>
        /// Construct a new object.
        /// </summary>
        /// <remarks>Objects of this type are typically created and destroyed automatically by the source generators.</remarks>
        /// <seealso cref="Deconstruct"/>
        /// <param name="item1">The value for <typeparamref name="T1"/>.</param>
        /// <param name="item2">The value for <typeparamref name="T2"/>.</param>
        /// <param name="item3">The value for <typeparamref name="T3"/>.</param>
        /// <param name="item4">The value for <typeparamref name="T4"/>.</param>
        /// <param name="item5">The value for <typeparamref name="T5"/>.</param>
        /// <param name="item6">The value for <typeparamref name="T6"/>.</param>
        /// <param name="entity">The entity</param>
        public QueryEnumerableWithEntity(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, Entity entity)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
            Item6 = item6;
            Entity = entity;
        }

        /// <summary>
        /// Clean up an existing object.
        /// </summary>
        /// <remarks>Objects of this type are typically created and destroyed automatically by the source generators.</remarks>
        /// <seealso cref="QueryEnumerableWithEntity(T1, T2, T3, T4, T5, T6, Entity)"/>
        /// <param name="item1">The value for <typeparamref name="T1"/>.</param>
        /// <param name="item2">The value for <typeparamref name="T2"/>.</param>
        /// <param name="item3">The value for <typeparamref name="T3"/>.</param>
        /// <param name="item4">The value for <typeparamref name="T4"/>.</param>
        /// <param name="item5">The value for <typeparamref name="T5"/>.</param>
        /// <param name="item6">The value for <typeparamref name="T6"/>.</param>
        /// <param name="entity">The entity</param>
        public void Deconstruct(out T1 item1, out T2 item2, out T3 item3, out T4 item4, out T5 item5, out T6 item6, out Entity entity)
        {
            item1 = Item1;
            item2 = Item2;
            item3 = Item3;
            item4 = Item4;
            item5 = Item5;
            item6 = Item6;
            entity = Entity;
        }

        /// <summary>
        /// Only select chunks that have a specified value for a shared component.
        /// </summary>
        /// <typeparam name="TSharedComponent1">The shared component type</typeparam>
        /// <param name="sharedComponent">The value of <typeparamref name="TSharedComponent1"/> which an entity must have in order to match this query</param>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6> WithSharedComponentFilter<TSharedComponent1>(TSharedComponent1 sharedComponent)
            where TSharedComponent1 : struct, ISharedComponentData
                => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select chunks that have a specified value for a shared component.
        /// </summary>
        /// <summary>
        /// Only select chunks that have the specified values for two shared components.
        /// </summary>
        /// <typeparam name="TSharedComponent1">The first shared component type</typeparam>
        /// <typeparam name="TSharedComponent2">The second shared component type</typeparam>
        /// <param name="sharedComponent1">The value of <typeparamref name="TSharedComponent1"/> which an entity must have in order to match this query</param>
        /// <param name="sharedComponent2">The value of <typeparamref name="TSharedComponent2"/> which an entity must have in order to match this query</param>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6> WithSharedComponentFilter<TSharedComponent1, TSharedComponent2>(TSharedComponent1 sharedComponent1, TSharedComponent2 sharedComponent2)
            where TSharedComponent1 : struct, ISharedComponentData
            where TSharedComponent2 : struct, ISharedComponentData
                => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select components in chunks in which the specified component might have changed since the last time the system updated.
        /// </summary>
        /// <typeparam name="TChangeFilter1">A component type</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6> WithChangeFilter<TChangeFilter1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select components in chunks in which the specified component might have changed since the last time the system updated.
        /// </summary>
        /// <typeparam name="TChangeFilter1">A component type</typeparam>
        /// <typeparam name="TChangeFilter2">A component type</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6> WithChangeFilter<TChangeFilter1, TChangeFilter2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify all read-only component types that must be present.
        /// </summary>
        /// <typeparam name="TComponent1">A component type</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6> WithAll<TComponent1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify all read-only component types that must be present.
        /// </summary>
        /// <typeparam name="TComponent1">A component type</typeparam>
        /// <typeparam name="TComponent2">A component type</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6> WithAll<TComponent1, TComponent2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify all read-only component types that must be present.
        /// </summary>
        /// <typeparam name="TComponent1">A component type</typeparam>
        /// <typeparam name="TComponent2">A component type</typeparam>
        /// <typeparam name="TComponent3">A component type</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6> WithAll<TComponent1, TComponent2, TComponent3>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify optional read-only component types.
        /// </summary>
        /// <typeparam name="TComponent1">Optional component</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6> WithAny<TComponent1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify optional read-only component types.
        /// </summary>
        /// <typeparam name="TComponent1">Optional component</typeparam>
        /// <typeparam name="TComponent2">Optional component</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6> WithAny<TComponent1, TComponent2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify optional read-only component types.
        /// </summary>
        /// <typeparam name="TComponent1">Optional component</typeparam>
        /// <typeparam name="TComponent2">Optional component</typeparam>
        /// <typeparam name="TComponent3">Optional component</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6> WithAny<TComponent1, TComponent2, TComponent3>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify component types that must be absent.
        /// </summary>
        /// <typeparam name="TComponent1">Absent component</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6> WithNone<TComponent1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify component types that must be absent.
        /// </summary>
        /// <typeparam name="TComponent1">Absent component</typeparam>
        /// <typeparam name="TComponent2">Absent component</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6> WithNone<TComponent1, TComponent2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify component types that must be absent.
        /// </summary>
        /// <typeparam name="TComponent1">Absent component</typeparam>
        /// <typeparam name="TComponent2">Absent component</typeparam>
        /// <typeparam name="TComponent3">Absent component</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6> WithNone<TComponent1, TComponent2, TComponent3>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify your own `EntityQueryOptions`.
        /// </summary>
        /// <remarks>
        /// This method may not be invoked more than once for each query description. Subsequent calls will override
        /// previous options, rather than adding to them. Use the bitwise OR operator '|' to combine multiple options.
        /// </remarks>
        /// <param name="options">The options for this query</param>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6> WithOptions(EntityQueryOptions options) => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Returns an enumerator over the entities in this query.
        /// </summary>
        /// <returns>An IEnumerator interface into the entities matched by this query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public IEnumerator<(T1, T2, T3, T4, T5, T6, Entity)> GetEnumerator() => throw InternalCompilerInterface.ThrowCodeGenException();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    /// <summary>An enumeration interface for the entities that match an <see cref="EntityQuery"/>.</summary>
    /// <remarks>
    /// This feature is primarily intended as a backend implementation for <see cref="SystemAPI.Query"/>. Application code
    /// should prefer to use that interface rather than using this type directly.
    /// </remarks>
    /// <typeparam name="T1">A component type</typeparam>
    /// <typeparam name="T2">A component type</typeparam>
    /// <typeparam name="T3">A component type</typeparam>
    /// <typeparam name="T4">A component type</typeparam>
    /// <typeparam name="T5">A component type</typeparam>
    /// <typeparam name="T6">A component type</typeparam>
    /// <typeparam name="T7">A component type</typeparam>
    public struct QueryEnumerable<T1, T2, T3, T4, T5, T6, T7> : IEnumerable<(T1, T2, T3, T4, T5, T6, T7)>
    {
        /// <summary>
        /// Specify all read-only component types that must be present.
        /// </summary>
        /// <typeparam name="TComponent1">A component type</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4, T5, T6, T7> WithAll<TComponent1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify all read-only component types that must be present.
        /// </summary>
        /// <typeparam name="TComponent1">A component type</typeparam>
        /// <typeparam name="TComponent2">A component type</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4, T5, T6, T7> WithAll<TComponent1, TComponent2>()  => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify all read-only component types that must be present.
        /// </summary>
        /// <typeparam name="TComponent1">A component type</typeparam>
        /// <typeparam name="TComponent2">A component type</typeparam>
        /// <typeparam name="TComponent3">A component type</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4, T5, T6, T7> WithAll<TComponent1, TComponent2, TComponent3>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify optional read-only component types.
        /// </summary>
        /// <typeparam name="TComponent1">Optional component</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4, T5, T6, T7> WithAny<TComponent1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify optional read-only component types.
        /// </summary>
        /// <typeparam name="TComponent1">Optional component</typeparam>
        /// <typeparam name="TComponent2">Optional component</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4, T5, T6, T7> WithAny<TComponent1, TComponent2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify optional read-only component types.
        /// </summary>
        /// <typeparam name="TComponent1">Optional component</typeparam>
        /// <typeparam name="TComponent2">Optional component</typeparam>
        /// <typeparam name="TComponent3">Optional component</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4, T5, T6, T7> WithAny<TComponent1, TComponent2, TComponent3>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify component types that must be absent.
        /// </summary>
        /// <typeparam name="TComponent1">Absent component</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4, T5, T6, T7> WithNone<TComponent1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify component types that must be absent.
        /// </summary>
        /// <typeparam name="TComponent1">Absent component</typeparam>
        /// <typeparam name="TComponent2">Absent component</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4, T5, T6, T7> WithNone<TComponent1, TComponent2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify component types that must be absent.
        /// </summary>
        /// <typeparam name="TComponent1">Absent component</typeparam>
        /// <typeparam name="TComponent2">Absent component</typeparam>
        /// <typeparam name="TComponent3">Absent component</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4, T5, T6, T7> WithNone<TComponent1, TComponent2, TComponent3>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select components in chunks in which the specified component might have changed since the last time the system updated.
        /// </summary>
        /// <typeparam name="TChangeFilter1">A component type</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4, T5, T6, T7> WithChangeFilter<TChangeFilter1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select components in chunks in which the specified component might have changed since the last time the system updated.
        /// </summary>
        /// <typeparam name="TChangeFilter1">A component type</typeparam>
        /// <typeparam name="TChangeFilter2">A component type</typeparam>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4, T5, T6, T7> WithChangeFilter<TChangeFilter1, TChangeFilter2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify your own `EntityQueryOptions`.
        /// </summary>
        /// <remarks>
        /// This method may not be invoked more than once for each query description. Subsequent calls will override
        /// previous options, rather than adding to them. Use the bitwise OR operator '|' to combine multiple options.
        /// </remarks>
        /// <param name="options">The options for this query</param>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4, T5, T6, T7> WithOptions(EntityQueryOptions options) => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select chunks that have a specified value for a shared component.
        /// </summary>
        /// <typeparam name="TSharedComponent1">The shared component type</typeparam>
        /// <param name="sharedComponent">The value of <typeparamref name="TSharedComponent1"/> which an entity must have in order to match this query</param>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4, T5, T6, T7> WithSharedComponentFilter<TSharedComponent1>(TSharedComponent1 sharedComponent)
            where TSharedComponent1 : struct, ISharedComponentData
                => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select chunks that have the specified values for two shared components.
        /// </summary>
        /// <typeparam name="TSharedComponent1">The first shared component type</typeparam>
        /// <typeparam name="TSharedComponent2">The second shared component type</typeparam>
        /// <param name="sharedComponent1">The value of <typeparamref name="TSharedComponent1"/> which an entity must have in order to match this query</param>
        /// <param name="sharedComponent2">The value of <typeparamref name="TSharedComponent2"/> which an entity must have in order to match this query</param>
        /// <returns>QueryEnumerable, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO and EnabledRefRW that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerable<T1, T2, T3, T4, T5, T6, T7> WithSharedComponentFilter<TSharedComponent1, TSharedComponent2>(TSharedComponent1 sharedComponent1, TSharedComponent2 sharedComponent2)
            where TSharedComponent1 : struct, ISharedComponentData
            where TSharedComponent2 : struct, ISharedComponentData
                => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Invoke this method if you wish to retrieve a tuple with an `Entity` parameter, thus giving you direct access to an entity.
        /// </summary>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6, T7> WithEntityAccess() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Returns an enumerator over the entities in this query.
        /// </summary>
        /// <returns>An IEnumerator interface into the entities matched by this query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public IEnumerator<(T1, T2, T3, T4, T5, T6, T7)> GetEnumerator() => throw InternalCompilerInterface.ThrowCodeGenException();
        IEnumerator IEnumerable.GetEnumerator() => throw InternalCompilerInterface.ThrowCodeGenException();
    }

    /// <summary>An enumeration interface for the entities that match an <see cref="EntityQuery"/>. This variant includes access the the entity and its component values.</summary>
    /// <remarks>
    /// This feature is primarily intended as a backend implementation for <see cref="SystemAPI.Query"/>. Application code
    /// should prefer to use that interface rather than using this type directly.
    /// </remarks>
    /// <typeparam name="T1">A component type</typeparam>
    /// <typeparam name="T2">A component type</typeparam>
    /// <typeparam name="T3">A component type</typeparam>
    /// <typeparam name="T4">A component type</typeparam>
    /// <typeparam name="T5">A component type</typeparam>
    /// <typeparam name="T6">A component type</typeparam>
    /// <typeparam name="T7">A component type</typeparam>
    public readonly struct QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6, T7> : IEnumerable<(T1, T2, T3, T4, T5, T6, T7, Entity)>
    {
        /// <summary>A component value for the current entity.</summary>
        public readonly T1 Item1;
        /// <summary>A component value for the current entity.</summary>
        public readonly T2 Item2;
        /// <summary>A component value for the current entity.</summary>
        public readonly T3 Item3;
        /// <summary>A component value for the current entity.</summary>
        public readonly T4 Item4;
        /// <summary>A component value for the current entity.</summary>
        public readonly T5 Item5;
        /// <summary>A component value for the current entity.</summary>
        public readonly T6 Item6;
        /// <summary>A component value for the current entity.</summary>
        public readonly T7 Item7;
        /// <summary>A component value for the current entity.</summary>
        public readonly Entity Entity;

        /// <summary>
        /// Construct a new object.
        /// </summary>
        /// <remarks>Objects of this type are typically created and destroyed automatically by the source generators.</remarks>
        /// <seealso cref="Deconstruct"/>
        /// <param name="item1">The value for <typeparamref name="T1"/>.</param>
        /// <param name="item2">The value for <typeparamref name="T2"/>.</param>
        /// <param name="item3">The value for <typeparamref name="T3"/>.</param>
        /// <param name="item4">The value for <typeparamref name="T4"/>.</param>
        /// <param name="item5">The value for <typeparamref name="T5"/>.</param>
        /// <param name="item6">The value for <typeparamref name="T6"/>.</param>
        /// <param name="item7">The value for <typeparamref name="T7"/>.</param>
        /// <param name="entity">The entity</param>
        public QueryEnumerableWithEntity(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, Entity entity)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
            Item6 = item6;
            Item7 = item7;
            Entity = entity;
        }

        /// <summary>
        /// Clean up an existing object.
        /// </summary>
        /// <remarks>Objects of this type are typically created and destroyed automatically by the source generators.</remarks>
        /// <seealso cref="QueryEnumerableWithEntity(T1, T2, T3, T4, T5, T6, T7, Entity)"/>
        /// <param name="item1">The value for <typeparamref name="T1"/>.</param>
        /// <param name="item2">The value for <typeparamref name="T2"/>.</param>
        /// <param name="item3">The value for <typeparamref name="T3"/>.</param>
        /// <param name="item4">The value for <typeparamref name="T4"/>.</param>
        /// <param name="item5">The value for <typeparamref name="T5"/>.</param>
        /// <param name="item6">The value for <typeparamref name="T6"/>.</param>
        /// <param name="item7">The value for <typeparamref name="T7"/>.</param>
        /// <param name="entity">The entity</param>
        public void Deconstruct(out T1 item1, out T2 item2, out T3 item3, out T4 item4, out T5 item5, out T6 item6, out T7 item7, out Entity entity)
        {
            item1 = Item1;
            item2 = Item2;
            item3 = Item3;
            item4 = Item4;
            item5 = Item5;
            item6 = Item6;
            item7 = Item7;
            entity = Entity;
        }

        /// <summary>
        /// Only select chunks that have a specified value for a shared component.
        /// </summary>
        /// <typeparam name="TSharedComponent1">The shared component type</typeparam>
        /// <param name="sharedComponent">The value of <typeparamref name="TSharedComponent1"/> which an entity must have in order to match this query</param>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6, T7> WithSharedComponentFilter<TSharedComponent1>(TSharedComponent1 sharedComponent)
            where TSharedComponent1 : struct, ISharedComponentData
                => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select chunks that have a specified value for a shared component.
        /// </summary>
        /// <summary>
        /// Only select chunks that have the specified values for two shared components.
        /// </summary>
        /// <typeparam name="TSharedComponent1">The first shared component type</typeparam>
        /// <typeparam name="TSharedComponent2">The second shared component type</typeparam>
        /// <param name="sharedComponent1">The value of <typeparamref name="TSharedComponent1"/> which an entity must have in order to match this query</param>
        /// <param name="sharedComponent2">The value of <typeparamref name="TSharedComponent2"/> which an entity must have in order to match this query</param>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6, T7> WithSharedComponentFilter<TSharedComponent1, TSharedComponent2>(TSharedComponent1 sharedComponent1, TSharedComponent2 sharedComponent2)
            where TSharedComponent1 : struct, ISharedComponentData
            where TSharedComponent2 : struct, ISharedComponentData
                => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select components in chunks in which the specified component might have changed since the last time the system updated.
        /// </summary>
        /// <typeparam name="TChangeFilter1">A component type</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6, T7> WithChangeFilter<TChangeFilter1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Only select components in chunks in which the specified component might have changed since the last time the system updated.
        /// </summary>
        /// <typeparam name="TChangeFilter1">A component type</typeparam>
        /// <typeparam name="TChangeFilter2">A component type</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6, T7> WithChangeFilter<TChangeFilter1, TChangeFilter2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify all read-only component types that must be present.
        /// </summary>
        /// <typeparam name="TComponent1">A component type</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6, T7> WithAll<TComponent1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify all read-only component types that must be present.
        /// </summary>
        /// <typeparam name="TComponent1">A component type</typeparam>
        /// <typeparam name="TComponent2">A component type</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6, T7> WithAll<TComponent1, TComponent2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify all read-only component types that must be present.
        /// </summary>
        /// <typeparam name="TComponent1">A component type</typeparam>
        /// <typeparam name="TComponent2">A component type</typeparam>
        /// <typeparam name="TComponent3">A component type</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6, T7> WithAll<TComponent1, TComponent2, TComponent3>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify optional read-only component types.
        /// </summary>
        /// <typeparam name="TComponent1">Optional component</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6, T7> WithAny<TComponent1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify optional read-only component types.
        /// </summary>
        /// <typeparam name="TComponent1">Optional component</typeparam>
        /// <typeparam name="TComponent2">Optional component</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6, T7> WithAny<TComponent1, TComponent2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify optional read-only component types.
        /// </summary>
        /// <typeparam name="TComponent1">Optional component</typeparam>
        /// <typeparam name="TComponent2">Optional component</typeparam>
        /// <typeparam name="TComponent3">Optional component</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6, T7> WithAny<TComponent1, TComponent2, TComponent3>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify component types that must be absent.
        /// </summary>
        /// <typeparam name="TComponent1">Absent component</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6, T7> WithNone<TComponent1>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify component types that must be absent.
        /// </summary>
        /// <typeparam name="TComponent1">Absent component</typeparam>
        /// <typeparam name="TComponent2">Absent component</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6, T7> WithNone<TComponent1, TComponent2>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify component types that must be absent.
        /// </summary>
        /// <typeparam name="TComponent1">Absent component</typeparam>
        /// <typeparam name="TComponent2">Absent component</typeparam>
        /// <typeparam name="TComponent3">Absent component</typeparam>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6, T7> WithNone<TComponent1, TComponent2, TComponent3>() => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Specify your own `EntityQueryOptions`.
        /// </summary>
        /// <remarks>
        /// This method may not be invoked more than once for each query description. Subsequent calls will override
        /// previous options, rather than adding to them. Use the bitwise OR operator '|' to combine multiple options.
        /// </remarks>
        /// <param name="options">The options for this query</param>
        /// <returns>QueryEnumerableWithEntity, which allows enumerating over all Aspects, RefRO, RefRW, components, EnabledRefRO, EnabledRefRW and entities that match the constructed Query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public QueryEnumerableWithEntity<T1, T2, T3, T4, T5, T6, T7> WithOptions(EntityQueryOptions options) => throw InternalCompilerInterface.ThrowCodeGenException();

        /// <summary>
        /// Returns an enumerator over the entities in this query.
        /// </summary>
        /// <returns>An IEnumerator interface into the entities matched by this query.</returns>
        /// <exception cref="InternalCompilerInterface.ThrowCodeGenException">Exception indicating that this method invocation should have been rewritten/replaced during source-generation.</exception>
        public IEnumerator<(T1, T2, T3, T4, T5, T6, T7, Entity)> GetEnumerator() => throw InternalCompilerInterface.ThrowCodeGenException();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
