using System;
using System.Runtime.CompilerServices;

namespace Unity.Entities.SourceGen.Aspect
{
    public readonly struct ComponentTypeHandle : IDotsPrimitiveSyntax
    {
        public static readonly string Tag = "Cth";
        string IDotsPrimitiveSyntax.Tag => Tag;

        readonly string m_ComponentTypeName;
        readonly bool m_IsReadOnly;

        public ComponentTypeHandle(string componentTypename, bool isReadOnly)
        {
            m_ComponentTypeName = componentTypename;
            m_IsReadOnly = isReadOnly;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Declare(Printer printer, string fieldName)
        {
            if(m_IsReadOnly)
                printer.PrintLine($"[global::Unity.Collections.ReadOnly]");
            printer.PrintLine($"global::Unity.Entities.ComponentTypeHandle<global::{m_ComponentTypeName}> {fieldName};");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Construct(Printer printer, string fieldName)
        {
            printer.PrintLine($"this.{fieldName} = state.GetComponentTypeHandle<global::{m_ComponentTypeName}>({(m_IsReadOnly? "true" : "isReadOnly")});");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Update(Printer printer, string fieldName)
        {
            printer.PrintLine($"this.{fieldName}.Update(ref state);");
        }
    }
}
