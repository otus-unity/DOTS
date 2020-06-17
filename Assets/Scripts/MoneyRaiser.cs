using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

public class MoneyRaiser : MonoBehaviour
{
    public struct Data
    {
        public float originalZ;
        public float counter;
        public Vector3 position;
        public bool destroy;

        public Data(MoneyRaiser raiser)
        {
            originalZ = raiser.originalZ;
            counter = raiser.counter;
            position = raiser.transform.position;
            destroy = false;
        }

        public void Update(float deltaTime)
        {
            float t = deltaTime;
            counter += t * 10.0f;

            position.y += t;
            position.z = originalZ + math.sin(counter) * 0.1f;

            if (position.y > 4.0f)
                destroy = true;
        }

        public bool Apply(MoneyRaiser raiser)
        {
            if (destroy) {
                Destroy(raiser.gameObject);
                return false;
            }

            raiser.counter = counter;
            raiser.transform.position = position;
            return true;
        }
    }

    public struct UpdateJob : IJobParallelFor
    {
        public float deltaTime;
        public NativeArray<MoneyRaiser.Data> moneyRaiserData;

        public void Execute(int index)
        {
            var data = moneyRaiserData[index];
            data.Update(deltaTime);
            moneyRaiserData[index] = data;
        }
    }

    float originalZ;
    float counter;

    void Awake()
    {
        originalZ = transform.position.z;
    }
}
