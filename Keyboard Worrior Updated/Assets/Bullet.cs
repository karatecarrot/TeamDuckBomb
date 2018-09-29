using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed = 2;

    private void Start()
    {
        Destroy(gameObject, 3);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed);

        if(GetComponent<LineRenderer>().GetPosition(1) != new Vector3(0, 0, 3))
            GetComponent<LineRenderer>().SetPosition(1, new Vector3(0, 0, Mathf.Lerp(0, 3, speed)));
    }
}