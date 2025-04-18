using UnityEngine;
using UnityEngine.UIElements;

public class ItemsMenuController : MonoBehaviour
{
    private UIDocument _uiDocument;
    private VisualElement _root;
    private Button _itemOne;
    private Button _itemTwo;
    private Button _itemThree;
    private Label _itemOneCountLabel;
    private Label _itemTwoCountLabel;
    private Label _itemThreeCountLabel;

    private void Start()
    {
        _uiDocument = GetComponent<UIDocument>();
        _root = _uiDocument.rootVisualElement;
        _itemOne = _root.Q<Button>("Item1");
        _itemTwo = _root.Q<Button>("Item2");
        _itemThree = _root.Q<Button>("Item3");
        _itemOneCountLabel = _root.Q<Label>("Count1");
        _itemTwoCountLabel = _root.Q<Label>("Count2");
        _itemThreeCountLabel = _root.Q<Label>("Count3");
        _itemOne.clicked += () => OnItemButtonClicked(1);
        _itemTwo.clicked += () => OnItemButtonClicked(2);
        _itemThree.clicked += () => OnItemButtonClicked(3);
        // Initialize the inventory display
        _itemOneCountLabel.text = InventoryManager.Instance.GetItemCount(Item.ItemType.Bomb).ToString();
        _itemTwoCountLabel.text = InventoryManager.Instance.GetItemCount(Item.ItemType.Freeze).ToString();
        _itemThreeCountLabel.text = InventoryManager.Instance.GetItemCount(Item.ItemType.Heal).ToString();
    }

    private void OnEnable()
    {
        InventoryManager.ItemAdded += UpdateInventoryDisplay;
        InventoryManager.ItemRemoved += UpdateInventoryDisplay;
        
    }

    private void OnDisable()
    {
        InventoryManager.ItemAdded -= UpdateInventoryDisplay;
        InventoryManager.ItemRemoved -= UpdateInventoryDisplay;
    }


    private void UpdateInventoryDisplay(int _)
    {
        // Safely get the counts from InventoryManager
        int bombCount = InventoryManager.Instance.GetItemCount(Item.ItemType.Bomb);
        int freezeCount = InventoryManager.Instance.GetItemCount(Item.ItemType.Freeze);
        int healCount = InventoryManager.Instance.GetItemCount(Item.ItemType.Heal);

        _itemOneCountLabel.text = bombCount.ToString();
        _itemTwoCountLabel.text = freezeCount.ToString();
        _itemThreeCountLabel.text = healCount.ToString();
    }

    private void OnItemButtonClicked(int itemIndex)
    {
        switch (itemIndex)
        {
            case 1:
                InventoryManager.Instance.UseItem(Item.ItemType.Bomb);
                break;
            case 2:
                InventoryManager.Instance.UseItem(Item.ItemType.Freeze);
                break;
            case 3:
                InventoryManager.Instance.UseItem(Item.ItemType.Heal);
                break;
        }
    }

}
