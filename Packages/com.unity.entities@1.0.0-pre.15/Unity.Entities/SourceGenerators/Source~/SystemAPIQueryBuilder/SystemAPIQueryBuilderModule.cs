using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Unity.Entities.SourceGen.SystemGeneratorCommon;
using static Unity.Entities.SourceGen.Common.SourceGenHelpers.QueryVerification;

namespace Unity.Entities.SourceGen.SystemAPIQueryBuilder
{
    public class SystemAPIQueryBuilderModule : ISystemModule
    {
        private readonly List<QueryCandidate> _queryCandidates = new List<QueryCandidate>();
        private Dictionary<TypeDeclarationSyntax, QueryCandidate[]> _candidatesGroupedByContainingSystemTypes;

        private Dictionary<TypeDeclarationSyntax, QueryCandidate[]> CandidatesGroupedByContainingSystemTypes
        {
            get
            {
                if (_candidatesGroupedByContainingSystemTypes == null)
                {
                    _candidatesGroupedByContainingSystemTypes =
                        _queryCandidates
                            .GroupBy(c => c.ContainingTypeNode)
                            .ToDictionary(group => group.Key, group => group.ToArray());
                }
                return _candidatesGroupedByContainingSystemTypes;
            }
        }

        public IEnumerable<(SyntaxNode SyntaxNode, TypeDeclarationSyntax SystemType)> Candidates
            => _queryCandidates.Select(candidate => (SyntaxNode: candidate.BuildNode, ContainingType: candidate.ContainingTypeNode));

        public bool RequiresReferenceToBurst { get; private set; }

        public void OnReceiveSyntaxNode(SyntaxNode node)
        {
            if (node is InvocationExpressionSyntax invocationExpressionSyntax)
            {
                switch (invocationExpressionSyntax.Expression)
                {
                    case MemberAccessExpressionSyntax memberAccessExpressionSyntax:
                    {
                        switch (memberAccessExpressionSyntax.Name)
                        {
                            case IdentifierNameSyntax { Identifier: { ValueText: "QueryBuilder" } } identifierNameSyntax:
                            {
                                var (success, result) = QueryCandidate.TryCreateFrom(invocationExpressionSyntax);
                                if (success)
                                    _queryCandidates.Add(result);
                                break;
                            }
                        }
                        break;
                    }
                    case IdentifierNameSyntax { Identifier: { ValueText: "QueryBuilder" } } identifierNameSyntax:
                    {
                        var (success, result) = QueryCandidate.TryCreateFrom(invocationExpressionSyntax);
                        if (success)
                            _queryCandidates.Add(result);
                        break;
                    }
                }
            }
        }

        public bool RegisterChangesInSystem(SystemDescription systemDescription)
        {
            var systemApiQueryBuilderDescriptions = new List<SystemAPIQueryBuilderDescription>();
            foreach (var queryCandidate in CandidatesGroupedByContainingSystemTypes[systemDescription.SystemTypeSyntax])
            {
                var description = new SystemAPIQueryBuilderDescription(systemDescription, queryCandidate);

                foreach (var archetypeInfo in description.Archetypes.Zip(description.QueryFinalizingLocations, (a, l) => (Archetype: a, Location: l)))
                {
                    description.Success &=
                        VerifyQueryTypeCorrectness(
                            description.SystemDescription,
                            archetypeInfo.Location,
                            archetypeInfo.Archetype.All,
                            invokedMethodName: "WithAll");

                    description.Success &=
                        VerifyQueryTypeCorrectness(
                            description.SystemDescription,
                            archetypeInfo.Location,
                            archetypeInfo.Archetype.None,
                            invokedMethodName: "WithNone");

                    description.Success &=
                        VerifyQueryTypeCorrectness(
                            description.SystemDescription,
                            archetypeInfo.Location,
                            archetypeInfo.Archetype.Any,
                            invokedMethodName: "WithAny");

                    description.Success &=
                        VerifyNoMutuallyExclusiveQueries(
                            description.SystemDescription,
                            archetypeInfo.Location,
                            archetypeInfo.Archetype.None,
                            archetypeInfo.Archetype.All,
                            "WithNone",
                            "WithAll");

                    description.Success &=
                        VerifyNoMutuallyExclusiveQueries(
                            description.SystemDescription,
                            archetypeInfo.Location,
                            archetypeInfo.Archetype.None,
                            archetypeInfo.Archetype.Any,
                            "WithNone",
                            "WithAny");

                    description.Success &=
                        VerifyNoMutuallyExclusiveQueries(
                            description.SystemDescription,
                            archetypeInfo.Location,
                            archetypeInfo.Archetype.Any,
                            archetypeInfo.Archetype.All,
                            "WithAny",
                            "WithAll");
                }

                if (!description.Success)
                    continue;

                if (description.IsBurstEnabled)
                    RequiresReferenceToBurst = true;

                systemApiQueryBuilderDescriptions.Add(description);
            }
            foreach (var description in systemApiQueryBuilderDescriptions)
            {
                description.GeneratedEntityQueryFieldName
                    = systemDescription.GetOrCreateQueryField(new MultipleArchetypeQueryFieldDescription(description.Archetypes.ToArray(), description.GetQueryBuilderBodyBeforeBuild()));
            }

            systemDescription.Rewriters.Add(new SystemAPIQueryBuilderRewriter(systemApiQueryBuilderDescriptions));
            return true;
        }
    }
}
