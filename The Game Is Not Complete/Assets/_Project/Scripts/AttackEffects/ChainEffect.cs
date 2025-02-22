using System.Collections.Generic;
using UnityEngine;

public class ChainEffect : IAttackEffect
{
    private float chainRange = 8;

    public void ApplyEffect(Enemy target)
    {
        if (target == null) return;

        // Keep track of the last affected enemy and affected enemies in this chain
        Enemy lastAffectedEnemy = target;
        HashSet<Enemy> affectedEnemies = new HashSet<Enemy> { target }; // Track already affected enemies

        for (int i = 0; i < 2; i++) // Apply to two closest enemies
        {
            // Find nearby enemies within range of the last affected enemy
            Collider[] hitColliders = Physics.OverlapSphere(lastAffectedEnemy.transform.position, chainRange);
            List<Enemy> nearbyEnemies = new List<Enemy>();

            foreach (var hitCollider in hitColliders)
            {
                Enemy enemy = hitCollider.GetComponent<Enemy>();
                // Add enemy to the list if it's valid and hasn't been affected yet
                if (enemy != null && enemy != lastAffectedEnemy && !affectedEnemies.Contains(enemy))
                {
                    nearbyEnemies.Add(enemy);
                }
            }

            // Sort the nearby enemies by distance to the last affected enemy
            nearbyEnemies.Sort((a, b) =>
                Vector3.Distance(lastAffectedEnemy.transform.position, a.transform.position)
                .CompareTo(Vector3.Distance(lastAffectedEnemy.transform.position, b.transform.position)));

            // Check if there are any nearby enemies to apply the effect to
            if (nearbyEnemies.Count > 0)
            {
                // Apply the effect to the closest enemy
                lastAffectedEnemy = nearbyEnemies[0];
                lastAffectedEnemy.health.TakeDamage(2.5f);

                // Add this enemy to the affected enemies set
                affectedEnemies.Add(lastAffectedEnemy);
            }
            else
            {
                // If no enemies are left, break the loop
                break;
            }
        }
    }
}
