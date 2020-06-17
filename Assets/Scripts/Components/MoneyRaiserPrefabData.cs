
using UnityEngine;
using Unity.Entities;
using System;

public struct MoneyRaiserPrefabData : ISharedComponentData, IEquatable<MoneyRaiserPrefabData>
{
    public GameObject prefab;
    public Entity entity;

    public bool Equals(MoneyRaiserPrefabData other)
    {
        return prefab == other.prefab;
    }

    public override int GetHashCode()
    {
        return prefab.GetHashCode();
    }
}
