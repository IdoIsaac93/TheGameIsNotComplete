using System.Collections.Generic;
using UnityEngine;

public class AreaSlowEffect : IAreaEffect
{
    public float slowAmount;
    public float duration;

    public AreaSlowEffect(float slowAmount, float duration)
    {
        this.slowAmount = slowAmount;
        this.duration = duration;
    }

    public void ApplyAreaEffect(HashSet<EnemyController> enemiesInRange)
    {
        foreach (var enemy in enemiesInRange)
        {
            enemy.StartSlow(slowAmount, duration);
        }
    }
}
