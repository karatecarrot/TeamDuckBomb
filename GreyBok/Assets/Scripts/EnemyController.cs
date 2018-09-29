using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

    #region EnemyTargets
    [SerializeField]
    public enum EnemyTarget
    {
        Player,
        Heart,
        Both
    }
    public EnemyTarget target;
    #endregion

    private NavMeshAgent agent;

    [SerializeField] private float chaseRadius = 4;
    private bool hasChasedPlayer = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void FixedUpdate ()
    {
        ChooseTarget();
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
    }
}
