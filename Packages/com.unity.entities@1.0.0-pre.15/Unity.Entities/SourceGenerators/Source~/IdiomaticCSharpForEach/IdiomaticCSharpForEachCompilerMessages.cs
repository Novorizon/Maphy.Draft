﻿using Microsoft.CodeAnalysis;
using Unity.Entities.SourceGen.SystemGeneratorCommon;

namespace Unity.Entities.SourceGen.IdiomaticCSharpForEach
{
    public static class IdiomaticCSharpForEachCompilerMessages
    {
        private const string ErrorTitle = "ForEachIterationError";
        private const string InfoTitle = "ForEachIterationInfo";

        public static void SGFE001(SystemDescription systemDescription, Location errorLocation)
        {
            systemDescription.LogError(
                nameof(SGFE001),
                ErrorTitle,
                "Invocations of `SystemAPI.Query<T>() are currently supported only if they take place inside `foreach` statements.",
                errorLocation);
        }

        public static void SGFE002(SystemDescription systemDescription, string aspectTypeFullName, string propertyAccessorFullName, Location errorLocation)
        {
            systemDescription.LogError(
                nameof(SGFE002),
                ErrorTitle,
                $"You attempted to iterate through Query<{aspectTypeFullName}>() in {propertyAccessorFullName}. " +
                "Idiomatic `foreach` iteration through queries is only allowed inside methods with a `ref SystemState` parameter in `ISystem` types, OR in all methods in `SystemBase` types.",
                errorLocation);
        }

        public static void SGFE003(SystemDescription systemDescription, int numChangeFilterTypes, Location errorLocation)
        {
            systemDescription.LogError(
                nameof(SGFE003),
                ErrorTitle,
                $"You specified {numChangeFilterTypes} change filter types in the same `SystemAPI.Query<T>()` invocation. This is not allowed. " +
                "You may specify at most 2 shared component filter types for each `SystemAPI.Query<T>()` invocation.",
                errorLocation);
        }

        public static void SGFE007(SystemDescription systemDescription, int numSharedComponentFilters, Location errorLocation)
        {
            systemDescription.LogError(
                nameof(SGFE007),
                ErrorTitle,
                $"You specified {numSharedComponentFilters} shared component filters in the same `SystemAPI.Query<T>()` invocation. This is not allowed. " +
                "You may specify at most 2 shared component filter types for each `SystemAPI.Query<T>()` invocation.",
                errorLocation);
        }

        public static void SGFE008(SystemDescription systemDescription, int numInvocations, Location errorLocation)
        {
            systemDescription.LogError(
                nameof(SGFE008),
                ErrorTitle,
                $"You invoked `.WithOptions(EntityQueryOptions options)` {numInvocations} times in the same `SystemAPI.Query<T>()` invocation. This is not allowed. " +
                "Subsequent calls will override previous options, rather than adding to them. Use the bitwise OR operator '|' to combine multiple options.",
                errorLocation);
        }

        // Report this as an `Info` message rather than a `Warning` message. If the user's `.rsp` file specifies that all warnings should be treated as errors,
        // they won't be able to write SystemAPI.Query<ValueType>()` if we report this as a `Warning`.
        public static void SGFI009(SystemDescription systemDescription, string valueTypeComponentFullName, Location errorLocation)
        {
            systemDescription.LogInfo(
                nameof(SGFI009),
                InfoTitle,
                $"`{valueTypeComponentFullName}` is a value-type and is passed by value. Any modifications made to its data will not be persisted. " +
                $"If you wish to modify the data of `{valueTypeComponentFullName}`, use `RefRW<{valueTypeComponentFullName}>()` instead.",
                errorLocation);
        }
    }
}
