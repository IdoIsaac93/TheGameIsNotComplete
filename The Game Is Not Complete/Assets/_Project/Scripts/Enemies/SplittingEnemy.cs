using System.IO;
using UnityEngine;

public class SplittingEnemy : EnemyHealth
{
    public GameObject miniEnemyPrefab;
    public int splitAmount;
    public Enemy self;

    private void Awake()
    {
        self = GetComponent<Enemy>();
        base.Awake();
    }
    public override void Die()
    {
        //TODO//
        //Find PlayerResources and add system points and score
        for (int i = 0; i < splitAmount; i++)
        {
            Enemy newEnemy = Instantiate(miniEnemyPrefab, transform.position, Quaternion.identity).GetComponent<Enemy>();
            newEnemy.path = self.path;
            newEnemy.pathIndex = self.pathIndex;
        }
        Destroy(gameObject);
    }

    //TESTING REMOVE THIS
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Die();
        }
    }
}
