using UnityEngine;
using UnityEngine.UIElements;

public class BuildSpot : MonoBehaviour
{
    Tower currentTower;
    bool isOccupied = false;
    [SerializeField] Tower[] towers;

    public void Update()
    {
        TestFunctionality();
    }

    public void BuildTower(Tower towerPrefab)
    {
        if (!isOccupied)
        {
            currentTower = Instantiate(towerPrefab, transform.position, Quaternion.identity);
            Vector3 towerPosition = new Vector3(transform.position.x, towerPrefab.transform.position.y, transform.position.z);
            currentTower.transform.position = towerPosition;
            isOccupied = true;
        }
    }

    public void SellTower()
    {
        if (isOccupied)
        {
            Destroy(currentTower.gameObject);
            isOccupied = false;
            //Refund the player
        }
    }

    public void UpgradeTower(int upgradeIndex)
    {
        if (!isOccupied || currentTower == null)
        {
            Debug.LogWarning("No tower to upgrade!");
            return;
        }

        if (!currentTower.CanUpgrade())
        {
            Debug.LogWarning("This tower cannot be upgraded further!");
            return;
        }

        Tower[] upgrades = currentTower.GetUpgradeOptions();

        if (upgradeIndex < 0 || upgradeIndex >= upgrades.Length)
        {
            Debug.LogWarning("Invalid upgrade index.");
            return;
        }

        Tower upgradedTowerPrefab = upgrades[upgradeIndex];

        if (upgradedTowerPrefab != null)
        {
            Vector3 towerPosition = currentTower.transform.position;
            Destroy(currentTower.gameObject);

            currentTower = Instantiate(upgradedTowerPrefab, towerPosition, Quaternion.identity);
            Debug.Log("Tower upgraded to " + currentTower.name);
        }
    }

    public void TestBuild()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            BuildTower(towers[0]);
            Debug.Log("Built");
        }
    }

    public void TestSell()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SellTower();
            Debug.Log("Sold");
        }
    }

    public void TestFunctionality()
    {
        TestBuild();
        TestSell();
    }

}
