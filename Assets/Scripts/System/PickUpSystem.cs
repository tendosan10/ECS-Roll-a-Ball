using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

[UpdateInGroup(typeof(AfterPhysicsSystemGroup))]
public partial struct PickUpSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PickUp>();
        state.RequireForUpdate<Player>();
        state.RequireForUpdate<Score>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var simulation = SystemAPI.GetSingleton<SimulationSingleton>();
        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        var job = new PickUpJob
        {
            PlayerGroup = SystemAPI.GetComponentLookup<Player>(),
            PickUpGroup = SystemAPI.GetComponentLookup<PickUp>(),
            EntityCommandBuffer = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged),
            scoreEntity = SystemAPI.GetSingletonEntity<Score>()
        };
        state.Dependency = job.Schedule(simulation, state.Dependency);

        var rotate = new RotationJob
        {
            deltaTime = SystemAPI.Time.DeltaTime
        };
        rotate.ScheduleParallel();

        JobHandle.ScheduleBatchedJobs();
    }
}

[BurstCompile]
struct PickUpJob : ITriggerEventsJob
{
    [ReadOnly] public ComponentLookup<Player> PlayerGroup;
    public ComponentLookup<PickUp> PickUpGroup;
    public EntityCommandBuffer EntityCommandBuffer;
    public Entity scoreEntity;

    [BurstCompile]
    public void Execute(TriggerEvent ev)
    {
        var aIsPickUp = PickUpGroup.HasComponent(ev.EntityA);
        var bIsPickUp = PickUpGroup.HasComponent(ev.EntityB);

        var aIsPlayer = PlayerGroup.HasComponent(ev.EntityA);
        var bIsPlayer = PlayerGroup.HasComponent(ev.EntityB);

        if (!(aIsPickUp ^ bIsPickUp)) return;
        if (!(aIsPlayer ^ bIsPlayer)) return;

        var (pickUpEntity, playerEntity) =
          aIsPickUp ? (ev.EntityA, ev.EntityB) : (ev.EntityB, ev.EntityA);

        EntityCommandBuffer.AddComponent<Disabled>(pickUpEntity);
        EntityCommandBuffer.AddComponent<CountUpComponent>(scoreEntity);
    }
}

[BurstCompile]
partial struct RotationJob : IJobEntity
{
    public float deltaTime;
    [BurstCompile]
    void Execute(in PickUp pickUp, ref LocalTransform transform)
    {
        transform.Rotation = math.mul(quaternion.AxisAngle(new float3(15, 30, 45), 0.05f * deltaTime), math.normalize(transform.Rotation));
    }
}