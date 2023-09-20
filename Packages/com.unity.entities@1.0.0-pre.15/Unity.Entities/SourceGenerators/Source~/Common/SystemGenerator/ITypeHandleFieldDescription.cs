using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Unity.Entities.SourceGen.SystemGeneratorCommon
{
    public interface INonQueryFieldDescription
    {
        FieldDeclarationSyntax FieldDeclaration { get; }
        string GetFieldAssignment(string systemStateName);
    }
}
