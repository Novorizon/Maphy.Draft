using System;
using System.Runtime.CompilerServices;

namespace Unity.Entities.SourceGen.Aspect
{
    /// <summary>
    /// A DependentDotsPrimitive represents a DotsPrimitive such as 'ComponentLookup' or 'ComponentTypeHandle'
    /// on which any 'AspectField' may depend on to perform their functionality.
    /// The type of dependency is handled through the IDotsPrimitiveDependency
    /// </summary>
    /// <typeparam name="TPrimitive"></typeparam>
    /// <typeparam name="TDependency"></typeparam>
    public struct DependentDotsPrimitive<TPrimitive, TDependency>
        where TPrimitive : IDotsPrimitiveSyntax
        where TDependency : IDotsPrimitiveDependency
    {
        public TDependency Dependencies;

        public TPrimitive Primitive;

        public string PrimitiveFieldName => Dependencies.DeclaringField.InternalFieldName + Primitive.Tag;

        public DependentDotsPrimitive(TPrimitive primitive)
        {
            Dependencies = default;
            Primitive = primitive;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Declare(Printer printer) => Primitive.Declare(printer, PrimitiveFieldName);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Construct(Printer printer) => Primitive.Construct(printer, PrimitiveFieldName);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Update(Printer printer) => Primitive.Update(printer, PrimitiveFieldName);
    }
}
