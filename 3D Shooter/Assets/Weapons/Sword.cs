using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public GameObject sword;
    public float attackCooldown;
    public AudioClip attackSound;
    private Animator anim;

    [Header("Attacking")]
    [SerializeField] private float attackDistance = 3f;
    [SerializeField] private float attackDelay = 0.4f;
    [SerializeField] private float attackSpeed = 1f;
    [SerializeField] private int attackDamage = 25;
    [SerializeField] private LayerMask attackLayer;
    [SerializeField] private Camera cam;

    private bool attacking = false;
    private bool canAttack = true;
    private bool readyToAttack = true;
    int attackCount;

    private void OnEnable()
    {
        anim = sword.GetComponent<Animator>();
        attacking = false;
        canAttack = true;
        readyToAttack = true;
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (canAttack)
            {
                Attack();
                SwordAttack();
            }
        }
        if (Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.Alpha3))
        {
            if ((attacking))
            {
                anim.Play("Idle");
            }
        }
    }

    public void SwordAttack()
    {
        canAttack = false;
        anim.SetTrigger("Attack");
        AudioSource audio = GetComponent<AudioSource>();
        audio.PlayOneShot(attackSound);
        StartCoroutine(ResetAttackCooldown());
    }

    IEnumerator ResetAttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    public void Attack()
    {
        if (!readyToAttack || attacking) return;
        readyToAttack = false;
        attacking = true;

        Invoke(nameof(ResetAttack), attackSpeed);
        Invoke(nameof(AttackRayCast), attackDelay);
    }

    private void ResetAttack()
    {
        attacking = false;
        readyToAttack = true;
    }

    private void AttackRayCast()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out  hit, attackDistance, attackLayer))
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.DummyTakeDamage(attackDamage);
            }
        }
    }

}
