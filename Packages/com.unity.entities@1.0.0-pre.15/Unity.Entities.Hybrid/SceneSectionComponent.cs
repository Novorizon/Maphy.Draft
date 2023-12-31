using UnityEngine;
using UnityEngine.Serialization;

namespace Unity.Entities
{
    /// <summary>
    /// Component to indicate that a GameObject and its children belong to the specified scene section.
    /// </summary>
    public class SceneSectionComponent : MonoBehaviour
    {
        /// <summary>
        /// Index of the scene section where the GameObject and its children belong to.
        /// </summary>
        [FormerlySerializedAs("SectionId")] public int SectionIndex;

        class SceneSectionComponentBaker : Baker<SceneSectionComponent>
        {
            public override void Bake(SceneSectionComponent authoring)
            {
                AddSharedComponent(new SceneSection { SceneGUID = GetSceneGUID(), Section = authoring.SectionIndex });
            }
        }
    }
}
