using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BuildSpotTestManager : MonoBehaviour , IDataPersistance
{
    [SerializeField] private BuildSpot[] allBuildSpots;
    private static int selectedIndex = 0;
    [SerializeField] private GameObject buildSpotPointerInstance;
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
        TestNextSpot();
        HandleInput();
    }

    private void TestNextSpot()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            selectedIndex = (selectedIndex + 1) % allBuildSpots.Length;
            UpdatePointerPosition();
        }
        if (Input.GetKeyDown(KeyCode.B))
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

    private void HandleInput()
    {
        if (allBuildSpots.Length > 0)
        {
            for (int i = 0; i < 9; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    allBuildSpots[selectedIndex].BuildTower(TowerDictionary.GetTowerPrefab(TowerId.BasicTower + i));
                }
            }
            for (int i = 0; i < 9; i++)
            {
                if (Input.GetKeyDown(KeyCode.Keypad1 + i))
                {
                    allBuildSpots[selectedIndex].BuildTower(TowerDictionary.GetTowerPrefab(TowerId.BasicTower + i));
                }
            }
            //if (Input.GetKeyDown(KeyCode.Keypad1))
            //{
            //    allBuildSpots[selectedIndex].BuildTower(TowerDictionary.GetTowerPrefab(TowerId.BasicTower));
            //}

            //if (Input.GetKeyDown(KeyCode.Keypad2))
            //{
            //    allBuildSpots[selectedIndex].BuildTower(TowerDictionary.GetTowerPrefab(TowerId.SlowTower));
            //}

            //if (Input.GetKeyDown(KeyCode.Keypad3))
            //{
            //    allBuildSpots[selectedIndex].BuildTower(TowerDictionary.GetTowerPrefab(TowerId.HeavyTower));
            //}

            //if (Input.GetKeyDown(KeyCode.Keypad4))
            //{
            //    allBuildSpots[selectedIndex].BuildTower(TowerDictionary.GetTowerPrefab(TowerId.ChainTower));
            //}

            //if (Input.GetKeyDown(KeyCode.Keypad5))
            //{
            //    allBuildSpots[selectedIndex].BuildTower(TowerDictionary.GetTowerPrefab(TowerId.SnipeTower));
            //}

            if (Input.GetKeyDown(KeyCode.Keypad0))
            {
                allBuildSpots[selectedIndex].SellTower();
            }

            //if (Input.GetKeyDown(KeyCode.H))
            //{
            //    allBuildSpots[selectedIndex].UpgradeTower(0);
            //}

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //Pause Game
                if (Time.timeScale != 0) { /*Pause()*/ }
                else { /*Unpause()*/ }
            }
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("Manual Save");
            DataPersistanceManager.Instance.SaveGame();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Manual Load");
            DataPersistanceManager.Instance.LoadGame();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            DataPersistanceManager.Instance.LoadData();
        }
    }

    public void LoadData(GameData data)
    {
        towers = data.towers;
        for (int i = 0;i < allBuildSpots.Length;i++)
        {
            GameObject towerPrefab = TowerDictionary.GetTowerPrefab(towers[i]);
            allBuildSpots[i].LoadBuild(towerPrefab);
        }
    }

    public void SaveData(ref GameData data)
    {
        foreach (BuildSpot spot in allBuildSpots)
        {
            Debug.Log($"Saving towerId: {spot.towerId} at index {Array.IndexOf(allBuildSpots, spot)}");
            towers.Add(spot.towerId);
        }
        data.towers = towers;
    }
}
