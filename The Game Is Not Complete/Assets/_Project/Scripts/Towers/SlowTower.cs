using UnityEngine;

public class SlowTower : Tower
{
    protected override void SetValues()
    {
        attackDamage = 0;
        attackSpeed = 2;
        attackRange = 5;
        price = 200;
        SetAreaEffect(new AreaSlowEffect(2.5f, 2));
        towerId = TowerId.SlowTower;
    }
}
