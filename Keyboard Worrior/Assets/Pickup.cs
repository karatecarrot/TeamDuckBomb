using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

    [SerializeField]
	public enum PickupType
    {
        health,
        Battery
    }
    public PickupType pickupType;

    public float speed;
    public float pickupDistance;

    private void Update()
    {
        if(Vector3.Distance(transform.position, _GameManager.instance.player.transform.position) < pickupDistance)
            PickUp();
    }

    private void PickUp ()
    {
        float step = speed * Time.deltaTime;

        Vector3.MoveTowards(transform.position, _GameManager.instance.player.transform.position, step);
    }
}
