using System;
using Unity.Entities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Unity.Scenes.Editor.Tests
{
    [AddComponentMenu("")]
    public class SubSceneLoadTestAssetAuthoring : MonoBehaviour
    {
        public Object Asset;
    }

#if !NET_DOTS && !UNITY_DISABLE_MANAGED_COMPONENTS
    public class SubSceneLoadTestAssetComponent : IComponentData
    {
        public Object Asset;
    }
#endif

    public class SubSceneLoadTestAssetBaker : Baker<SubSceneLoadTestAssetAuthoring>
    {
        public override void Bake(SubSceneLoadTestAssetAuthoring authoring)
        {
#if !NET_DOTS && !UNITY_DISABLE_MANAGED_COMPONENTS
            AddComponentObject(new SubSceneLoadTestAssetComponent
            {
                Asset = authoring.Asset
            });
#endif
        }
    }
}
