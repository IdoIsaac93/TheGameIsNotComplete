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

    public void UpgradeTower(Tower towerPrefab)
    {
        //Upgrade the tower
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
