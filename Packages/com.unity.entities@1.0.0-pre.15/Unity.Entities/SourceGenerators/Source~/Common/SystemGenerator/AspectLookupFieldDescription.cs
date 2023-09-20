using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Unity.Entities.SourceGen.Common;

namespace Unity.Entities.SourceGen.SystemGeneratorCommon
{
    public readonly struct AspectLookupFieldDescription : IEquatable<AspectLookupFieldDescription>, INonQueryFieldDescription
    {
        bool IsReadOnly { get; }
        ITypeSymbol TypeSymbol { get; }

        public string GeneratedFieldName { get; }
        public FieldDeclarationSyntax FieldDeclaration { get; }

        public string GetFieldAssignment(string systemStateName) =>
            $@"{GeneratedFieldName} = new {TypeSymbol.ToFullName()}.Lookup(ref {systemStateName}, {(IsReadOnly ? "true" : "false")});";

        public AspectLookupFieldDescription(ITypeSymbol typeSymbol, bool isReadOnly)
        {
            TypeSymbol = typeSymbol;
            IsReadOnly = isReadOnly;

            GeneratedFieldName = $"__{TypeSymbol.ToFullName().Replace(".", "_")}_{(IsReadOnly ? "RO" : "RW")}_AspectLookup";
            FieldDeclaration = (FieldDeclarationSyntax)SyntaxFactory.ParseMemberDeclaration($"{TypeSymbol.ToFullName()}.Lookup {GeneratedFieldName};");
        }

        public bool Equals(AspectLookupFieldDescription other) =>
            IsReadOnly == other.IsReadOnly && SymbolEqualityComparer.Default.Equals(TypeSymbol, other.TypeSymbol);

        public override bool Equals(object obj) => obj is AspectLookupFieldDescription other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                return (IsReadOnly.GetHashCode() * 397) ^ (TypeSymbol != null ? TypeSymbol.GetHashCode() : 0);
            }
        }

        public static bool operator ==(AspectLookupFieldDescription left, AspectLookupFieldDescription right) => left.Equals(right);
        public static bool operator !=(AspectLookupFieldDescription left, AspectLookupFieldDescription right) => !left.Equals(right);
    }
}
