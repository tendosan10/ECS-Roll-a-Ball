using Unity.Entities;
using Unity.Transforms;

[UpdateInGroup(typeof(PresentationSystemGroup))]
public partial struct MainCameraSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<MainEntityCamera>();
    }

    public void OnUpdate(ref SystemState state)
    {
        if (MainGameObjectCamera.Instance != null)
        {
            Entity mainEntityCameraEntity = SystemAPI.GetSingletonEntity<MainEntityCamera>();
            LocalToWorld targetLocalToWorld = SystemAPI.GetComponent<LocalToWorld>(mainEntityCameraEntity);
            MainGameObjectCamera.Instance.transform.SetPositionAndRotation(targetLocalToWorld.Position, targetLocalToWorld.Rotation);
        }
    }
}