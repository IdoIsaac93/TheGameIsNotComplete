using System.Collections.Generic;
using UnityEngine;

public interface IAreaEffect
{
    void ApplyAreaEffect(HashSet<EnemyController> enemiesInRange);
}
