using System.IO;
using UnityEngine;

public class EnemyHealthSplitter : EnemyHealth
{
    public GameObject miniEnemyPrefab;
    public int splitAmount;
    public EnemyController self;

    //"New" keyword so both this and the base class "awake" methods run
    new private void Awake()
    {
        self = GetComponent<EnemyController>();
    }
    public override void Die()
    {
        //TODO//
        //Find PlayerResources and add system points and score
        for (int i = 0; i < splitAmount; i++)
        {
            EnemyController newEnemy = Instantiate(miniEnemyPrefab, transform.position, Quaternion.identity).GetComponent<EnemyController>();
            newEnemy.path = self.path;
            newEnemy.pathIndex = self.pathIndex;
        }
        Destroy(gameObject);
    }
}