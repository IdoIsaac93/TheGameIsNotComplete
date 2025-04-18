//using System.Diagnostics;
using NUnit.Framework.Internal.Filters;
using UnityEngine;
using UnityEngine.UIElements;

public class BuildSpot : MonoBehaviour
{
    public Tower currentTower;
    public TowerId towerId = TowerId.Empty;
    public Tower[] baseTowerOptions;

    [SerializeField] private bool isOccupied = false;
    public bool IsOccupied { get { return isOccupied; } }

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
                currentTower = Instantiate(towerScript, towerPosition, towerScript.transform.rotation, transform);
                QuestEvents.OnQuestProgress?.Invoke(QuestType.BuildTower);
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

        Debug.Log("Building tower for free");
        Vector3 newTowerPosition = new(transform.position.x, towerPrefab.transform.position.y + transform.position.y, transform.position.z);
        currentTower = Instantiate(towerPrefab, newTowerPosition, Quaternion.identity).GetComponent<Tower>();
        isOccupied = true;
        currentTower.SetBuildSpot(this);
        towerId = currentTower.GetTowerId();
    }

    public void SellTower()
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

    public void UpgradeTower(GameObject towerPrefab)
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

        if (towerPrefab != null)
        {
            Tower towerScript = towerPrefab.GetComponent<Tower>();
            if (PlayerResources.Instance.SpendSystemPoints(towerScript.GetPrice()))
            {
                Vector3 towerPosition = new(transform.position.x, transform.position.y + towerPrefab.transform.position.y, transform.position.z);

                Destroy(currentTower.gameObject);

                currentTower = Instantiate(towerScript, towerPosition, Quaternion.identity);
                currentTower.SetBuildSpot(this);
                towerId = currentTower.GetTowerId();
                QuestEvents.OnQuestProgress?.Invoke(QuestType.UpgradeTower);
            }
        }
    }
}
