using UnityEngine;

public class SplashHeavyTower : Tower
{
    protected override void SetValues()
    {
        attackDamage = 40;
        attackSpeed = 2f;
        attackRange = 8;
        price = 80;
        SetAttackEffect(new SplashEffect());
        towerId = TowerId.SplashHeavyTower;
    }
}
