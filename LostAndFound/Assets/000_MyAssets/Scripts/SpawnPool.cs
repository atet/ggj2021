using UnityEngine;
using UltimateSpawner;
using KennethDevelops.ProLibrary.Managers;

public class SpawnPool : MonoBehaviour
{
    PoolManager pool;

    void Awake()
    {
        UltimateSpawning.OnUltimateSpawnerInstantiate = HandleSpawnerInstantiate;
        pool = PoolManager.GetInstance("CrowdPool");
    }

    Object HandleSpawnerInstantiate(Object prefab, Vector3 position, Quaternion rotation)
    {
        // return Object.Instantiate(prefab, position, rotation);
        return pool.AcquireObject<CrowdPerson>(position, rotation);
    }
}
