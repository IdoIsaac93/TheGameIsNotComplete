using UnityEngine;

public class SplashHeavyTower : Tower
{
    protected override void SetValues()
    {
        attackDamage = 25;
        attackSpeed = 2f;
        attackRange = 8;
        price = 200;
        SetAttackEffect(new SplashEffect());
        towerId = TowerId.SplashHeavyTower;
    }
}
