﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;
using Unity.Entities.SourceGen.Common;
using Unity.Entities.SourceGen.SystemGeneratorCommon;

namespace Unity.Entities.SourceGen.SystemAPIQueryBuilder
{
    public partial class SystemAPIQueryBuilderDescription
    {
        readonly QueryCandidate _queryCandidate;
        readonly StringBuilder _invocationsBeforeBuild = new StringBuilder();

        public readonly bool IsBurstEnabled;
        public readonly List<Archetype> Archetypes = new List<Archetype>();
        public readonly List<Location> QueryFinalizingLocations = new List<Location>();
        public readonly SystemDescription SystemDescription;

        public SyntaxNode NodeToReplace => _queryCandidate.BuildNode;
        public SyntaxNode GetReplacementNode() => SyntaxFactory.IdentifierName(GeneratedEntityQueryFieldName);

        public string GeneratedEntityQueryFieldName { get; set; }
        public bool Success { get; set; } = true;

        /*
         E.g.: Given the following:

            `SystemAPI.QueryBuilder().WithAll<Comp1>().WithAny<Comp2>().Build()`

         return

            `.WithAll<FullyQualified.Comp1>().WithAny<FullyQualified.Comp2>()`

         so that we can use the above when generating

            `_generatedQueryField = new EntityQueryBuilder(Allocator.Temp).WithAll<FullyQualified.Comp1>().WithAny<FullyQualified.Comp2>().Build()`
         */
        public string GetQueryBuilderBodyBeforeBuild() => _invocationsBeforeBuild.ToString();

        public SystemAPIQueryBuilderDescription(SystemDescription systemDescription, QueryCandidate queryCandidate)
        {
            SystemDescription = systemDescription;

            if (!SystemDescription.TryGetSystemStateParameterName(queryCandidate, out _))
            {
                Success = false;
                return;
            }

            if (systemDescription.SemanticModel.GetOperation(queryCandidate.BuildNode) is IInvocationOperation invocationOperation
                && invocationOperation.TargetMethod.ToString() == "Unity.Entities.SystemAPIQueryBuilder.Build()")
            {
                _queryCandidate = queryCandidate;

                SystemDescription = systemDescription;
                queryCandidate.BuildNode.GetLocation();

                var containingMethod = queryCandidate.BuildNode.AncestorOfKindOrDefault<MethodDeclarationSyntax>();
                if (containingMethod != null)
                {
                    var methodSymbol = (IMethodSymbol)ModelExtensions.GetDeclaredSymbol(SystemDescription.SemanticModel, containingMethod);
                    IsBurstEnabled = methodSymbol.HasAttribute("Unity.Burst.BurstCompileAttribute");

                    var noneQueryTypes = new List<Query>();
                    var anyQueryTypes = new List<Query>();
                    var allQueryTypes = new List<Query>();
                    var entityQueryOptions = new List<EntityQueryOptions>();

                    foreach (var node in queryCandidate.SystemAPIQueryBuilderNode.Ancestors().OfType<InvocationExpressionSyntax>())
                    {
                        switch (node.Expression)
                        {
                            case MemberAccessExpressionSyntax { Name: GenericNameSyntax genericNameSyntax }:
                            {
                                Query[] typeArguments;
                                switch (genericNameSyntax.Identifier.ValueText)
                                {
                                    case "WithAll":
                                        typeArguments =
                                            genericNameSyntax.TypeArgumentList.Arguments.Select(typeArg =>
                                                new Query
                                                {
                                                    TypeSymbol = (ITypeSymbol)ModelExtensions.GetSymbolInfo(systemDescription.SemanticModel, typeArg).Symbol,
                                                    Type = SystemGeneratorCommon.QueryType.All,
                                                    IsReadOnly = true
                                                }).ToArray();

                                        _invocationsBeforeBuild.AppendLine($".WithAll<{typeArguments.Select(t => t.TypeSymbol.ToFullName()).SeparateByCommaAndSpace()}>()");
                                        allQueryTypes.AddRange(typeArguments);
                                        break;
                                    case "WithAllChunkComponent":
                                        typeArguments =
                                            genericNameSyntax.TypeArgumentList.Arguments.Select(typeArg =>
                                                new Query
                                                {
                                                    TypeSymbol = (ITypeSymbol)ModelExtensions
                                                        .GetSymbolInfo(systemDescription.SemanticModel, typeArg).Symbol,
                                                    Type = SystemGeneratorCommon.QueryType.All,
                                                    IsReadOnly = true
                                                }).ToArray();

                                        _invocationsBeforeBuild.AppendLine($".WithAllChunkComponent<{typeArguments.Select(t => t.TypeSymbol.ToFullName()).SeparateByCommaAndSpace()}>()");
                                        allQueryTypes.AddRange(typeArguments);
                                        break;
                                    case "WithAllRW":
                                        typeArguments =
                                            genericNameSyntax.TypeArgumentList.Arguments.Select(typeArg =>
                                                new Query
                                                {
                                                    TypeSymbol = (ITypeSymbol)ModelExtensions
                                                        .GetSymbolInfo(systemDescription.SemanticModel, typeArg).Symbol,
                                                    Type = SystemGeneratorCommon.QueryType.All,
                                                    IsReadOnly = false
                                                }).ToArray();

                                        _invocationsBeforeBuild.AppendLine($".WithAllRW<{typeArguments.Select(t => t.TypeSymbol.ToFullName()).SeparateByCommaAndSpace()}>()");
                                        allQueryTypes.AddRange(typeArguments);
                                        break;
                                    case "WithAllChunkComponentRW":
                                        typeArguments =
                                            genericNameSyntax.TypeArgumentList.Arguments.Select(typeArg =>
                                                new Query
                                                {
                                                    TypeSymbol = (ITypeSymbol)ModelExtensions
                                                        .GetSymbolInfo(systemDescription.SemanticModel, typeArg).Symbol,
                                                    Type = SystemGeneratorCommon.QueryType.All,
                                                    IsReadOnly = false
                                                }).ToArray();

                                        _invocationsBeforeBuild.AppendLine($".WithAllChunkComponentRW<{typeArguments.Select(t => t.TypeSymbol.ToFullName()).SeparateByCommaAndSpace()}>()");
                                        allQueryTypes.AddRange(typeArguments);
                                        break;
                                    case "WithAny":
                                        typeArguments =
                                            genericNameSyntax.TypeArgumentList.Arguments.Select(typeArg =>
                                                new Query
                                                {
                                                    TypeSymbol = (ITypeSymbol)ModelExtensions
                                                        .GetSymbolInfo(systemDescription.SemanticModel, typeArg).Symbol,
                                                    Type = SystemGeneratorCommon.QueryType.Any,
                                                    IsReadOnly = true
                                                }).ToArray();

                                        _invocationsBeforeBuild.AppendLine($".WithAny<{typeArguments.Select(t => t.TypeSymbol.ToFullName()).SeparateByCommaAndSpace()}>()");
                                        anyQueryTypes.AddRange(typeArguments);
                                        break;
                                    case "WithAnyChunkComponent":
                                        typeArguments = genericNameSyntax.TypeArgumentList.Arguments.Select(typeArg =>
                                            new Query
                                            {
                                                TypeSymbol = (ITypeSymbol)ModelExtensions.GetSymbolInfo(systemDescription.SemanticModel, typeArg).Symbol,
                                                Type = SystemGeneratorCommon.QueryType.Any,
                                                IsReadOnly = true
                                            }).ToArray();

                                        _invocationsBeforeBuild.AppendLine($".WithAnyChunkComponent<{typeArguments.Select(t => t.TypeSymbol.ToFullName()).SeparateByCommaAndSpace()}>()");
                                        anyQueryTypes.AddRange(typeArguments);
                                        break;
                                    case "WithAnyRW":
                                         typeArguments = genericNameSyntax.TypeArgumentList.Arguments.Select(typeArg =>
                                            new Query
                                            {
                                                TypeSymbol = (ITypeSymbol)ModelExtensions.GetSymbolInfo(systemDescription.SemanticModel, typeArg).Symbol,
                                                Type = SystemGeneratorCommon.QueryType.Any,
                                                IsReadOnly = false
                                            }).ToArray();

                                        _invocationsBeforeBuild.AppendLine($".WithAnyRW<{typeArguments.Select(t => t.TypeSymbol.ToFullName()).SeparateByCommaAndSpace()}>()");
                                        anyQueryTypes.AddRange(typeArguments);
                                        break;
                                    case "WithAnyChunkComponentRW":
                                        typeArguments = genericNameSyntax.TypeArgumentList.Arguments.Select(typeArg =>
                                            new Query
                                            {
                                                TypeSymbol = (ITypeSymbol)ModelExtensions.GetSymbolInfo(systemDescription.SemanticModel, typeArg).Symbol,
                                                Type = SystemGeneratorCommon.QueryType.Any,
                                                IsReadOnly = false
                                            }).ToArray();

                                        _invocationsBeforeBuild.AppendLine($".WithAnyChunkComponentRW<{typeArguments.Select(t => t.TypeSymbol.ToFullName()).SeparateByCommaAndSpace()}>()");
                                        anyQueryTypes.AddRange(typeArguments);
                                        break;
                                    case "WithNone":
                                        typeArguments =
                                            genericNameSyntax.TypeArgumentList.Arguments.Select(typeArg =>
                                                new Query
                                                {
                                                    TypeSymbol = (ITypeSymbol)ModelExtensions
                                                        .GetSymbolInfo(systemDescription.SemanticModel, typeArg).Symbol,
                                                    Type = SystemGeneratorCommon.QueryType.None,
                                                    IsReadOnly = true
                                                }).ToArray();

                                        _invocationsBeforeBuild.AppendLine($".WithNone<{typeArguments.Select(t => t.TypeSymbol.ToFullName()).SeparateByCommaAndSpace()}>()");
                                        noneQueryTypes.AddRange(typeArguments);
                                        break;
                                    case "WithNoneChunkComponent":
                                        typeArguments =
                                            genericNameSyntax.TypeArgumentList.Arguments.Select(typeArg =>
                                                new Query
                                                {
                                                    TypeSymbol = (ITypeSymbol)ModelExtensions
                                                        .GetSymbolInfo(systemDescription.SemanticModel, typeArg).Symbol,
                                                    Type = SystemGeneratorCommon.QueryType.None,
                                                    IsReadOnly = true
                                                }).ToArray();

                                        _invocationsBeforeBuild.AppendLine($".WithNoneChunkComponent<{typeArguments.Select(t => t.TypeSymbol.ToFullName()).SeparateByCommaAndSpace()}>()");
                                        noneQueryTypes.AddRange(typeArguments);
                                        break;
                                }
                                break;
                            }
                            case MemberAccessExpressionSyntax { Name: IdentifierNameSyntax identifierNameSyntax }:
                            {
                                Archetype archetype;

                                switch (identifierNameSyntax.Identifier.ValueText)
                                {
                                    case "WithOptions":
                                        var argumentSyntax = node.ArgumentList.Arguments.Single();
                                        var (options, argumentWithFullyQualifiedName) = GetEntityQueryOptionsArgument(argumentSyntax);
                                        entityQueryOptions.Add(options);

                                        _invocationsBeforeBuild.AppendLine($".WithOptions({argumentWithFullyQualifiedName})");
                                        break;
                                    case "AddAdditionalQuery":
                                        _invocationsBeforeBuild.AppendLine(".AddAdditionalQuery()");

                                        if (entityQueryOptions.Count > 1)
                                        {
                                            SystemAPIQueryBuilderErrors.SGQB001(SystemDescription, node.GetLocation());
                                            Success = false;
                                        }
                                        else
                                        {
                                            archetype = new Archetype(allQueryTypes, anyQueryTypes, noneQueryTypes, entityQueryOptions.Any() ? entityQueryOptions.SingleOrDefault() : EntityQueryOptions.Default);
                                            Archetypes.Add(archetype);
                                            QueryFinalizingLocations.Add(node.GetLocation());
                                        }

                                        allQueryTypes.Clear();
                                        anyQueryTypes.Clear();
                                        noneQueryTypes.Clear();
                                        entityQueryOptions.Clear();
                                        break;
                                    case "Build":
                                        if (entityQueryOptions.Count > 1)
                                        {
                                            SystemAPIQueryBuilderErrors.SGQB001(SystemDescription, node.GetLocation());
                                            Success = false;
                                        }
                                        else
                                        {
                                            archetype = new Archetype(allQueryTypes, anyQueryTypes, noneQueryTypes, entityQueryOptions.Any() ? entityQueryOptions.SingleOrDefault() : EntityQueryOptions.Default);
                                            Archetypes.Add(archetype);
                                            QueryFinalizingLocations.Add(node.GetLocation());
                                        }
                                        break;
                                }
                                break;
                            }
                        }
                    }
                }
            }
            else
                Success = false;
        }

        public (EntityQueryOptions Options, string ArgumentWithFullyQualifiedName)
            GetEntityQueryOptionsArgument(ArgumentSyntax argumentSyntax)
        {
            var options = EntityQueryOptions.Default;
            var argumentExpression = argumentSyntax.Expression;

            while (argumentExpression is BinaryExpressionSyntax binaryExpressionSyntax)
            {
                if (SourceGenHelpers.TryParseQualifiedEnumValue(binaryExpressionSyntax.Right.ToString(),
                        out EntityQueryOptions optionArg))
                    options |= optionArg;

                argumentExpression = binaryExpressionSyntax.Left;
            }

            if (SourceGenHelpers.TryParseQualifiedEnumValue(argumentExpression.ToString(), out EntityQueryOptions option))
                options |= option;

            return (options, options.GetFlags().Select(flag => $"Unity.Entities.EntityQueryOptions.{flag.ToString()}").SeparateByBinaryOr());
        }
    }
}
