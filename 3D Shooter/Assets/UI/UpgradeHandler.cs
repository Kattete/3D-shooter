using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeHandler : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    [SerializeField] private Sword sword;
    [SerializeField] private Bullet gun;
    [SerializeField] private Movement movement;
    [SerializeField] private XpHandler handler;
    private bool checkLVL;

    private void Update()
    {
        if (Input.GetKey(KeyCode.L)) panel.SetActive(true);
        else panel.SetActive(false);
        if (Input.GetKey(KeyCode.L) || Input.GetKey(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if(handler != null)
        {
            if(handler.lvl >= 1)
            {
                checkLVL = true;
            }
            else if(handler.lvl <= 1) {
                checkLVL = false;
            }
        }
    }

    public void AddDamage()
    {
        if(checkLVL)
        {
            gun.damage += 2;
            sword.attackDamage += 2;
            handler.lvl -= 1;
        }
    }

    public void AddAttackSpeed()
    {
        if (checkLVL)
        {
            sword.attackSpeed -= 0.1f;
            handler.lvl -= 1;
        }
    }

    public void AddSpeed()
    {
        if(checkLVL)
        {
            movement.walkSpeed += 0.5f;
            movement.sprintSpeed += 0.5f;
            movement.crouchSpeed += 0.5f;
            handler.lvl -= 1;
            print("INCREASED MOVEMENT");
        }
    }
}
