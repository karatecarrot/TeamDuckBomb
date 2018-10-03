using UnityEngine;

[System.Serializable]
public class PickupSpawnObject
{

    public GameObject objectToSpawn;
    public int count;
    [Range(0, 10)] public float chance;
}
