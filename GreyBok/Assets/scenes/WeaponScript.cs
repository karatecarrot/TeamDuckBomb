using System.Collections;
using UnityEngine.Networking;
using UnityEngine;

public class WeaponScript : MonoBehaviour {

    public string FireButton = "Fire1";             //Button used to Fire
    public GameObject weaponObject;                 // Model Of The Object, Should Be Located On The Hand Root

    [System.Serializable]
    public class WeaponVariables
    {
        public LayerMask whatToHitMask;
        public string weaponName = "AR15";

        public float weaponDamage = 10f;
        public float weaponCrtiDamage = 30f;
        public float maxRange = 100f;
    }
    [Space]
    public WeaponVariables weaponVariables;

    [Header("---Weapon Stats---")]
    public ShootMode shootMode;                     // Referance to The Enum
    public enum ShootMode { Auto, Semi, Burst }
    public float timeBetweenShots;                  //The Weapons Fire Rate
    public float reloadTime;                        //Time Taken to Reload

    [Header("---Burst Stats---")]
    public int burstShots = 3;                      //Shots Per Burst
    public float timeBetweenBurst;

    [HideInInspector] public int burstShotsFired;                    //Shots Taken
    [HideInInspector] public bool burstState;                        //Bool for Burst Fire

    [Header("---Bullet Stats---")]
    public Transform firePoint;                     //The Weapons Fire Point
    public ParticleSystem muzzleFlash;              //The Flash Effect of the Gun;
    public GameObject bulletEffect;

    [HideInInspector] public float shotCounter;                      //Time Counter for Fire Rate

    [Header("---Ammo Stats---")]
    public float currentBullets;                    //Current Amount of Bullets
    public int amountToDeduct = 5;
    [Space]
    [HideInInspector] public float bulletsLeft = 50;                 // Total Bullets Left

    [Header("---Weapon Bools---")]
    [HideInInspector] public bool canFire;                            //If the Weapon can Fire
    [HideInInspector] public bool isReloading;                        //If the Player is Reloading

    [HideInInspector] public bool usingBurst;                         //If the Weapon Type is Burst
    [HideInInspector] public bool usingAuto;                          //If the Weapon Type is Auto
    [HideInInspector] public bool usingSemi;                          //If the Weapon Type is Semi

    private bool shootInput;                        //Bool for the type of Fire

    private void Start ()
    {
        currentBullets = GetComponentInParent<PlayerStats>().energy;
    }
	
	private void Update ()
    {
        FireType();

        BurstFire();

        WeaponShootMode();
        AmmoCounter();

        bulletsLeft = GetComponentInParent<PlayerStats>().energy;
    }

    private void FireType()
    {
        if (shootInput && canFire)
        {
            if (!usingBurst)
            {
                WeaponFire();
            }
            else
                burstState = true;            
        }

        if (shotCounter >= -1)
            shotCounter -= Time.deltaTime;

        //PlayerAnimator playerAnim = GetComponentInParent<PlayerAnimator>();

        //if (playerAnim != null)
        //{
        //    if(canFire)
        //    {
        //        if(usingAuto)
        //            playerAnim.isShooting_Auto = shootInput;
        //        else if (usingBurst)
        //            playerAnim.isShooting_Burst = shootInput;
        //        else if (usingSemi)
        //            playerAnim.isShooting_Semi = shootInput;
        //    }
        //    else
        //    {
        //        playerAnim.isShooting_Auto = false;
        //        playerAnim.isShooting_Burst = false;
        //        playerAnim.isShooting_Semi = false;
        //    }
        //}
    }

    public void RegularFire()
    {
        if (shotCounter <= 0)
        {
            //GetComponentInParent<NetworkAnimator>().SetTrigger("FireWeaponOnce");

            shotCounter = timeBetweenShots;
            currentBullets -= amountToDeduct;
            GetComponentInParent<PlayerStats>().energy -= amountToDeduct;

            Debug.Log("Fire");
            Instantiate(bulletEffect, firePoint.position, firePoint.rotation);

            if (burstState)
                burstShotsFired++;
        }
    }    

    private void BurstFire()
    {
        if (!usingBurst || !burstState)
            return;

        if (burstShotsFired < burstShots)
            WeaponFire();

        if (burstShotsFired == burstShots || isReloading)
        {
            burstState = false;
            burstShotsFired = 0;
            StartCoroutine(Wait());
        }
    }

    public void WeaponFire()
    {
        if (shotCounter <= 0)
        {
            RaycastHit _hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out _hit, weaponVariables.maxRange, weaponVariables.whatToHitMask))
            {
                if (_hit.collider.tag == "CritHit")
                {
                    Debug.Log("CRITICAL!!!");
                    _hit.transform.gameObject.GetComponentInParent<EnemyController>().TakeDamage(weaponVariables.weaponCrtiDamage);
                }
                else if (_hit.collider.tag == "Enemy")
                {
                    Debug.Log("Regular Damage");
                    _hit.transform.gameObject.GetComponentInParent<EnemyController>().TakeDamage(weaponVariables.weaponDamage);
                }
            }
            RegularFire();
        }
    }

    private IEnumerator Wait()
    {
        canFire = false;

        yield return new WaitForSeconds(timeBetweenBurst);

        canFire = true;
    }

    private void WeaponShootMode()
    {
        switch (shootMode)
        {
            case ShootMode.Auto:
                shootInput = Input.GetButton(FireButton);
                usingBurst = false;
                usingAuto = true;
                usingSemi = false;
                break;

            case ShootMode.Semi:
                shootInput = Input.GetButtonDown(FireButton);
                usingBurst = false;
                usingAuto = false;
                usingSemi = true;
                break;

            case ShootMode.Burst:
                shootInput = Input.GetButtonDown(FireButton);
                usingBurst = true;
                usingAuto = false;
                usingSemi = false;
                break;
        }
    }

    private void AmmoCounter()
    {
        if((Input.GetKeyDown(KeyCode.R) && currentBullets < 50) || currentBullets <= 0)
        {
            canFire = false;
            StartCoroutine(DoReload());
        }

        if (currentBullets >= 0 && canFire == false)
        {
            isReloading = false;
            StartCoroutine(DoReload());
        }
    }

    private IEnumerator DoReload()
    {
        if (!isReloading)
        {
            isReloading = true;

            //GetComponentInParent<NetworkAnimator>().SetTrigger("ReloadWeapon");           

            yield return new WaitForSeconds(reloadTime);

            Reload();
        }        
    }

    private void Reload()
    {
        if (bulletsLeft <= 0)
            return;

        float bulletsToLoad = GetComponentInParent<PlayerStats>().energy - currentBullets;
        //                                                         ? = then        : = else
        float bulletsToDeduct = (bulletsLeft >= bulletsToLoad) ? bulletsToLoad : bulletsLeft;    // Amount Needed to Take From Bullets Left       (Smart if Statement)
        // if bulletsLeft >= bulletsToLoad Than use bulletsToLoad, if it is not use bulletsLeft

        bulletsLeft -= bulletsToDeduct;
        currentBullets += bulletsToDeduct;
        isReloading = false;
        canFire = true;
    }
}