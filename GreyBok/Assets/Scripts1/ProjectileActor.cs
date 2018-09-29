using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileActor : MonoBehaviour
{
    public PlayerActor owner;

    public float speed;
    public Vector3 direction;
    public float lifetime;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        lifetime -= Time.deltaTime;

        if (lifetime < 0)
        {

            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision hit)
    {
        if (hit.collider.tag == "Enemy")
        {
            Destroy(hit.collider.gameObject);
            owner.score_manager.score++;
            Destroy(gameObject);
        }
    }
}