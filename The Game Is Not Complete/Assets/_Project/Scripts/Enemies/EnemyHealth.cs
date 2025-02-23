using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float Maxhealth;
    [SerializeField] private float currentHealth;
    [SerializeField] private Slider healthBar;

    public void Awake()
    {
        healthBar = GetComponentInChildren<Slider>();
        healthBar.maxValue = Maxhealth;
        currentHealth = Maxhealth;
        UpdateHealthBar();
    }
    public void TakeDamage(float damage)
    {
        //If damage is less than current hp, take damage. otherwise hp is set to 0 and die
        currentHealth = damage < currentHealth ? currentHealth - damage : 0;
        if (currentHealth <= 0) Die();
        UpdateHealthBar();
    }

    public virtual void Die()
    {
        EnemyController enemy = GetComponent<EnemyController>();
        PlayerResources.Instance.GainScore(enemy.scoreWorth);
        PlayerResources.Instance.GainSystemPoints(enemy.resourceWorth);
        Destroy(gameObject);
    }

    public void UpdateHealthBar()
    {
        healthBar.value = currentHealth;
    }
}
