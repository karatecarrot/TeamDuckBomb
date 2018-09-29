using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnActor : MonoBehaviour
{
    public GameObject enemy_prefab;
    public float spawn_time; // seconds between spawns
    public float spawn_radius; // distance from player to spawn

    private PlayerActor player;
    private float spawn_timer; // The timer that counts and controls spawning

    // Use this for initialization
    void Start()
    {
        spawn_timer = spawn_time;
        player = GameObject.FindObjectOfType<PlayerActor>();
    }

    // Update is called once per frame
    void Update()
    {
        // Count our timer down each frame
        spawn_timer -= Time.deltaTime;

        if (spawn_timer < 0)
        {
            // reset the timer
            spawn_timer = spawn_time;

            // Pick a random angle in radians and set the spawn point.
            float spawn_angle = Random.Range(0, 2 * Mathf.PI);
            Vector3 spawn_direction = new Vector3(Mathf.Sin(spawn_angle), 0, Mathf.Cos(spawn_angle));
            spawn_direction *= spawn_radius;

            Vector3 spawn_point = player.transform.position + spawn_direction;
            // Spawn the enemy at the desired location
            Instantiate(enemy_prefab, spawn_point, Quaternion.identity);
        }
    }
}