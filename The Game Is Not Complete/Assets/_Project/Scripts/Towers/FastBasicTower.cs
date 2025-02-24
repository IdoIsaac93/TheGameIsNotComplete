using UnityEngine;

public class FastBasicTower : Tower
{
    protected override void SetValues()
    {
        attackDamage = 5;
        attackSpeed = 0.5f;
        attackRange = 10;
        price = 120;
        towerId = TowerId.FastBasicTower;
    }
}
