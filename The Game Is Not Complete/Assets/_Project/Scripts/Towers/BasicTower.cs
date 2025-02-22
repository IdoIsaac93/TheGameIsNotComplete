using UnityEngine;

public class BasicTower : Tower
{
    protected override void SetValues()
    {
        attackDamage = 5;
        attackSpeed = 1;
        attackRange = 3;
        price = 100;
    }
}
