using UnityEngine;

public class SplashHeavyTower : Tower
{
    protected override void SetValues()
    {
        attackDamage = 25;
        attackSpeed = 2f;
        attackRange = 3;
        price = 250;
        SetAttackEffect(new SplashEffect());
        towerId = TowerId.SplashHeavyTower;
    }
}
