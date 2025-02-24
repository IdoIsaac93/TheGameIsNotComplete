using UnityEngine;

public class DamageSlowTower : Tower
{
    protected override void SetValues()
    {
        attackDamage = 1;
        attackSpeed = 0.1f;
        attackRange = 10;
        price = 200;
        SetAreaEffect(new AreaSlowEffect(2.5f, 2));
        towerId = TowerId.DamageSlowTower;
    }
}
