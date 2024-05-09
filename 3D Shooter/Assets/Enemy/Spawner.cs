using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;

    private void Start()
    {
        EnemySpawn();
    }
    private void OnEnable()
    {
        Enemy.OnEnemyKilled += EnemySpawn;
    }
    void EnemySpawn()
    {
        int random = Mathf.RoundToInt(Random.Range(0f, spawnPoints.Length - 1));
        Instantiate(enemyPrefab, spawnPoints[random].transform.position, Quaternion.identity);
    }
}
