using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;
using Unity.Entities.SourceGen.Common;

namespace Unity.Entities.Analyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public partial class EntitiesAnalyzer : DiagnosticAnalyzer
    {
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterCompilationStartAction(compilationCtx =>
            {
                var myCompileData = new CompileData(new ConcurrentDictionary<string, byte>(), new ConcurrentDictionary<string, byte>());
                compilationCtx.RegisterOperationAction(op => AnalyzeInvocation(op, myCompileData), OperationKind.Invocation);
                compilationCtx.RegisterOperationAction(op => AnalyzeVariableDeclarator(op, myCompileData),OperationKind.VariableDeclarator);
            });
        }

        readonly struct CompileData
        {
            public readonly ConcurrentDictionary<string, byte> nonBlobRestrictedTypes;
            public readonly ConcurrentDictionary<string, byte> blobWithRefType;
            public CompileData(ConcurrentDictionary<string, byte> nonBlobRestrictedTypes, ConcurrentDictionary<string, byte> blobWithRefType)
            {
                this.nonBlobRestrictedTypes = nonBlobRestrictedTypes;
                this.blobWithRefType = blobWithRefType;
            }
        }

        static void AnalyzeVariableDeclarator(OperationAnalysisContext context, CompileData compileData)
        {
            var declarator = (IVariableDeclaratorOperation)context.Operation;
            var localSymbol = declarator.Symbol;
            if (localSymbol.RefKind != RefKind.Ref && BlobUtility.IsTypeRestrictedToBlobAssetStorage(localSymbol.Type, compileData.nonBlobRestrictedTypes))
            {
                var fieldName = declarator.Initializer.Value.Syntax.ToString();
                var statement = context.Operation.Syntax.AncestorsAndSelf().OfType<StatementSyntax>().First();
                var location = statement.GetLocation();
                context.ReportDiagnostic(declarator.Initializer.Value.Kind == OperationKind.ObjectCreation
                    ? Diagnostic.Create(k_Ea0002Descriptor, location, fieldName)
                    : Diagnostic.Create(k_Ea0001Descriptor, location, fieldName, declarator.GetVarTypeName(), declarator.Symbol.Name));
            }
        }

        static void AnalyzeInvocation(OperationAnalysisContext context, CompileData compileData)
        {
            var invocationOperation = (IInvocationOperation)context.Operation;
            var targetMethod = invocationOperation.TargetMethod;
            if (targetMethod.ContainingType.IsUnmanagedType
                && targetMethod.TypeArguments.Length == 1
                && targetMethod.Name == "ConstructRoot"
                && targetMethod.ContainingType.ToFullName() == "Unity.Entities.BlobBuilder")
            {
                var blobAssetType = targetMethod.TypeArguments[0];
                if (blobAssetType.TypeKind == TypeKind.TypeParameter || blobAssetType is INamedTypeSymbol { IsGenericType: true }) // Unresolved T
                    return;
                if (BlobUtility.ContainBlobRefType(blobAssetType, out var fieldDescription, out var errorDescription, compileData.blobWithRefType))
                {
                    var blobAssetTypeFullName = blobAssetType.ToFullName();
                    context.ReportDiagnostic(Diagnostic.Create(k_Ea0003Descriptor, invocationOperation.Syntax.GetLocation(),
                        blobAssetTypeFullName, $"{blobAssetTypeFullName}{fieldDescription}", errorDescription));
                }
            }
        }
    }
}
