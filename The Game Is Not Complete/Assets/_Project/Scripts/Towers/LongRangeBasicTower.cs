using UnityEngine;

public class LongRangeBasicTower : Tower
{
    protected override void SetValues()
    {
        attackDamage = 5;
        attackSpeed = 1;
        attackRange = 6;
        price = 200;
    }
}
