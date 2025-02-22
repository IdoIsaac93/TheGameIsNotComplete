using UnityEngine;

public class SuperSlowTower : Tower
{
    protected override void SetValues()
    {
        attackDamage = 0;
        attackSpeed = 2;
        attackRange = 5;
        price = 200;
        SetAreaEffect(new AreaSlowEffect(3, 2));
    }
}
