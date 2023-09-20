using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;
using Unity.Entities.SourceGen.Common;
using Unity.Entities.SourceGen.SystemGeneratorCommon;
using static Unity.Entities.SourceGen.Common.SourceGenHelpers;

namespace Unity.Entities.SourceGen.IdiomaticCSharpForEach
{
    public class IdiomaticCSharpForEachDescription
    {
        internal struct QueryData
        {
            public string TypeSymbolFullName { get; set; }
            public TypeSyntax TypeSyntaxNode { get; set; }
            public ITypeSymbol TypeSymbol { get; set; }
            public ITypeSymbol TypeParameterSymbol { get; set; }
            public QueryType QueryType { get; set; }
            public bool IsGeneric => TypeParameterSymbol != null;
            public bool IsReadOnly => QueryType == QueryType.RefRO ||
                                      QueryType == QueryType.EnabledRefRO ||
                                      QueryType == QueryType.ValueTypeComponent ||
                                      QueryType == QueryType.UnmanagedSharedComponent ||
                                      QueryType == QueryType.ManagedSharedComponent;
        }

        internal (bool IsGenerated, ContainerType Value) ContainerType { get; }
        internal string AspectLookupTypeHandleFieldName { get; set; }
        internal IReadOnlyCollection<QueryData> QueryDatas { get; }
        internal bool IsBurstEnabled { get; }
        internal Location Location { get; }
        internal SystemDescription SystemDescription { get; }
        internal bool RequiresAspectLookupField { get; }

        internal IReadOnlyCollection<Query> NoneQueryTypes => _noneQueryTypes;
        internal IReadOnlyCollection<Query> AnyQueryTypes => _anyQueryTypes;
        internal IReadOnlyCollection<Query> AllQueryTypes => _allQueryTypes;
        internal IReadOnlyCollection<Query> ChangeFilterQueryTypes => _changeFilterQueryTypes;
        internal IReadOnlyCollection<Query> SharedComponentFilterQueryTypes => _sharedComponentFilterQueryTypes;
        internal SyntaxNode NodeToReplace => _queryCandidate.FullInvocationChainSyntaxNode;

        public bool Success { get; internal set; } = true;
        public string ContainerOrAspectTypeHandleFieldName { get; set; }
        public string SourceGeneratedEntityQueryFieldName { get; set; }

        readonly QueryCandidate _queryCandidate;
        readonly string _systemStateName;
        readonly List<Query> _noneQueryTypes = new List<Query>();
        readonly List<Query> _anyQueryTypes = new List<Query>();
        readonly List<Query> _allQueryTypes = new List<Query>();
        readonly List<Query> _changeFilterQueryTypes = new List<Query>();
        readonly List<Query> _sharedComponentFilterQueryTypes = new List<Query>();
        readonly List<ArgumentSyntax> _sharedComponentFilterArguments = new List<ArgumentSyntax>();
        readonly List<ArgumentSyntax> _entityQueryOptionsArguments = new List<ArgumentSyntax>();
        readonly bool _entityAccessRequired;
        readonly string _typeContainingTargetEnumerator_FullyQualifiedName;

        internal EntityQueryOptions GetEntityQueryOptionsArgument()
        {
            if (!_entityQueryOptionsArguments.Any())
                return EntityQueryOptions.Default;

            var options = EntityQueryOptions.Default;
            var argumentExpression = _entityQueryOptionsArguments.First().Expression;

            while (argumentExpression is BinaryExpressionSyntax binaryExpressionSyntax)
            {
                if (TryParseQualifiedEnumValue(binaryExpressionSyntax.Right.ToString(), out EntityQueryOptions optionArg))
                    options |= optionArg;

                argumentExpression = binaryExpressionSyntax.Left;
            }

            if (TryParseQualifiedEnumValue(argumentExpression.ToString(), out EntityQueryOptions option))
                options |= option;

            return options;
        }


        public IEnumerable<string> GetStatementsToInsertBefore()
        {
            yield return $"{ContainerOrAspectTypeHandleFieldName}.Update(ref {_systemStateName});";

            if (RequiresAspectLookupField)
                yield return $"{AspectLookupTypeHandleFieldName}.Update(ref {_systemStateName});";

            yield return $"{_typeContainingTargetEnumerator_FullyQualifiedName}.CompleteDependencyBeforeRW(ref {_systemStateName});";

            foreach (var arg in _sharedComponentFilterArguments)
                yield return $"{SourceGeneratedEntityQueryFieldName}.SetSharedComponentFilter({arg});";
        }

        public InvocationExpressionSyntax GetQueryInvocationNodeReplacement()
        {
            var memberAccessExpressionSyntax =
                SyntaxFactory.MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    SyntaxFactory.IdentifierName(_typeContainingTargetEnumerator_FullyQualifiedName),
                    SyntaxFactory.IdentifierName("Query"));

            return SyntaxFactory.InvocationExpression(memberAccessExpressionSyntax, SyntaxFactory.ArgumentList(GetArgumentsForStaticQueryMethod()));

            SeparatedSyntaxList<ArgumentSyntax> GetArgumentsForStaticQueryMethod()
            {
                var entityQueryArg = SyntaxFactory.Argument(SyntaxFactory.IdentifierName(SourceGeneratedEntityQueryFieldName));
                var containerOrAspectTypeHandleArgument = SyntaxFactory.Argument(SyntaxFactory.IdentifierName(ContainerOrAspectTypeHandleFieldName));
                return SyntaxFactory.SeparatedList(new[] { entityQueryArg, containerOrAspectTypeHandleArgument });
            }
        }
        public IdiomaticCSharpForEachDescription(SystemDescription systemDescription, QueryCandidate queryCandidate, int numForEachsPreviouslySeenInSystem)
        {
            if (systemDescription.SemanticModel.GetOperation(queryCandidate.FullInvocationChainSyntaxNode) is IInvocationOperation invocationOperation
                && IsSystemAPIQueryInvocation(invocationOperation))
            {
                _queryCandidate = queryCandidate;

                SystemDescription = systemDescription;
                Location = queryCandidate.FullInvocationChainSyntaxNode.GetLocation();
                QueryDatas = GetQueryDatas().ToArray();

                var containingMethod = queryCandidate.FullInvocationChainSyntaxNode.AncestorOfKindOrDefault<MethodDeclarationSyntax>();
                if (containingMethod != null)
                {
                    var methodSymbol = SystemDescription.SemanticModel.GetDeclaredSymbol(containingMethod);

                    foreach (var node in queryCandidate.MethodInvocationNodes)
                    {
                        switch (node.Expression)
                        {
                            case MemberAccessExpressionSyntax { Name: GenericNameSyntax genericNameSyntax }:
                            {
                                switch (genericNameSyntax.Identifier.ValueText)
                                {
                                    case "WithAll":
                                        _allQueryTypes.AddRange(
                                            genericNameSyntax.TypeArgumentList.Arguments.Select(typeArg =>
                                                new Query
                                                {
                                                    TypeSymbol = (ITypeSymbol)systemDescription.SemanticModel.GetSymbolInfo(typeArg).Symbol,
                                                    Type = SystemGeneratorCommon.QueryType.All,
                                                    IsReadOnly = true
                                                }));
                                        break;
                                    case "WithAny":
                                        _anyQueryTypes.AddRange(
                                            genericNameSyntax.TypeArgumentList.Arguments.Select(typeArg =>
                                                new Query
                                                {
                                                    TypeSymbol = (ITypeSymbol)systemDescription.SemanticModel.GetSymbolInfo(typeArg).Symbol,
                                                    Type = SystemGeneratorCommon.QueryType.Any,
                                                    IsReadOnly = true
                                                }));
                                        break;
                                    case "WithNone":
                                        _noneQueryTypes.AddRange(
                                            genericNameSyntax.TypeArgumentList.Arguments.Select(typeArg =>
                                                new Query
                                                {
                                                    TypeSymbol = (ITypeSymbol)systemDescription.SemanticModel.GetSymbolInfo(typeArg).Symbol,
                                                    Type = SystemGeneratorCommon.QueryType.None,
                                                    IsReadOnly = true
                                                }));
                                        break;
                                    case "WithChangeFilter":
                                        _changeFilterQueryTypes.AddRange(
                                            genericNameSyntax.TypeArgumentList.Arguments.Select(typeArg =>
                                                new Query
                                                {
                                                    TypeSymbol = (ITypeSymbol)systemDescription.SemanticModel.GetSymbolInfo(typeArg).Symbol,
                                                    Type = SystemGeneratorCommon.QueryType.ChangeFilter,
                                                    IsReadOnly = true
                                                }));
                                        break;
                                    case "WithSharedComponentFilter":
                                        _sharedComponentFilterQueryTypes.AddRange(
                                            genericNameSyntax.TypeArgumentList.Arguments.Select(typeArg =>
                                                new Query
                                                {
                                                    TypeSymbol = (ITypeSymbol)systemDescription.SemanticModel.GetSymbolInfo(typeArg).Symbol,
                                                    Type = SystemGeneratorCommon.QueryType.All,
                                                    IsReadOnly = true
                                                }));
                                        _sharedComponentFilterArguments.AddRange(node.ArgumentList.Arguments);
                                        break;
                                }
                                break;
                            }
                            case MemberAccessExpressionSyntax { Name: IdentifierNameSyntax identifierNameSyntax }:
                            {
                                switch (identifierNameSyntax.Identifier.ValueText)
                                {
                                    case "WithSharedComponentFilter":
                                        _sharedComponentFilterQueryTypes.AddRange(
                                            node.ArgumentList.Arguments.Select(arg =>
                                                new Query
                                                {
                                                    TypeSymbol = systemDescription.SemanticModel.GetTypeInfo(arg.Expression).Type,
                                                    Type = SystemGeneratorCommon.QueryType.All,
                                                    IsReadOnly = true
                                                }));
                                        _sharedComponentFilterArguments.AddRange(node.ArgumentList.Arguments);
                                        break;
                                    case "WithOptions":
                                        _entityQueryOptionsArguments.Add(node.ArgumentList.Arguments.Single());
                                        break;
                                    case "WithEntityAccess":
                                        _entityAccessRequired = true;
                                        break;
                                }
                                break;
                            }
                        }
                    }
                    if (!_queryCandidate.IsContainedInForEachStatement)
                    {
                        IdiomaticCSharpForEachCompilerMessages.SGFE001(SystemDescription, Location);
                        Success = false;
                        return;
                    }

                    if (_changeFilterQueryTypes.Count > 2)
                        IdiomaticCSharpForEachCompilerMessages.SGFE003(SystemDescription, _changeFilterQueryTypes.Count, Location);
                    if (_sharedComponentFilterQueryTypes.Count > 2)
                        IdiomaticCSharpForEachCompilerMessages.SGFE007(SystemDescription, _sharedComponentFilterQueryTypes.Count, Location);
                    if (_entityQueryOptionsArguments.Count > 1)
                        IdiomaticCSharpForEachCompilerMessages.SGFE008(SystemDescription, _entityQueryOptionsArguments.Count, Location);

                    if (SystemDescription.TryGetSystemStateParameterName(_queryCandidate, out var systemStateExpression))
                        _systemStateName = systemStateExpression.ToFullString();
                    else
                    {
                        Success = false;
                        return;
                    }

                    var mustGenerateContainerType = MustGenerateContainerType(QueryDatas, _entityAccessRequired);

                    bool useAspect = !mustGenerateContainerType;
                    // Aspects always have nested `Lookup`s, which must be updated whenever there is a system update
                    RequiresAspectLookupField = useAspect;

                    IsBurstEnabled = methodSymbol.HasAttribute("Unity.Burst.BurstCompileAttribute");

                    if (mustGenerateContainerType)
                    {
                        var containerType =
                            GenerateContainerType(QueryDatas, queryCandidate, $"Container_{systemDescription.UniqueId}_{numForEachsPreviouslySeenInSystem}");

                        ContainerType = (true, containerType);
                        SourceGeneratedEntityQueryFieldName = $"{ContainerType.Value.TypeName}_Query";
                        _typeContainingTargetEnumerator_FullyQualifiedName = containerType.FullyQualifiedTypeName;
                    }
                    else
                        _typeContainingTargetEnumerator_FullyQualifiedName = QueryDatas.Single().TypeSymbolFullName; // Use `Aspect` enumerator
                }
                else
                {
                    var propertyDeclarationSyntax = queryCandidate.FullInvocationChainSyntaxNode.AncestorOfKind<PropertyDeclarationSyntax>();
                    var propertySymbol = ModelExtensions.GetDeclaredSymbol(systemDescription.SemanticModel, propertyDeclarationSyntax);

                    IdiomaticCSharpForEachCompilerMessages.SGFE002(
                        systemDescription,
                        systemDescription.SystemTypeSymbol.ToFullName(),
                        propertySymbol.OriginalDefinition.ToString(),
                        queryCandidate.ContainingTypeNode.GetLocation());
                    Success = false;
                }
            }
            else
                Success = false;

            static bool IsSystemAPIQueryInvocation(IInvocationOperation operation)
            {
                var constructedFrom = operation.TargetMethod.ConstructedFrom.ToString();
                if (constructedFrom.StartsWith("Unity.Entities.QueryEnumerable<"))
                    return true;
                if (constructedFrom.StartsWith("Unity.Entities.QueryEnumerableWithEntity<"))
                    return true;
                return constructedFrom.StartsWith("Unity.Entities.SystemAPI.Query<");
            }

            // Returns true if we are querying only a single aspect without `.WithEntityAccess()` --
            // we don't need a container type in that case; we can just use the existing aspect type along with
            // its generated enumerators, nested `ResolvedChunk`, etc.
            static bool MustGenerateContainerType(IReadOnlyCollection<QueryData> queryDatas, bool entityAccessRequired)
            {
                if (queryDatas.Count > 1)
                    return true;
                if (entityAccessRequired)
                    return true;
                return queryDatas.Single().QueryType != QueryType.Aspect;
            }
        }

        private IEnumerable<QueryData> GetQueryDatas()
        {
            foreach (var typeSyntax in _queryCandidate.QueryTypeNodes)
            {
                var typeSymbol = SystemDescription.SemanticModel.GetTypeInfo(typeSyntax).Type;
                var typeParameterSymbol = default(ITypeSymbol);

                var genericNameCandidate = typeSyntax;
                if (typeSyntax is QualifiedNameSyntax qualifiedNameSyntax) // This is the case when people type out their syntax Query<MyNameSpace.MyThing>
                    genericNameCandidate = qualifiedNameSyntax.Right;
                if (genericNameCandidate is GenericNameSyntax genericNameSyntax)
                    typeParameterSymbol = SystemDescription.SemanticModel.GetTypeInfo(genericNameSyntax.TypeArgumentList.Arguments.Single()).Type;

                var queryType = GetIdiomaticCSharpForEachQueryType(typeSymbol);

                if (queryType == QueryType.ValueTypeComponent)
                    IdiomaticCSharpForEachCompilerMessages.SGFI009(SystemDescription, typeSymbol.ToFullName(), Location);

                yield return
                    new QueryData
                    {
                        TypeSyntaxNode = typeSyntax,
                        TypeParameterSymbol = typeParameterSymbol,
                        TypeSymbol = typeSymbol,
                        TypeSymbolFullName = typeSymbol.ToFullName(),
                        QueryType = queryType
                    };
            }

            QueryType GetIdiomaticCSharpForEachQueryType(ITypeSymbol typeSymbol)
            {
                if (typeSymbol.IsAspect())
                    return QueryType.Aspect;

                if (typeSymbol.IsSharedComponent())
                    return typeSymbol.IsUnmanagedType ? QueryType.UnmanagedSharedComponent : QueryType.ManagedSharedComponent;

                if (typeSymbol.IsComponent())
                {
                    if (typeSymbol.InheritsFromType("System.ValueType"))
                        return
                            !typeSymbol.GetMembers().OfType<IFieldSymbol>().Any()
                                ? QueryType.TagComponent
                                : QueryType.ValueTypeComponent;
                    return QueryType.ManagedComponent;
                }

                return typeSymbol.Name switch
                {
                    "DynamicBuffer" => QueryType.DynamicBuffer,
                    "RefRW" when ((INamedTypeSymbol)typeSymbol).TypeArguments[0].GetMembers().OfType<IFieldSymbol>().Any() => QueryType.RefRW,
                    "RefRO" when ((INamedTypeSymbol)typeSymbol).TypeArguments[0].GetMembers().OfType<IFieldSymbol>().Any() => QueryType.RefRO,
                    "RefRW" => QueryType.TagComponent,
                    "RefRO" => QueryType.TagComponent,
                    "EnabledRefRW" => QueryType.EnabledRefRW,
                    "EnabledRefRO" => QueryType.EnabledRefRO,
                    "UnityEngineComponent" => QueryType.UnityEngineComponent, // These also act as ManagedComponents (this is Run only, no schedule, but IFE is already restricted to that!), just needs to use .Value instead
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        ContainerType GenerateContainerType(IEnumerable<QueryData> queryDatas, QueryCandidate queryCandidate, string containerTypeName)
        {
            var containerType = new ContainerType
            {
                PerformsCollectionChecks = SystemDescription.PreprocessorSymbolNames.Contains("ENABLE_UNITY_COLLECTIONS_CHECKS"),
                AllowsDebugging = SystemDescription.PreprocessorSymbolNames.Contains("UNITY_DOTS_DEBUG"),
                MustReturnEntityDuringIteration = _entityAccessRequired,
                TypeName = containerTypeName,
                RequiresAspectLookupField = RequiresAspectLookupField,
                FullyQualifiedTypeName =
                    queryCandidate
                        .FullInvocationChainSyntaxNode
                        .GetContainingTypesAndNamespacesFromMostToLeastNested()
                        .Select(GetIdentifier)
                        .Reverse()
                        .Append(containerTypeName)
                        .SeparateByDot()
            };

            switch (queryCandidate.ContainingStatementNode)
            {
                case ForEachVariableStatementSyntax forEachVariableStatementSyntax:
                {
                    // I.e. foreach (var (int i, int j) in ...)
                    var parenthesizedVariableDesignationSyntax =
                        forEachVariableStatementSyntax.DescendantNodes().OfType<ParenthesizedVariableDesignationSyntax>().FirstOrDefault();

                    string[] userDefinedVariableNames;

                    if (parenthesizedVariableDesignationSyntax != null)
                    {
                        userDefinedVariableNames =
                            parenthesizedVariableDesignationSyntax.ChildNodes().OfType<VariableDesignationSyntax>().Select((syntax, index) =>
                            {
                                return syntax switch
                                {
                                    SingleVariableDesignationSyntax designationSyntax => designationSyntax.Identifier.ValueText,
                                    _ => $"item{index + 1}"
                                };
                            }).ToArray();
                    }
                    else
                    {
                        // I.e. foreach ((var i, var j) in ...)
                        var tupleExpressionSyntax = forEachVariableStatementSyntax.DescendantNodes().OfType<TupleExpressionSyntax>().First();
                        userDefinedVariableNames = tupleExpressionSyntax.ChildNodes().OfType<ArgumentSyntax>().Select((arg, index) =>
                        {
                            var declarationExpressionSyntax = (DeclarationExpressionSyntax)arg.Expression;
                            return declarationExpressionSyntax.Designation switch
                            {
                                SingleVariableDesignationSyntax designationSyntax => designationSyntax.Identifier.ValueText,
                                _ => $"item{index + 1}"
                            };
                        }).ToArray();
                    }

                    containerType.ReturnedTupleElementsDuringEnumeration =
                        userDefinedVariableNames
                            .Zip(queryDatas, (userDefinedVariableName, queryData) => (userDefinedVariableName: userDefinedVariableName, queryData))
                            .Select((result, index) =>
                                new ReturnedTupleElementDuringEnumeration(
                                    typeSymbolFullName: result.queryData.TypeSymbolFullName,
                                    typeArgumentFullName: result.queryData.TypeParameterSymbol is { } symbol ? symbol.ToFullName() : string.Empty,
                                    preferredElementName: result.userDefinedVariableName,
                                    type: result.queryData.QueryType))
                            .ToArray();
                    break;
                }
                case ForEachStatementSyntax forEachStatementSyntax:
                {
                    // I.e. foreach ((MyAspect myAspect, MyComponent myComponent) result in ...)
                    if (forEachStatementSyntax.Type is TupleTypeSyntax tupleTypeSyntax)
                    {
                        containerType.ReturnedTupleElementsDuringEnumeration =
                            queryDatas.Zip(tupleTypeSyntax.Elements, (queryData, tupleElement) => (queryData, tupleElement))
                                .Select((result, index) =>
                                {
                                    bool hasIdentifier = result.tupleElement.Identifier.Value != null;

                                    return
                                        new ReturnedTupleElementDuringEnumeration(
                                            result.queryData.TypeSymbolFullName,
                                            typeArgumentFullName: result.queryData.TypeParameterSymbol != null
                                                ? result.queryData.TypeParameterSymbol.ToFullName()
                                                : string.Empty,
                                            preferredElementName: hasIdentifier
                                                ? result.tupleElement.Identifier.ValueText
                                                : $"item{index + 1}", result.queryData.QueryType);
                                }).ToArray();
                    }
                    else
                    {
                        // I.e. foreach (var myTuple in ...)
                        containerType.ReturnedTupleElementsDuringEnumeration =
                            queryDatas.Select((query, index) =>
                                new ReturnedTupleElementDuringEnumeration(
                                    query.TypeSymbolFullName,
                                    typeArgumentFullName: GetTypeArgumentFullName(query.QueryType, query.TypeSyntaxNode, SystemDescription.SemanticModel),
                                    preferredElementName: $"item{index + 1}",
                                    query.QueryType))
                            .ToArray();
                    }
                    break;
                }
            }

            return containerType;

            static string GetIdentifier(MemberDeclarationSyntax memberDeclarationSyntax)
            {
                switch (memberDeclarationSyntax)
                {
                    case ClassDeclarationSyntax classDeclarationSyntax:
                        return classDeclarationSyntax.Identifier.ValueText;
                    case StructDeclarationSyntax structDeclarationSyntax:
                        return structDeclarationSyntax.Identifier.ValueText;
                    case NamespaceDeclarationSyntax namespaceDeclarationSyntax:
                        var identifierName = namespaceDeclarationSyntax.ChildNodes().OfType<IdentifierNameSyntax>().FirstOrDefault();
                        return
                            identifierName != null
                                ? identifierName.Identifier.ValueText
                                : namespaceDeclarationSyntax.ChildNodes().OfType<QualifiedNameSyntax>().First().ToString();
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            static string GetTypeArgumentFullName(QueryType queryType, SyntaxNode queryTypeSyntaxNode, SemanticModel semanticModel)
            {
                switch (queryType)
                {
                    case QueryType.Aspect:
                        return default;
                    case QueryType.ManagedComponent:
                    case QueryType.ValueTypeComponent:
                    case QueryType.TagComponent:
                    case QueryType.ManagedSharedComponent:
                    case QueryType.UnmanagedSharedComponent:
                        return semanticModel.GetSymbolInfo(queryTypeSyntaxNode).Symbol.GetSymbolTypeName();
                    default:
                        // Will throw if someone were to write a namespace, pls be wary of this .ChildNodes mean Entities.RefRW would e.g. be invalid.
                        var typeArgument = queryTypeSyntaxNode.DescendantNodes().OfType<TypeArgumentListSyntax>().Single().Arguments.Single();
                        return ((ITypeSymbol)semanticModel.GetSymbolInfo(typeArgument).Symbol).ToFullName();
                }
            }
        }
    }
}
