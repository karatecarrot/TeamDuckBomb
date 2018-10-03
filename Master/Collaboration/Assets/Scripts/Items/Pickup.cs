using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

    [SerializeField]
	public enum PickupType
    {
        health,
        Battery,
        Both
    }
    public PickupType pickupType;

    public float pickupAmount;
    public float lifeTime = 25;
    public AudioClip pickupSound;

    private bool destroy = false;

    [Header("Pickup")]
    [SerializeField] private float speed;
    [SerializeField] private float pickupDistance;

    private bool pickupObject;

    private bool canPickup = true; 

    private void Start()
    {
        StartCoroutine(LifeTimeEnd());
    }

    private void Update()
    {

        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, 5, transform.position.z), 1f * Time.deltaTime);

        if (Vector3.Distance(transform.position, _GameManager.instance.player.transform.position) < pickupDistance)
            pickupObject = true;

        if(pickupObject && canPickup)
            PickUp();

        if (Vector3.Distance(transform.position, _GameManager.instance.player.transform.position) < .01f)
        {
            switch (pickupType)
            {
                case PickupType.health:
                    Debug.Log("Health Pickup");
                    _GameManager.instance.player.GetComponent<PlayerStats>().AddAmount(pickupAmount, PickupType.health);

                    AudioSource HealthSound = new GameObject("Audio Effect").AddComponent<AudioSource>();
                    HealthSound.transform.position = transform.position;
                    HealthSound.PlayOneShot(pickupSound);
                    HealthSound.volume = 0.7f;
                    Destroy(HealthSound, 5f);

                    break;

                case PickupType.Battery:
                    Debug.Log("Battery Pickup");
                    _GameManager.instance.player.GetComponent<PlayerStats>().AddAmount(pickupAmount, PickupType.Battery);

                    AudioSource BatterySound = new GameObject("Audio Effect").AddComponent<AudioSource>();
                    BatterySound.transform.position = transform.position;
                    BatterySound.PlayOneShot(pickupSound);
                    BatterySound.volume = 0.7f;
                    Destroy(BatterySound, 5f);

                    break;

                case PickupType.Both:
                    Debug.Log("Battery Health Pack Pickup");
                    _GameManager.instance.player.GetComponent<PlayerStats>().AddAmount(pickupAmount, PickupType.Both);

                    AudioSource PackSound = new GameObject("Audio Effect").AddComponent<AudioSource>();
                    PackSound.transform.position = transform.position;
                    PackSound.PlayOneShot(pickupSound);
                    PackSound.volume = 0.7f;
                    Destroy(PackSound, 5f);

                    break;
            }
            Destroy(gameObject);
        }

        if(destroy)
        {
            float step = speed * Time.deltaTime;
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, step / 2);

            if (transform.localScale == Vector3.zero)
                Destroy(gameObject);

            canPickup = false;
        }
    }

    private void PickUp ()
    {
        float step = speed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, _GameManager.instance.player.transform.position, step);
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, step / 2);
    }

    IEnumerator LifeTimeEnd ()
    {
        yield return new WaitForSeconds(lifeTime);
        destroy = true;
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
            case PickupType.Both:
                Gizmos.color = Color.magenta;
                break;
        }
        Gizmos.DrawWireSphere(transform.position, pickupDistance);
    }
}
