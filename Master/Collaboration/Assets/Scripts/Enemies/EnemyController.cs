using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

    public float health = 100;
    public float damage = 50;
    [Space]
    public GameObject deathEffect;
    public float deathOffset = 1;

    [Header("Sound")]
    public AudioClip deathSound;
    public float soundOffset = 0;
    public AudioClip hitSound;

    #region EnemyTargets
    [SerializeField]
    public enum EnemyTarget
    {
        Player,
        Heart,
        Both
    }
    [Space]
    public EnemyTarget targetType;
    #endregion

    [Header("Attack")]
    public float range = 5;
    public float baseOffset;
    public float attackRange = 5;

    private NavMeshAgent agent;

    private Transform target;

    [SerializeField] private float chaseRadius = 4;
    private bool hasChasedPlayer = false;

    private bool isDead = false;
    private bool attack = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void FixedUpdate ()
    {
        ChooseTarget();

        if (health <= 0 && !isDead)
            Die();

        CheckDistance();
	}

    private void CheckDistance ()
    {
        if (Vector3.Distance(transform.position, target.position) <= range)
        {
            if (attack)
                return;

            GetComponent<Animator>().SetTrigger("Attack");
            agent.baseOffset = baseOffset;
            attack = true;
        }
        else
            attack = false;
    }

    private void ChooseTarget()
    {
        if (isDead)
            return;

        switch (targetType)
        {
            case EnemyTarget.Player:
                target = _GameManager.instance.player.transform;
                agent.SetDestination(target.position);
                break;

            case EnemyTarget.Heart:
                target = _GameManager.instance.heart.transform;
                agent.SetDestination(target.position);
                break;

            case EnemyTarget.Both:

                if (Vector3.Distance(transform.position, _GameManager.instance.player.transform.position) < chaseRadius)
                {
                    target = _GameManager.instance.player.transform;
                    agent.SetDestination(target.position);
                    hasChasedPlayer = true;
                }
                else if (hasChasedPlayer)
                {
                    targetType = EnemyTarget.Heart;
                }
                else
                {
                    target = _GameManager.instance.heart.transform;
                    agent.SetDestination(target.position);

                }
                break;
        }
    }

    private void Die ()
    {
        Debug.Log("Dead");

        StartCoroutine(PlaySound(deathSound));

        GetComponent<Animator>().SetTrigger("Dead");
        Destroy(gameObject, 2.5f);
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = false;
        StartCoroutine(Explosion());
        isDead = true;
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(2.4f);
        Explode();
    }
    IEnumerator PlaySound(AudioClip _Sound)
    {
        yield return new WaitForSeconds(soundOffset);

        AudioSource sound = new GameObject("Audio Effect").AddComponent<AudioSource>();
        sound.transform.position = transform.position;
        sound.PlayOneShot(_Sound);
        Destroy(sound, 7.2f);
    }


    public void TakeDamage (float amount)
    {
        health -= amount;
    }

    public void Attack ()
    {
        Debug.Log("Attack");

        if (target.gameObject == _GameManager.instance.heart)
        {
            targetType = EnemyTarget.Heart;

            if(Vector3.Distance(transform.position, target.position) < attackRange)
                _GameManager.instance.heart.GetComponent<Heart>().TakeAmount(damage);

            StartCoroutine(PlaySound(hitSound));
        }

        if (target.gameObject == _GameManager.instance.player)
        {
            targetType = EnemyTarget.Player;

            if (Vector3.Distance(transform.position, target.position) < attackRange)
                _GameManager.instance.player.GetComponent<PlayerStats>().TakeAmount(damage);

            AudioSource sound = new GameObject("Audio Effect").AddComponent<AudioSource>();
            sound.transform.position = transform.position;
            sound.PlayOneShot(hitSound);
            Destroy(sound, 7.2f);
        }
    }

    public void Explode ()
    {
        GameObject explosion = Instantiate(deathEffect, transform.position, transform.rotation);
        _GameManager.instance.GetComponent<WaveSpawner>().EnemiesAlive--;
        GetComponent<ItemDrop>().Spawn();
        Destroy(gameObject);
        Destroy(explosion, 1);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
    }
}
