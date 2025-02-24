using UnityEngine;

public class SnipeTower : Tower
{
    protected override void SetValues()
    {
        attackDamage = 30;
        attackSpeed = 3;
        attackRange = 20;
        price = 100;
        towerId = TowerId.SnipeTower;
    }
}
