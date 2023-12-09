using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[BurstCompile]
public partial struct MainEntityCameraSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<MainEntityCamera>();
        state.RequireForUpdate<Player>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (camera, transform) in SystemAPI.Query<RefRO<MainEntityCamera>, RefRW<LocalTransform>>().WithAll<Simulate>())
        {
            Entity playerEntity = SystemAPI.GetSingletonEntity<Player>();
            transform.ValueRW.Position = SystemAPI.GetComponent<LocalTransform>(playerEntity).Position + camera.ValueRO.Offset;
        }
    }
}
