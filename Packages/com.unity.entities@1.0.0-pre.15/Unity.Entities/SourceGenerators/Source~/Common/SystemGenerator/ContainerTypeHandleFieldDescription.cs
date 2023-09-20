using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Unity.Entities.SourceGen.SystemGeneratorCommon
{
    public readonly struct ContainerTypeHandleFieldDescription : IEquatable<ContainerTypeHandleFieldDescription>, INonQueryFieldDescription
    {
        string ContainerTypeFullName { get; }

        public FieldDeclarationSyntax FieldDeclaration { get; }
        public string GetFieldAssignment(string systemStateName)
            => $@"{GeneratedFieldName} = new {ContainerTypeFullName}.TypeHandle(ref {systemStateName}, isReadOnly: false);";

        public string GeneratedFieldName { get; }

        public ContainerTypeHandleFieldDescription(string containerTypeFullName)
        {
            ContainerTypeFullName = containerTypeFullName;
            GeneratedFieldName = $"__{containerTypeFullName.Replace(".", "_")}_RW_TypeHandle";
            FieldDeclaration = (FieldDeclarationSyntax)SyntaxFactory.ParseMemberDeclaration($"{containerTypeFullName}.TypeHandle {GeneratedFieldName};");
        }

        public bool Equals(ContainerTypeHandleFieldDescription other) => ContainerTypeFullName == other.ContainerTypeFullName;
        public override bool Equals(object obj) => obj is ContainerTypeHandleFieldDescription other && Equals(other);
        public override int GetHashCode() => ContainerTypeFullName != null ? ContainerTypeFullName.GetHashCode() : 0;
        public static bool operator ==(ContainerTypeHandleFieldDescription left, ContainerTypeHandleFieldDescription right) => left.Equals(right);
        public static bool operator !=(ContainerTypeHandleFieldDescription left, ContainerTypeHandleFieldDescription right) => !left.Equals(right);
    }
}
