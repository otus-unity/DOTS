
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Mathematics;

public class BuildingsSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = UnityEngine.Time.deltaTime;

        Entities.ForEach(
            (ref BuildingData building, ref Translation translation, in MoneyRaiserPrefabData prefab) => {
                building.timeLeft -= deltaTime;
                if (building.timeLeft < 0.0f) {
                    building.timeLeft = 10.0f;
                    GameObject.Instantiate(prefab.prefab, translation.Value, Quaternion.identity);
                }
            }).WithoutBurst().Run();

        return default;
    }
}
