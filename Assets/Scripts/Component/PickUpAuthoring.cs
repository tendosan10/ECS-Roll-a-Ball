using UnityEngine;
using Unity.Entities;

/// <summary>
/// アイテムのタグ
/// </summary>
public struct PickUp : IComponentData
{
}

public class PickUpAuthoring : MonoBehaviour
{
    class Baker : Baker<PickUpAuthoring>
    {
        public override void Bake(PickUpAuthoring authoring)
        {
            var data = new PickUp();
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), data);
        }
    }
}
