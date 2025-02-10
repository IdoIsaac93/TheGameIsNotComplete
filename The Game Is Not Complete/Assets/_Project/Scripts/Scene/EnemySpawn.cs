using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public List<Vector3> path;
    public float spawnDelay;
    public SceneController controller;

    private void Awake()
    {
        controller = FindAnyObjectByType<SceneController>();
    }

    public void SpawnWave(Wave currentWave)
    {
        StartCoroutine(SpawnEnemies(currentWave));
    }

    private IEnumerator SpawnEnemies(Wave currentWave)
    {
        foreach (Enemy enemy in currentWave.enemyList)
        {
            // Instantiate a new enemy and assign it a path
            Enemy newEnemy = Instantiate(enemy.enemyPrefab, transform.position, Quaternion.identity).GetComponent<Enemy>();
            newEnemy.path = path;
            // Wait beofre spawning another enemy
            yield return new WaitForSeconds(spawnDelay);
        }

        // Notify SceneController that the wave is completed
        controller.WaveCompleted();
    }
}
