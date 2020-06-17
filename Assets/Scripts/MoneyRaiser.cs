using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyRaiser : MonoBehaviour
{
    float originalZ;
    float counter;

    void Start()
    {
        originalZ = transform.position.z;
    }

    void Update()
    {
        float t = Time.deltaTime;
        counter += t * 10.0f;

        var pos = transform.position;
        pos.y += t;
        pos.z = originalZ + Mathf.Sin(counter) * 0.1f;
        transform.position = pos;

        if (pos.y > 4.0f)
            Destroy(gameObject);
    }
}
