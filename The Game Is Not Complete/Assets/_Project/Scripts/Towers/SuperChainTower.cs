using UnityEngine;

public class SuperChainTower : Tower
{
    protected override void SetValues()
    {
        attackDamage = 2.5f;
        attackSpeed = 1;
        attackRange = 8;
        price = 70;
        SetAttackEffect(new ChainEffect(5, shootParticleEffect));
        towerId = TowerId.SuperChainTower;
        towerName = "Chain Upgrade";
    }
}
