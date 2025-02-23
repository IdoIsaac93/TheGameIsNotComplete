using UnityEngine;

public class FastBasicTower : Tower
{
    protected override void SetValues()
    {
        attackDamage = 5;
        attackSpeed = 0.5f;
        attackRange = 3;
        price = 200;
        towerId = TowerId.FastBasicTower;
    }
}
