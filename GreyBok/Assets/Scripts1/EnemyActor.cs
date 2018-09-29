using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActor : MonoBehaviour
{
    public float speed;
    private PlayerActor player;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerActor>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vector_to_player = player.transform.position - this.transform.position;

        vector_to_player.Normalize();

        this.transform.forward = vector_to_player;

        this.transform.position = this.transform.position + this.transform.forward * speed * Time.deltaTime;
    }
}