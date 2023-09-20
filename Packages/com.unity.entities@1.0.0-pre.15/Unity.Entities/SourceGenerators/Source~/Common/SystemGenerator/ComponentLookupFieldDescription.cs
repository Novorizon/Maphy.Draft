using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Unity.Entities.SourceGen.Common;

namespace Unity.Entities.SourceGen.SystemGeneratorCommon
{
    public readonly struct ComponentLookupFieldDescription : IEquatable<ComponentLookupFieldDescription>, INonQueryFieldDescription
    {
        ITypeSymbol TypeSymbol { get; }
        bool IsReadOnly { get; }
        public string GeneratedFieldName{ get; }

        public FieldDeclarationSyntax FieldDeclaration { get; }
        public string GetFieldAssignment(string systemStateName) =>
            $@"{GeneratedFieldName} = {systemStateName}.GetComponentLookup<{TypeSymbol.ToFullName()}>({(IsReadOnly ? "true" : "false")});";

        public ComponentLookupFieldDescription(ITypeSymbol typeSymbol, bool isReadOnly)
        {
            TypeSymbol = typeSymbol;
            IsReadOnly = isReadOnly;

            GeneratedFieldName = $"__{TypeSymbol.ToFullName().Replace(".", "_")}_{(IsReadOnly ? "RO" : "RW")}_ComponentLookup";
            FieldDeclaration = (FieldDeclarationSyntax)SyntaxFactory.ParseMemberDeclaration($"Unity.Entities.ComponentLookup<{TypeSymbol.ToFullName()}> {GeneratedFieldName};");
        }

        public bool Equals(ComponentLookupFieldDescription other)
        {
            return SymbolEqualityComparer.Default.Equals(TypeSymbol, other.TypeSymbol) && IsReadOnly == other.IsReadOnly;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (obj.GetType() != GetType())
                return false;
            return Equals((ComponentLookupFieldDescription)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((TypeSymbol != null ? TypeSymbol.GetHashCode() : 0) * 397) ^ IsReadOnly.GetHashCode();
            }
        }

        public static bool operator ==(ComponentLookupFieldDescription left, ComponentLookupFieldDescription right) => Equals(left, right);
        public static bool operator !=(ComponentLookupFieldDescription left, ComponentLookupFieldDescription right) => !Equals(left, right);
    }
}
