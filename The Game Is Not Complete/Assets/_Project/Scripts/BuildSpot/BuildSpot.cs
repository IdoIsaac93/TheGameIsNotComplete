using UnityEngine;
using UnityEngine.UIElements;

public class BuildSpot : MonoBehaviour
{
    public Tower currentTower;
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
            //Check if player has enough money
            //Deduct the money from the player
            currentTower = Instantiate(towerPrefab, transform.position, Quaternion.identity);
            Vector3 towerPosition = new Vector3(transform.position.x, towerPrefab.transform.position.y, transform.position.z);
            currentTower.transform.position = towerPosition;
            isOccupied = true;
            currentTower.SetBuildSpot(this);
        }
    }

    public void SellTower()
    {
        if (isOccupied)
        {
            //Refund the player
            Destroy(currentTower.gameObject);
            isOccupied = false;
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
            Vector3 towerPosition = new Vector3(transform.position.x, upgradedTowerPrefab.transform.position.y, transform.position.z);

            Destroy(currentTower.gameObject);

            currentTower = Instantiate(upgradedTowerPrefab, towerPosition, Quaternion.identity);
            currentTower.SetBuildSpot(this);
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

    public void TestUpgrade()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            UpgradeTower(0);
            Debug.Log("Upgraded");
        }
    }

    public void TestFunctionality()
    {
        TestBuild();
        TestSell();
        TestUpgrade();
    }

}
