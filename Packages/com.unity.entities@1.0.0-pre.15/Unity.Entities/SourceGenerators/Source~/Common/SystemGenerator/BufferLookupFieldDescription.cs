using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Unity.Entities.SourceGen.Common;

namespace Unity.Entities.SourceGen.SystemGeneratorCommon
{
    public readonly struct BufferLookupFieldDescription : IEquatable<BufferLookupFieldDescription>, INonQueryFieldDescription
    {
        ITypeSymbol TypeSymbol { get; }
        bool IsReadOnly { get; }

        public string GeneratedFieldName { get; }

        public string GetFieldAssignment(string systemStateName) =>
            $@"{GeneratedFieldName} = {systemStateName}.GetBufferLookup<{TypeSymbol.ToFullName()}>({(IsReadOnly ? "true" : "false")});";
        public FieldDeclarationSyntax FieldDeclaration { get; }

        public BufferLookupFieldDescription(ITypeSymbol typeSymbol, bool isReadOnly)
        {
            TypeSymbol = typeSymbol;
            IsReadOnly = isReadOnly;

            GeneratedFieldName = $"__{TypeSymbol.ToFullName().Replace(".", "_")}_{(IsReadOnly ? "RO" : "RW")}_BufferLookup";
            FieldDeclaration = (FieldDeclarationSyntax)SyntaxFactory.ParseMemberDeclaration($"Unity.Entities.BufferLookup<{TypeSymbol.ToFullName()}> {GeneratedFieldName};");
        }

        public bool Equals(BufferLookupFieldDescription other) =>
            SymbolEqualityComparer.Default.Equals(TypeSymbol, other.TypeSymbol) && IsReadOnly == other.IsReadOnly;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (obj.GetType() != GetType())
                return false;
            return Equals((BufferLookupFieldDescription)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((TypeSymbol != null ? TypeSymbol.GetHashCode() : 0) * 397) ^ IsReadOnly.GetHashCode();
            }
        }

        public static bool operator ==(BufferLookupFieldDescription left, BufferLookupFieldDescription right) => Equals(left, right);
        public static bool operator !=(BufferLookupFieldDescription left, BufferLookupFieldDescription right) => !Equals(left, right);
    }
}
