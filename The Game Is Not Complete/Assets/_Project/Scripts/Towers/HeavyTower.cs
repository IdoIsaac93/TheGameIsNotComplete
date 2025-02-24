using UnityEngine;

public class HeavyTower : Tower
{
    protected override void SetValues()
    {
        attackDamage = 25;
        attackSpeed = 2;
        attackRange = 8;
        price = 80;
        towerId = TowerId.HeavyTower;
    }
}
