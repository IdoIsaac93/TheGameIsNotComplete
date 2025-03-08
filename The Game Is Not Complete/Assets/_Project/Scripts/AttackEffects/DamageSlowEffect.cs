using System.Collections.Generic;
using UnityEngine;

public class DamageSlowEffect : IAreaEffect
{
    public float slowAmount;
    public float duration;
    public float damage;

    public DamageSlowEffect(float slowAmount, float duration, float damage)
    {
        this.slowAmount = slowAmount;
        this.duration = duration;
        this.damage = damage;
    }

    public void ApplyAreaEffect(HashSet<EnemyController> enemiesInRange)
    {
        foreach (var enemy in enemiesInRange)
        {
            enemy.StartSlow(slowAmount, duration);
            enemy.health.TakeDamage(damage);
        }
    }
}

