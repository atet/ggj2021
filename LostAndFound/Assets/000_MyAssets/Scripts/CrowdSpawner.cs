using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdSpawner : MonoBehaviour
{
    [Range(0.1f, 2.0f)]
    public float TimeBetweenSpawns = 0.5f;
    public int PeoplePerSpawn = 10;
    public float SpawnDistance = 50f;
    public GameObject SpawnPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
