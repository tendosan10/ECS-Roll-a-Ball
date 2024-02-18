using Unity.Entities;
using Unity.Mathematics;
using Unity.Burst;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Physics.Extensions;
using Unity.Transforms;

/// <summary>
/// PlayerEntityの剛体に力を加えるシステム
/// </summary>
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
        foreach (var (player, mass, velocity, transform) in SystemAPI.Query<RefRW<Player>, RefRO<PhysicsMass>, RefRW<PhysicsVelocity>, RefRO<LocalTransform>>().WithAll<Simulate>())
        {
            var moveInput = new float2(player.ValueRO.Horizontal, player.ValueRO.Vertical);
            moveInput = math.normalizesafe(moveInput) * player.ValueRO.Speed * deltaTime;
            player.ValueRW.PosY = transform.ValueRO.Position.y; // 現在のY座標を記録
            velocity.ValueRW.ApplyLinearImpulse(mass.ValueRO, new float3(moveInput.x, 0, moveInput.y)); // 力を加える
        }
    }
}

/// <summary>
/// PlayerEntityのY座標を固定するシステム
/// </summary>
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
        foreach (var (player, transform) in SystemAPI.Query<RefRO<Player>, RefRW<LocalTransform>>().WithAll<Simulate>())
        {
            transform.ValueRW.Position.y = player.ValueRO.PosY;
        }
    }
}