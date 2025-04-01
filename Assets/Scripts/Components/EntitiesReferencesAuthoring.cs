using Unity.Entities;
using UnityEngine;

public class EntitiesReferencesAuthoring : MonoBehaviour
{

    public GameObject crossPrefabGO;
    public GameObject circlePrefabGO;
    public GameObject lineWinnerPrefabGO;
    public GameObject placeSFXPrefabGO;
    public GameObject winSFXPrefabGO;
    public GameObject loseSFXPrefabGO;

    public class EntitiesReferencesBaker : Baker<EntitiesReferencesAuthoring>
    {
        public override void Bake(EntitiesReferencesAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EntitiesReferences
            {
                crossPrefabEntity = GetEntity(authoring.crossPrefabGO, TransformUsageFlags.Dynamic),
                circlePrefabEntity = GetEntity(authoring.circlePrefabGO, TransformUsageFlags.Dynamic),
                lineWinnerPrefabEntity = GetEntity(authoring.lineWinnerPrefabGO, TransformUsageFlags.Dynamic),
                placeSFXPrefabEntity = GetEntity(authoring.placeSFXPrefabGO, TransformUsageFlags.Dynamic),
                winSFXPrefabEntity = GetEntity(authoring.winSFXPrefabGO, TransformUsageFlags.Dynamic),
                loseSFXPrefabEntity = GetEntity(authoring.loseSFXPrefabGO, TransformUsageFlags.Dynamic),
            });
        }
    }

}

public struct EntitiesReferences : IComponentData
{
    public Entity crossPrefabEntity;
    public Entity circlePrefabEntity;
    public Entity lineWinnerPrefabEntity;
    public Entity placeSFXPrefabEntity;
    public Entity winSFXPrefabEntity;
    public Entity loseSFXPrefabEntity;
}
