using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;

public class MoneyRaiserManager : MonoBehaviour
{
    List<MoneyRaiser> moneyRaisers;
    MoneyRaiser.UpdateJob updateJob;

    void Start()
    {
        moneyRaisers = new List<MoneyRaiser>();
        updateJob = new MoneyRaiser.UpdateJob();
    }

    void OnDestroy()
    {
    }

    public void Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
    {
        var instance = Instantiate(prefab, position, rotation, parent);
        moneyRaisers.Add(instance.GetComponent<MoneyRaiser>());
    }

    void Update()
    {
        int n = moneyRaisers.Count;
        var moneyRaisersArray = new NativeArray<MoneyRaiser.Data>(n,
            Allocator.TempJob, NativeArrayOptions.UninitializedMemory);

        for (int i = 0; i < n; i++)
            moneyRaisersArray[i] = new MoneyRaiser.Data(moneyRaisers[i]);

        updateJob.deltaTime = Time.deltaTime;
        updateJob.moneyRaiserData = moneyRaisersArray;

        JobHandle jobHandle = updateJob.Schedule(n, 1);
        jobHandle.Complete();

        while (n-- > 0) {
            var raiser = moneyRaisersArray[n];
            if (!raiser.Apply(moneyRaisers[n]))
                moneyRaisers.RemoveAt(n);
        }

        moneyRaisersArray.Dispose();
    }
}
