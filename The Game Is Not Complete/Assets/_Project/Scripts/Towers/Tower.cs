using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [SerializeField] protected float attackDamage;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float attackRange;
    [SerializeField] protected int price;
    [SerializeField] protected SphereCollider rangeCollider;
    [SerializeField] protected Tower[] upgradeOptions; //For Raz: This is an array of towers that a tower can be upgraded to. Each tower prefab contains its own array with each upgrade option. My idea is that the Ui will load each of these options and selecting build/buy will send the index of that tower to the buildspot upgrade method.
    [SerializeField] protected ParticleSystem shootParticleEffect;

    [SerializeField] private Transform towerParticlePoint;
    [SerializeField] private ParticleSystem hitParticleEffect;

    protected TowerId towerId;
    protected float attackTimer;
    protected BuildSpot buildSpot;
    

    protected HashSet<EnemyController> enemiesInRange = new();
    private IAttackEffect attackEffect;
    private IAreaEffect areaEffect;


    private void Start()
    {
        rangeCollider = gameObject.GetComponent<SphereCollider>();
        rangeCollider.isTrigger = true;
        rangeCollider.radius = attackRange;

    }

    private void Awake()
    {
        SetValues();
    }
    


    protected virtual void Update()
    {

        CleanUpEnemies();
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0 && hasTargets())
        {
            Attack(GetClosestEnemy());
        }

    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemiesInRange.Add(enemy);
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemiesInRange.Remove(enemy);
        }
    }

    protected EnemyController GetClosestEnemy()
    {
        if (enemiesInRange.Count == 0)
        {
            return null;
        }

        EnemyController closestEnemy = null;
        float closestDistance = rangeCollider.radius;
        foreach (EnemyController enemy in enemiesInRange)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance <= closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }
        return closestEnemy;
    }

    protected bool hasTargets()
    {
        return enemiesInRange.Count > 0;
    }

    protected void Attack(EnemyController target)
    {
        if (target == null)
        {
            return;
        }

        PointAtEnemy(target.transform.position);

        if (shootParticleEffect != null)
        {
            Instantiate(shootParticleEffect, towerParticlePoint.position, towerParticlePoint.transform.rotation);
        }

        if (hitParticleEffect != null)
        {
            Instantiate(hitParticleEffect, target.transform.position, Quaternion.identity);
        }

        attackTimer = attackSpeed;
        target.health.TakeDamage(attackDamage);
        attackEffect?.ApplyEffect(target);
        areaEffect?.ApplyAreaEffect(enemiesInRange);

    }

    private void PointAtEnemy(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    protected void SetAttackEffect(IAttackEffect attackEffect)
    {
        this.attackEffect = attackEffect;
    }

    protected void SetAreaEffect(IAreaEffect areaEffect)
    {
        this.areaEffect = areaEffect;
    }

    protected abstract void SetValues();

    public bool CanUpgrade() //For Raz: This method checks if a tower can be upgraded.
                             //Could possibly be used to decide if a build menu should pop up based on whether a tower can be upgraded or not.
    {
        return upgradeOptions != null && upgradeOptions.Length > 0;
    }

    private void CleanUpEnemies()
    {
        enemiesInRange.RemoveWhere(enemy => enemy == null);
    }

    public Tower[] GetUpgradeOptions() //For Raz: This method returns the upgrade options of a tower.
                                       //Could be used to decide what to display in an upgrade option menu
    {
        return upgradeOptions;
    }

    public TowerId[] GetUpgradeId() //For Raz: This method returns the upgrade options of a tower as TowerIds.
                                    //Could be used to decide what to display in an upgrade option menu
    {
        TowerId[] upgradeIds = new TowerId[upgradeOptions.Length];

        for (int i = 0; i < upgradeOptions.Length; i++)
        {
            upgradeIds[i] = upgradeOptions[i].GetTowerId();
        }

        return upgradeIds;
    }

    public void SetBuildSpot(BuildSpot buildSpot)
    {
        this.buildSpot = buildSpot;
    }

    public int GetPrice()
    {
        if (price == 0) //This literally should not be needed, but somehow the price was not being set properly.
                        //For some reason it works completly fine now and I have only seen this debug message once since implamenting this. - Shane
        {
            Debug.LogWarning("Tower price was not set somehow.");
            SetValues();
        }
        return price;
    }

    public void Upgrade(int upgradeIndex)//For Raz: My idea is that the ui will load the upgrade options and selecting one will send the index of the tower to this method which will in turn send the index to the buildspot upgrade method.
    {
        if (!CanUpgrade())
        {
            Debug.LogWarning("This tower cannot be upgraded further!");
            return;
        }

        else if (buildSpot != null) buildSpot.UpgradeTower(upgradeIndex);
    }

    public void Sell()//For Raz: This method will be called by the UI when the player selects to sell a tower.
    {
        if (buildSpot != null) buildSpot.SellTower();
    }

    public TowerId GetTowerId()
    {
        return towerId;
    }


}
