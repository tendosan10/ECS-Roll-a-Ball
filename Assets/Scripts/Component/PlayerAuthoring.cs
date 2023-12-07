using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct Player : IComponentData
{
    public float Speed;
}

public class PlayerAuthoring : MonoBehaviour
{
    public float Speed;

    class Baker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            var data = new Player() { Speed = authoring.Speed };
            AddComponent(GetEntity(TransformUsageFlags.None), data);
        }
    }
}
