using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Unity.Entities.SourceGen.Common;

namespace Unity.Entities.SourceGen.SystemGeneratorCommon
{
    public readonly struct TypeHandleFieldDescription : IEquatable<TypeHandleFieldDescription>, INonQueryFieldDescription
    {
        private enum TypeHandleSource
        {
            Aspect,
            Component,
            SharedComponent,
            BufferElement
        }
        readonly bool _isReadOnly;
        readonly ITypeSymbol _typeSymbol;
        readonly TypeHandleSource _source;

        public string GeneratedFieldName { get; }

        public string GetFieldAssignment(string systemStateName)
        {
            return _source switch
            {
                TypeHandleSource.Aspect => $"{GeneratedFieldName} = new {_typeSymbol.ToFullName()}.TypeHandle(ref {systemStateName}, {(_isReadOnly ? "true" : "false")});",
                TypeHandleSource.BufferElement => $"{GeneratedFieldName} = {systemStateName}.GetBufferTypeHandle<{_typeSymbol.ToFullName()}>({(_isReadOnly ? "true" : "false")});",
                TypeHandleSource.Component => $"{GeneratedFieldName} = {systemStateName}.GetComponentTypeHandle<{_typeSymbol.ToFullName()}>({(_isReadOnly ? "true" : "false")});",
                _ => $"{GeneratedFieldName} = {systemStateName}.GetSharedComponentTypeHandle<{_typeSymbol.ToFullName()}>();"
            };
        }

        public FieldDeclarationSyntax FieldDeclaration{ get; }

        public TypeHandleFieldDescription(ITypeSymbol typeSymbol, bool isReadOnly)
        {
            _typeSymbol = typeSymbol;
            _isReadOnly = isReadOnly;

            var typeSymbolFullName = _typeSymbol.ToFullName();

            if (_typeSymbol.IsAspect())
            {
                GeneratedFieldName = $"__{typeSymbolFullName.Replace(".", "_")}_{(_isReadOnly ? "RO" : "RW")}_AspectTypeHandle";
                FieldDeclaration = (FieldDeclarationSyntax)SyntaxFactory.ParseMemberDeclaration($"{typeSymbolFullName}.TypeHandle {GeneratedFieldName};");

                _source = TypeHandleSource.Aspect;
            }
            else if (typeSymbol.InheritsFromInterface("Unity.Entities.IBufferElementData"))
            {
                GeneratedFieldName = $"__{typeSymbolFullName.Replace(".", "_")}_{(_isReadOnly ? "RO" : "RW")}_BufferTypeHandle";
                FieldDeclaration = (FieldDeclarationSyntax)SyntaxFactory.ParseMemberDeclaration($"Unity.Entities.BufferTypeHandle<{typeSymbolFullName}> {GeneratedFieldName};");

                _source = TypeHandleSource.BufferElement;
            }
            else if (typeSymbol.IsSharedComponent())
            {
                GeneratedFieldName = $"__{typeSymbolFullName.Replace(".", "_")}_SharedComponentTypeHandle";
                FieldDeclaration = (FieldDeclarationSyntax)SyntaxFactory.ParseMemberDeclaration($"Unity.Entities.SharedComponentTypeHandle<{typeSymbolFullName}> {GeneratedFieldName};");

                _source = TypeHandleSource.SharedComponent;
            }
            else
            {
                GeneratedFieldName = $"__{typeSymbolFullName.Replace(".", "_")}_{(_isReadOnly ? "RO" : "RW")}_ComponentTypeHandle";
                FieldDeclaration = (FieldDeclarationSyntax)SyntaxFactory.ParseMemberDeclaration($"Unity.Entities.ComponentTypeHandle<{typeSymbolFullName}> {GeneratedFieldName};");

                _source = TypeHandleSource.Component;
            }
        }

        public bool Equals(TypeHandleFieldDescription other) =>
            _isReadOnly == other._isReadOnly && _source == other._source && SymbolEqualityComparer.Default.Equals(_typeSymbol, other._typeSymbol);

        public override bool Equals(object obj) => obj is TypeHandleFieldDescription other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _isReadOnly.GetHashCode();
                hashCode = (hashCode * 397) ^ (_typeSymbol != null ? _typeSymbol.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int)_source;
                return hashCode;
            }
        }
    }
}
