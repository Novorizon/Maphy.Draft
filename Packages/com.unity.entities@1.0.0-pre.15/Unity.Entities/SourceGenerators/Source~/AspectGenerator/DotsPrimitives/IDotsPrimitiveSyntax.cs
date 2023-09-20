using System;

namespace Unity.Entities.SourceGen.Aspect
{
    /// <summary>
    /// A dots primitive is a type (such as 'ComponentTypeHandle'
    /// or 'ComponentLookup') from the entity package that can be
    /// used to declare, construct and update a field of that dots primitive type.
    /// That field can then be used to access dots data.
    /// </summary>
    /// <remarks>This interface is only used as a generic type constrain.</remarks>
    public interface IDotsPrimitiveSyntax
    {
        /// <summary>
        /// Declare a field of the primitive type.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        void Declare(Printer printer, string fieldName);

        /// <summary>
        /// Construct the field from a constructor.
        /// Available symbols:
        ///     "state" : the current SystemState
        ///     "isReadOnly" : if the aspect instance is read-only
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        void Construct(Printer printer, string fieldName);

        /// <summary>
        /// Call update on the primitive field
        /// Available symbols:
        ///     "state" : the current SystemState
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        void Update(Printer printer, string fieldName);

        /// <summary>
        /// A dots primitive tag is used to build unique names for each primitive type
        /// </summary>
        string Tag { get; }
    }
}
