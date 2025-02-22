using UnityEngine;

public class SnipeTower : Tower
{
    protected override void SetValues()
    {
        attackDamage = 50;
        attackSpeed = 3f;
        attackRange = 20;
        price = 300;
    }
}
