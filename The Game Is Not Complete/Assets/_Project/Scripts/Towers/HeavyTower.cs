using UnityEngine;

public class HeavyTower : Tower
{
    protected override void SetValues()
    {
        attackDamage = 40;
        attackSpeed = 2;
        attackRange = 8;
        price = 60;
        towerId = TowerId.HeavyTower;
    }
}
