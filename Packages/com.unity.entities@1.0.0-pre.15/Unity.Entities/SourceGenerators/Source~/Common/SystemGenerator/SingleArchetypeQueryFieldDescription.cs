using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Unity.Entities.SourceGen.Common;

namespace Unity.Entities.SourceGen.SystemGeneratorCommon
{
    public readonly struct SingleArchetypeQueryFieldDescription : IEquatable<SingleArchetypeQueryFieldDescription>, IQueryFieldDescription
    {
        readonly Archetype _archetype;
        readonly IReadOnlyCollection<Query> _changeFilterTypes;
        readonly string _queryStorageFieldName;

        public FieldDeclarationSyntax GetFieldDeclarationSyntax(string generatedQueryFieldName)
            => (FieldDeclarationSyntax)SyntaxFactory.ParseMemberDeclaration($"Unity.Entities.EntityQuery {generatedQueryFieldName};");

        public SingleArchetypeQueryFieldDescription(
            Archetype archetype,
            IReadOnlyCollection<Query> changeFilterTypes = null,
            string queryStorageFieldName = null)
        {
            _archetype = archetype;
            _changeFilterTypes = changeFilterTypes ?? Array.Empty<Query>();
            _queryStorageFieldName = queryStorageFieldName;
        }

        public bool Equals(SingleArchetypeQueryFieldDescription other)
        {
            return _archetype.Equals(other._archetype)
                   && _changeFilterTypes.SequenceEqual(other._changeFilterTypes)
                   && _queryStorageFieldName == other._queryStorageFieldName;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (obj.GetType() != GetType())
                return false;
            return Equals((SingleArchetypeQueryFieldDescription)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 19;
                hash = hash * 31 + _archetype.GetHashCode();

                foreach (var changeFilterType in _changeFilterTypes)
                    hash = hash * 31 + changeFilterType.GetHashCode();

                return hash;
            }
        }
        public static bool operator ==(SingleArchetypeQueryFieldDescription left, SingleArchetypeQueryFieldDescription right) => Equals(left, right);
        public static bool operator !=(SingleArchetypeQueryFieldDescription left, SingleArchetypeQueryFieldDescription right) => !Equals(left, right);

        public string EntityQueryFieldAssignment(string systemStateName, string generatedQueryFieldName)
        {
            var entityQuerySetup =
                $@"{generatedQueryFieldName} = {systemStateName}.GetEntityQuery
                    (
                        new Unity.Entities.EntityQueryDesc
                        {{
                            All = {DistinctQueryTypesFor()},
                            Any = new Unity.Entities.ComponentType[] {{
                                {_archetype.Any.Select(q => q.ToString()).Distinct().SeparateByCommaAndNewLine()}
                            }},
                            None = new Unity.Entities.ComponentType[] {{
                                {_archetype.None.Select(q => q.ToString()).Distinct().SeparateByCommaAndNewLine()}
                            }},
                            Options =
                                {_archetype.Options.GetFlags().Select(flag => $"Unity.Entities.EntityQueryOptions.{flag.ToString()}").SeparateByBinaryOr()}
                        }}
                    );";

            if (_queryStorageFieldName != null)
                entityQuerySetup = $"{_queryStorageFieldName} = " + entityQuerySetup;

            if (_changeFilterTypes.Any())
            {
                entityQuerySetup +=
                    $@"{generatedQueryFieldName}.SetChangedVersionFilter(new ComponentType[{_changeFilterTypes.Count}]
				    {{
                        {_changeFilterTypes.Select(q => q.ToString()).SeparateByComma()}
                    }});";
            }

            return entityQuerySetup;
        }

        string DistinctQueryTypesFor()
        {
            var readOnlyTypeNames = new HashSet<string>();
            var readWriteTypeNames = new HashSet<string>();
            var readOnlyAspectTypeNames = new HashSet<string>();
            var readWriteAspectTypeNames = new HashSet<string>();

            int componentCount = 0;
            int aspectCount = 0;

            void AddQueryType(ITypeSymbol queryType, bool isReadOnly)
            {
                if (queryType == null)
                    return;

                var queryTypeFullName = queryType.ToFullName();
                if (queryType.IsAspect())
                {
                    ++aspectCount;
                    if (!isReadOnly)
                    {
                        readOnlyAspectTypeNames.Remove(queryTypeFullName);
                        readWriteAspectTypeNames.Add(queryTypeFullName);
                    }
                    else
                    {
                        if (!readWriteAspectTypeNames.Contains(queryTypeFullName) &&
                            !readOnlyAspectTypeNames.Contains(queryTypeFullName))
                        {
                            readOnlyAspectTypeNames.Add(queryTypeFullName);
                        }
                    }
                }
                else
                {
                    ++componentCount;
                    if (!isReadOnly)
                    {
                        readOnlyTypeNames.Remove(queryTypeFullName);
                        readWriteTypeNames.Add(queryTypeFullName);
                    }
                    else
                    {
                        if (!readWriteTypeNames.Contains(queryTypeFullName) &&
                            !readOnlyTypeNames.Contains(queryTypeFullName))
                        {
                            readOnlyTypeNames.Add(queryTypeFullName);
                        }
                    }
                }
            }

            foreach (var allComponentType in _archetype.All)
                AddQueryType(allComponentType.TypeSymbol, allComponentType.IsReadOnly);

            foreach (var changeFilterType in _changeFilterTypes)
                AddQueryType(changeFilterType.TypeSymbol, changeFilterType.IsReadOnly);

            if (componentCount == 0 && aspectCount == 0)
            {
                return "new Unity.Entities.ComponentType[]{}";
            }

            var strBuilder = new System.Text.StringBuilder();
            bool needComma = false;

            // need combine if we have any of these conditions:
            //   * 2 or more aspects
            //   * 1 or more components and 1 or more aspects
            bool needCombine = (aspectCount >= 2)
                               || (componentCount >= 1 && aspectCount >= 1);
            if (needCombine)
            {
                strBuilder.Append("ComponentType.Combine(");
            }

            // Output Components
            if (componentCount > 0)
            {
                var eComponents = readOnlyTypeNames
                    .Select(type => $@"Unity.Entities.ComponentType.ReadOnly<{type}>()")
                    .Concat(readWriteTypeNames.Select(type => $@"Unity.Entities.ComponentType.ReadWrite<{type}>()"));
                strBuilder.Append($"new Unity.Entities.ComponentType[] {{{eComponents.Distinct().SeparateByCommaAndNewLine()}}}");
                needComma = true;
            }

            // Output Aspects
            if (aspectCount > 0)
            {
                if (needComma)
                {
                    strBuilder.Append(",");
                }
                var eAspects = readOnlyAspectTypeNames
                    .Select(type => $"{type}.RequiredComponentsRO")
                    .Concat(readWriteAspectTypeNames.Select(type => $"{type}.RequiredComponents"));
                strBuilder.Append(eAspects.Distinct().SeparateByCommaAndNewLine());
            }

            if (needCombine)
            {
                strBuilder.Append(")");
            }

            return strBuilder.ToString();
        }
    }
}
