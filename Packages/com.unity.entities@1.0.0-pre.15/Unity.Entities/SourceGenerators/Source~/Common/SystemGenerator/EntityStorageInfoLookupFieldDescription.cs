using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Unity.Entities.SourceGen.SystemGeneratorCommon
{
    public struct EntityStorageInfoLookupFieldDescription : IEquatable<EntityStorageInfoLookupFieldDescription>, INonQueryFieldDescription
    {
        public string GeneratedFieldName{ get; private set; }
        public FieldDeclarationSyntax FieldDeclaration { get; private set; }

        public string GetFieldAssignment(string systemStateName) =>
            $@"{GeneratedFieldName} = {systemStateName}.GetEntityStorageInfoLookup();";

        public void Init()
        {
            GeneratedFieldName = "__EntityStorageInfoLookup";
            FieldDeclaration = (FieldDeclarationSyntax)SyntaxFactory.ParseMemberDeclaration($"Unity.Entities.EntityStorageInfoLookup {GeneratedFieldName};");
        }

        public bool Equals(EntityStorageInfoLookupFieldDescription other) => true;

        public override bool Equals(object obj) => !ReferenceEquals(null, obj);

        public override int GetHashCode() => 0;
        public static bool operator ==(EntityStorageInfoLookupFieldDescription left, EntityStorageInfoLookupFieldDescription right) => Equals(left, right);
        public static bool operator !=(EntityStorageInfoLookupFieldDescription left, EntityStorageInfoLookupFieldDescription right) => !Equals(left, right);
    }
}
