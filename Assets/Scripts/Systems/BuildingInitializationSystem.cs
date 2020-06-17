
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Mathematics;

public class BuildingInitializationSystem : JobComponentSystem
{
    EndSimulationEntityCommandBufferSystem system;
    static int counter;

    protected override void OnCreate()
    {
        base.OnCreate();
        system = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var ecs = system.CreateCommandBuffer().ToConcurrent();
        var job = Entities.ForEach(
            (Entity entity, int entityInQueryIndex, ref BuildingInitializationData initData) => {
                BuildingData building = new BuildingData();
                building.timeLeft = (entityInQueryIndex % 100) * 0.1f;
                ecs.AddComponent<BuildingData>(entityInQueryIndex, entity, building);
                ecs.RemoveComponent<BuildingInitializationData>(entityInQueryIndex, entity);
            }).Schedule(inputDeps);

        system.AddJobHandleForProducer(job);
        return job;
    }
}
