using UnityEngine;

[CreateAssetMenu(fileName = "FreezeItem", menuName = "Items/Freeze")]
public class FreezeItem : Item
{
    [SerializeField] private float slowAmount = 5f;
    [SerializeField] private float duration = 3f;

    public override void UseItem()
    {
        // Find all enemies
        EnemyController[] enemies = FindObjectsByType<EnemyController>(FindObjectsSortMode.None);

        // Slow each enemy
        foreach (EnemyController enemy in enemies)
        {
            enemy.StartSlow(slowAmount,duration);
        }
    }
}
