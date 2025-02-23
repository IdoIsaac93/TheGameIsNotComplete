using UnityEngine;

public class BuildSpotTestManager : MonoBehaviour
{
    private BuildSpot[] allBuildSpots;
    private static int selectedIndex = 0;

    private void Awake()
    {
        allBuildSpots = FindObjectsByType<BuildSpot>(FindObjectsSortMode.InstanceID);
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
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.B) && allBuildSpots.Length > 0)
        {
            allBuildSpots[selectedIndex].BuildTower(allBuildSpots[selectedIndex].GetTowers()[0]);
        }

        if (Input.GetKeyDown(KeyCode.S) && allBuildSpots.Length > 0)
        {
            allBuildSpots[selectedIndex].SellTower();
        }

        if (Input.GetKeyDown(KeyCode.U) && allBuildSpots.Length > 0)
        {
            allBuildSpots[selectedIndex].UpgradeTower(0);
        }
    }
}
