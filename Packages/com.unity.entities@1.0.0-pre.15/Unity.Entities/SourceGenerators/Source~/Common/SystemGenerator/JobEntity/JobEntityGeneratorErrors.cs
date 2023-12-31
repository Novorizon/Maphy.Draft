﻿using System.Linq;
using Microsoft.CodeAnalysis;
using Unity.Entities.SourceGen.Common;

namespace Unity.Entities.SourceGen.SystemGeneratorCommon
{
    public static class JobEntityGeneratorErrors
    {
        const string k_WarningTitle = "IJobEntity Warning";
        const string k_ErrorTitle = "IJobEntity Error";

        public static void SGJE0003(
            ISourceGeneratorDiagnosable context,
            Location location,
            string parameterName,
            string parameterType)
        {
            context.LogError(
                nameof(SGJE0003),
                k_ErrorTitle,
                $"The parameter '{parameterName}' of type {parameterType} will be ignored.",
                location);
        }

        public static void SGJE0004(ISourceGeneratorDiagnosable context, Location location, string nonPartialJobEntityStructName)
        {
            context.LogError(
                nameof(SGJE0004),
                k_ErrorTitle,
                $"{nonPartialJobEntityStructName} is an IJobEntity job struct, but is not defined with partial. " +
                "IJobEntity job structs are source generated. Please add the `partial` keyword as part of the struct definition.",
                location);
        }

        public static void SGJE0006(ISourceGeneratorDiagnosable context, Location location, string jobEntityTypeName, string methodSignature, string nonIntegerEntityQueryParameter, string attributeName)
        {
            context.LogError(
                nameof(SGJE0006),
                k_ErrorTitle,
                $"{jobEntityTypeName}.{methodSignature} accepts a non-integer parameter ('{nonIntegerEntityQueryParameter}') with the [{attributeName}] attribute. " +
                $"This is not allowed. The [{attributeName}] attribute may only be applied on integer parameters.",
                location);
        }

        public static void SGJE0007(ISourceGeneratorDiagnosable context, Location location, string jobEntityTypeName, string methodSignature, string attributeName)
        {
            context.LogError(
                nameof(SGJE0007),
                k_ErrorTitle,
                $"{jobEntityTypeName}.{methodSignature} accepts more than one integer parameters with the [{attributeName}] attribute. " +
                $"This is not allowed. The [{attributeName}] attribute can only be applied EXACTLY ONCE on an integer parameter in {jobEntityTypeName}.{methodSignature}.",
                location);
        }

        public static void SGJE0008(ISourceGeneratorDiagnosable context, Location location, string jobEntityTypeName, IMethodSymbol[] userDefinedExecuteMethods)
        {
            context.LogError(nameof(SGJE0008), k_ErrorTitle,
                $"You have defined {userDefinedExecuteMethods.Length} Execute() method(s) in {jobEntityTypeName}. "
                + "Please define exactly one Execute() method in each IJobEntity type. "
                + $"List of perpetrators: {userDefinedExecuteMethods.Select(method => method.ToString()).SeparateByCommaAndSpace()}. "
                //+$"With Attributes: {userDefinedExecuteMethods.Select(m => m.GetAttributes().Select(a => a.AttributeClass.ToFullName()).SeparateByComma()).SeparateByComma()}"
                , location);
        }

        public static void SGJE0009(ISourceGeneratorDiagnosable context, Location location, string jobEntityTypeName, string symbolName)
        {
            context.LogError(
                nameof(SGJE0009),
                k_ErrorTitle,
                $"{jobEntityTypeName} contains non-value type field {symbolName}.",
                location);
        }

        public static void SGJE0010(ISourceGeneratorDiagnosable context, Location location, string parameter, string parameterType)
        {
            context.LogError(
                nameof(SGJE0010),
                k_ErrorTitle,
                $"IJobEntity.Execute() parameter '{parameter}' of type {parameterType} is not supported.",
                location);
        }

        public static void SGJE0011(ISourceGeneratorDiagnosable diagnosable, Location location, string notValidParam)
        {
            diagnosable.LogError(
                nameof(SGJE0011),
                k_ErrorTitle,
                $"Execute() parameter '{notValidParam}' is not a supported parameter in an IJobEntity type.",
                location);
        }

        public static void SGJE0012(ISourceGeneratorDiagnosable diagnosable, Location location, string parameterType)
        {
            diagnosable.LogError(
                nameof(SGJE0012),
                k_ErrorTitle,
                $"{parameterType} implements IBufferElementData and must be used as DynamicBuffer<{parameterType}>.",
                location);
        }

        public static void SGJE0013(ISourceGeneratorDiagnosable diagnosable, Location location, string name, string parameterType)
        {
            diagnosable.LogError(
                nameof(SGJE0013),
                k_ErrorTitle,
                $"{name} has a shared component, {parameterType}, that uses the Ref keyword, which is not safe for shared components, instead pass by value or use the 'in' keyword.",
                location);
        }

        public static void SGJE0016(ISourceGeneratorDiagnosable diagnosable, Location location, string name, string parameterType)
        {
            diagnosable.LogError(
                nameof(SGJE0016),
                k_ErrorTitle,
                $"`{name}` has an empty component of type `{parameterType}`. Empty components must not be used with the `ref` keyword, nor with `RefRW<T>`, `RefRO<T>`, `EnabledRefRW<T>` and `EnabledRefRO<T>`.",
                location);
        }

        public static void SGJE0017(ISourceGeneratorDiagnosable diagnosable, Location location, string jobName, string typeName)
        {
            diagnosable.LogError(
                nameof(SGJE0017),
                k_ErrorTitle,
                $"{jobName} has duplicate components of same type {typeName}. Remove all but one to fix.",
                location);
        }

        public static void SGJE0018(ISourceGeneratorDiagnosable diagnosable, Location location)
        {
            diagnosable.LogError(
                nameof(SGJE0018),
                k_ErrorTitle,
                "`RefRW<T>`, `RefRO<T>`, `EnabledRefRW<T>`, and `EnabledRefRO<T>` may not be used with `ref` or `in`.",
                location);
        }

        public static void SGJE0019(ISourceGeneratorDiagnosable diagnosable, Location location, string typeFullName)
        {
            diagnosable.LogError(
                nameof(SGJE0019),
                k_ErrorTitle,
                $"`{typeFullName}` does not implement `IComponentData`, and thus cannot be used with `EnabledRefRW<T>` or `EnabledRefRO<T>`.",
                location);
        }
    }
}
