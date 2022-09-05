using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshSpawnManager : MonoBehaviour
{
    public static cshSpawnManager Instance;

    [SerializeField] cshSpawnpoint[] spawnpoints;

    void Awake()
    {
        Instance = this;
        //spawnpoints = GetComponentsInChildren<cshSpawnpoint>();
    }

    public Transform GetSpawnpoint()
    {
        int rnd = Random.Range(0, spawnpoints.Length);
        return spawnpoints[rnd].transform;
    }
}
