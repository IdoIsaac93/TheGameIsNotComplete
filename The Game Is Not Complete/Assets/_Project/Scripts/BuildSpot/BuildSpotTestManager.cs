using System.Linq;
using UnityEngine;

public class BuildSpotTestManager : MonoBehaviour
{
    private BuildSpot[] allBuildSpots;
    private static int selectedIndex = 0;
    [SerializeField] private GameObject buildSpotPointerInstance;
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
        if (Input.GetKeyDown(KeyCode.Keypad1) && allBuildSpots.Length > 0)
        {
            allBuildSpots[selectedIndex].BuildTower(allBuildSpots[selectedIndex].GetTowers()[(int)TowerId.BasicTower]);
        }

        if (Input.GetKeyDown(KeyCode.Keypad2) && allBuildSpots.Length > 0)
        {
            allBuildSpots[selectedIndex].BuildTower(allBuildSpots[selectedIndex].GetTowers()[(int)TowerId.SlowTower]);
        }

        if (Input.GetKeyDown(KeyCode.Keypad3) && allBuildSpots.Length > 0)
        {
            allBuildSpots[selectedIndex].BuildTower(allBuildSpots[selectedIndex].GetTowers()[(int)TowerId.HeavyTower]);
        }

        if (Input.GetKeyDown(KeyCode.Keypad4) && allBuildSpots.Length > 0)
        {
            allBuildSpots[selectedIndex].BuildTower(allBuildSpots[selectedIndex].GetTowers()[(int)TowerId.ChainTower]);
        }

        if (Input.GetKeyDown(KeyCode.Keypad5) && allBuildSpots.Length > 0)
        {
            allBuildSpots[selectedIndex].BuildTower(allBuildSpots[selectedIndex].GetTowers()[(int)TowerId.SnipeTower]);
        }

        if (Input.GetKeyDown(KeyCode.Keypad0) && allBuildSpots.Length > 0)
        {
            allBuildSpots[selectedIndex].SellTower();
        }

        if (Input.GetKeyDown(KeyCode.H) && allBuildSpots.Length > 0)
        {
            allBuildSpots[selectedIndex].UpgradeTower(0);
        }
    }
}
