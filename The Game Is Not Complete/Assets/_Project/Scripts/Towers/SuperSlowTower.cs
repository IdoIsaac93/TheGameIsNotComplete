using UnityEngine;

public class SuperSlowTower : Tower
{
    protected override void SetValues()
    {
        attackDamage = 0;
        attackSpeed = 2;
        attackRange = 10;
        price = 200;
        SetAreaEffect(new AreaSlowEffect(3, 2));
        towerId = TowerId.SuperSlowTower;
        towerName = "Slow Upgrade";
    }
}
