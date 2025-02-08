using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float Maxhealth;
    [SerializeField] private float currentHealth;
    [SerializeField] private Slider healthBar;

    private void Awake()
    {
        healthBar = GetComponent<Slider>();
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

    public void Die()
    {
        //TODO//
        //Find PlayerResources and add system points and score
        Destroy(gameObject);
    }

    public void UpdateHealthBar()
    {
        healthBar.value = currentHealth;
    }
}
