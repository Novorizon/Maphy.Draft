using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Unity.Entities.SourceGen.Common;
using Unity.Entities.SourceGen.SystemGeneratorCommon;
using static Unity.Entities.SourceGen.Common.SourceGenHelpers.QueryVerification;

namespace Unity.Entities.SourceGen.LambdaJobs
{
    static class LambdaJobsVerification
    {
        public static void Verify(this LambdaJobDescription description)
        {
            // If our lambda expression is badly formed, early out as we will likely flag other false errors (capturing of this)
            if (!VerifyLambdaExpression(description))
            {
                description.Success = false;
                return;
            }

            if ((description.Burst.IsEnabled || description.Schedule.Mode != ScheduleMode.Run) && description.VariablesCaptured.Any(var => var.Type.IsReferenceType))
            {
                foreach (var variable in description.VariablesCaptured.Where(var => var.Type.IsReferenceType))
                    LambdaJobsErrors.DC0004(description.SystemDescription, description.Location, variable.OriginalVariableName, description.LambdaJobKind);
                description.Success = false;
            }

            if (description.LambdaJobKind == LambdaJobKind.Job && description.WithStructuralChanges)
            {
                LambdaJobsErrors.DC0057(description.SystemDescription, description.Location);
                description.Success = false;
            }

            if (!VerifyDC0073(description))
            {
                description.Success = false;
            }
        }

        static bool VerifyLambdaExpression(LambdaJobDescription description)
        {
            var success = true;

            // Check method calls inside lambda for:
            // a) if any structural changes are made
            // b) nested Entities.ForEach
            foreach (var invocationExpression in
                     description.OriginalLambdaExpression.DescendantNodes().OfType<InvocationExpressionSyntax>())
            {
                if ((!description.WithStructuralChanges || description.Schedule.Mode != ScheduleMode.Run) &&
                    invocationExpression.DoesPerformStructuralChange(description.SystemDescription.SemanticModel))
                {
                    LambdaJobsErrors.DC0027(description.SystemDescription, invocationExpression.GetLocation(), description.LambdaJobKind);
                    success = false;
                }

                var theInvocationSymbol = description.SystemDescription.SemanticModel.GetSymbolInfo(invocationExpression.Expression).Symbol;
                if (theInvocationSymbol == null)
                    continue;

                var isForEach = theInvocationSymbol is IMethodSymbol {Name: "ForEach"} methodSymbolForEach // Is ForEach method called as LambdaForEach
                    && methodSymbolForEach.ContainingType.Is("LambdaForEachDescriptionConstructionMethods");

               var isJobWithCode =  theInvocationSymbol is IMethodSymbol {Name: "WithCode"} methodSymbolWithCode // Is WithCode method called as LambdaSingleJob
                    && methodSymbolWithCode.ContainingType.Is("LambdaSingleJobDescriptionConstructionMethods");

                if (isForEach || isJobWithCode)
                {
                    LambdaJobsErrors.DC0029(description.SystemDescription, invocationExpression.GetLocation(), description.LambdaJobKind);
                    success = false;
                }
            }

            foreach (var methodInvocations in description.MethodInvocations)
            {
                if (methodInvocations.Key != "ForEach" && methodInvocations.Key != "WithCode")
                {
                    foreach (var methodInvocation in methodInvocations.Value.Where(methodInvocation => methodInvocation.ContainsDynamicCode()))
                    {
                        LambdaJobsErrors.DC0010(description.SystemDescription, methodInvocation.GetLocation(), methodInvocations.Key, description.LambdaJobKind);
                        description.Success = false;
                    }
                }
            }

            foreach (var methods in description.MethodInvocations.Where(methods => methods.Value.Count > 1))
            {
                var methodInvocationSymbol = description.SystemDescription.SemanticModel.GetSymbolInfo(methods.Value.First()).Symbol;
                if (!methodInvocationSymbol.HasAttribute(
                    "Unity.Entities.LambdaJobDescriptionConstructionMethods.AllowMultipleInvocationsAttribute"))
                {
                    LambdaJobsErrors.DC0009(description.SystemDescription, description.Location, methods.Key);
                    description.Success = false;
                }
            }

            foreach (var storeInQuerySyntax in description.WithStoreEntityQueryInFieldArgumentSyntaxes)
            {
                if (!(storeInQuerySyntax.Expression is IdentifierNameSyntax storeInQueryIdentifier) || !(description.SystemDescription.SemanticModel.GetSymbolInfo(storeInQueryIdentifier).Symbol is IFieldSymbol))
                {
                    LambdaJobsErrors.DC0031(description.SystemDescription, description.Location, description.SystemType);
                    description.Success = false;
                }
            }

            if (description.Burst.IsEnabled || description.Schedule.Mode is ScheduleMode.Schedule || description.Schedule.Mode is ScheduleMode.ScheduleParallel)
            {
                foreach (var param in description.LambdaParameters.OfType<IManagedComponentParamDescription>())
                {
                    LambdaJobsErrors.DC0223(description.SystemDescription, description.Location, param.Name, description.SystemType, true, description.LambdaJobKind);
                    description.Success = false;
                }
                foreach (var param in description.LambdaParameters.OfType<ISharedComponentParamDescription>())
                {
                    LambdaJobsErrors.DC0223(description.SystemDescription, description.Location, param.TypeSymbol.Name, description.SystemType, false, description.LambdaJobKind);
                    description.Success = false;
                }
            }

            var duplicateTypes =
                description
                    .LambdaParameters
                    .Where(desc => !desc.AllowDuplicateTypes && !desc.IsSourceGeneratedParam)
                    .FindDuplicatesBy(desc => desc.TypeSymbol.ToFullName());

            foreach (var duplicate in duplicateTypes)
            {
                LambdaJobsErrors.DC0070(description.SystemDescription, description.Location, duplicate.TypeSymbol);
                description.Success = false;
            }

            foreach (var param in description.LambdaParameters.OfType<IComponentParamDescription>().Where(param => string.IsNullOrEmpty(param.Syntax.GetModifierString())))
            {
                LambdaJobsErrors.DC0055(description.SystemDescription, description.Location, param.Name, description.LambdaJobKind);
            }

            foreach (var param in description.LambdaParameters.OfType<IManagedComponentParamDescription>().Where(param => param.IsByRef()))
            {
                if (param.IsByRef())
                {
                    LambdaJobsErrors.DC0024(description.SystemDescription, description.Location, param.Name);
                    description.Success = false;
                }
            }

            foreach (var param in description.LambdaParameters.OfType<ISharedComponentParamDescription>().Where(param => param.IsByRef()))
            {
                LambdaJobsErrors.DC0020(description.SystemDescription, description.Location, param.TypeSymbol.Name);
                description.Success = false;
            }

            foreach (var param in description.LambdaParameters.OfType<IDynamicBufferParamDescription>())
            {
                if (param.TypeSymbol is INamedTypeSymbol namedTypeSymbol)
                {
                    foreach (var typeArg in namedTypeSymbol.TypeArguments.OfType<INamedTypeSymbol>().Where(
                        typeArg => typeArg is { } namedTypeArg && namedTypeArg.TypeArguments.Any()))
                    {
                        LambdaJobsErrors.DC0050(description.SystemDescription, description.Location, typeArg.Name, description.LambdaJobKind);
                        description.Success = false;
                    }
                }
            }

            description.Success &=
                VerifyQueryTypeCorrectness(description.SystemDescription, description.Location, description.WithAllTypes, invokedMethodName: "WithAll");
            description.Success &=
                VerifyQueryTypeCorrectness(description.SystemDescription, description.Location, description.WithNoneTypes, invokedMethodName: "WithNone");
            description.Success &=
                VerifyQueryTypeCorrectness(description.SystemDescription, description.Location, description.WithAnyTypes, invokedMethodName: "WithAny");

            description.Success &=
                VerifySharedComponentFilterTypesAgainstOtherQueryTypes(description.SystemDescription, description.Location, description.WithSharedComponentFilterTypes, description.WithAllTypes);
            description.Success &=
                VerifySharedComponentFilterTypesAgainstOtherQueryTypes(description.SystemDescription, description.Location, description.WithSharedComponentFilterTypes, description.WithAnyTypes);
            description.Success &=
                VerifySharedComponentFilterTypesAgainstOtherQueryTypes(description.SystemDescription, description.Location, description.WithSharedComponentFilterTypes, description.WithNoneTypes);

            description.Success &=
                VerifyNoMutuallyExclusiveQueries(description.SystemDescription, description.Location, description.WithNoneTypes, description.WithAllTypes, "WithNone", "WithAll");
            description.Success &=
                VerifyNoMutuallyExclusiveQueries(description.SystemDescription, description.Location, description.WithNoneTypes, description.WithAnyTypes, "WithNone", "WithAny");
            description.Success &=
                VerifyNoMutuallyExclusiveQueries(description.SystemDescription, description.Location, description.WithAnyTypes, description.WithAllTypes, "WithAny", "WithAll");

            var lambdaParameters =
                description.LambdaParameters
                    .Where(param => param.IsQueryableType)
                    .Select(param =>
                        new Query
                        {
                            TypeSymbol = param.TypeSymbol,
                            Type = QueryType.All,
                            IsReadOnly = param.QueryTypeIsReadOnly()
                        })
                    .ToArray();

            description.Success &= VerifyNoMutuallyExclusiveQueries(
                description.SystemDescription,
                description.Location,
                description.WithNoneTypes,
                lambdaParameters,
                "WithNone",
                "lambda parameter",
                compareTypeSymbolsOnly: true);

            description.Success &= VerifyNoMutuallyExclusiveQueries(
                description.SystemDescription,
                description.Location,
                description.WithAnyTypes,
                lambdaParameters,
                "WithAny",
                "lambda parameter",
                compareTypeSymbolsOnly: true);

            var containingSystemTypeSymbol = (INamedTypeSymbol)description.SystemDescription.SemanticModel.GetDeclaredSymbol(description.SystemDescription.SystemTypeSyntax);
            if (containingSystemTypeSymbol.GetMembers().OfType<IMethodSymbol>().Any(method => method.Name == "OnCreateForCompiler"))
            {
                LambdaJobsErrors.DC0025(description.SystemDescription, description.Location, containingSystemTypeSymbol.Name);
                description.Success = false;
            }

            if (containingSystemTypeSymbol.TypeParameters.Any())
            {
                LambdaJobsErrors.DC0053(description.SystemDescription, description.Location, containingSystemTypeSymbol.Name, description.LambdaJobKind);
                description.Success = false;
            }

            var containingMethodSymbol = description.SystemDescription.SemanticModel.GetDeclaredSymbol(description.ContainingMethod);
            if (containingMethodSymbol is IMethodSymbol methodSymbol && methodSymbol.TypeArguments.Any())
            {
                LambdaJobsErrors.DC0054(description.SystemDescription, description.Location, methodSymbol.Name, description.LambdaJobKind);
                description.Success = false;
            }

            return success;
        }

        static bool VerifyDC0073(LambdaJobDescription description)
        {
            if ( description.WithScheduleGranularityArgumentSyntaxes.Count > 0
                && description.Schedule.Mode != ScheduleMode.ScheduleParallel)
            {
                LambdaJobsErrors.DC0073(description.SystemDescription, description.Location);
                return false;
            }
            return true;
        }
    }
}
