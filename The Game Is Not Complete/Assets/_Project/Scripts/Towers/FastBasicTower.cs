using UnityEngine;

public class FastBasicTower : Tower
{
    protected override void SetValues()
    {
        attackDamage = 20;
        attackSpeed = 0.5f;
        attackRange = 10;
        price = 60;
        towerId = TowerId.FastBasicTower;
        towerName = "Speed Upgrade";
    }
}
