using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class BuildMenu : MonoBehaviour
{
    public BuildSpot currentSpot;
    public Tower[] availableTowers;
    [SerializeField] private List<Button> buttons;

    [SerializeField] Transform container;
    [SerializeField] Transform buttonTemplate;

    private void Awake()
    {
        if (buttonTemplate != null)
        {
            buttonTemplate.gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        if (buttons.Count >= availableTowers.Length)
        {
            for (int i = 0; i < availableTowers.Length; i++)
            {
                // Check if the player has enough points to buy the tower
                bool hasEnoughPoints = PlayerResources.Instance.GetSystemPoints() >= availableTowers[i].GetPrice();

                // Enable or disable the button accordingly
                buttons[i].interactable = hasEnoughPoints;
            }
        }
    }
    public void OpenMenu(BuildSpot spot, Tower[] towers)
    {
        gameObject.SetActive(true);
        currentSpot = spot;
        availableTowers = towers;
        UpdateList();
    }

    public void UpdateList()
    {
        // Clear existing buttons
        foreach (var button in buttons)
        {
            Destroy(button.gameObject);
        }
        buttons = new();

        // Create new buttons for the available towers
        for (int i = 0; i < availableTowers.Length; i++)
        {
            CreateButton(availableTowers[i], i);
        }

        // Update the container's width
        float totalWidth = availableTowers.Length * 200f;
        container.GetComponent<RectTransform>().sizeDelta = new Vector2(totalWidth, container.GetComponent<RectTransform>().sizeDelta.y);
    }

    public void CreateButton(Tower tower, int index)
    {
        // Instantiate a new button from the template
        Transform newButton = Instantiate(buttonTemplate, container);
        newButton.gameObject.SetActive(true);

        //Move the button to the side
        float buttonWidth = 200f;
        newButton.localPosition = new Vector3(index * buttonWidth, 0, 0);
        Debug.Log(newButton.position);

        // Set the name of the tower/upgrade
        newButton.GetComponentInChildren<TextMeshProUGUI>().text = tower.GetTowerName();

        // Set the OnClick function of the button
        newButton.GetComponent<Button>().onClick.AddListener(() => { BuyTower(tower); });

        // Add the button to the list of buttons
        buttons.Add(newButton.GetComponent<Button>());
    }

    public void CloseMenu()
    {
        gameObject.SetActive(false);
    }

    public void BuyTower(Tower tower)
    {
        // Get the tower ID -> Get the tower prefab -> Build that tower
        if (currentSpot.IsOccupied)
        {
            currentSpot.UpgradeTower(TowerDictionary.GetTowerPrefab(tower.GetTowerId()));
        }
        else
        {
            currentSpot.BuildTower(TowerDictionary.GetTowerPrefab(tower.GetTowerId()));
        }
        CloseMenu();
    }
}
