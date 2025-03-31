using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class MinimapController : MonoBehaviour
{
    public UIDocument uiDocument;
    [SerializeField] private Transform cubeCenter; // Cubes Transform
    [SerializeField] private Transform core; // Core GameObjects Transform
    private VisualElement minimapContainer;
    private VisualElement coreDot; // Core dot UI element
    private const float mapSize = 50f; // Cubes world size (50x50)
    private const float uiWidth = 81f; // Minimap UI width
    private const float uiHeight = 87f; // Minimap UI height// odd number cuz I took a screenshot of 1061x1060
    private List<Transform> enemies = new List<Transform>();
    private List<VisualElement> enemyDots = new List<VisualElement>();

    //Adding on enable for gampause functionality instead of awake
    void OnEnable()
    {
        if (cubeCenter == null)
        {
            Debug.LogError("CubeCenter not assigned in MinimapController!");
            return;
        }
        if (uiDocument == null)
        {
            Debug.LogError("UIDocument not assigned in MinimapController!");
            return;
        }

        minimapContainer = uiDocument.rootVisualElement.Q<VisualElement>("MinimapContainer");
        if (minimapContainer == null)
        {
            Debug.LogError("MinimapContainer not found in UI Document!");
            return;
        }

        // Initialize core dot
        if (core != null)
        {
            coreDot = new VisualElement();
            coreDot.name = "CoreDot";
            coreDot.style.width = 4;
            coreDot.style.height = 4;
            coreDot.style.backgroundColor = Color.green;
            coreDot.style.borderTopLeftRadius = 5;
            coreDot.style.borderTopRightRadius = 5;
            coreDot.style.borderBottomLeftRadius = 5;
            coreDot.style.borderBottomRightRadius = 5;
            coreDot.style.position = Position.Absolute;
            minimapContainer.Add(coreDot);
        }
    }

    void OnDisable()
    {
        // Clean up all dots
        if (minimapContainer != null)
        {
            if (coreDot != null)
            {
                minimapContainer.Remove(coreDot);
                coreDot = null;
            }
            foreach (var dot in enemyDots)
            {
                minimapContainer.Remove(dot);
            }
        }
        enemyDots.Clear();
        enemies.Clear();
    }

    void Update()
    {
        if (cubeCenter == null || minimapContainer == null) return;

        // Update Core dot if it exists
        if (core != null && coreDot != null)
        {
            Vector3 centerPos = cubeCenter.position;
            Vector3 corePos = core.position;
            float relativeX = corePos.x - centerPos.x;
            float relativeZ = corePos.z - centerPos.z;

            float uiX = ((relativeX + (mapSize / 2)) / mapSize) * uiWidth;
            float uiY = ((relativeZ + (mapSize / 2)) / mapSize) * uiHeight;

            uiX = Mathf.Clamp(uiX, 0, uiWidth - 10);
            uiY = Mathf.Clamp(uiY, 0, uiHeight - 10);

            coreDot.style.left = uiX;
            coreDot.style.top = uiY;
        }

        // Update enemy dots
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
        enemies.Clear();
        foreach (var enemy in enemyObjects)
        {
            enemies.Add(enemy.transform);
        }

        SyncEnemyDots();
    }

    void SyncEnemyDots()
    {
        while (enemyDots.Count > enemies.Count)
        {
            var dot = enemyDots[enemyDots.Count - 1];
            minimapContainer.Remove(dot);
            enemyDots.RemoveAt(enemyDots.Count - 1);
        }

        while (enemyDots.Count < enemies.Count)
        {
            VisualElement dot = new VisualElement();
            dot.name = "EnemyDot";
            dot.style.width = 4;
            dot.style.height = 4;
            dot.style.backgroundColor = Color.red;
            dot.style.borderTopLeftRadius = 5;
            dot.style.borderTopRightRadius = 5;
            dot.style.borderBottomLeftRadius = 5;
            dot.style.borderBottomRightRadius = 5;
            dot.style.position = Position.Absolute;
            minimapContainer.Add(dot);
            enemyDots.Add(dot);
        }

        Vector3 centerPos = cubeCenter.position;
        for (int i = 0; i < enemies.Count; i++)
        {
            Vector3 enemyPos = enemies[i].position;
            float relativeX = enemyPos.x - centerPos.x;
            float relativeZ = enemyPos.z - centerPos.z;

            float uiX = ((relativeX + (mapSize / 2)) / mapSize) * uiWidth;
            float uiY = ((relativeZ + (mapSize / 2)) / mapSize) * uiHeight;

            uiX = Mathf.Clamp(uiX, 0, uiWidth - 10);
            uiY = Mathf.Clamp(uiY, 0, uiHeight - 10);

            enemyDots[i].style.left = uiX;
            enemyDots[i].style.top = uiY;
        }
    }
}