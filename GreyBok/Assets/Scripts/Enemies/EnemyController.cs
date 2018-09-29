using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

    public float health = 100;
    public GameObject deathEffect;
    public float deathOffset = 1;

    #region EnemyTargets
    [SerializeField]
    public enum EnemyTarget
    {
        Player,
        Heart,
        Both
    }
    [Space]
    public EnemyTarget target;
    #endregion

    private NavMeshAgent agent;

    [SerializeField] private float chaseRadius = 4;
    private bool hasChasedPlayer = false;

    private bool isDead = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void FixedUpdate ()
    {
        health = Mathf.Clamp(health, 0, 100);
        ChooseTarget();

        if (health <= 0 && !isDead)
            Die();
	}

    private void ChooseTarget()
    {
        switch (target)
        {
            case EnemyTarget.Player:
                agent.SetDestination(_GameManager.instance.player.transform.position);
                break;

            case EnemyTarget.Heart:
                agent.SetDestination(_GameManager.instance.heart.transform.position);
                break;

            case EnemyTarget.Both:

                if(Vector3.Distance(transform.position, _GameManager.instance.player.transform.position) < chaseRadius)
                {
                    agent.SetDestination(_GameManager.instance.player.transform.position);
                    hasChasedPlayer = true;
                }
                else if (hasChasedPlayer)
                {
                    target = EnemyTarget.Heart;
                }
                else
                    agent.SetDestination(_GameManager.instance.heart.transform.position);

                break;
        }
    }

    private void Die ()
    {
        Debug.Log("Dead");
        GetComponent<Animator>().SetTrigger("Dead");
        Destroy(gameObject, 2.5f);
        _GameManager.instance.GetComponent<WaveSpawner>().EnemiesAlive--;
        GetComponent<NavMeshAgent>().speed = 0;
        GetComponent<NavMeshAgent>().angularSpeed = 0;
        GetComponent<NavMeshAgent>().baseOffset = deathOffset;
        StartCoroutine(Explosion());
        isDead = true;
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(2.4f);
        GameObject explosion = Instantiate(deathEffect, transform.position, transform.rotation);
        Destroy(explosion, 1);
    }

    public void TakeDamage (float amount)
    {
        health -= amount;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
    }
}
