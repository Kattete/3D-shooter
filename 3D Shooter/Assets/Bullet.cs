using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 20;
    private int baseDamage = 20;

    private void Start()
    {
        damage = baseDamage;
    }
    private void Update()
    {
        Invoke("DestroyItem", 10f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Destroy Enemy
        if(collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.DummyTakeDamage(damage);
        }
        
        Destroy(gameObject);
    }

    private void DestroyItem()
    {
        Destroy(gameObject);
    }
}
