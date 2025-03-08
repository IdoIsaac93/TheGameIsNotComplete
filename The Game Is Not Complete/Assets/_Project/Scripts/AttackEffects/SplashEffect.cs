using System.Collections.Generic;
using UnityEngine;

public class SplashEffect : IAttackEffect
{
    private float splashRange = 6;
    public void ApplyEffect(EnemyController target)
    {
        Collider[] hitColliders = Physics.OverlapSphere(target.transform.position, splashRange);
        foreach (var hitCollider in hitColliders)
        {
            EnemyController enemy = hitCollider.GetComponent<EnemyController>();
            if (enemy != null && enemy != target)
            {
                enemy.health.TakeDamage(20);
            }
        }
    }
}
