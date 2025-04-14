using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BombItem", menuName = "Items/Bomb")]
public class BombItem : Item
{
    [SerializeField] private float damageAmount;

    public override void UseItem()
    {
        // Find all enemies
        EnemyHealth[] enemies = FindObjectsByType<EnemyHealth>(FindObjectsSortMode.None);

        // Deal damage to them
        foreach (EnemyHealth enemy in enemies)
        {
            enemy.TakeDamage(damageAmount);
        }
    }
}
