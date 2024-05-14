using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeHandler : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    [SerializeField] private Sword sword;
    [SerializeField] private Bullet gun;
    [SerializeField] private Movement movement;


    private void Update()
    {
        if(Input.GetKey(KeyCode.L)) panel.SetActive(true);
        else panel.SetActive(false);
    }

    public void AddDamage()
    {
        gun.damage += 2;
        sword.attackDamage += 2;
    }

    public void AddAttackSpeed()
    {
        sword.attackSpeed -= 0.1f;
    }

    public void AddSpeed()
    {
        movement.walkSpeed += 0.5f;
        movement.sprintSpeed += 0.5f;
        movement.crouchSpeed += 0.5f;
    }
}
