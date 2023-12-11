using Unity.Burst;
using Unity.Entities;
using UnityEngine;

public partial struct CountUpSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Score>();
    }

    public void OnUpdate(ref SystemState state)
    {
        EndSimulationEntityCommandBufferSystem.Singleton ecbSystem = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        foreach (var(score, countup) in SystemAPI.Query<RefRW<Score>, RefRW<CountUpComponent>>().WithAll<Simulate>())
        {
            score.ValueRW.Value += 1;
            score.ValueRW.SetText();
            ecbSystem.CreateCommandBuffer(state.WorldUnmanaged).RemoveComponent<CountUpComponent>(SystemAPI.GetSingletonEntity<Score>());
        }
    }
}