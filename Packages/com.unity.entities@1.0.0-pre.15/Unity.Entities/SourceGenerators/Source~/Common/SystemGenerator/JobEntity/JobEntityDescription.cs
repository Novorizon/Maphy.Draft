using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Unity.Entities.SourceGen.Common;

namespace Unity.Entities.SourceGen.SystemGeneratorCommon
{
    public partial class JobEntityDescription
    {
        readonly INamedTypeSymbol m_JobEntityTypeSymbol;
        public string FullTypeName => m_JobEntityTypeSymbol.ToFullName();
        string TypeName => m_JobEntityTypeSymbol.Name;

        public bool Invalid { get; private set; }
        public readonly JobEntityParam[] UserExecuteMethodParams;

        public readonly List<Query> QueryAllTypes = new List<Query>();
        public readonly List<Query> QueryAnyTypes = new List<Query>();
        public readonly List<Query> QueryNoneTypes = new List<Query>();
        public readonly List<Query> QueryChangeFilterTypes = new List<Query>();
        public EntityQueryOptions EntityQueryOptions = EntityQueryOptions.Default;

        readonly string m_UserExecuteMethodSignature;

        public bool HasEntityIndexInQuery() => UserExecuteMethodParams.Any(e => e is JobEntityParam_EntityIndexInQuery);
        public bool HasManagedComponents() => UserExecuteMethodParams.Any(e => e is JobEntityParam_ManagedComponent);

        public bool RequiresEntityManagerAccess => UserExecuteMethodParams.OfType<IRequireEntityManager>().Any(p => p.RequiresEntityManagerAccess);

        public JobEntityDescription(BaseTypeDeclarationSyntax candidate, SemanticModel semanticModel, ISourceGeneratorDiagnosable diagnosable) :
            this(semanticModel.GetDeclaredSymbol(candidate), diagnosable) {}

        public JobEntityDescription(INamedTypeSymbol jobEntityType, ISourceGeneratorDiagnosable diagnosable)
        {
            Invalid = false;
            m_JobEntityTypeSymbol = jobEntityType;

            // Find valid user made execute method
            var executeMethods = m_JobEntityTypeSymbol.GetMembers().OfType<IMethodSymbol>().Where(m => m.Name == "Execute").ToArray();
            var userExecuteMethods = executeMethods.Where(m => !m.HasAttribute("System.Runtime.CompilerServices.CompilerGeneratedAttribute")).ToArray();
            if (userExecuteMethods.Length != 1)
            {
                JobEntityGeneratorErrors.SGJE0008(diagnosable, m_JobEntityTypeSymbol.Locations.First(), FullTypeName, userExecuteMethods);
                Invalid = true;
                return;
            }

            // Generate JobEntityParams
            var userExecuteMethod = userExecuteMethods[0];
            UserExecuteMethodParams = userExecuteMethod.Parameters.Select(p =>
            {
                var param = JobEntityParam.Create(p, diagnosable, out var invalid);
                Invalid |= invalid;
                return param;
            }).ToArray();

            // Setup rest
            FillQueryInfoFromAttributes();
            m_UserExecuteMethodSignature = $"{userExecuteMethod.Name}({userExecuteMethod.Parameters.Select(p => $"{p.Type.Name} {p.Name}").SeparateByComma().TrimEnd('\n')})";

            // Do remaining checks to see if this description is valid
            Invalid |= OutputErrors(diagnosable);
        }

        /// <summary>
        /// Fills <see cref="QueryAllTypes"/>, <see cref="QueryAnyTypes"/>,
        /// <see cref="QueryNoneTypes"/>, <see cref="QueryChangeFilterTypes"/>
        /// and <see cref="EntityQueryOptions"/> using C# attributes found on the JobEntity struct.
        /// </summary>
        void FillQueryInfoFromAttributes()
        {
            var attributes = m_JobEntityTypeSymbol.GetAttributes();

            foreach (var attribute in attributes)
            {
                switch (attribute.AttributeClass.ToFullName())
                {
                    case "Unity.Entities.WithAllAttribute":
                        foreach (var argument in attribute.ConstructorArguments)
                            switch (argument.Kind)
                            {
                                case TypedConstantKind.Array:
                                    foreach (var value in argument.Values)
                                    {
                                        QueryAllTypes.Add(new Query
                                        {
                                            TypeSymbol = (ITypeSymbol)value.Value,
                                            Type = QueryType.All,
                                            IsReadOnly = true
                                        });
                                    }
                                    break;
                                case TypedConstantKind.Type:
                                    QueryAllTypes.Add(new Query
                                    {
                                        TypeSymbol = (ITypeSymbol)argument.Value,
                                        Type = QueryType.All,
                                        IsReadOnly = true
                                    });
                                    break;
                            }
                        break;
                    case "Unity.Entities.WithNoneAttribute":
                        foreach (var argument in attribute.ConstructorArguments)
                            switch (argument.Kind)
                            {
                                case TypedConstantKind.Array:
                                    foreach (var value in argument.Values)
                                    {
                                        QueryNoneTypes.Add(new Query
                                        {
                                            TypeSymbol = (ITypeSymbol)value.Value,
                                            Type = QueryType.None,
                                            IsReadOnly = true
                                        });
                                    }
                                    break;
                                case TypedConstantKind.Type:
                                    QueryNoneTypes.Add(new Query
                                    {
                                        TypeSymbol = (ITypeSymbol)argument.Value,
                                        Type = QueryType.None,
                                        IsReadOnly = true
                                    });
                                    break;
                            }
                        break;

                    case "Unity.Entities.WithAnyAttribute":
                        foreach (var argument in attribute.ConstructorArguments)
                            switch (argument.Kind)
                            {
                                case TypedConstantKind.Array:
                                    foreach (var value in argument.Values)
                                    {
                                        QueryAnyTypes.Add(new Query
                                        {
                                            TypeSymbol = (ITypeSymbol)value.Value,
                                            Type = QueryType.Any,
                                            IsReadOnly = true
                                        });
                                    }
                                    break;
                                case TypedConstantKind.Type:
                                    QueryAnyTypes.Add(new Query
                                    {
                                        TypeSymbol = (ITypeSymbol)argument.Value,
                                        Type = QueryType.Any,
                                        IsReadOnly = true
                                    });
                                    break;
                            }
                        break;

                    case "Unity.Entities.WithChangeFilterAttribute":
                        foreach (var argument in attribute.ConstructorArguments)
                            switch (argument.Kind)
                            {
                                case TypedConstantKind.Array:
                                    foreach (var value in argument.Values)
                                    {
                                        QueryChangeFilterTypes.Add(new Query
                                        {
                                            TypeSymbol = (ITypeSymbol)value.Value,
                                            Type = QueryType.ChangeFilter,
                                            IsReadOnly = true
                                        });
                                    }
                                    break;
                                case TypedConstantKind.Type:
                                    QueryChangeFilterTypes.Add(new Query
                                    {
                                        TypeSymbol = (ITypeSymbol)argument.Value,
                                        Type = QueryType.ChangeFilter,
                                        IsReadOnly = true
                                    });
                                    break;
                            }
                        break;

                    case "Unity.Entities.WithOptionsAttribute":
                        var firstArgument = attribute.ConstructorArguments[0];
                        if (firstArgument.Kind == TypedConstantKind.Array)
                            foreach (var entityQueryOptions in firstArgument.Values)
                                EntityQueryOptions |= (EntityQueryOptions) (int) entityQueryOptions.Value;
                        else
                            EntityQueryOptions |= (EntityQueryOptions) (int) firstArgument.Value;
                        break;
                }
            }

            QueryChangeFilterTypes.AddRange(
                UserExecuteMethodParams.OfType<IHasChangeFilter>()
                .Where(param => param.HasChangeFilter)
                .Select(param => new Query
                {
                    TypeSymbol = ((JobEntityParam)param).TypeSymbol,
                    Type = QueryType.ChangeFilter,
                    IsReadOnly = true
                }));
        }

        /// <summary>
        /// Checks if any errors are present, if so, also outputs the error.
        /// </summary>
        /// <param name="systemTypeGeneratorContext"></param>
        /// <returns>True if error found</returns>
        bool OutputErrors(ISourceGeneratorDiagnosable systemTypeGeneratorContext)
        {
            var invalid = false;
            var foundChunkIndexInQuery = false;
            var foundEntityIndexInQuery = false;
            var foundEntityIndexInChunk = false;

            var alreadySeenComponents = new HashSet<ITypeSymbol>();

            foreach (var param in UserExecuteMethodParams)
            {
                if (param is null)
                    continue;

                if (!(param is IAttributeParameter { IsInt: true }))
                    if (!alreadySeenComponents.Add(param.TypeSymbol))
                        JobEntityGeneratorErrors.SGJE0017(systemTypeGeneratorContext, param.ParameterSymbol.Locations.First(), FullTypeName, param.TypeSymbol.ToDisplayString());

                switch (param)
                {
                    case IAttributeParameter attributeParameter:
                    {
                        if (attributeParameter.IsInt) {
                            switch (attributeParameter)
                            {
                                case JobEntityParam_ChunkIndexInQuery _ when !foundChunkIndexInQuery:
                                    foundChunkIndexInQuery = true;
                                    continue;
                                case JobEntityParam_EntityIndexInQuery _ when !foundEntityIndexInQuery:
                                    foundEntityIndexInQuery = true;
                                    continue;
                                case JobEntityParam_EntityIndexInChunk _ when !foundEntityIndexInChunk:
                                    foundEntityIndexInChunk = true;
                                    continue;
                            }

                            JobEntityGeneratorErrors.SGJE0007(systemTypeGeneratorContext, param.ParameterSymbol.Locations.Single(),
                                FullTypeName, m_UserExecuteMethodSignature, attributeParameter.AttributeName);
                            invalid = true;
                            continue;
                        }

                        JobEntityGeneratorErrors.SGJE0006(systemTypeGeneratorContext, param.ParameterSymbol.Locations.Single(),
                            FullTypeName, m_UserExecuteMethodSignature, param.ParameterSymbol.Name, attributeParameter.AttributeName);
                        invalid = true;
                        continue;
                    }
                    case JobEntityParam_SharedComponent sharedComponent:
                        if (sharedComponent.ParameterSymbol.RefKind == RefKind.Ref)
                        {
                            var text = sharedComponent.ParameterSymbol.DeclaringSyntaxReferences.First().GetSyntax() is ParameterSyntax {Identifier: var i}
                                ? i.ValueText
                                : sharedComponent.ParameterSymbol.ToDisplayString();
                                JobEntityGeneratorErrors.SGJE0013(systemTypeGeneratorContext, sharedComponent.ParameterSymbol.Locations.Single(), FullTypeName, text);
                            invalid = true;
                        }
                        break;
                    case JobEntityParam_TagComponent tag:
                        switch (tag.RefWrapperType)
                        {
                            case RefWrapperType.None:
                                if (tag.ParameterSymbol.RefKind == RefKind.Ref)
                                {
                                    JobEntityGeneratorErrors.SGJE0016(systemTypeGeneratorContext, tag.ParameterSymbol.Locations.Single(), FullTypeName, tag.ParameterSymbol.ToDisplayString());
                                    invalid = true;
                                }
                                break;
                            default:
                                JobEntityGeneratorErrors.SGJE0016(systemTypeGeneratorContext, tag.ParameterSymbol.Locations.Single(), FullTypeName, tag.ParameterSymbol.ToDisplayString());
                                invalid = true;
                                break;
                        }
                        break;
                    case JobEntityParam_ComponentData componentData when componentData.RefWrapperType != RefWrapperType.None
                                                                         && componentData.ParameterSymbol.RefKind != RefKind.None:
                    {
                        JobEntityGeneratorErrors.SGJE0018(systemTypeGeneratorContext, componentData.ParameterSymbol.Locations.Single());
                        break;
                    }
                }
            }

            var refFieldSymbols = m_JobEntityTypeSymbol.GetMembers().OfType<IFieldSymbol>().Where(m => m.Type.IsReferenceType).ToArray();
            foreach (var symbol in refFieldSymbols)
            {
                JobEntityGeneratorErrors.SGJE0009(systemTypeGeneratorContext, refFieldSymbols[0].Locations.FirstOrDefault(), FullTypeName, symbol.Name);
                invalid = true;
            }
            return invalid;
        }
    }

    public class JobEntityParam_SharedComponent : JobEntityParam, IHasChangeFilter, IRequireEntityManager
    {
        internal JobEntityParam_SharedComponent(IParameterSymbol parameterSymbol, bool hasChangeFilter) : base(parameterSymbol)
        {
            var typeName = TypeSymbol.Name;
            HasChangeFilter = hasChangeFilter;

            TypeHandleFieldDeclaration = $"{(IsReadOnly ? "[Unity.Collections.ReadOnly]" : "")}public Unity.Entities.SharedComponentTypeHandle<{FullyQualifiedTypeName}> {TypeHandleFieldName};";

            RequiresEntityManagerAccess = !TypeSymbol.IsUnmanagedType;
            var variableName = $"{char.ToLowerInvariant(typeName[0])}{typeName.Substring(1)}Data";
            VariableDeclarationAtStartOfExecuteMethod = TypeSymbol.IsUnmanagedType
                ? $"var {variableName} = chunk.GetSharedComponent({TypeHandleFieldName});"
                : $"var {variableName} = chunk.GetSharedComponentManaged({TypeHandleFieldName}, __EntityManager);";

            ExecuteMethodArgumentValue = parameterSymbol.RefKind == RefKind.In ? $"in {variableName}" : variableName;
        }

        public bool HasChangeFilter { get; }
        public bool RequiresEntityManagerAccess { get; set; }
    }

    public class JobEntityParam_Entity : JobEntityParam
    {
        internal JobEntityParam_Entity(IParameterSymbol parameterSymbol) : base(parameterSymbol)
        {
            TypeHandleFieldDeclaration = $"[Unity.Collections.ReadOnly] public Unity.Entities.EntityTypeHandle {TypeHandleFieldName};";

            const string entityArrayPointer = "entityPointer";
            VariableDeclarationAtStartOfExecuteMethod = $@"var {entityArrayPointer} = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, {TypeHandleFieldName});";

            const string argumentInExecuteMethod = "entity";
            ExecuteMethodArgumentSetup = $"var {argumentInExecuteMethod} = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>({entityArrayPointer}, entityIndexInChunk);";

            ExecuteMethodArgumentValue = argumentInExecuteMethod;
            IsQueryableType = false;
        }
    }

    public class JobEntityParam_DynamicBuffer : JobEntityParam
    {
        internal JobEntityParam_DynamicBuffer(IParameterSymbol parameterSymbol, ITypeSymbol typeArgSymbol) : base(parameterSymbol)
        {
            var fullyQualifiedTypeName = typeArgSymbol.GetSymbolTypeName();

            TypeSymbol = typeArgSymbol;
            TypeHandleFieldName = $"__{fullyQualifiedTypeName.Replace('.', '_')}BufferTypeHandle";
            TypeHandleFieldDeclaration = $"public Unity.Entities.BufferTypeHandle<{fullyQualifiedTypeName}> {TypeHandleFieldName};";

            var bufferAccessorVariableName = $"{parameterSymbol.Name}BufferAccessor";
            VariableDeclarationAtStartOfExecuteMethod = $"var {bufferAccessorVariableName} = chunk.GetBufferAccessor(ref {TypeHandleFieldName});";

            var executeArgumentName = $"retrievedByIndexIn{bufferAccessorVariableName}";
            ExecuteMethodArgumentSetup = $"var {executeArgumentName} = {bufferAccessorVariableName}[entityIndexInChunk];";

            ExecuteMethodArgumentValue = parameterSymbol.RefKind switch
            {
                RefKind.Ref => $"ref {executeArgumentName}",
                RefKind.In => $"in {executeArgumentName}",
                _ => executeArgumentName
            };
        }
    }

    public class JobEntityParam_Aspect : JobEntityParam
    {
        internal JobEntityParam_Aspect(IParameterSymbol parameterSymbol) : base(parameterSymbol)
        {
            // Field init
            TypeHandleFieldDeclaration = $"{"[Unity.Collections.ReadOnly] ".EmitIfTrue(IsReadOnly)}public {FullyQualifiedTypeName}.TypeHandle {TypeHandleFieldName};";

            // Per chunk
            var variableName = $"{TypeHandleFieldName}Array";
            VariableDeclarationAtStartOfExecuteMethod = $"var {variableName} = {TypeHandleFieldName}.Resolve(chunk);";

            // Per entity
            var executeMethodArgument = $"{variableName}Array";
            ExecuteMethodArgumentSetup = $"var {executeMethodArgument} = {variableName}[entityIndexInChunk];";

            ExecuteMethodArgumentValue = parameterSymbol.RefKind switch
            {
                RefKind.Ref => $"ref {executeMethodArgument}",
                RefKind.In => $"in {executeMethodArgument}",
                _ => executeMethodArgument
            };
        }
    }

    public class JobEntityParam_ManagedComponent : JobEntityParam, IHasChangeFilter, IRequireEntityManager
    {
        public bool IsUnityEngineComponent;
        internal JobEntityParam_ManagedComponent(IParameterSymbol parameterSymbol, bool hasChangeFilter, bool isUnityEngineComponent) : base(parameterSymbol)
        {
            IsUnityEngineComponent = isUnityEngineComponent;

            TypeHandleFieldDeclaration = $"{(IsReadOnly ? "[Unity.Collections.ReadOnly]" : "")}public Unity.Entities.ComponentTypeHandle<{FullyQualifiedTypeName}> {TypeHandleFieldName};";
            TypeHandleFieldAssignment = $"EntityManager.GetComponentTypeHandle<{FullyQualifiedTypeName}>({(IsReadOnly ? "true" : "false")})";

            HasChangeFilter = hasChangeFilter;
            RequiresTypeHandleFieldInSystemBase = false;
            RequiresEntityManagerAccess = true;

            var accessorVariableName = $"{parameterSymbol.Name}ManagedComponentAccessor";
            VariableDeclarationAtStartOfExecuteMethod = $"var {accessorVariableName} = chunk.GetManagedComponentAccessor(ref {TypeHandleFieldName}, __EntityManager);";

            var localName = ExecuteMethodArgumentValue = $"retrievedByIndexIn{accessorVariableName}";
            ExecuteMethodArgumentSetup = $"var {localName} = {accessorVariableName}[entityIndexInChunk];";

            // We do not allow managed components to be used by ref. See SourceGenerationErrors.DC0024.
            if(parameterSymbol.RefKind == RefKind.In)
                ExecuteMethodArgumentValue = $"in {localName}";
        }

        public bool HasChangeFilter { get;}
        public bool RequiresEntityManagerAccess { get; set; }
    }

    public class JobEntityParam_ComponentData : JobEntityParam, IHasChangeFilter
    {
        internal JobEntityParam_ComponentData(IParameterSymbol parameterSymbol, ITypeSymbol componentTypeSymbol, bool hasChangeFilter, bool isReadOnly, RefWrapperType refWrapperType)
            : base(parameterSymbol)
        {
            TypeSymbol = componentTypeSymbol;
            HasChangeFilter = hasChangeFilter;
            IsReadOnly = isReadOnly;
            RefWrapperType = refWrapperType;

            var typeName = componentTypeSymbol.Name;
            var fullyQualifiedTypeName = componentTypeSymbol.ToFullName();

            TypeHandleFieldName = $"__{fullyQualifiedTypeName.Replace('.', '_')}ComponentTypeHandle";
            TypeHandleFieldDeclaration = $"{(IsReadOnly ? "[Unity.Collections.ReadOnly]" : "")}public Unity.Entities.ComponentTypeHandle<{fullyQualifiedTypeName}> {TypeHandleFieldName};";
            TypeHandleFieldAssignment = $"EntityManager.GetComponentTypeHandle<{fullyQualifiedTypeName}>({(IsReadOnly ? "true" : "false")})";

            bool isEnabledRef = RefWrapperType == RefWrapperType.EnabledRefRO || RefWrapperType == RefWrapperType.EnabledRefRW;
            if (isEnabledRef)
            {
                var enabledMaskVariableName = $"{parameterSymbol.Name}EnabledMask";
                VariableDeclarationAtStartOfExecuteMethod = $"var {enabledMaskVariableName} = chunk.GetEnabledMask(ref {TypeHandleFieldName});";

                var getEnabledRefFromMaskMethodName = IsReadOnly ? "GetEnabledRefRO" : "GetEnabledRefRW";
                ExecuteMethodArgumentValue = $"{enabledMaskVariableName}.{getEnabledRefFromMaskMethodName}<{fullyQualifiedTypeName}>(entityIndexInChunk)";
            }
            else
            {
                var arrayVariableName = $"{char.ToLowerInvariant(typeName[0])}{typeName.Substring(1)}Array";
                VariableDeclarationAtStartOfExecuteMethod = GetNativeArrayVariableAssignment();

                var executeArgument = $"{arrayVariableName}Ref";

                switch (RefWrapperType)
                {
                    case RefWrapperType.None:
                        ExecuteMethodArgumentSetup = $"ref var {executeArgument} = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<{fullyQualifiedTypeName}>({arrayVariableName}, entityIndexInChunk);";
                        ExecuteMethodArgumentValue = parameterSymbol.RefKind switch
                        {
                            RefKind.Ref => $"ref {executeArgument}",
                            RefKind.In => $"in {executeArgument}",
                            _ => executeArgument
                        };
                        break;
                    case RefWrapperType.RefRO:
                        ExecuteMethodArgumentSetup = $@"var {executeArgument} = new RefRO<{fullyQualifiedTypeName}>({arrayVariableName}, entityIndexInChunk);";
                        ExecuteMethodArgumentValue = executeArgument;
                        break;
                    case RefWrapperType.RefRW:
                        ExecuteMethodArgumentSetup = $@"var {executeArgument} = new RefRW<{fullyQualifiedTypeName}>({arrayVariableName}, entityIndexInChunk);";
                        ExecuteMethodArgumentValue = executeArgument;
                        break;
                }

                string GetNativeArrayVariableAssignment()
                {
                    switch (RefWrapperType)
                    {
                        case RefWrapperType.None:
                            return
                                IsReadOnly
                                    ? $"var {arrayVariableName} = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<{FullyQualifiedTypeName}>(chunk, ref {TypeHandleFieldName});"
                                    : $"var {arrayVariableName} = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<{FullyQualifiedTypeName}>(chunk, ref {TypeHandleFieldName});";
                        default:
                            return $"var {arrayVariableName} = chunk.GetNativeArray(ref {TypeHandleFieldName});";
                    }
                }
            }
        }

        public bool HasChangeFilter { get; }
        public RefWrapperType RefWrapperType { get; set; }
    }

    class JobEntityParam_TagComponent : JobEntityParam, IHasChangeFilter
    {
        public bool HasChangeFilter { get; }
        public RefWrapperType RefWrapperType { get; }

        internal JobEntityParam_TagComponent(
            IParameterSymbol parameterSymbol,
            bool hasChangeFilter,
            RefWrapperType refWrapperType)
            : base(parameterSymbol)
        {
            HasChangeFilter = hasChangeFilter;
            RequiresTypeHandleFieldInSystemBase = false;
            ExecuteMethodArgumentValue = "default";
            RefWrapperType = refWrapperType;
            IsReadOnly = true;
        }
    }

    public interface IAttributeParameter
    {
        public bool IsInt { get; }
        public string AttributeName { get; }
    }

    class JobEntityParamValueTypesPassedWithDefaultArguments : JobEntityParam
    {
        internal JobEntityParamValueTypesPassedWithDefaultArguments(IParameterSymbol parameterSymbol) : base(parameterSymbol)
        {
            RequiresTypeHandleFieldInSystemBase = false;
            ExecuteMethodArgumentValue = "default";
        }
    }

    class JobEntityParam_EntityIndexInQuery : JobEntityParam, IAttributeParameter
    {
        public bool IsInt => TypeSymbol.IsInt();
        public string AttributeName => "EntityIndexInQuery";

        internal JobEntityParam_EntityIndexInQuery(IParameterSymbol parameterSymbol) : base(parameterSymbol)
        {
            RequiresTypeHandleFieldInSystemBase = false;
            ExecuteMethodArgumentSetup = " var entityIndexInQuery = __ChunkBaseEntityIndices[chunkIndexInQuery] + matchingEntityCount;";
            ExecuteMethodArgumentValue = "entityIndexInQuery";
            IsQueryableType = false;
        }
    }

    class JobEntityParam_ChunkIndexInQuery : JobEntityParam, IAttributeParameter
    {
        public bool IsInt => TypeSymbol.IsInt();
        public string AttributeName => "ChunkIndexInQuery";
        internal JobEntityParam_ChunkIndexInQuery(IParameterSymbol parameterSymbol) : base(parameterSymbol)
        {
            RequiresTypeHandleFieldInSystemBase = false;
            // TODO(DOTS-6130): an extra helper job is needed to provided the chunk index in query when the query has chunk filtering enabled.
            // For now this is an unfiltered chunk index.
            ExecuteMethodArgumentValue = "chunkIndexInQuery";
            IsQueryableType = false;
        }
    }

    class JobEntityParam_EntityIndexInChunk : JobEntityParam, IAttributeParameter
    {
        public bool IsInt => TypeSymbol.IsInt();
        public string AttributeName => "EntityIndexInChunk";
        internal JobEntityParam_EntityIndexInChunk(IParameterSymbol parameterSymbol) : base(parameterSymbol)
        {
            RequiresTypeHandleFieldInSystemBase = false;
            ExecuteMethodArgumentValue = "entityIndexInChunk";
            IsQueryableType = false;
        }
    }

    public interface IHasChangeFilter
    {
        public bool HasChangeFilter { get; }
    }

    interface IRequireEntityManager
    {
        bool RequiresEntityManagerAccess { get; set; }
    }

    public abstract class JobEntityParam
    {
        public bool RequiresExecuteMethodArgumentSetup => !string.IsNullOrEmpty(ExecuteMethodArgumentSetup);
        public string ExecuteMethodArgumentSetup { get; protected set; }
        public string ExecuteMethodArgumentValue { get; protected set; }

        public bool RequiresTypeHandleFieldInSystemBase { get; protected set; } = true;

        protected string FullyQualifiedTypeName { get; }
        public IParameterSymbol ParameterSymbol { get; }
        public ITypeSymbol TypeSymbol { get; protected set; }

        public string TypeHandleFieldName { get; protected set; }
        public string TypeHandleFieldAssignment { get; protected set; }
        public string TypeHandleFieldDeclaration { get; protected set; }

        public bool IsReadOnly { get; protected set; }
        public string VariableDeclarationAtStartOfExecuteMethod { get; protected set; }

        public bool IsQueryableType { get; protected set; } = true;

        static (bool Success, JobEntityParam JobEntityParameter) TryParseComponentTypeSymbol(
                ITypeSymbol componentTypeSymbol,
                IParameterSymbol parameterSymbol,
                bool hasChangeFilter,
                bool isReadOnly,
                ISourceGeneratorDiagnosable diagnosable,
                string constructedFrom = null)
        {
            var refWrapperType = constructedFrom switch
            {
                "Unity.Entities.RefRW<T>" => RefWrapperType.RefRW,
                "Unity.Entities.RefRO<T>" => RefWrapperType.RefRO,
                "Unity.Entities.EnabledRefRW<T>" => RefWrapperType.EnabledRefRW,
                "Unity.Entities.EnabledRefRO<T>" => RefWrapperType.EnabledRefRO,
                _ => RefWrapperType.None
            };

            if (componentTypeSymbol is INamedTypeSymbol namedTypeSymbol && namedTypeSymbol.Arity != 0)
            {
                JobEntityGeneratorErrors.SGJE0011(diagnosable, parameterSymbol.Locations.Single(),
                    parameterSymbol.Name);
                return (false, default);
            }

            if (componentTypeSymbol.GetMembers().OfType<IFieldSymbol>().Any())
                return (true, new JobEntityParam_ComponentData(parameterSymbol, componentTypeSymbol, hasChangeFilter, isReadOnly, refWrapperType));

            return (true, new JobEntityParam_TagComponent(parameterSymbol, hasChangeFilter, refWrapperType));
        }

        static bool? IsUnityEngineComponent(ITypeSymbol typeSymbol)
        {
            return
                typeSymbol.InheritsFromInterface("Unity.Entities.IComponentData")
                    ? false
                    : typeSymbol.InheritsFromType("UnityEngine.Component")
                      || typeSymbol.InheritsFromType("UnityEngine.GameObject")
                      || typeSymbol.InheritsFromType("UnityEngine.ScriptableObject")
                        ? (bool?)true
                        : null;
        }

        internal JobEntityParam(IParameterSymbol parameterSymbol)
        {
            ParameterSymbol = parameterSymbol;
            TypeSymbol = parameterSymbol.Type;
            FullyQualifiedTypeName = TypeSymbol.GetSymbolTypeName();
            TypeHandleFieldName = $"__{FullyQualifiedTypeName.Replace('.', '_')}TypeHandle";
            IsReadOnly = parameterSymbol.IsReadOnly();
        }

        public static JobEntityParam Create(IParameterSymbol parameterSymbol, ISourceGeneratorDiagnosable diagnosable,
            out bool invalid)
        {
            var typeSymbol = parameterSymbol.Type;
            invalid = false;

            var hasChangeFilter = false;
            foreach (var attribute in parameterSymbol.GetAttributes())
            {
                switch (attribute.AttributeClass.ToFullName())
                {
                    case "Unity.Entities.ChunkIndexInQuery":
                        return new JobEntityParam_ChunkIndexInQuery(parameterSymbol);
                    case "Unity.Entities.EntityIndexInChunk":
                        return new JobEntityParam_EntityIndexInChunk(parameterSymbol);
                    case "Unity.Entities.EntityIndexInQuery":
                        return new JobEntityParam_EntityIndexInQuery(parameterSymbol);
                    case "Unity.Entities.WithChangeFilterAttribute":
                        hasChangeFilter = true;
                        break;
                }
            }

            switch (typeSymbol)
            {
                case INamedTypeSymbol namedTypeSymbol:
                {
                    switch (namedTypeSymbol.Arity)
                    {
                        case 1:
                        {
                            var typeArgSymbol = namedTypeSymbol.TypeArguments.Single();
                            var constructedFromFullName = namedTypeSymbol.ConstructedFrom.ToFullName();

                            switch (constructedFromFullName)
                            {
                                case "Unity.Entities.DynamicBuffer<T>":
                                    return new JobEntityParam_DynamicBuffer(parameterSymbol, typeArgSymbol);
                                case "Unity.Entities.RefRW<T>":
                                case "Unity.Entities.RefRO<T>":
                                case "Unity.Entities.EnabledRefRW<T>":
                                case "Unity.Entities.EnabledRefRO<T>":
                                {
                                    if (typeArgSymbol.InheritsFromInterface("Unity.Entities.IComponentData"))
                                    {
                                        var (success, jobEntityParameter) =
                                            TryParseComponentTypeSymbol(
                                                typeArgSymbol,
                                                parameterSymbol,
                                                hasChangeFilter,
                                                isReadOnly: constructedFromFullName == "Unity.Entities.RefRO<T>" ||
                                                            constructedFromFullName == "Unity.Entities.EnabledRefRO<T>",
                                                diagnosable,
                                                constructedFromFullName);

                                        if (success)
                                            return jobEntityParameter;

                                        invalid = true;
                                        return null;
                                    }

                                    invalid = true;
                                    JobEntityGeneratorErrors.SGJE0019(diagnosable, parameterSymbol.Locations.Single(), typeArgSymbol.ToFullName());
                                    return null;
                                }
                                default:
                                    invalid = true;
                                    return null;
                            }
                        }
                        case 0:
                        {
                            if (typeSymbol.InheritsFromInterface("Unity.Entities.ISharedComponentData"))
                                return new JobEntityParam_SharedComponent(parameterSymbol, hasChangeFilter);
                            if (typeSymbol.Is("Unity.Entities.Entity"))
                                return new JobEntityParam_Entity(parameterSymbol);
                            if (typeSymbol.IsValueType)
                            {
                                if (typeSymbol.InheritsFromInterface("Unity.Entities.IComponentData"))
                                {
                                    var (success, jobEntityParameter) =
                                        TryParseComponentTypeSymbol(
                                            typeSymbol,
                                            parameterSymbol,
                                            hasChangeFilter,
                                            isReadOnly: parameterSymbol.IsReadOnly(),
                                            diagnosable);

                                    if (success)
                                        return jobEntityParameter;

                                    invalid = true;
                                    return null;
                                }

                                if (typeSymbol.IsAspect())
                                    return new JobEntityParam_Aspect(parameterSymbol);

                                if (typeSymbol.InheritsFromInterface("Unity.Entities.IBufferElementData"))
                                {
                                    JobEntityGeneratorErrors.SGJE0012(diagnosable, parameterSymbol.Locations.Single(), typeSymbol.Name);
                                    invalid = true;
                                    return null;
                                }

                                JobEntityGeneratorErrors.SGJE0003(diagnosable, parameterSymbol.Locations.Single(), parameterSymbol.Name, typeSymbol.GetSymbolTypeName());
                                invalid = true;
                                return new JobEntityParamValueTypesPassedWithDefaultArguments(parameterSymbol);
                            }

                            var isUnityEngineComponent = IsUnityEngineComponent(typeSymbol);
                            if (isUnityEngineComponent != null)
                                return new JobEntityParam_ManagedComponent(parameterSymbol, hasChangeFilter, (bool)isUnityEngineComponent);

                            JobEntityGeneratorErrors.SGJE0010(diagnosable, parameterSymbol.Locations.Single(), parameterSymbol.Name, typeSymbol.ToFullName());
                            invalid = true;
                            return null;
                        }
                        default:
                        {
                            invalid = true;
                            return null;
                        }
                    }
                }
            }
            invalid = true;
            return null;
        }
    }
}
