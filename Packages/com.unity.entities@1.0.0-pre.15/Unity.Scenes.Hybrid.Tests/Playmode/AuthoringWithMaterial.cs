using System;
using Unity.Entities;
using Unity.Scenes.Hybrid.Tests;
using UnityEngine;

namespace Unity.Scenes.Hybrid.Tests
{
    public struct SharedWithMaterial : ISharedComponentData, IEquatable<SharedWithMaterial>
    {
        public Material material;

        public bool Equals(SharedWithMaterial other)
        {
            return Equals(material, other.material);
        }

        public override bool Equals(object obj)
        {
            return obj is SharedWithMaterial other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (material != null ? material.GetHashCode() : 0);
        }
    }

    [DisallowMultipleComponent]
    public class AuthoringWithMaterial : MonoBehaviour
    {
        public Material material;
    }

    public class AuthoringWithMaterialBaker : Baker<AuthoringWithMaterial>
    {
        public override void Bake(AuthoringWithMaterial authoring)
        {
            AddSharedComponentManaged(GetEntity(authoring), new SharedWithMaterial(){material = authoring.material});
        }
    }
}
