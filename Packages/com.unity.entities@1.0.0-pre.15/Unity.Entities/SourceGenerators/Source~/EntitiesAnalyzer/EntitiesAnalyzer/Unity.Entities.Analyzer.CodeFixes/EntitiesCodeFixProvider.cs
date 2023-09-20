using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;

namespace Unity.Entities.Analyzer
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(EntitiesCodeFixProvider)), Shared]
    public class EntitiesCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(EntitiesAnalyzer.ID_EA0001);
        public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;
        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            foreach (var diagnostic in context.Diagnostics)
            {
                if (root?.FindNode(diagnostic.Location.SourceSpan) is LocalDeclarationStatementSyntax {Declaration:{} variableDeclaration} ) {
                    context.RegisterCodeFix(
                        CodeAction.Create(title: "Use non-readonly reference",
                            createChangedDocument: c => MakeNonReadonlyReference(context.Document, variableDeclaration, c),
                            equivalenceKey: "NonReadonlyReference"),
                        diagnostic);
                }
            }
        }

        static async Task<Document> MakeNonReadonlyReference(Document document, VariableDeclarationSyntax variableDeclaration, CancellationToken cancellationToken)
        {
            var modifiedVarDecl = variableDeclaration;
            if (modifiedVarDecl.Type is RefTypeSyntax overallRefType)
            {
                // fixes snippets with readonly keyword e.g. `ref readonly MyBlob readonlyBlob = ref _blobAssetReference.Value`
                modifiedVarDecl = modifiedVarDecl.WithType(overallRefType.WithReadOnlyKeyword(default).WithTriviaFrom(overallRefType));
            }
            else
            {
                // fixes snippets missing ref keywords e.g. `MyBlob readonlyBlob = _blobAssetReference.Value`
                var originalTypeWithSpace = modifiedVarDecl.Type.WithoutTrivia();
                var type = SyntaxFactory.RefType(originalTypeWithSpace).WithTriviaFrom(modifiedVarDecl.Type);
                type = type.WithRefKeyword(type.RefKeyword.WithTrailingTrivia(SyntaxTriviaList.Create(SyntaxFactory.Space)));
                modifiedVarDecl = modifiedVarDecl.WithType(type);

                var modifiedVariables = modifiedVarDecl.Variables.ToArray();
                for (var index = 0; index < modifiedVariables.Length; index++)
                {
                    var modifiedValue = SyntaxFactory.RefExpression(modifiedVariables[index].Initializer.Value.WithoutTrivia().WithLeadingTrivia(SyntaxTriviaList.Create(SyntaxFactory.Space))).WithTriviaFrom(modifiedVariables[index].Initializer.Value);
                    modifiedVariables[index] = modifiedVariables[index].WithInitializer(modifiedVariables[index].Initializer.WithValue(modifiedValue));
                }
                modifiedVarDecl = modifiedVarDecl.WithVariables(SyntaxFactory.SeparatedList(modifiedVariables));
            }
            var oldRoot = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            var newRoot = oldRoot.ReplaceNode(variableDeclaration, modifiedVarDecl);
            return document.WithSyntaxRoot(newRoot);
        }
    }
}
