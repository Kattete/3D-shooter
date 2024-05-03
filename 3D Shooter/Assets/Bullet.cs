using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void Update()
    {
        Invoke("DestroyItem", 10f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Destroy Enemy
        if(collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.DummyTakeDamage(20);
        }
        
        Destroy(gameObject);
    }

    private void DestroyItem()
    {
        Destroy(gameObject);
    }
}
