using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;

public class BuildingManager : MonoBehaviour
{
    Building[] buildings;
    NativeArray<Building.Data> buildingData;
    Building.UpdateJob updateJob;
    MoneyRaiserManager moneyRaiserManager;
    Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        moneyRaiserManager = GetComponent<MoneyRaiserManager>();

        buildings = GetComponentsInChildren<Building>();
        buildingData = new NativeArray<Building.Data>(buildings.Length, Allocator.Persistent);
        for (int i = 0; i < buildings.Length; i++)
            buildingData[i] = new Building.Data(buildings[i]);

        updateJob = new Building.UpdateJob { buildingData = buildingData };
    }

    void OnDestroy()
    {
        buildingData.Dispose();
    }

    void Update()
    {
        var spawnList = new NativeArray<Building.MoneySpawn>(buildingData.Length,
            Allocator.TempJob, NativeArrayOptions.ClearMemory);

        updateJob.cameraPosition = mainCamera.transform.position;
        updateJob.deltaTime = Time.deltaTime;
        updateJob.spawnList = spawnList;

        JobHandle jobHandle = updateJob.Schedule(buildings.Length, 1);
        jobHandle.Complete();

        for (int i = 0; i < buildings.Length; i++) {
            var spawn = spawnList[i];
            if (spawn.shouldSpawn) {
                var building = buildings[i];
                moneyRaiserManager.Spawn(building.moneyPrefab, spawn.position, spawn.rotation, building.transform);
            }
        }

        spawnList.Dispose();
    }
}
