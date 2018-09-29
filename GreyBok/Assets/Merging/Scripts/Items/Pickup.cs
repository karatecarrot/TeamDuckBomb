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

    public float pickupAmount;

    [Header("Pickup")]
    [SerializeField] private float speed;
    [SerializeField] private float pickupDistance;

    private bool pickupObject;

    private void Update()
    {
        if (Vector3.Distance(transform.position, _GameManager.instance.player.transform.position) < pickupDistance)
            pickupObject = true;

        if(pickupObject)
            PickUp();

        if (Vector3.Distance(transform.position, _GameManager.instance.player.transform.position) < .01f)
        {
            switch (pickupType)
            {
                case PickupType.health:
                    Debug.Log("Health Pickup");
                    _GameManager.instance.player.GetComponent<PlayerStats>().AddAmount(pickupAmount, PickupType.health);
                    break;

                case PickupType.Battery:
                    Debug.Log("Battery Pickup");
                    _GameManager.instance.player.GetComponent<PlayerStats>().AddAmount(pickupAmount, PickupType.Battery);
                    break;
            }
            Destroy(gameObject);
        }
    }

    private void PickUp ()
    {
        float step = speed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, _GameManager.instance.player.transform.position, step);
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, step / 2);
    }

    private void OnDrawGizmos()
    {
        switch (pickupType)
        {
            case PickupType.health:
                Gizmos.color = Color.red;
                break;
            case PickupType.Battery:
                Gizmos.color = Color.yellow;
                break;
        }
        Gizmos.DrawWireSphere(transform.position, pickupDistance);
    }
}
