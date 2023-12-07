using Unity.Entities;
using Unity.Mathematics;
using Unity.Burst;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Physics.Extensions;

[UpdateInGroup(typeof(AfterPhysicsSystemGroup))]
public partial struct PlayerMovementSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var speed = SystemAPI.Time.DeltaTime;
        foreach (var (player, mass, velocity) in SystemAPI.Query<RefRO<Player>, RefRO<PhysicsMass>, RefRW<PhysicsVelocity>>().WithAll<Simulate>())
        {
            var moveInput = new float2(player.ValueRO.Horizontal, player.ValueRO.Vertical);
            moveInput = math.normalizesafe(moveInput) * player.ValueRO.Speed * speed;

            velocity.ValueRW.ApplyLinearImpulse(mass.ValueRO, new float3(moveInput.x, 0, moveInput.y));
        }
    }
}