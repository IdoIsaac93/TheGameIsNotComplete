using UnityEngine;

public class SuperSnipeTower : Tower
{
    protected override void SetValues()
    {
        attackDamage = 30;
        attackSpeed = 3;
        attackRange = 1000;
        price = 300;
        towerId = TowerId.SuperSnipeTower;
    }
}
