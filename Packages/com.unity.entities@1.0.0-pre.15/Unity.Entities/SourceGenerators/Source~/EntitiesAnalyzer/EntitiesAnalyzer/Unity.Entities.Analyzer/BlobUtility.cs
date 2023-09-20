using System.Collections.Concurrent;
using System.Linq;
using Microsoft.CodeAnalysis;
using Unity.Entities.SourceGen.Common;

namespace Unity.Entities.Analyzer
{
    public static class BlobUtility
    {
        // Checks if a type
        public static bool IsTypeRestrictedToBlobAssetStorage(ITypeSymbol  type, ConcurrentDictionary<string,byte> nonRestrictedTypes)
        {
            if (type.TypeKind == TypeKind.TypeParameter)
                return false;
            if (type is IPointerTypeSymbol)
                return false;
            if (type is IArrayTypeSymbol)
                return false;
            if (type is INamedTypeSymbol namedType)
            {
                if (namedType.SpecialType != SpecialType.None) // IsPrimitive
                    return false;

                var fullName = type.ToFullName();
                if (nonRestrictedTypes.ContainsKey(fullName))
                    return false;

                var containingNamespace = type.ContainingNamespace.ToString();
                if (containingNamespace == "UnityEngine" ||
                    containingNamespace == "UnityEditor" ||
                    containingNamespace == "System"      ||
                    containingNamespace == "System.Private.CoreLib")
                    return false;

                if (namedType.IsUnboundGenericType)
                {
                    nonRestrictedTypes.TryAdd(fullName, default);
                    return false;
                }

                if (namedType.IsValueType)
                {
                    if (namedType.GetAttributes().Any(attribute => attribute.AttributeClass.ToFullName() == "Unity.Entities.MayOnlyLiveInBlobStorageAttribute"))
                        return true;

                    foreach (var symbol in namedType.GetMembers())
                    {
                        if (symbol.IsStatic)
                            continue;
                        if (symbol is IFieldSymbol field && IsTypeRestrictedToBlobAssetStorage(field.Type, nonRestrictedTypes))
                            return true;
                    }
                }

                nonRestrictedTypes.TryAdd(fullName, default);
            }
            return false;
        }

        // Checks if a known blob contains any reference types
        public static bool ContainBlobRefType(ITypeSymbol type, out string firstFieldDescription, out string firstErrorDescription, ConcurrentDictionary<string,byte> blobWithRefType)
        {
            if (type.IsReferenceType)
            {
                firstErrorDescription = "is a reference.  Only non-reference types are allowed in Blobs.";
                firstFieldDescription = null;
                return true;
            }

            if (type is IPointerTypeSymbol)
            {
                firstErrorDescription = "is a pointer.  Only non-reference types are allowed in Blobs.";
                firstFieldDescription = null;
                return true;
            }

            if (type.SpecialType != SpecialType.None) // IsPrimitive
            {
                firstErrorDescription = null;
                firstFieldDescription = null;
                return false;
            }

            var typeFullName = type.ToFullName();
            if (blobWithRefType.ContainsKey(typeFullName))
            {
                firstFieldDescription = null;
                firstErrorDescription = null;
                return false;
            }
            blobWithRefType.TryAdd(typeFullName, default);

            if (type is INamedTypeSymbol { TypeArguments: { Length: 1 } } namedTypeSymbol && namedTypeSymbol.ContainingNamespace.ToString() == "Unity.Entities")
            {
                var isBlobArray = type.Name == "BlobArray";
                if (isBlobArray || type.Name == "BlobPtr")
                {
                    if (ContainBlobRefType(namedTypeSymbol.TypeArguments[0], out firstFieldDescription, out firstErrorDescription, blobWithRefType))
                    {
                        firstFieldDescription = (isBlobArray ? "[]" : ".Value") + firstFieldDescription;
                        return true;
                    }
                }
            }

            if (typeFullName == "Unity.Entities.Serialization.UntypedWeakReferenceId")
            {
                firstErrorDescription = "is an UntypedWeakReferenceId. Weak asset references are not yet supported in Blobs.";
                firstFieldDescription = null;
                return true;
            }

            foreach (var field in type.GetMembers())
            {
                if (field is IFieldSymbol fieldSymbol && !field.IsStatic)
                {
                    if (ContainBlobRefType(fieldSymbol.Type, out firstFieldDescription, out firstErrorDescription, blobWithRefType))
                    {
                        firstFieldDescription = $".{field.Name}{firstFieldDescription}";
                        return true;
                    }
                }
            }

            firstFieldDescription = null;
            firstErrorDescription = null;
            return false;
        }
    }
}
