using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using System.Collections;

public class Enemy : MonoBehaviour
{
    //Components
    public EnemyHealth health;
    public GameObject enemyPrefab;

    //Movement
    public float moveSpeed;
    public List<Vector3> path;
    public int pathIndex = 0;
    public NavMeshAgent agent;

    //Resource
    public int damage;
    public int resourceWorth;
    public int scoreWorth;

    //Status Effects
    public bool isSlowed = false;
    public float slowTimer = 0;

    private void Awake()
    {
        if (path.Count > 0)
        {
            agent.SetDestination(path[0]);
        }

        // Set speed
        agent.speed = moveSpeed;
    }

    private void Update()
    {
        FollowPath();
    }

    public void FollowPath()
    {
        // Stop when path ends, just in case
        if (pathIndex >= path.Count) return;

        // Move enemy towards target
        agent.SetDestination(path[pathIndex]);

        // When close enough to the target, change to next path point
        if (Vector3.Distance(transform.position, path[pathIndex]) <= 0.5f)
        {
            pathIndex++;
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("SystemCore"))
        {
            //TODO//
            // deal damage to the system core
            // reduce score

            Destroy(gameObject);
        }
    }
    public void StartSlow(float amount, float duration)
    {
        if (!isSlowed)
        {
            StartCoroutine(SlowMoveSpeed(amount, duration));
            isSlowed = true;
        }
        else
        {
            slowTimer = 0;
        }
    }

    private IEnumerator SlowMoveSpeed(float amount, float duration)
    {
        // Save original speed for resetting
        float originalSpeed = agent.speed;
        // Slow
        agent.speed -= amount;
        // Wait duration
        while (slowTimer < duration)
        {
            yield return new WaitForSeconds(1);
            slowTimer++;
        }
        // Reset
        agent.speed = originalSpeed;
        isSlowed = false;
        slowTimer = 0;
    }
}
