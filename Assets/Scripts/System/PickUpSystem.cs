using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

/// <summary>
/// アイテムの回転の実行とトリガーイベントの検知を行うシステム
/// </summary>
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
        // トリガーイベントジョブを発行
        var job = new PickUpJob
        {
            PlayerGroup = SystemAPI.GetComponentLookup<Player>(), // 衝突したPlayerコンポーネント群を登録
            PickUpGroup = SystemAPI.GetComponentLookup<PickUp>(), // 衝突したPickUpコンポーネント群を登録
            ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged),
            scoreEntity = SystemAPI.GetSingletonEntity<Score>()
        };
        state.Dependency = job.Schedule(simulation, state.Dependency);

        // 回転ジョブを発行
        var rotate = new RotationJob
        {
            deltaTime = SystemAPI.Time.DeltaTime
        };
        rotate.ScheduleParallel();

        JobHandle.ScheduleBatchedJobs(); // 発行したジョブを実行
    }
}

/// <summary>
/// アイテムのトリガーイベントジョブ
/// </summary>
[BurstCompile]
struct PickUpJob : ITriggerEventsJob
{
    [ReadOnly] public ComponentLookup<Player> PlayerGroup; // Playerコンポーネント群
    public ComponentLookup<PickUp> PickUpGroup; // PickUpコンポーネント群
    public EntityCommandBuffer ECB; // 発行されたコマンドバッファ
    public Entity scoreEntity;

    // トリガーイベント処理本文
    [BurstCompile]
    public void Execute(TriggerEvent ev)
    {
        // イベントを発行しているEntityがPickUpコンポーネントであるか判別
        var aIsPickUp = PickUpGroup.HasComponent(ev.EntityA);
        var bIsPickUp = PickUpGroup.HasComponent(ev.EntityB);

        // イベントを発行しているEntityがPlayerコンポーネントであるか判別
        var aIsPlayer = PlayerGroup.HasComponent(ev.EntityA);
        var bIsPlayer = PlayerGroup.HasComponent(ev.EntityB);

        // 2つのEntityが同種であれば終了
        if (!(aIsPickUp ^ bIsPickUp)) return;
        if (!(aIsPlayer ^ bIsPlayer)) return;

        // PickUp側のEntityのみ取得
        var pickUpEntity = aIsPickUp ? ev.EntityA : ev.EntityB;

        // 対象のPickUpEntityを非アクティブ状態にする
        ECB.AddComponent<Disabled>(pickUpEntity);
        // ScoreEntityに数え上げ用のタグを付与する
        ECB.AddComponent<CountUpComponent>(scoreEntity);
    }
}

/// <summary>
/// アイテムを回転させるジョブ
/// </summary>
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