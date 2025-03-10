using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MinimapController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private UIDocument uiDocument;

    [Header("Enemy Settings")]
    [Tooltip("Drag an enemy prefab here to automatically find all instances of this type")]
    [SerializeField] private GameObject enemyPrefab;
    [Tooltip("Alternative tag to use for finding enemies if no prefab is assigned")]
    [SerializeField] private string enemyTag = "Enemy";

    [Header("Minimap Settings")]
    [SerializeField] private float minimapRadius = 100f;
    [SerializeField] private float minimapScale = 0.1f; // How much world space to show on minimap
    [SerializeField] private Color enemyMarkerColor = Color.red;
    [SerializeField] private float enemyMarkerSize = 8f;

    // UI Elements
    private VisualElement minimapContent;
    private Dictionary<Transform, VisualElement> enemyMarkers = new Dictionary<Transform, VisualElement>();

    // Enemy tracking
    private List<Transform> enemies = new List<Transform>();

    private void Start()
    {
        // Get the root visual element
        var root = uiDocument.rootVisualElement;

        // Get the minimap content where we'll add enemy markers
        minimapContent = root.Q<VisualElement>("minimap-content");

        // Find all enemies in the scene
        FindEnemies();
    }

    private void FindEnemies()
    {
        if (enemyPrefab != null)
        {
            // Find all instances of the specified enemy prefab type
            // This works if all enemies are instantiated from the same prefab
            // or share the same component type
            FindEnemiesByType();
        }
        else
        {
            // Fallback to finding by tag
            FindEnemiesByTag();
        }
    }

    private void FindEnemiesByType()
    {
        // Get the type of the enemy from the prefab
        Component[] enemyComponents = enemyPrefab.GetComponents<Component>();

        if (enemyComponents.Length > 0)
        {
            // Use the first non-Transform component for type comparison
            System.Type enemyType = null;
            foreach (Component comp in enemyComponents)
            {
                if (comp.GetType() != typeof(Transform) &&
                    comp.GetType() != typeof(RectTransform))
                {
                    enemyType = comp.GetType();
                    break;
                }
            }

            // If we couldn't find a specific component, use the tag
            if (enemyType == null)
            {
                FindEnemiesByTag();
                return;
            }

            // Find all objects with this component type
            Component[] foundEnemies = FindObjectsOfType(enemyType) as Component[];

            Debug.Log($"Found {foundEnemies.Length} enemies of type {enemyType.Name}");

            foreach (Component enemy in foundEnemies)
            {
                AddEnemyToMinimap(enemy.transform);
            }
        }
        else
        {
            Debug.LogWarning("Enemy prefab doesn't have any components to search for. Falling back to tag search.");
            FindEnemiesByTag();
        }
    }

    private void FindEnemiesByTag()
    {
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag(enemyTag);

        Debug.Log($"Found {enemyObjects.Length} enemies with tag '{enemyTag}'");

        foreach (GameObject enemy in enemyObjects)
        {
            AddEnemyToMinimap(enemy.transform);
        }
    }

    private void AddEnemyToMinimap(Transform enemyTransform)
    {
        if (!enemies.Contains(enemyTransform))
        {
            enemies.Add(enemyTransform);

            // Create a marker for the enemy
            VisualElement enemyMarker = new VisualElement();
            enemyMarker.AddToClassList("enemy-marker");

            // Apply custom styling based on inspector settings
            enemyMarker.style.width = enemyMarkerSize;
            enemyMarker.style.height = enemyMarkerSize;
            enemyMarker.style.backgroundColor = enemyMarkerColor;
            //enemyMarker.style.borderRadius = enemyMarkerSize / 2;

            minimapContent.Add(enemyMarker);

            // Store reference to the marker
            enemyMarkers.Add(enemyTransform, enemyMarker);
        }
    }

    private void Update()
    {
        UpdateEnemyPositions();
    }

    private void UpdateEnemyPositions()
    {
        List<Transform> enemiesToRemove = new List<Transform>();

        foreach (Transform enemy in enemies)
        {
            if (enemy == null)
            {
                enemiesToRemove.Add(enemy);
                continue;
            }

            if (!enemyMarkers.ContainsKey(enemy))
                continue;

            VisualElement marker = enemyMarkers[enemy];

            // Calculate relative position of the enemy to the player
            Vector3 relativePos = enemy.position - player.position;

            // Scale and position within minimap
            float x = (relativePos.x * minimapScale) + minimapRadius;
            float y = (-relativePos.z * minimapScale) + minimapRadius; // Invert Z to match UI coordinates

            // Keep marker within minimap boundaries
            float distFromCenter = Vector2.Distance(new Vector2(x, y), new Vector2(minimapRadius, minimapRadius));
            if (distFromCenter <= minimapRadius)
            {
                marker.style.visibility = Visibility.Visible;
                marker.style.left = x;
                marker.style.top = y;
            }
            else
            {
                // Clamp marker to minimap border
                Vector2 direction = new Vector2(x - minimapRadius, y - minimapRadius).normalized;
                marker.style.left = minimapRadius + direction.x * (minimapRadius - 5);
                marker.style.top = minimapRadius + direction.y * (minimapRadius - 5);
            }
        }

        foreach (Transform enemy in enemiesToRemove)
        {
            RemoveEnemy(enemy);
        }
    }

    // Public method to manually add an enemy
    public void AddEnemy(Transform enemyTransform)
    {
        AddEnemyToMinimap(enemyTransform);
    }

    // Public method to manually remove an enemy
    public void RemoveEnemy(Transform enemyTransform)
    {
        if (enemies.Contains(enemyTransform) && enemyMarkers.ContainsKey(enemyTransform))
        {
            VisualElement marker = enemyMarkers[enemyTransform];
            minimapContent.Remove(marker);
            enemyMarkers.Remove(enemyTransform);
            enemies.Remove(enemyTransform);
        }
    }

    // Optional: Add this method to refresh enemies during gameplay
    public void RefreshEnemies()
    {
        // Clear existing enemies
        foreach (Transform enemy in new List<Transform>(enemies))
        {
            RemoveEnemy(enemy);
        }

        // Find enemies again
        FindEnemies();
    }
}