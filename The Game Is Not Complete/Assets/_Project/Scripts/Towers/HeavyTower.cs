using UnityEngine;

public class HeavyTower : Tower
{
    protected override void SetValues()
    {
        attackDamage = 25;
        attackSpeed = 2f;
        attackRange = 3;
        price = 250;
    }
}
