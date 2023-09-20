using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Unity.Entities.SourceGen.Common;
using Unity.Entities.SourceGen.SystemGeneratorCommon;

namespace Unity.Entities.SourceGen.SystemCodegenContext
{
    public partial class SystemContextSystemModule
    {
        class SystemApiSystemReplacer : SystemRewriter
        {
            SystemDescription m_Description;
            ReadOnlyCollection<CandidateSyntax> m_Candidates;

            public override IEnumerable<SyntaxNode> NodesToTrack => m_Candidates.Select(c=>c.Node);

            StatementHashStack m_StatementHashStack;
            public SystemApiSystemReplacer(ReadOnlyCollection<CandidateSyntax> candidates, SystemDescription description) {
                m_Description = description;
                m_Candidates = candidates;
                m_StatementHashStack = StatementHashStack.CreateInstance();
            }

            Dictionary<SyntaxNode, CandidateSyntax> m_SyntaxToCandidate;
            public override SyntaxNode VisitTrackedSystem(SyntaxNode systemRootNode, string originalFilePath)
            {
                m_OriginalFilePath = originalFilePath;

                m_SyntaxToCandidate = new Dictionary<SyntaxNode, CandidateSyntax>(m_Candidates.Count);
                foreach (var candidate in m_Candidates)
                {
                    var newNode = systemRootNode.GetCurrentNodes(candidate.Node).FirstOrDefault() ?? candidate.Node;
                    m_SyntaxToCandidate.Add(newNode, candidate);
                }

                return Visit(systemRootNode);
            }

            // These need to be saved as Visiting can ascend from recursion until we find the node we actually want to replace
            bool m_HasChangedMember;
            SyntaxNode m_NodeToReplace;
            SyntaxNode m_ReplacementNode;
            int m_OriginalLineNumber;

            // Actual statement insertion and replacement occurs here
            public override SyntaxNode Visit(SyntaxNode nodeIn)
            {
                if (nodeIn == null)
                    return null;

                // Visit children (and allow replacements to occur there)
                var replacedNodeAndChildren = base.Visit(nodeIn);

                // Perform replacement of candidates
                if (m_SyntaxToCandidate.TryGetValue(nodeIn, out var candidate)) {
                    var (original, replacement) = TryGetReplacementNodeFromCandidate(candidate, replacedNodeAndChildren, nodeIn);
                    if (replacement != null)
                    {
                        m_ReplacementNode = replacement;
                        m_NodeToReplace = original;
                        m_OriginalLineNumber = candidate.GetOriginalLineNumber();
                    }
                }

                if (replacedNodeAndChildren == m_NodeToReplace) {
                    replacedNodeAndChildren = m_ReplacementNode;
                    m_HasChangedMember = true;
                }

                // Insert statement prolog before replacement
                if (replacedNodeAndChildren is StatementSyntax statement && nodeIn == m_StatementHashStack.ActiveStatement) {
                    var poppedStatement = m_StatementHashStack.PopSyntax();
                    poppedStatement.Add(statement.WithHiddenLineTrivia() as StatementSyntax);
                    poppedStatement[0] = poppedStatement[0].WithLineTrivia(m_OriginalFilePath, m_OriginalLineNumber) as StatementSyntax;

                    replacedNodeAndChildren = SyntaxFactory.Block(new SyntaxList<AttributeListSyntax>(),
                        SyntaxFactory.MissingToken(SyntaxKind.OpenBraceToken),
                        new SyntaxList<StatementSyntax>(poppedStatement),
                        SyntaxFactory.MissingToken(SyntaxKind.CloseBraceToken));
                }

                if (replacedNodeAndChildren is MemberDeclarationSyntax memberSyntax && m_HasChangedMember) {
                    RecordChangedMember(memberSyntax);
                    m_HasChangedMember = false;
                }

                return replacedNodeAndChildren;
            }

            (SyntaxNode original, SyntaxNode replacement) TryGetReplacementNodeFromCandidate(CandidateSyntax candidate, SyntaxNode newestNode, SyntaxNode newestNonReplaced) {
                var semanticModel = m_Description.SemanticModel;

                var resolveCandidateSymbol = semanticModel.GetSymbolInfo(candidate.Node);
                var nodeSymbol = resolveCandidateSymbol.Symbol ?? resolveCandidateSymbol.CandidateSymbols.FirstOrDefault();
                var parentTypeInfo = nodeSymbol?.ContainingType;

                var fullName = parentTypeInfo?.ToFullName();
                var isSystemApi = fullName == "Unity.Entities.SystemAPI";
                var isManagedApi = fullName == "Unity.Entities.SystemAPI.ManagedAPI";
                if (!(isSystemApi || isManagedApi || (candidate.Type == CandidateType.Singleton && parentTypeInfo.Is("Unity.Entities.ComponentSystemBase"))))
                    return (null,null);

                switch (candidate.Type) {
                    case CandidateType.TimeData: {
                        return m_Description.TryGetSystemStateParameterName(candidate, out var systemStateExpression)
                            ? (CandidateSyntax.GetFieldExpression(newestNode), SyntaxFactory.ParseExpression($"{systemStateExpression}.WorldUnmanaged.Time"))
                            : (null, null);
                    }
                }

                // No type argument (EntityStorageInfoLookup, Exists)
                if (nodeSymbol is IMethodSymbol { TypeArguments: { Length: 0 } }) {
                    var invocationExpression = candidate.GetInvocationExpression(newestNode);
                    switch (candidate.Type) {
                        case CandidateType.GetEntityStorageInfoLookup:
                        case CandidateType.Exists:
                        {
                            var storageInfoLookup = m_Description.GetOrCreateEntityStorageInfoLookupField();
                            if (!m_Description.TryGetSystemStateParameterName(candidate, out var systemStateExpression))
                                return (null, null);

                            var statement = newestNonReplaced.AncestorOfKind<StatementSyntax>();
                            m_StatementHashStack.PushStatement(statement, $"{storageInfoLookup}.Update(ref {systemStateExpression});");

                            return candidate.Type switch
                            {
                                CandidateType.GetEntityStorageInfoLookup => (invocationExpression, SyntaxFactory.IdentifierName(storageInfoLookup)),
                                CandidateType.Exists => (invocationExpression, InvocationExpression(storageInfoLookup, "Exists", invocationExpression.ArgumentList)),
                                _ => throw new ArgumentOutOfRangeException() // Shouldn't be hit as outer switch is checking that only correct candidates go in!
                            };
                        }

                        case CandidateType.TypeHandle:
                        {
                            var typeHandleField = m_Description.GetOrCreateEntityTypeHandleField();
                            if (!m_Description.TryGetSystemStateParameterName(candidate, out var systemStateExpression)) return (null, null);
                            var statement = newestNonReplaced.AncestorOfKind<StatementSyntax>();
                            m_StatementHashStack.PushStatement(statement, $"{typeHandleField}.Update(ref {systemStateExpression});");
                            return (invocationExpression, SyntaxFactory.IdentifierName(typeHandleField));
                        }
                    }

                }

                // Based on type argument
                else if (nodeSymbol is IMethodSymbol { TypeArguments: { Length: 1 } } namedTypeSymbolWithTypeArg) {
                    var typeArgument = namedTypeSymbolWithTypeArg.TypeArguments.First();
                    var invocationExpression = candidate.GetInvocationExpression(newestNode);
                    if (TryGetSystemBaseGeneric(out var replacer))
                        return replacer;

                    switch (candidate.Type) {
                        // Component
                        case CandidateType.GetComponentLookup: {
                            var @readonly = false;
                            var args = invocationExpression.ArgumentList.Arguments.ToArray();
                            if (args.Length == 0 || bool.TryParse(args[0].Expression.ToString(), out @readonly)) {
                                var lookup = m_Description.GetOrCreateComponentLookupField(typeArgument, @readonly);
                                if (!m_Description.TryGetSystemStateParameterName(candidate, out var systemStateExpression))
                                    return (null, null);

                                var statement = newestNonReplaced.AncestorOfKind<StatementSyntax>();
                                m_StatementHashStack.PushStatement(statement, $"{lookup}.Update(ref {systemStateExpression});");
                                return (invocationExpression, SyntaxFactory.IdentifierName(lookup));
                            }

                            var methodDeclarationSyntax = candidate.Node.AncestorOfKind<MethodDeclarationSyntax>();
                            if (methodDeclarationSyntax.Identifier.ValueText == "OnCreate") {
                                var containingMethodSymbol = semanticModel.GetDeclaredSymbol(methodDeclarationSyntax);
                                if (containingMethodSymbol.Parameters.Length == 0 ||
                                    (containingMethodSymbol.Parameters.Length == 1 && containingMethodSymbol.Parameters[0].Type.Is("Unity.Entities.SystemState")))
                                {
                                    m_Description.TryGetSystemStateParameterName(candidate, out var systemStateExpression); // Ok to not handle as you can't be in OnCreate without it. By definition of above SystemState constraint.
                                    var sn = CandidateSyntax.GetSimpleName(newestNode);
                                    return (invocationExpression,
                                        SyntaxFactory.InvocationExpression(SyntaxFactory.ParseExpression($"{systemStateExpression}.{sn}"), invocationExpression.ArgumentList));
                                }
                            }

                            SystemAPIErrors.SGSA0002(m_Description, candidate);

                            break;
                        }
                        case CandidateType.GetComponent when isManagedApi: {
                            if (!m_Description.TryGetSystemStateParameterName(candidate, out var systemStateExpression))
                                return (null, null);
                            return (invocationExpression, InvocationExpression($"{systemStateExpression}.EntityManager","GetComponentObject",
                                candidate.Node.DescendantNodes().OfType<GenericNameSyntax>().First().TypeArgumentList, invocationExpression.ArgumentList));
                        }
                        case CandidateType.GetComponent: {
                            var lookup = m_Description.GetOrCreateComponentLookupField(typeArgument, true);
                            var entitySnippet = invocationExpression.ArgumentList.Arguments.First().Expression;
                            if (!m_Description.TryGetSystemStateParameterName(candidate, out var systemStateExpression))
                                return (null, null);

                            var statement = newestNonReplaced.AncestorOfKind<StatementSyntax>();
                            m_StatementHashStack.PushStatement(statement, $"{lookup}.Update(ref {systemStateExpression});");
                            m_StatementHashStack.PushStatement(statement, $"{systemStateExpression}.EntityManager.CompleteDependencyBeforeRO<{typeArgument.ToFullName()}>();");
                            return (invocationExpression, ElementAccessExpression(lookup, entitySnippet));
                        }
                        case CandidateType.GetComponentRW: {
                            var lookup = m_Description.GetOrCreateComponentLookupField(typeArgument, false);
                            if (!m_Description.TryGetSystemStateParameterName(candidate, out var systemStateExpression))
                                return (null, null);

                            var statement = newestNonReplaced.AncestorOfKind<StatementSyntax>();
                            m_StatementHashStack.PushStatement(statement, $"{lookup}.Update(ref {systemStateExpression});");
                            m_StatementHashStack.PushStatement(statement, $"{systemStateExpression}.EntityManager.CompleteDependencyBeforeRW<{typeArgument.ToFullName()}>();");
                            return (invocationExpression, InvocationExpression(lookup, "GetRefRW", invocationExpression.ArgumentList));
                        }
                        case CandidateType.SetComponent: {
                            var args = invocationExpression.ArgumentList.Arguments.ToArray();
                            if (args.Length != 2) return (null, null);
                            var (entityArg, componentArg) = args[0].NameColon?.Name.Identifier.ValueText == "component" ? (args[1], args[0]) : (args[0], args[1]);
                            typeArgument = typeArgument.TypeKind == TypeKind.TypeParameter
                                ? semanticModel.GetTypeInfo(componentArg.Expression).Type
                                : typeArgument;

                            var lookup = m_Description.GetOrCreateComponentLookupField(typeArgument, false);
                            if (!m_Description.TryGetSystemStateParameterName(candidate, out var systemStateExpression)) return (null, null);

                            var statement = newestNonReplaced.AncestorOfKind<StatementSyntax>();
                            m_StatementHashStack.PushStatement(statement, $"{lookup}.Update(ref {systemStateExpression});");
                            m_StatementHashStack.PushStatement(statement, $"{systemStateExpression}.EntityManager.CompleteDependencyBeforeRW<{typeArgument.ToFullName()}>();");
                            return (invocationExpression, SyntaxFactory.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, ElementAccessExpression(lookup, entityArg.Expression), componentArg.Expression));
                        }
                        case CandidateType.HasComponent when isManagedApi: {
                            if (!m_Description.TryGetSystemStateParameterName(candidate, out var systemStateExpression))
                                return (null, null);
                            return (invocationExpression, InvocationExpression($"{systemStateExpression}.EntityManager","HasComponent",
                                candidate.Node.DescendantNodes().OfType<GenericNameSyntax>().First().TypeArgumentList, invocationExpression.ArgumentList));
                        }
                        case CandidateType.HasComponent: {
                            var lookup = m_Description.GetOrCreateComponentLookupField(typeArgument, true);
                            if (!m_Description.TryGetSystemStateParameterName(candidate, out var systemStateExpression)) return (null, null);

                            var statement = newestNonReplaced.AncestorOfKind<StatementSyntax>();
                            m_StatementHashStack.PushStatement(statement, $"{lookup}.Update(ref {systemStateExpression});");
                            m_StatementHashStack.PushStatement(statement, $"{systemStateExpression}.EntityManager.CompleteDependencyBeforeRO<{typeArgument.ToFullName()}>();");
                            return (invocationExpression, InvocationExpression(lookup, "HasComponent", invocationExpression.ArgumentList));
                        }
                        case CandidateType.IsComponentEnabled when isManagedApi: {
                            if (!m_Description.TryGetSystemStateParameterName(candidate, out var systemStateExpression)) return (null, null);
                            return (invocationExpression, InvocationExpression($"{systemStateExpression}.EntityManager","IsComponentEnabled",
                                candidate.Node.DescendantNodes().OfType<GenericNameSyntax>().First().TypeArgumentList, invocationExpression.ArgumentList));
                        }
                        case CandidateType.IsComponentEnabled: {
                            var lookup = m_Description.GetOrCreateComponentLookupField(typeArgument, true);
                            if (!m_Description.TryGetSystemStateParameterName(candidate, out var systemStateExpression)) return (null, null);

                            var statement = newestNonReplaced.AncestorOfKind<StatementSyntax>();
                            m_StatementHashStack.PushStatement(statement, $"{lookup}.Update(ref {systemStateExpression});");
                            m_StatementHashStack.PushStatement(statement, $"{systemStateExpression}.EntityManager.CompleteDependencyBeforeRO<{typeArgument.ToFullName()}>();");
                            return (invocationExpression, InvocationExpression(lookup, "IsComponentEnabled", invocationExpression.ArgumentList));
                        }
                        case CandidateType.SetComponentEnabled when isManagedApi: {
                            if (!m_Description.TryGetSystemStateParameterName(candidate, out var systemStateExpression)) return (null, null);
                            return (invocationExpression, InvocationExpression($"{systemStateExpression}.EntityManager","SetComponentEnabled",
                                candidate.Node.DescendantNodes().OfType<GenericNameSyntax>().First().TypeArgumentList, invocationExpression.ArgumentList));
                        }
                        case CandidateType.SetComponentEnabled: {
                            var lookup = m_Description.GetOrCreateComponentLookupField(typeArgument, false);
                            if (!m_Description.TryGetSystemStateParameterName(candidate, out var systemStateExpression)) return (null, null);

                            var statement = newestNonReplaced.AncestorOfKind<StatementSyntax>();
                            m_StatementHashStack.PushStatement(statement, $"{lookup}.Update(ref {systemStateExpression});");
                            m_StatementHashStack.PushStatement(statement, $"{systemStateExpression}.EntityManager.CompleteDependencyBeforeRW<{typeArgument.ToFullName()}>();");
                            return (invocationExpression, InvocationExpression(lookup, "SetComponentEnabled", invocationExpression.ArgumentList));
                        }

                        // Buffer
                        case CandidateType.GetBufferLookup: {
                            var @readonly = false;
                            var args = invocationExpression.ArgumentList.Arguments.ToArray();
                            if (args.Length == 0 || bool.TryParse(args[0].Expression.ToString(), out @readonly)) {
                                var bufferLookup = m_Description.GetOrCreateBufferLookupField(typeArgument, @readonly);
                                if (!m_Description.TryGetSystemStateParameterName(candidate, out var systemStateExpression)) return (null, null);

                                var statement = newestNonReplaced.AncestorOfKind<StatementSyntax>();
                                m_StatementHashStack.PushStatement(statement, $"{bufferLookup}.Update(ref {systemStateExpression});");
                                return (invocationExpression, SyntaxFactory.IdentifierName(bufferLookup));
                            }

                            var methodDeclarationSyntax = candidate.Node.AncestorOfKind<MethodDeclarationSyntax>();
                            if (methodDeclarationSyntax.Identifier.ValueText == "OnCreate") {
                                var containingMethodSymbol = semanticModel.GetDeclaredSymbol(methodDeclarationSyntax);
                                if (containingMethodSymbol.Parameters.Length == 0 ||
                                    (containingMethodSymbol.Parameters.Length == 1 && containingMethodSymbol.Parameters[0].Type.Is("Unity.Entities.SystemState")))
                                {
                                    m_Description.TryGetSystemStateParameterName(candidate, out var systemStateExpression);
                                    var sn = CandidateSyntax.GetSimpleName(newestNode);
                                    return (invocationExpression, SyntaxFactory.InvocationExpression(SyntaxFactory.ParseExpression($"{systemStateExpression}.{sn}"),
                                        invocationExpression.ArgumentList));
                                }
                            }

                            SystemAPIErrors.SGSA0002(m_Description, candidate);

                            break;
                        }
                        case CandidateType.GetBuffer: {
                            var bufferLookup = m_Description.GetOrCreateBufferLookupField(typeArgument, false);
                            var entitySnippet = invocationExpression.ArgumentList.Arguments.First().Expression;
                            if (!m_Description.TryGetSystemStateParameterName(candidate, out var systemStateExpression))
                                return (null, null);

                            var statement = newestNonReplaced.AncestorOfKind<StatementSyntax>();
                            m_StatementHashStack.PushStatement(statement, $"{bufferLookup}.Update(ref {systemStateExpression});");
                            m_StatementHashStack.PushStatement(statement,$"{systemStateExpression}.EntityManager.CompleteDependencyBeforeRW<{typeArgument.ToFullName()}>();"); // Todo in next PR, change SysApi signature to match GetBuffer of EntityManager
                            return (invocationExpression, ElementAccessExpression(bufferLookup, entitySnippet));
                        }
                        case CandidateType.HasBuffer: {
                            var bufferLookup = m_Description.GetOrCreateBufferLookupField(typeArgument, true);
                            if (!m_Description.TryGetSystemStateParameterName(candidate, out var systemStateExpression))
                                return (null, null);

                            var statement = newestNonReplaced.AncestorOfKind<StatementSyntax>();
                            m_StatementHashStack.PushStatement(statement, $"{bufferLookup}.Update(ref {systemStateExpression});");
                            m_StatementHashStack.PushStatement(statement,$"{systemStateExpression}.EntityManager.CompleteDependencyBeforeRO<{typeArgument.ToFullName()}>();");
                            return (invocationExpression, InvocationExpression(bufferLookup, "HasBuffer", invocationExpression.ArgumentList));
                        }
                        case CandidateType.IsBufferEnabled: {
                            var bufferLookup = m_Description.GetOrCreateBufferLookupField(typeArgument, true);
                            if (!m_Description.TryGetSystemStateParameterName(candidate, out var systemStateExpression))
                                return (null, null);

                            var statement = newestNonReplaced.AncestorOfKind<StatementSyntax>();
                            m_StatementHashStack.PushStatement(statement, $"{bufferLookup}.Update(ref {systemStateExpression});");
                            m_StatementHashStack.PushStatement(statement,$"{systemStateExpression}.EntityManager.CompleteDependencyBeforeRO<{typeArgument.ToFullName()}>();");
                            return (invocationExpression, InvocationExpression(bufferLookup, "IsBufferEnabled", invocationExpression.ArgumentList));
                        }
                        case CandidateType.SetBufferEnabled: {
                            var bufferLookup = m_Description.GetOrCreateBufferLookupField(typeArgument, false);
                            if (!m_Description.TryGetSystemStateParameterName(candidate, out var systemStateExpression))
                                return (null, null);

                            var statement = newestNonReplaced.AncestorOfKind<StatementSyntax>();
                            m_StatementHashStack.PushStatement(statement, $"{bufferLookup}.Update(ref {systemStateExpression});");
                            m_StatementHashStack.PushStatement(statement,$"{systemStateExpression}.EntityManager.CompleteDependencyBeforeRW<{typeArgument.ToFullName()}>();");
                            return (invocationExpression, InvocationExpression(bufferLookup, "SetBufferEnabled", invocationExpression.ArgumentList));
                        }

                        // Singleton
                        case CandidateType.Singleton: {
                            var queryFieldName = m_Description.GetOrCreateQueryField(
                                new SingleArchetypeQueryFieldDescription(
                                    new Archetype(
                                        new[]
                                        {
                                            new Query
                                            {
                                                IsReadOnly = (candidate.Flags & CandidateFlags.ReadOnly) == CandidateFlags.ReadOnly,
                                                Type = QueryType.All,
                                                TypeSymbol = typeArgument
                                            }
                                        },
                                        Array.Empty<Query>(),
                                        Array.Empty<Query>(),
                                        EntityQueryOptions.Default | EntityQueryOptions.IncludeSystems)
                                ));

                            var sn = CandidateSyntax.GetSimpleName(newestNode);
                            var noGenericGeneration = (candidate.Flags & CandidateFlags.NoGenericGeneration) == CandidateFlags.NoGenericGeneration;
                            var memberAccess = noGenericGeneration ? sn.Identifier.ValueText : sn.ToString(); // e.g. GetSingletonEntity<T> -> query.GetSingletonEntity (with no generic)

                            return (invocationExpression, InvocationExpression($"{queryFieldName}", memberAccess, invocationExpression.ArgumentList));
                        }

                        // Aspect
                        case CandidateType.Aspect: {
                            var @readonly = (candidate.Flags == CandidateFlags.ReadOnly);

                            if (!m_Description.TryGetSystemStateParameterName(candidate, out var systemStateExpression))
                                return (null, null);

                            var entitySnippet = invocationExpression.ArgumentList.Arguments.First();
                            var aspectLookup = m_Description.GetOrCreateAspectLookup(typeArgument, @readonly);

                            var statement = newestNonReplaced.AncestorOfKind<StatementSyntax>();
                            m_StatementHashStack.PushStatement(statement, $"{aspectLookup}.Update(ref {systemStateExpression});");
                            var completeDependencyStatement = @readonly switch
                                {
                                    true => $"{typeArgument.ToFullName()}.CompleteDependencyBeforeRO(ref {systemStateExpression});",
                                    false => $"{typeArgument.ToFullName()}.CompleteDependencyBeforeRW(ref {systemStateExpression});"
                                };
                            m_StatementHashStack.PushStatement(statement, completeDependencyStatement);

                            var replacementElementAccessExpression = SyntaxFactory.ElementAccessExpression(SyntaxFactory.IdentifierName(aspectLookup))
                                .WithArgumentList(SyntaxFactory.BracketedArgumentList(SyntaxFactory.SingletonSeparatedList(entitySnippet)));

                            return (invocationExpression, replacementElementAccessExpression);
                        }

                        // TypeHandle
                        case CandidateType.TypeHandle: {
                            var @readonly = false;
                            var args = invocationExpression.ArgumentList.Arguments.ToArray();
                            if (args.Length == 0 || bool.TryParse(args[0].Expression.ToString(), out @readonly))
                            {
                                var typeHandleField = m_Description.GetOrCreateTypeHandleField(typeArgument, @readonly);
                                if (!m_Description.TryGetSystemStateParameterName(candidate, out var systemStateExpression)) return (null, null);

                                var statement = newestNonReplaced.AncestorOfKind<StatementSyntax>();
                                m_StatementHashStack.PushStatement(statement, $"{typeHandleField}.Update(ref {systemStateExpression});");
                                return (invocationExpression, SyntaxFactory.IdentifierName(typeHandleField));
                            }

                            var methodDeclarationSyntax = candidate.Node.AncestorOfKind<MethodDeclarationSyntax>();
                            if (methodDeclarationSyntax.Identifier.ValueText == "OnCreate") {
                                var containingMethodSymbol = semanticModel.GetDeclaredSymbol(methodDeclarationSyntax);
                                if (containingMethodSymbol.Parameters.Length == 0 ||
                                    (containingMethodSymbol.Parameters.Length == 1 && containingMethodSymbol.Parameters[0].Type.Is("Unity.Entities.SystemState")))
                                {
                                    m_Description.TryGetSystemStateParameterName(candidate, out var systemStateExpression);
                                    var sn = CandidateSyntax.GetSimpleName(newestNode);
                                    return (invocationExpression, SyntaxFactory.InvocationExpression(SyntaxFactory.ParseExpression($"{systemStateExpression}.{sn}"),
                                        invocationExpression.ArgumentList));
                                }
                            }

                            SystemAPIErrors.SGSA0002(m_Description, candidate);

                            break;
                        }
                    }

                    // If using a generic that takes part of a method, then it should default to a `InternalCompilerInterface.DontUseThisGetSingleQuery<T>(this).` reference in SystemBase, and cause a compile error in ISystem.
                    bool TryGetSystemBaseGeneric(out (SyntaxNode original, SyntaxNode replacement) replacer)
                    {
                        replacer = (null, null);
                        var usesGenericTypeParameter = typeArgument.TypeKind == TypeKind.TypeParameter || typeArgument is INamedTypeSymbol { IsGenericType: true };
                        if (m_Description.SystemTypeSyntax.TypeParameterList != null)
                            usesGenericTypeParameter &= m_Description.SystemTypeSyntax.TypeParameterList.Parameters.All(p => p.Identifier.ValueText != typeArgument.Name);

                        if (usesGenericTypeParameter) {
                            if (m_Description.SystemType==SystemType.ISystem)
                                SystemAPIErrors.SGSA0001(m_Description, candidate);
                            else if (isSystemApi) // Enabled you to use type parameters for SystemAPI inside SystemBase
                            {
                                var sn = CandidateSyntax.GetSimpleName(newestNode);
                                var noGenericGenerationGeneric = (candidate.Flags & CandidateFlags.NoGenericGeneration) == CandidateFlags.NoGenericGeneration;
                                var memberAccessGeneric = noGenericGenerationGeneric ? sn.Identifier.ValueText : sn.ToString(); // e.g. GetSingletonEntity<T> -> query.GetSingletonEntity (with no generic)

                                var dontUseThisGetSingleQueryInvocation = InvocationExpression(
                                    "Unity.Entities.InternalCompilerInterface",
                                    "OnlyAllowedInSourceGeneratedCodeGetSingleQuery",
                                    TypeArgumentListSyntax(typeArgument.ToFullName()),
                                    ArgumentListSyntax(SyntaxFactory.ThisExpression()));
                                replacer = (invocationExpression, InvocationExpression(dontUseThisGetSingleQueryInvocation, memberAccessGeneric, invocationExpression.ArgumentList));
                            }
                            return true;
                        }

                        return false;
                    }
                }

                return (null, null);
            }

            static ArgumentListSyntax ArgumentListSyntax(ExpressionSyntax expression)
                => SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(new[] { SyntaxFactory.Argument(expression) }));
            static TypeArgumentListSyntax TypeArgumentListSyntax(string typeName)
                => SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList(new[] { (TypeSyntax)SyntaxFactory.IdentifierName(typeName)}));
            static InvocationExpressionSyntax InvocationExpression(ExpressionSyntax from, string invocation, ArgumentListSyntax args)
                => SyntaxFactory.InvocationExpression(SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, from, SyntaxFactory.IdentifierName(invocation)), args);
            static InvocationExpressionSyntax InvocationExpression(string from, string invocation, TypeArgumentListSyntax typeArgs, ArgumentListSyntax args)
                => SyntaxFactory.InvocationExpression(SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.IdentifierName(@from), SyntaxFactory.GenericName(SyntaxFactory.Identifier(invocation), typeArgs)), args);
            static InvocationExpressionSyntax InvocationExpression(string from, string invocation, ArgumentListSyntax args)
                => SyntaxFactory.InvocationExpression(SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SyntaxFactory.IdentifierName(@from), SyntaxFactory.IdentifierName(invocation)), args);
            static ElementAccessExpressionSyntax ElementAccessExpression(string componentLookup, ExpressionSyntax entitySnippet)
                => SyntaxFactory.ElementAccessExpression(SyntaxFactory.IdentifierName(componentLookup), SyntaxFactory.BracketedArgumentList(SyntaxFactory.SeparatedList(new []{SyntaxFactory.Argument(entitySnippet)})));
        }
    }
}
