/*using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using TMPro;

[UpdateInGroup(typeof(PresentationSystemGroup))]
public partial class ScoreSystem : SystemBase
{
    protected override void OnUpdate()
    {
        if (ScoreTextGameObject.Instance != null && SystemAPI.HasSingleton<Score>())
        {
            Entity mainEntityCameraEntity = SystemAPI.GetSingletonEntity<MainEntityCamera>();
            LocalToWorld targetLocalToWorld = SystemAPI.GetComponent<LocalToWorld>(mainEntityCameraEntity);
            MainGameObjectCamera.Instance.transform.SetPositionAndRotation(targetLocalToWorld.Position, targetLocalToWorld.Rotation);
        }
    }
}
*/