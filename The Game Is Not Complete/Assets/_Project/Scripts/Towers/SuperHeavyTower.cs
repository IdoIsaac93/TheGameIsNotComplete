using UnityEngine;

public class SuperHeavyTower : Tower
{
    protected override void SetValues()
    {
        attackDamage = 50;
        attackSpeed = 2f;
        attackRange = 3;
        price = 250;
    }

}
