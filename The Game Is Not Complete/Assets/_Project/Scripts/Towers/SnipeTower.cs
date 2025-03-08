using UnityEngine;

public class SnipeTower : Tower
{
    protected override void SetValues()
    {
        attackDamage = 50;
        attackSpeed = 3;
        attackRange = 20;
        price = 80;
        towerId = TowerId.SnipeTower;
    }
}
