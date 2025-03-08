using UnityEngine;

public class LongRangeBasicTower : Tower
{
    protected override void SetValues()
    {
        attackDamage = 20;
        attackSpeed = 1;
        attackRange = 15;
        price = 50;
        towerId = TowerId.LongRangeBasicTower;
    }
}
