using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Burst;

public struct MainEntityCamera : IComponentData
{
    public float3 Offset;
}

[DisallowMultipleComponent]
public class MainEntityCameraAuthoring : MonoBehaviour
{
    public class Baker : Baker<MainEntityCameraAuthoring>
    {
        public override void Bake(MainEntityCameraAuthoring authoring)
        {
            var data = new MainEntityCamera();
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), data);
        }
    }
}

[UpdateBefore(typeof(MainEntityCameraSystem))]
public partial struct MainCameraOffSet : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<MainEntityCamera>();
        state.RequireForUpdate<Player>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        Entity player = SystemAPI.GetSingletonEntity<Player>();
        RefRW<MainEntityCamera> camera = SystemAPI.GetSingletonRW<MainEntityCamera>();
        camera.ValueRW.Offset = SystemAPI.GetComponent<LocalTransform>(SystemAPI.GetSingletonEntity<MainEntityCamera>()).Position
            - SystemAPI.GetComponent<LocalTransform>(player).Position;

        state.Enabled = false;
    }
}