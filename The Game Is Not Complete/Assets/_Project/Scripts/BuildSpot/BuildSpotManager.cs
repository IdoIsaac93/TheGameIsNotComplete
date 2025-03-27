using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BuildSpotManager : MonoBehaviour, IDataPersistance
{
    [SerializeField] private BuildSpot[] allBuildSpots;
    private List<TowerId> towers = new();
    [SerializeField] private GameObject buildMenu;
    private BuildMenu buildMenuScript;

    private int selectedIndex = 1;

    private void Awake()
    {
        // Find all build spots and put them in the list
        allBuildSpots = FindObjectsByType<BuildSpot>(FindObjectsSortMode.InstanceID)
            .OrderBy(spot => spot.transform.position.z)
            .ThenBy(spot => spot.transform.position.x).ToArray();

        if (buildMenu != null) { buildMenuScript = buildMenu.GetComponent<BuildMenu>(); }
        else { Debug.Log("BuildSpotManager did not find the BuildMenu object"); }
    }

    private void Update()
    {
        HandleInput();
        // Mouse left click
        HandleMouseClick();
    }

    //This is the old method for handlinf moush clicks

    /*
    private void HandleMouseClick()
    {
        // Send a raycast from the camera to the mouse position
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            // Check if the hit object is a BuildSpot
            BuildSpot clickedSpot = hit.collider?.GetComponentInParent<BuildSpot>();
            if (clickedSpot != null)
            {
                if (clickedSpot.currentTower == null)
                {
                    buildMenuScript.OpenMenu(clickedSpot, clickedSpot.baseTowerOptions);
                }
                else
                {
                    // Open the menu at the clicked build spot and with its available towers
                    buildMenuScript.OpenMenu(clickedSpot, clickedSpot.currentTower.GetUpgradeOptions());
                }
            }
        }
    }
    */

    //This should handle touch inputs for mobile
    private void HandleMouseClick()
    {
        // Check if there's at least one touch on the screen
        if (Input.touchCount > 0)
        {
            // Get the first touch (for simplicity, we handle the first touch here)
            Touch touch = Input.GetTouch(0);

            // Check if the touch is a tap (phase is Began)
            if (touch.phase == TouchPhase.Began)
            {
                // Convert the touch position to world position (like a raycast from camera)
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    // Check if the hit object is a BuildSpot
                    BuildSpot clickedSpot = hit.collider?.GetComponentInParent<BuildSpot>();

                    if (clickedSpot != null)
                    {
                        if (clickedSpot.currentTower == null)
                        {
                            buildMenuScript.OpenMenu(clickedSpot, clickedSpot.baseTowerOptions);
                        }
                        else
                        {
                            // Open the menu at the clicked build spot and with its available towers
                            buildMenuScript.OpenMenu(clickedSpot, clickedSpot.currentTower.GetUpgradeOptions());
                        }
                    }
                }
            }
        }
    }


    //Temporary inputs for testing
    private void HandleInput()
    {
        if (allBuildSpots.Length > 0)
        {
            //Build tower
            for (int i = 0; i < 9; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i) || Input.GetKeyDown(KeyCode.Keypad1 + i))
                {
                    allBuildSpots[selectedIndex].BuildTower(TowerDictionary.GetTowerPrefab(TowerId.BasicTower + i));
                }
            }

            //Sell tower
            if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0))
            {
                allBuildSpots[selectedIndex].SellTower();
            }

            // Open/Close pause menu
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (Time.timeScale != 0) { /*Pause()*/ }
                else { /*Unpause()*/ }
            }
        }

        //Saving
        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("Manual Save");
            DataPersistanceManager.Instance.SaveGame();
        }

        //Loading
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Manual Load");
            DataPersistanceManager.Instance.LoadGame();
        }
    }

    public void LoadData(GameData data)
    {
        // Takes the list of towers from the save file
        towers = data.towers;

        // Loops through all build spots and builds the coresponding tower
        for (int i = 0; i < allBuildSpots.Length; i++)
        {
            GameObject towerPrefab = TowerDictionary.GetTowerPrefab(towers[i]);
            allBuildSpots[i].LoadBuild(towerPrefab);
        }
    }

    public void SaveData(ref GameData data)
    {
        // Loops through all build spots and stores their coresponding towers
        foreach (BuildSpot spot in allBuildSpots)
        {
            Debug.Log($"Saving towerId: {spot.towerId} at index {Array.IndexOf(allBuildSpots, spot)}");
            towers.Add(spot.towerId);
        }

        // Updates the save file with the list of towers
        data.towers = towers;
    }
}
