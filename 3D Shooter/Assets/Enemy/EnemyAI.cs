using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    [Header("Prefab")]
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    [Header("Patroling")]
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    [Header("Attacking")]
    public float timeBetweenAtacks;
    bool alreadyAttacked;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float attackDistance = 5f;
    [SerializeField] private float attackDelay = 1f;
    [SerializeField] private LayerMask layer;
    [SerializeField] private Transform attackPoint;

    [Header("States")]
    public float sightRange;
    public float attackRange;
    public bool playerInSightRange;
    public bool playerInAttackRange;

    private Animator animator;
    


    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        //Check if player in sight or attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();



    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet) agent.SetDestination(walkPoint);

        Vector3 distanceToWalk = transform.position - walkPoint;
        //Walkpoint Reached
        if (distanceToWalk.magnitude < 1f) walkPointSet = false;

    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y,transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            //Attack Code here
            if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Mutant Attack"))
            {
                animator.SetTrigger("Attack");
            }

            Invoke(nameof(Attack), attackDelay);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAtacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void Attack()
    {
        Collider[] hitPlayers = Physics.OverlapSphere(transform.position, attackRange, layer);

        foreach (Collider playerCollider in hitPlayers)
        {
            RaycastHit hit;
            Vector3 rayDirection = (playerCollider.transform.position - attackPoint.position).normalized;
            Debug.DrawRay(attackPoint.position, rayDirection * attackRange, Color.red, 0.5f); // Draw the ray for visualization

            if (Physics.Raycast(attackPoint.position, rayDirection, out hit, attackRange))
            {
                if (hit.collider.gameObject == playerCollider.gameObject)
                {
                    AttackPlayers(playerCollider.gameObject);
                    break;
                }
            }
        }
    }

    private void AttackPlayers(GameObject pl)
    {
        Player player = pl.GetComponent<Player>();
        if(player != null)
        {
            player.TakeDamage(attackDamage);
        }

        Invoke(nameof(ResetAttack), attackDelay);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
