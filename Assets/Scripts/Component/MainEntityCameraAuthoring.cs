using UnityEngine;
using Unity.Entities;
using System;

[Serializable]
public struct MainEntityCamera : IComponentData
{

}

[DisallowMultipleComponent]
public class MainEntityCameraAuthoring : MonoBehaviour
{
    public class Baker : Baker<MainEntityCameraAuthoring>
    {
        public override void Bake(MainEntityCameraAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<MainEntityCamera>(entity);
        }
    }
}