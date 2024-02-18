using Unity.Entities;

/// <summary>
/// スコアを増やすシステム
/// </summary>
public partial struct CountUpSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Score>();
    }

    public void OnUpdate(ref SystemState state)
    {
        EndSimulationEntityCommandBufferSystem.Singleton ecbSystem = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        // ScoreEntityがCountUpComponentを保持しているときのみ処理を行う
        foreach (var(score, countup) in SystemAPI.Query<RefRW<Score>, RefRW<CountUpComponent>>().WithAll<Simulate>())
        {
            score.ValueRW.Value += 1;
            score.ValueRW.SetText();
            // 二重に数え上げないため、CountUpComponentを1つ除外する
            ecbSystem.CreateCommandBuffer(state.WorldUnmanaged).RemoveComponent<CountUpComponent>(SystemAPI.GetSingletonEntity<Score>());
        }
    }
}