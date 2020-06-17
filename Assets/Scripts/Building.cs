using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;

public class Building : MonoBehaviour
{
    public struct Data
    {
        public Vector3 position;
        public float timeLeft;

        public Data(Building building)
        {
            position = building.transform.position;
            timeLeft = building.timeLeft;
        }

        public bool Update(float deltaTime)
        {
            timeLeft -= deltaTime;
            if (timeLeft < 0.0f) {
                timeLeft = 10.0f;
                return true;
            }
            return false;
        }
    }

    public struct MoneySpawn
    {
        public bool shouldSpawn;
        public Vector3 position;
        public Quaternion rotation;
    }

    public struct UpdateJob : IJobParallelFor
    {
        public Vector3 cameraPosition;
        public float deltaTime;
        public NativeArray<Building.Data> buildingData;
        [WriteOnly] public NativeArray<Building.MoneySpawn> spawnList;

        public void Execute(int index)
        {
            var data = buildingData[index];
            if (data.Update(deltaTime)) {
                spawnList[index] = new MoneySpawn {
                        shouldSpawn = true,
                        position = data.position + Vector3.up,
                        rotation = Quaternion.LookRotation(cameraPosition - data.position)
                    };
            }
            buildingData[index] = data;
        }
    }

    static int counter;

    float timeLeft;
    public GameObject moneyPrefab;

    void Awake()
    {
        timeLeft = (counter++ % 100) * 0.1f;
    }
}
