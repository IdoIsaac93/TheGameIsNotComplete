using UnityEngine;

[CreateAssetMenu(fileName = "HealItem", menuName = "Items/Heal")]
public class HealItem : Item
{
    [SerializeField] private int healAmount;

    public override void UseItem()
    {
        PlayerResources.Instance.HealDamage(healAmount);
    }
}
