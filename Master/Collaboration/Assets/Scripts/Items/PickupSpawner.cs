using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour {

    [Header("Repeating Spawn Rate")]
    public float minTime = 10;
    public float maxTime = 20;

    [Space]
    public PickupSpawnObject[] objectsToSpawn;
    [Space]
    public Transform[] spawnPoints;

    [HideInInspector] public float _totalSpawnWeight;

    [HideInInspector] public float cumulativeWeight;
    [HideInInspector] public int chosenIndex = 0;


    void Awake()
    {
        OnValidate();
    }

    void OnValidate()
    {
        _totalSpawnWeight = 0f;

        foreach (PickupSpawnObject spawnable in objectsToSpawn)
            _totalSpawnWeight += spawnable.chance;
    }

    private void Start()
    {
        InvokeRepeating("SpawnObject", 0, Random.Range(minTime, maxTime));
    }

    private void SpawnObject ()
    {
        Spawn();
    }

    public void Spawn()
    {
        float pick = Random.value * _totalSpawnWeight;
        chosenIndex = 0;
        cumulativeWeight = objectsToSpawn[0].chance;

        // Step through the list until we've accumulated more weight than this.
        // The length check is for safety in case rounding errors accumulate.
        while (pick > cumulativeWeight && chosenIndex < objectsToSpawn.Length - 1)
        {
            chosenIndex++;
            cumulativeWeight += objectsToSpawn[chosenIndex].chance;
        }

        for (int i = 0; i <= Random.Range(0, objectsToSpawn[chosenIndex].count); i++)
        {
            int spawnNumber = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[spawnNumber];

            GameObject spawnedObject = Instantiate(objectsToSpawn[chosenIndex].objectToSpawn, RandomPointInBox(spawnPoint.position, spawnPoint.localScale), spawnPoint.rotation);
            Debug.LogWarning("Spawned: " + spawnedObject.name);
        }
    }

    private static Vector3 RandomPointInBox(Vector3 center, Vector3 size)
    {

        return center + new Vector3((Random.value - 0.5f) * size.x, 0, (Random.value - 0.5f) * size.z);
    }
}
