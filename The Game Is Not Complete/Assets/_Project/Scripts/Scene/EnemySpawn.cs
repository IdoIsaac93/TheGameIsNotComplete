using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public List<Vector3> path;
    public float spawnDelay;
    //public Wave currentWave;

    public void SpawnWave(Wave currentWave)
    {
        foreach (Enemy enemy in currentWave.enemyList)
        {
            Instantiate(enemy.enemyPrefab, transform.position, Quaternion.identity);
            enemy.path = path;
            StartCoroutine(SpawnDelay());
        }
    }

    public IEnumerator SpawnDelay()
    {
        yield return new WaitForSeconds(spawnDelay);
    }
}