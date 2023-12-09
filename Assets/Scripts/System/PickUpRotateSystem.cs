using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

public partial struct PickUpRotateSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PickUp>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var job = new RotationJob
        {
            deltaTime = SystemAPI.Time.DeltaTime
        };

        job.ScheduleParallel();
    }
}

[BurstCompile]
partial struct RotationJob : IJobEntity
{
    public float deltaTime;
    void Execute(in PickUp pickUp, ref LocalTransform transform)
    {
        transform.Rotation = math.mul(quaternion.AxisAngle(new float3(15, 30, 45), 0.05f * deltaTime), math.normalize(transform.Rotation));
    }
}
