using UnityEngine;

public class SuperChainTower : Tower
{
    protected override void SetValues()
    {
        attackDamage = 2.5f;
        attackSpeed = 1;
        attackRange = 8;
        price = 200;
        SetAttackEffect(new ChainEffect(5));
        towerId = TowerId.SuperChainTower;
    }
}
