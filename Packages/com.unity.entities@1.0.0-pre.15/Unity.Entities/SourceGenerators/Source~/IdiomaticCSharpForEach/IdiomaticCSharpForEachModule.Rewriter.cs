using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Unity.Entities.SourceGen.Common;
using Unity.Entities.SourceGen.SystemGeneratorCommon;

namespace Unity.Entities.SourceGen.IdiomaticCSharpForEach
{
    public partial class IdiomaticCSharpForEachModule
    {
        class SystemAPIQueryRewriter : SystemRewriter
        {
            readonly IReadOnlyCollection<IdiomaticCSharpForEachDescription> _descriptions;
            readonly Dictionary<SyntaxNode, (IdiomaticCSharpForEachDescription description, int originalLineNumber)> _queryInvocationNodeAndDescriptionPairs;
            readonly Dictionary<SyntaxNode, (HashSet<string> statementSnippets, int originalLineNumber)> _targetAndStatementsToInsertBefore;
            bool _replacedNode;

            public override IEnumerable<SyntaxNode> NodesToTrack => _descriptions.Select(d => d.NodeToReplace);

            public SystemAPIQueryRewriter(IReadOnlyCollection<IdiomaticCSharpForEachDescription> descriptionsInSameSystemType)
            {
                _descriptions = descriptionsInSameSystemType;
                _targetAndStatementsToInsertBefore = new Dictionary<SyntaxNode, (HashSet<string>, int)>();
                _queryInvocationNodeAndDescriptionPairs = new Dictionary<SyntaxNode, (IdiomaticCSharpForEachDescription, int)>();
            }

            // By the time this method is invoked, the system has already been rewritten at least once.
            // In other words, the `systemRootNode` argument passed to this method is the root node of the REWRITTEN system --
            // i.e., a copy of the original system with changes applied.
            public override SyntaxNode VisitTrackedSystem(SyntaxNode systemRootNode, string originalFilePath)
            {
                m_OriginalFilePath = originalFilePath;

                foreach (var description in _descriptions)
                {
                    var rewrittenQueryInvocationNode = systemRootNode.GetCurrentNodes(description.NodeToReplace).FirstOrDefault() ?? description.NodeToReplace;
                    _queryInvocationNodeAndDescriptionPairs.Add(rewrittenQueryInvocationNode, (description, description.NodeToReplace.GetLineNumber()));
                }
                return Visit(systemRootNode);
            }

            public override SyntaxNode Visit(SyntaxNode syntaxNode)
            {
                if (syntaxNode == null)
                    return null;

                var replacedNodeAndChildren = base.Visit(syntaxNode);

                // If the current node is a node we want to replace -- e.g. `SystemAPI.Query<MyComponent>()`
                if (_queryInvocationNodeAndDescriptionPairs.TryGetValue(syntaxNode, out var descKvp))
                {
                    // Replace the current node
                    replacedNodeAndChildren = descKvp.description.GetQueryInvocationNodeReplacement();
                    _replacedNode = true;

                    var targetStatement = syntaxNode.Ancestors().OfType<StatementSyntax>().First();

                    // Update `_targetHashAndStatementsToInsert` -- the key is `targetHash`, the value is a hashset of statements to insert.
                    if (!_targetAndStatementsToInsertBefore.ContainsKey(targetStatement))
                        _targetAndStatementsToInsertBefore[targetStatement] = (new HashSet<string>(), descKvp.originalLineNumber);

                    foreach (var newStatement in descKvp.description.GetStatementsToInsertBefore())
                        _targetAndStatementsToInsertBefore[targetStatement].statementSnippets.Add(newStatement);
                }

                // If we want to insert additional statements
                if (replacedNodeAndChildren is StatementSyntax statementSyntax)
                {
                    if (_targetAndStatementsToInsertBefore.TryGetValue(syntaxNode, out var statementKvp))
                    {
                        var statements = new List<StatementSyntax>(statementKvp.statementSnippets.Select(s => SyntaxFactory.ParseStatement(s).WithHiddenLineTrivia() as StatementSyntax))
                            { statementSyntax.WithHiddenLineTrivia() as StatementSyntax };
                        statements[0] = statements[0].WithLineTrivia(m_OriginalFilePath, statementKvp.originalLineNumber) as StatementSyntax;

                        replacedNodeAndChildren =
                            SyntaxFactory.Block(
                                new SyntaxList<AttributeListSyntax>(),
                                SyntaxFactory.MissingToken(SyntaxKind.OpenBraceToken),
                                new SyntaxList<StatementSyntax>(statements),
                                SyntaxFactory.MissingToken(SyntaxKind.CloseBraceToken));
                    }
                }

                // If we have performed any replacements, we need to update the `RewrittenMemberHashCodeToSyntaxNode` dictionary accordingly
                if (replacedNodeAndChildren is MemberDeclarationSyntax memberDeclarationSyntax && _replacedNode)
                {
                    RecordChangedMember(memberDeclarationSyntax);
                    _replacedNode = false;
                }
                return replacedNodeAndChildren;
            }
        }
    }
}
