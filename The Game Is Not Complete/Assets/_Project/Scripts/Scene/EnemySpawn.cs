using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public List<Vector3> path;
    public float spawnDelay;

    public void SpawnWave(Wave currentWave)
    {
        StartCoroutine(SpawnEnemies(currentWave));
    }

    private IEnumerator SpawnEnemies(Wave currentWave)
    {
        foreach (EnemyController enemy in currentWave.enemyList)
        {
            // Instantiate a new enemy and assign it a path
            EnemyController newEnemy = Instantiate(enemy.enemyPrefab, transform.position, Quaternion.identity).GetComponent<EnemyController>();
            newEnemy.path = path;
            // Count the enemies spawned to track when wave ends
            SceneController.Instance.numberOfEnemiesAlive++;
            // Wait beofre spawning another enemy
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    //Makes the path easier to see
    private void OnDrawGizmos()
    {
        if (path.Count > 0)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < path.Count; i++)
            {
                Gizmos.DrawSphere(path[i], 0.5f);
                if (i > 0)
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
