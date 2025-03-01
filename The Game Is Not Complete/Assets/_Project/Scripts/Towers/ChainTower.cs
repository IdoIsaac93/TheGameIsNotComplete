using UnityEngine;

public class ChainTower : Tower
{
    protected override void SetValues()
    {
        attackDamage = 2.5f;
        attackSpeed = 1;
        attackRange = 8;
        price = 50;
        SetAttackEffect(new ChainEffect(2));
        towerId = TowerId.ChainTower;
    }
}
