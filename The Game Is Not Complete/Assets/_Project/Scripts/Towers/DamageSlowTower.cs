using UnityEngine;

public class DamageSlowTower : Tower
{
    protected override void SetValues()
    {
        attackDamage = 0;
        attackSpeed = 0.1f;
        attackRange = 10;
        price = 80;
        SetAreaEffect(new DamageSlowEffect(2.5f, 2, 0.25f));
        towerId = TowerId.DamageSlowTower;
    }
}
