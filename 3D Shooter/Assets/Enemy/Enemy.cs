using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int maxHealth;
    [SerializeField] private DummyHealthBar healthBar;

    private void Start()
    {
        health = maxHealth;
        healthBar.SetMaxHealthDummy(maxHealth);
    }

    public void DummyTakeDamage(int damage)
    {
        health -= damage;
        healthBar.SetHealthDummy(health);

        if(health < 0)
        {
            health = maxHealth;
        }
    }
}
