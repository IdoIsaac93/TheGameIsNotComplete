using UnityEngine;

public class Item : ScriptableObject
{
    public enum ItemType
    {
        Bomb,
        Freeze,
        Heal
    }

    public ItemType itemType;

    public virtual void UseItem() { }
}