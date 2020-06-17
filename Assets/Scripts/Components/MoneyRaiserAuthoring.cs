using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class MoneyRaiserAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    public GameObject prefab;
    public MoneyRaiserPrefabData prefabData;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        prefabData.prefab = prefab;
        prefabData.entity = conversionSystem.GetPrimaryEntity(prefab); // optional
        dstManager.AddSharedComponentData(entity, prefabData);
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.Add(prefab);
    }
}
