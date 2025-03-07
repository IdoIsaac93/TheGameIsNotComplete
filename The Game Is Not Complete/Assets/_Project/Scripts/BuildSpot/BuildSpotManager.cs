using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BuildSpotManager : MonoBehaviour, IDataPersistance
{
    [SerializeField] private BuildSpot[] allBuildSpots;
    [SerializeField] private GameObject buildSpotPointerInstance;
    private static int selectedIndex = 0;
    private List<TowerId> towers = new();

    private void Awake()
    {
        allBuildSpots = FindObjectsByType<BuildSpot>(FindObjectsSortMode.InstanceID).OrderBy(spot => spot.transform.position.z).ThenBy(spot => spot.transform.position.x).ToArray();
        if (buildSpotPointerInstance != null && allBuildSpots.Length > 0)
        {
            buildSpotPointerInstance = Instantiate(buildSpotPointerInstance);
            UpdatePointerPosition();
        }
    }

    private void Update()
    {
        NextSpot();
        HandleInput();
    }

    private void NextSpot()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            selectedIndex = (selectedIndex + 1) % allBuildSpots.Length;
            UpdatePointerPosition();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            selectedIndex = (selectedIndex - 1 + allBuildSpots.Length) % allBuildSpots.Length;
            UpdatePointerPosition();
        }
    }

    private void UpdatePointerPosition()
    {
        if (buildSpotPointerInstance != null)
        {
            Vector3 newPos = allBuildSpots[selectedIndex].transform.position + Vector3.up * 4f;
            buildSpotPointerInstance.transform.position = newPos;
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

            //Upgrade tower
            if (Input.GetKeyDown(KeyCode.F))
            {
                allBuildSpots[selectedIndex].UpgradeTower(0);
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
