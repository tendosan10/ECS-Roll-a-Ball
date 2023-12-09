using Unity.Entities;
using Unity.Mathematics;
using Unity.Burst;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Physics.Extensions;
using Unity.Transforms;

[UpdateInGroup(typeof(PhysicsSystemGroup))]
public partial struct PlayerMovementSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Player>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var deltaTime = SystemAPI.Time.DeltaTime;
        foreach (var (player, mass, velocity) in SystemAPI.Query<RefRO<Player>, RefRO<PhysicsMass>, RefRW<PhysicsVelocity>>().WithAll<Simulate>())
        {
            var moveInput = new float2(player.ValueRO.Horizontal, player.ValueRO.Vertical);
            moveInput = math.normalizesafe(moveInput) * player.ValueRO.Speed * deltaTime;

            velocity.ValueRW.ApplyLinearImpulse(mass.ValueRO, new float3(moveInput.x, 0, moveInput.y));
        }
    }
}

[UpdateInGroup(typeof(AfterPhysicsSystemGroup))]
public partial struct PlayerPosModification : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Player>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var deltaTime = SystemAPI.Time.DeltaTime;
        foreach (var (player, transform) in SystemAPI.Query<RefRO<Player>, RefRW<LocalTransform>>().WithAll<Simulate>())
        {
            transform.ValueRW.Position.y = 0.5f;
        }
    }
}