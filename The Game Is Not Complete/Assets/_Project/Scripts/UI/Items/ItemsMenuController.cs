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
    }

    private void OnItemButtonClicked(int itemIndex)
    {
        
        Debug.Log($"Item {itemIndex} clicked");
        
    }

}
