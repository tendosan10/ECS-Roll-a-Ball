using UnityEngine;
using Unity.Entities;
using Unity.Burst;

/// <summary>
/// Playerのコンポーネント
/// </summary>
public struct Player : IComponentData
{
    public float Speed;
    public float Horizontal;
    public float Vertical;
    public float PosY;
}

public class PlayerAuthoring : MonoBehaviour
{
    [Header("Playerの移動スピード")]
    public float Speed;

    class Baker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            var data = new Player() {Speed = authoring.Speed };
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), data);
        }
    }
}

/// <summary>
/// Playerコンポーネントに入力値を与える
/// </summary>
public partial struct PlayerInputSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Player>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        foreach (var playerInput in SystemAPI.Query<RefRW<Player>>())
        {
            playerInput.ValueRW.Horizontal = horizontal;
            playerInput.ValueRW.Vertical = vertical;
        }
    }
}
