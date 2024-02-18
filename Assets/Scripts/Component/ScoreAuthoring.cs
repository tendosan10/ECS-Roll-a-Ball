using Unity.Entities;
using UnityEngine;

/// <summary>
/// スコアを保持、Gameシーン上のスコアUIへ反映するコンポーネント
/// </summary>
public struct Score : IComponentData
{
    public int Value;

    /// <summary>
    /// UIへの反映
    /// </summary>
    public void SetText()
    {
        ScoreGameObject.instance.SetText(Value);
    }
}

public class ScoreAuthoring : MonoBehaviour
{
    [Header("スコアの初期値")]
    public int Initial;

    class Baker : Baker<ScoreAuthoring>
    {
        public override void Bake(ScoreAuthoring authoring)
        {
            var data = new Score() { Value = authoring.Initial };
            AddComponent(GetEntity(TransformUsageFlags.None), data);
        }
    }
}
