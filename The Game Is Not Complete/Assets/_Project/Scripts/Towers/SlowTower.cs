using UnityEngine;

public class SlowTower : Tower
{
    protected override void SetValues()
    {
        attackDamage = 0;
        attackSpeed = 2;
        attackRange = 10;
        price = 40;
        SetAreaEffect(new AreaSlowEffect(2.5f, 2));
        towerId = TowerId.SlowTower;
    }
}
