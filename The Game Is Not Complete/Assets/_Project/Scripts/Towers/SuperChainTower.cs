using UnityEngine;

public class SuperChainTower : Tower
{
    protected override void SetValues()
    {
        attackDamage = 5;
        attackSpeed = 1;
        attackRange = 8;
        price = 70;
        SetAttackEffect(new ChainEffect(5, shootParticleEffect));
        towerId = TowerId.SuperChainTower;
        towerName = "Chain Upgrade";
    }
}
