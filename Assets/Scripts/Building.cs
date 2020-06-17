using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    static int counter;

    Camera mainCamera;
    float timeLeft;
    public GameObject moneyPrefab;

    void Awake()
    {
        mainCamera = Camera.main;
        timeLeft = (counter++ % 100) * 0.1f;
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0.0f) {
            timeLeft = 10.0f;
            Vector3 position = transform.position + Vector3.up;
            Quaternion rotation = Quaternion.LookRotation(mainCamera.transform.position - transform.position);
            Instantiate(moneyPrefab, position, rotation, transform);
        }
    }
}
