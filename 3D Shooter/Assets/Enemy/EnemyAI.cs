using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BoxCollider bxLeft;
    [SerializeField] private BoxCollider bxRight;

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

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAtacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void EnableAttack()
    {
        bxLeft.enabled = true;
        bxRight.enabled = true;
    }

    private void DisableAttack()
    {
        bxLeft.enabled = false;
        bxRight.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        var Player = other.GetComponent<Player>();

        if(Player != null)
        {
            Player.TakeDamage(attackDamage);
        }
    }
}
