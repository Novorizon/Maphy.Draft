using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Unity.Entities.SourceGen.Common;

namespace Unity.Entities.SourceGen.SystemGeneratorCommon
{
    public readonly struct EntityTypeHandleFieldDescription : INonQueryFieldDescription, IEquatable<EntityTypeHandleFieldDescription>
    {
        public const string GeneratedFieldName = "__Unity_Entities_Entity_TypeHandle";
        public FieldDeclarationSyntax FieldDeclaration { get; }
        public string GetFieldAssignment(string systemState) => $@"{GeneratedFieldName} = {systemState}.GetEntityTypeHandle();";
        EntityTypeHandleFieldDescription(FieldDeclarationSyntax fieldDeclarationSyntax) => FieldDeclaration=fieldDeclarationSyntax;
        public static EntityTypeHandleFieldDescription CreateInstance()
            => new EntityTypeHandleFieldDescription((FieldDeclarationSyntax)SyntaxFactory.ParseMemberDeclaration($"Unity.Entities.EntityTypeHandle {GeneratedFieldName};"));
        public bool Equals(EntityTypeHandleFieldDescription other) => true;
        public override bool Equals(object obj) => obj is EntityTypeHandleFieldDescription;
        public override int GetHashCode() => GeneratedFieldName.GetHashCode();
    }
}
