using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [SerializeField] protected float attackDamage;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float price;
    [SerializeField] protected SphereCollider rangeCollider;
    [SerializeField] protected Tower[] upgradeOptions;

    protected float attackTimer;
    protected BuildSpot buildSpot;

    protected HashSet<Enemy> enemiesInRange = new();
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
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemiesInRange.Add(enemy);
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemiesInRange.Remove(enemy);
        }
    }

    protected Enemy GetClosestEnemy()
    {
        if (enemiesInRange.Count == 0)
        {
            return null;
        }

        Enemy closestEnemy = null;
        float closestDistance = rangeCollider.radius;
        foreach (Enemy enemy in enemiesInRange)
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

    protected void Attack(Enemy target)
    {
        if (target == null)
        {
            return;
        }
        attackTimer = attackSpeed;
        target.health.TakeDamage(attackDamage);
        attackEffect?.ApplyEffect(target);
        areaEffect?.ApplyAreaEffect(enemiesInRange);

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

    public bool CanUpgrade()
    {
        return upgradeOptions != null && upgradeOptions.Length > 0;
    }

    private void CleanUpEnemies()
    {
        enemiesInRange.RemoveWhere(enemy => enemy == null);
    }

    public Tower[] GetUpgradeOptions()
    {
        return upgradeOptions;
    }

    public void SetBuildSpot(BuildSpot buildSpot)
    {
        this.buildSpot = buildSpot;
    }

    public void Upgrade(int upgradeIndex)
    {
        if (!CanUpgrade() && buildSpot != null)
        {
            Debug.LogWarning("This tower cannot be upgraded further!");
            return;
        }
        buildSpot.UpgradeTower(upgradeIndex);
    }




}
