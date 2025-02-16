using UnityEngine;

public class BasicTower : Tower
{


    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {
        SetValues();
    }

    protected override void SetValues()
    {
        attackDamage = 10;
        attackSpeed = 1;
        attackRange = 5;
        price = 100;
    }

    // Update is called once per frame

}
