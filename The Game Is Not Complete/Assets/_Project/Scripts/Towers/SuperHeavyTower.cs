using UnityEngine;

public class SuperHeavyTower : Tower
{
    protected override void SetValues()
    {
        attackDamage = 50;
        attackSpeed = 2;
        attackRange = 8;
        price = 150;
    }

}
