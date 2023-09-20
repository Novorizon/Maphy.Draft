using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Unity.Entities.SourceGen.SystemGeneratorCommon;
using static Unity.Entities.SourceGen.Common.SourceGenHelpers.QueryVerification;

namespace Unity.Entities.SourceGen.IdiomaticCSharpForEach
{
    public partial class IdiomaticCSharpForEachModule : ISystemModule
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
            => _queryCandidates.Select(candidate => (SyntaxNode: candidate.FullInvocationChainSyntaxNode, ContainingType: candidate.ContainingTypeNode));

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
                            case GenericNameSyntax { Identifier: { ValueText: "Query" } } genericNameSyntax:
                            {
                                var candidate = QueryCandidate.From(invocationExpressionSyntax, genericNameSyntax.TypeArgumentList.Arguments);
                                _queryCandidates.Add(candidate);
                                break;
                            }
                        }
                        break;
                    }
                    case GenericNameSyntax { Identifier: { ValueText: "Query" } } genericNameSyntax:
                    {
                        var candidate = QueryCandidate.From(invocationExpressionSyntax, genericNameSyntax.TypeArgumentList.Arguments);
                        _queryCandidates.Add(candidate);
                        break;
                    }
                }
            }
        }

        public bool RegisterChangesInSystem(SystemDescription systemDescription)
        {
            var idiomaticCSharpForEachDescriptions = new List<IdiomaticCSharpForEachDescription>();
            foreach (var queryCandidate in CandidatesGroupedByContainingSystemTypes[systemDescription.SystemTypeSyntax])
            {
                var description = new IdiomaticCSharpForEachDescription(systemDescription, queryCandidate, idiomaticCSharpForEachDescriptions.Count);

                description.Success &=
                    VerifyQueryTypeCorrectness(description.SystemDescription, description.Location, description.AllQueryTypes, invokedMethodName: "WithAll");
                description.Success &=
                    VerifyQueryTypeCorrectness(description.SystemDescription, description.Location, description.NoneQueryTypes, invokedMethodName: "WithNone");
                description.Success &=
                    VerifyQueryTypeCorrectness(description.SystemDescription, description.Location, description.AnyQueryTypes, invokedMethodName: "WithAny");

                description.Success &=
                    VerifyNoMutuallyExclusiveQueries(description.SystemDescription, description.Location, description.NoneQueryTypes, description.AllQueryTypes, "WithNone", "WithAll");
                description.Success &=
                    VerifyNoMutuallyExclusiveQueries(description.SystemDescription, description.Location, description.NoneQueryTypes, description.AnyQueryTypes, "WithNone", "WithAny");
                description.Success &=
                    VerifyNoMutuallyExclusiveQueries(description.SystemDescription, description.Location, description.AnyQueryTypes, description.AllQueryTypes, "WithAny", "WithAll");

                if (!description.Success) // if !description.Success, TypeSymbolsForEntityQueryCreation might not be right, thus causing an exception
                    continue;

                var queriedTypes =
                    description.QueryDatas.Select(data => new Query { TypeSymbol = data.TypeSymbol, Type = SystemGeneratorCommon.QueryType.All, IsReadOnly = data.IsReadOnly }).ToArray();

                description.Success &= VerifyNoMutuallyExclusiveQueries(
                    description.SystemDescription,
                    description.Location,
                    description.NoneQueryTypes,
                    queriedTypes,
                    "WithNone",
                    "main queried Aspect type",
                    compareTypeSymbolsOnly: true);

                description.Success &= VerifyNoMutuallyExclusiveQueries(
                    description.SystemDescription,
                    description.Location,
                    description.AnyQueryTypes,
                    queriedTypes,
                    "WithAny",
                    "main queried Aspect type",
                    compareTypeSymbolsOnly: true);

                if (description.Success)
                {
                    if (description.IsBurstEnabled)
                        RequiresReferenceToBurst = true;
                    idiomaticCSharpForEachDescriptions.Add(description);
                }
            }
            foreach (var description in idiomaticCSharpForEachDescriptions)
            {
                if (description.ContainerType.IsGenerated)
                {
                    systemDescription.NewMiscellaneousMembers.Add(description.ContainerType.Value.StructDeclarationSyntax);

                    description.ContainerOrAspectTypeHandleFieldName =
                        systemDescription.GetOrCreateSourceGeneratedTypeHandleField(description.ContainerType.Value.FullyQualifiedTypeName);
                }
                else
                {
                    // We do not generate container types when querying a single Aspect without `.WithEntityAccess()`
                    description.ContainerOrAspectTypeHandleFieldName =
                        systemDescription.GetOrCreateTypeHandleField(description.QueryDatas.Single().TypeSymbol, isReadOnly: false);

                    if (description.RequiresAspectLookupField)
                    {
                        description.AspectLookupTypeHandleFieldName = systemDescription.GetOrCreateAspectLookup(description.QueryDatas.Single().TypeSymbol, isReadOnly: false);
                    }
                }
                description.SourceGeneratedEntityQueryFieldName =
                    systemDescription.GetOrCreateQueryField(
                        new SingleArchetypeQueryFieldDescription(
                            new Archetype(
                                description.AllQueryTypes
                                    .Concat(description.SharedComponentFilterQueryTypes)
                                    .Concat(description.QueryDatas.Select(queryData =>
                                        new Query
                                        {
                                            IsReadOnly = queryData.IsReadOnly,
                                            TypeSymbol = queryData.IsGeneric
                                                ? queryData.TypeParameterSymbol
                                                : queryData.TypeSymbol,
                                            Type = SystemGeneratorCommon.QueryType.All
                                        }))
                                    .ToArray(),
                                description.AnyQueryTypes,
                                description.NoneQueryTypes,
                                description.GetEntityQueryOptionsArgument()),
                            description.ChangeFilterQueryTypes));
            }

            systemDescription.Rewriters.Add(new SystemAPIQueryRewriter(idiomaticCSharpForEachDescriptions));
            return true;
        }
    }
}
