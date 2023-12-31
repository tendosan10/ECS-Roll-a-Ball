using Unity.Entities;
using UnityEngine;

public struct Score : IComponentData
{
    public int Value;
    public void SetText()
    {
        ScoreGameObject.instance.SetText(Value);
    }
}

public class ScoreAuthoring : MonoBehaviour
{
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
