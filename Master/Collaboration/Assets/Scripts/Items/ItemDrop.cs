using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour {

    public PickupSpawnObject[] objectsToSpawn;
    
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
            GameObject spawnedObject = Instantiate(objectsToSpawn[chosenIndex].objectToSpawn, transform.position, transform.rotation);
            Debug.LogWarning("Spawned: " + spawnedObject.name);
        }
    }
}
