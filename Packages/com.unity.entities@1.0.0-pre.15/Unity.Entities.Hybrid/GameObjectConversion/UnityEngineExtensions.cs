using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityObject = UnityEngine.Object;
using static Unity.Debug;

namespace Unity.Entities.Conversion
{
    static class UnityEngineExtensions
    {
        /// <summary>
        /// Obsolete. Returns a hash that can be used as a guid within a (non-persistent) session to refer to this UnityEngine.Object.
        /// </summary>
        [Obsolete("This function is no longer supported. (RemovedAfter 2021-03-01)")]
        public static Hash128 ComputeInstanceHash(this UnityObject @this)
        {
            if (@this is Component component)
                @this = component.gameObject;

            var instanceID = @this.GetInstanceID();

            var hash = new UnityEngine.Hash128();
            HashUtilities.ComputeHash128(ref instanceID, ref hash);
            return hash;
        }

        /// <summary>
        /// Returns an EntityGuid that can be used as a guid within a (non-persistent) session to refer to an entity generated
        /// from a UnityEngine.Object. The primary entity will be index 0, and additional entities will have increasing
        /// indices.
        /// </summary>
        public static EntityGuid ComputeEntityGuid(this UnityObject @this, uint namespaceId, int serial)
        {
            if (@this is Component component)
                @this = component.gameObject;
            return new EntityGuid(@this.GetInstanceID(), 0, namespaceId, (uint)serial);
        }

        public static bool IsPrefab(this UnityObject @this) =>
            @this is GameObject go && IsPrefab(go);

        public static bool IsPrefab(this GameObject @this) =>
            !@this.scene.IsValid();

        public static bool IsAsset(this UnityObject @this) =>
            !(@this is GameObject) && !(@this is Component);

        public static bool IsActiveIgnorePrefab(this GameObject @this)
        {
            if (!@this.IsPrefab())
                return @this.activeInHierarchy;

            var parent = @this.transform;
            while (parent != null)
            {
                if (!parent.gameObject.activeSelf)
                    return false;

                parent = parent.parent;
            }

            return true;
        }

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        public static void CheckObjectIsNotComponent(this UnityObject @this)
        {
            if (@this is Component)
            {
                // should not be possible to get here (user-callable API's will always auto request the Component's GameObject, unless we have a bug)
                throw new InvalidOperationException();
            }
        }

        public static bool IsComponentDisabled(this Component @this)
        {
            switch (@this)
            {
                case Renderer  r: return !r.enabled;
#if LEGACY_PHYSICS
                case Collider  c: return !c.enabled;
#endif
                case LODGroup  l: return !l.enabled;
                case Behaviour b: return !b.enabled;
            }

            return false;
        }

        public static bool GetComponentsBaking(this GameObject gameObject, List<Component> componentsCache)
        {
            int outputIndex = 0;
            gameObject.GetComponents(componentsCache);

            for (var i = 0; i != componentsCache.Count; i++)
            {
                var component = componentsCache[i];

                if (component == null)
                    LogWarning($"The referenced script is missing on {gameObject.name} (index {i} in components list)", gameObject);
                else
                {
                    componentsCache[outputIndex] = component;

                    outputIndex++;
                }
            }

            componentsCache.RemoveRange(outputIndex, componentsCache.Count - outputIndex);
            return true;
        }
        public static unsafe bool GetComponents(this GameObject @this, ComponentType* componentTypes, int maxComponentTypes, List<Component> componentsCache)
        {
            int outputIndex = 0;
            @this.GetComponents(componentsCache);

            if (maxComponentTypes < componentsCache.Count)
            {
                LogWarning($"Too many components on {@this.name}", @this);
                return false;
            }

            for (var i = 0; i != componentsCache.Count; i++)
            {
                var component = componentsCache[i];

                if (component == null)
                    LogWarning($"The referenced script is missing on {@this.name} (index {i} in components list)", @this);
                else if (!component.IsComponentDisabled())
                {
                    var componentType = component.GetType();
                    var isUniqueType = true;
                    for (var j = 0; j < outputIndex; ++j)
                    {
                        if (componentTypes[j].Equals(componentType))
                        {
                            isUniqueType = false;
                            break;
                        }
                    }
                    if (!isUniqueType)
                        continue;

                    componentsCache[outputIndex] = component;
                    componentTypes[outputIndex] = componentType;

                    outputIndex++;
                }
            }

            componentsCache.RemoveRange(outputIndex, componentsCache.Count - outputIndex);
            return true;
        }
    }
}
