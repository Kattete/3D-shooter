using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int enemyNumb = 10;
    [SerializeField] private float enemyCooldown = 1f;
    private int xPos;
    private int zPos;
    private int enemyCount;

    private void Update()
    {
        StartCoroutine(EnemySpawn());
    }

    IEnumerator EnemySpawn()
    {
        while(enemyCount < enemyNumb)
        {
            xPos = Random.Range(-1000, 1000);
            zPos = Random.Range(-1000, 1000);
            Instantiate(enemyPrefab, new Vector3(xPos, 30, zPos), Quaternion.identity);
            yield return new WaitForSeconds(enemyCooldown);
            enemyCount++;
        }
    }
}
