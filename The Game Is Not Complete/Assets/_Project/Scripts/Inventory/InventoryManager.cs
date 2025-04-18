using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class InventoryManager : Singleton<InventoryManager>, IDataPersistance
{
    private Dictionary<Item.ItemType, int> itemInventory = new Dictionary<Item.ItemType, int>();

    //Actions to control UI
    public static event UnityAction <int> ItemAdded = delegate { };
    public static event UnityAction<int> ItemRemoved = delegate { };

    [Header("Drop Chance")]
    [SerializeField] private float baseDropChance = 0.20f;
    [SerializeField] private float reductionPerItem = 0.02f;

    [Header("Item References")]
    [SerializeField] private BombItem bombItem;
    [SerializeField] private FreezeItem freezeItem;
    [SerializeField] private HealItem healItem;

    private void OnEnable()
    {
        EnemyHealth.OnEnemyDeath += EnemyDeath;
    }

    private void OnDisable()
    {
        EnemyHealth.OnEnemyDeath -= EnemyDeath;
    }

    public void EnemyDeath()
    {
        // Calculate the drop chance
        float dropChance = baseDropChance - (itemInventory.Values.Sum() * reductionPerItem);
        dropChance = Mathf.Clamp(dropChance, 0f, 1f);

        // Randomize a value between 0 and 1
        if (Random.value <= dropChance)
        {
            // Get all values of the ItemType enum
            Item.ItemType[] itemTypes = (Item.ItemType[])System.Enum.GetValues(typeof(Item.ItemType));

            // Choose a random item from the enum
            int randomIndex = Random.Range(0, itemTypes.Length);
            Item.ItemType randomItem = itemTypes[randomIndex];

            // Add the random item to the inventory
            AddItem(randomItem);
        }
    }



    public void AddItem(Item.ItemType itemType)
    {
        int quantity = 1;
        if (itemInventory.ContainsKey(itemType))
        {
            itemInventory[itemType] += quantity;
        }
        else
        {
            itemInventory[itemType] = quantity;
        }
        ItemAdded?.Invoke(quantity);
    }

    // Use item
    public void UseItem(Item.ItemType itemType)
    {
        //Check that you have at least 1 of the item
        if (itemInventory.ContainsKey(itemType) && itemInventory[itemType] > 0)
        {
            Item itemToUse = null;

            // Determine the correct item based on its type
            switch (itemType)
            {
                case Item.ItemType.Bomb:
                    itemToUse = bombItem;
                    break;
                case Item.ItemType.Freeze:
                    itemToUse = freezeItem;
                    break;
                case Item.ItemType.Heal:
                    itemToUse = healItem;
                    break;
            }

            // Use item and decrease its count
            if (itemToUse != null)
            {
                itemToUse.UseItem();
                itemInventory[itemType] -= 1;
                ItemRemoved?.Invoke(-1);
                Debug.Log($"{itemType} used!");
            }
        }
        else
        {
            Debug.Log($"Not enough {itemType}s to use!");
        }
    }

    public void LoadData(GameData data)
    {
        itemInventory = data.items;
    }

    public void SaveData(ref GameData data)
    {
        data.items = itemInventory;
    }

    public int GetItemCount(Item.ItemType itemType)
    {
        return itemInventory.ContainsKey(itemType) ? itemInventory[itemType] : 0;
    }


    //TESTING - REMOVE THIS
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            EnemyDeath();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            UseItem(Item.ItemType.Bomb);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            UseItem(Item.ItemType.Freeze);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            UseItem(Item.ItemType.Heal);
        }
    }
}
