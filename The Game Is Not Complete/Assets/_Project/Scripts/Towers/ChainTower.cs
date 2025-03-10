using UnityEngine;

public class ChainTower : Tower
{
    protected override void SetValues()
    {
        attackDamage = 10;
        attackSpeed = 1;
        attackRange = 8;
        price = 50;
        SetAttackEffect(new ChainEffect(2, shootParticleEffect));
        towerId = TowerId.ChainTower;
        towerName = "Chain Tower";
    }
}
