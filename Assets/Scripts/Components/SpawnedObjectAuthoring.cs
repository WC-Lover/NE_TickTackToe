using Unity.Entities;
using UnityEngine;

class SpawnedObjectAuthoring : MonoBehaviour
{
    public class SpawnedObjectAuthoringBaker : Baker<SpawnedObjectAuthoring>
    {
        public override void Bake(SpawnedObjectAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new SpawnedObject());
        }
    }
}

public struct SpawnedObject : IComponentData
{

}


