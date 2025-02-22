using System.Collections.Generic;
using UnityEngine;

public class SplashEffect : IAttackEffect
{
    private float splashRange = 5;
    public void ApplyEffect(Enemy target)
    {
        Collider[] hitColliders = Physics.OverlapSphere(target.transform.position, splashRange);
        foreach (var hitCollider in hitColliders)
        {
            Enemy enemy = hitCollider.GetComponent<Enemy>();
            if (enemy != null && enemy != target)
            {
                enemy.health.TakeDamage(20);
            }
        }
    }
}
