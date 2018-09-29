using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActor : MonoBehaviour
{
    [HideInInspector]
    public Vector3 offset;

    public PlayerActor owner;
    public CameraActor camera_actor;
    public ParticleSystem hitscan_system;
    public ScoreManager score_manager;
    public GameObject projectile_prefab;

    public float speed = 5.0f;

    private CharacterController controller;

    public enum WeaponType
    {
        WEAPON_HITSCAN,
        WEAPON_SINGLESHOT,
        WEAPON_SHOTGUN,
    }

    public WeaponType weapon_type;

    private Vector3 PlatformGetPlayerFireDirection()
    {
        Vector3 mouse_pos = Input.mousePosition;
        // Use the current camera to convert it to a ray
        Ray mouse_ray = Camera.main.ScreenPointToRay(mouse_pos);
        // Create a plane that faces up at the same position as the player
        Plane player_plane = new Plane(Vector3.up, transform.position);
        // Find out how far along the ray the intersection is
        float ray_distance = 0;
        player_plane.Raycast(mouse_ray, out ray_distance);
        // find the collision point from the distance
        Vector3 cast_point = mouse_ray.GetPoint(ray_distance);

        Vector3 to_cast_point = cast_point - transform.position;
        to_cast_point.Normalize();

        return to_cast_point;
    }

    bool PlatformPlayerShouldFire()
    {
        return Input.GetMouseButtonDown(0);
    }

    bool PlatformPlayerSwitchGuns()
    {
        return Input.GetMouseButtonDown(1);
    }

    void SwitchWeapon()
    {
        if (weapon_type == WeaponType.WEAPON_HITSCAN)
        {
            weapon_type = WeaponType.WEAPON_SINGLESHOT;
        }

        else if (weapon_type == WeaponType.WEAPON_SINGLESHOT)
        {
            weapon_type = WeaponType.WEAPON_SHOTGUN;
        }

        else
        {
            weapon_type = WeaponType.WEAPON_HITSCAN;
        }
    }

    void FireHitscan()
    {
        // Changed all the raycast code to this function call
        Vector3 fire_direction = PlatformGetPlayerFireDirection();

        RaycastHit info;
        Ray fire_ray = new Ray(transform.position, fire_direction);

        if (Physics.Raycast(fire_ray, out info, 1000))
        {
            if (info.collider.tag == "Enemy")
            {
                score_manager.score++;
                Destroy(info.collider.gameObject);
            }

            if (hitscan_system != null)
            {
                hitscan_system.Play();
            }
        }
    }

    void FireSingleShot()
    {
        GameObject projectile = (GameObject)Instantiate(projectile_prefab);

        projectile.transform.position = transform.position;

        ProjectileActor projectile_component = projectile.GetComponent<ProjectileActor>();

        projectile_component.owner = this;

        projectile_component.direction = PlatformGetPlayerFireDirection();
    }

    void FireShotGun()
    {
        for (int i = 0; i < 100; i++)
        {
            GameObject projectile = (GameObject)Instantiate(projectile_prefab);

            projectile.transform.position = transform.position;

            ProjectileActor projectile_component = projectile.GetComponent<ProjectileActor>();

            projectile_component.owner = this;

            projectile_component.direction = PlatformGetPlayerFireDirection();
        }
    }


    // Use this for initialization
    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float h_input = Input.GetAxis("Horizontal");
        float v_input = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(h_input, 0, v_input);

        direction *= Time.deltaTime * speed;
        controller.Move(direction);

        Vector3 fire_direction = PlatformGetPlayerFireDirection();
        transform.forward = fire_direction;
        camera_actor.offset = fire_direction;

        if (PlatformPlayerShouldFire())
        {
            switch (weapon_type)
            {
                case WeaponType.WEAPON_HITSCAN:
                    FireHitscan();
                    break;
                case WeaponType.WEAPON_SINGLESHOT:
                    FireSingleShot();
                    break;
                case WeaponType.WEAPON_SHOTGUN:
                    FireShotGun();
                    break;
                default:
                    break;
            }
        }

        if (PlatformPlayerSwitchGuns())
        {
            SwitchWeapon();
        }
    }
}