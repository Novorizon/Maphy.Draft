using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;

namespace Unity.Entities.Analyzer
{
    public partial class EntitiesAnalyzer : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(k_Ea0001Descriptor, k_Ea0002Descriptor, k_Ea0003Descriptor);

        public const string ID_EA0001 = "EA0001";
        static readonly DiagnosticDescriptor k_Ea0001Descriptor
            = new DiagnosticDescriptor(ID_EA0001, "You may only access BlobAssetStorage by (non-readonly) ref",
                "You may only access {0} by (non-readonly) ref, as it may only live in blob storage. Try `ref {1} {2} = ref {0}`.",
                "BlobAsset", DiagnosticSeverity.Error, isEnabledByDefault: true, description: "Expression contains BlobAssetStorage not passed by (non-readonly) ref.");

        public const string ID_EA0002 = "EA0002";
        static readonly DiagnosticDescriptor k_Ea0002Descriptor
            = new DiagnosticDescriptor(ID_EA0002, "Potentially harmful construction of a BlobAsset with keywords `new` or `default`",
                "You should only construct {0} through a BlobBuilder, as it may only live in blob storage. Try using BlobBuilder .Construct/.ConstructRoot/.SetPointer.",
                "BlobAsset", DiagnosticSeverity.Warning, isEnabledByDefault: true, description: "Expression contains potentially harmful construction of BlobAssetStorage with `new` or `default` keywords.");

        public const string ID_EA0003 = "EA0003";
        static readonly DiagnosticDescriptor k_Ea0003Descriptor
            = new DiagnosticDescriptor(ID_EA0003, "You cannot use the BlobBuilder to build a type containing Non-Blob References",
                "You may not build the type `{0}`, with BlobBuilder.ConstructRoot, as `{1}` {2}",
                "BlobAsset", DiagnosticSeverity.Error, isEnabledByDefault: true, description: "Expression contains BlobBuilder construction of type with non blob reference.");
    }
}
