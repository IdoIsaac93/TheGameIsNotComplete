//using System.Diagnostics;
using NUnit.Framework.Internal.Filters;
using UnityEngine;
using UnityEngine.UIElements;

public class BuildSpot : MonoBehaviour
{
    public Tower currentTower;
    public TowerId towerId = TowerId.Empty; // This will be used for presistance later to know which tower is built on this build spot


    [SerializeField] private bool isOccupied = false;
    [SerializeField] Tower[] towers;

    public void BuildTower(GameObject towerPrefab) //For Raz: This method will be called by the UI when the player selects a tower to build.
                                                   //Each buildspot has a list of towers that can be built on it. The UI needs to send the correct tower prefab to this method
                                                   //You may wish to do this with tower ids instead of prefabs. There a method here that returns the tower ids of the towers that can be built on this build spot
                                                   //GetTowers() returns the towers that can be built on this build spot, this could also be used instead
    {
        //Build nothing if prefab is null, used when TowerId.Empty
        if (towerPrefab == null) { return; }
        Tower towerScript = towerPrefab.GetComponent<Tower>();
        if (!isOccupied)
        {
            //Check if player has enough money and deduct the money from the player
            if (PlayerResources.Instance.SpendSystemPoints(towerScript.GetPrice()))
            {
                Debug.Log("Spending " + towerScript.GetPrice() + " points to build ");
                Vector3 towerPosition = new(transform.position.x, towerScript.transform.position.y + transform.position.y, transform.position.z);
                currentTower = Instantiate(towerScript, towerPosition, Quaternion.identity);
                isOccupied = true;
                currentTower.SetBuildSpot(this);
                towerId = currentTower.GetTowerId();
            }
            else { Debug.Log("Not enough System Points to buy tower"); }
        }
        else { Debug.Log("Build spot is occupied"); }
    }

    public void LoadBuild(GameObject towerPrefab)
    {

        //Remove the tower
        if (isOccupied)
        {
            Debug.Log("Removing current tower while loading");
            Destroy(currentTower.gameObject);
            currentTower = null;
            isOccupied = false;
            towerId = TowerId.Empty;
        }

        // Build nothing if prefab is null, used when TowerId.Empty
        if (towerPrefab == null) { return; }
        Tower towerScript = towerPrefab.GetComponent<Tower>();

        Debug.Log("Building tower for free");
        Vector3 newTowerPosition = new(transform.position.x, towerPrefab.transform.position.y + transform.position.y, transform.position.z);
        currentTower = Instantiate(towerPrefab, newTowerPosition, Quaternion.identity).GetComponent<Tower>();
        isOccupied = true;
        currentTower.SetBuildSpot(this);
        // I have no idea why, but when loading it gave towers an ID that is 1 lower than it should be
        // I couldn't figure out why so I just added this +1 at the end
        towerId = currentTower.GetTowerId();
    }

    public void SellTower() //For Raz: This method will be called by the UI when the player selects to sell a tower.
                            //Some towers cover the build spot so it might be better to sell by clicking on a tower rather than the build spot. There is a method in tower that can call this method
    {
        if (isOccupied)
        {
            //Refund the player
            PlayerResources.Instance.GainSystemPoints(currentTower.GetPrice() / 2);

            //Remove the tower
            Destroy(currentTower.gameObject);
            currentTower = null;
            isOccupied = false;
            towerId = TowerId.Empty;
        }
    }

    public void UpgradeTower(int upgradeIndex) //For Raz: This is the method to upgrade a tower.
                                               //Some towers cover the build spot so it might be better to upgrade by clicking on a tower rather than the build spot. There is a method in tower that can call this method
                                               //Each tower has an array of upgrade options that can be accessed by calling GetUpgradeOptions() on the tower
                                               //The UI needs to send the index of the correct tower to this method
                                               //It may be difficult to use tower id's here as the id enum will not have the same index as the upgrade options array


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

        if (upgradedTowerPrefab != null && PlayerResources.Instance.SpendSystemPoints(upgradedTowerPrefab.GetPrice()))
        {
            Vector3 towerPosition = new(transform.position.x, transform.position.y + upgradedTowerPrefab.transform.position.y, transform.position.z);

            Destroy(currentTower.gameObject);

            currentTower = Instantiate(upgradedTowerPrefab, towerPosition, Quaternion.identity);
            currentTower.SetBuildSpot(this);
        }
    }


    public Tower[] GetTowers()// For Raz: This method returns the towers that can be built on this build spot.
    {
        return towers;
    }

    public TowerId[] GetTowerIds()// For Raz : This method returns the tower ids of the towers that can be built on this build spot.
                                  //This can be used to determine which tower to build when the player clicks on a build spot.
                                  //The UI object could have a list of towers, then use a switch statement to determine which tower to build based on the tower id selected in the UI
    {

        TowerId[] towerIds = new TowerId[towers.Length];

        for (int i = 0; i < towers.Length; i++)
        {
            towerIds[i] = towers[i].GetTowerId();
        }

        return towerIds;
    }

}
