using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class TowerDictionary
{
    // Map TowerId to prefabs
    public static readonly Dictionary<TowerId, GameObject> towerPrefabs = new Dictionary<TowerId, GameObject>
    {
        { TowerId.Empty, null },
        { TowerId.BasicTower, Resources.Load<GameObject>("Prefabs/Tower/BasicTower") },
        { TowerId.SlowTower, Resources.Load<GameObject>("Prefabs/Tower/SlowTower") },
        { TowerId.HeavyTower, Resources.Load<GameObject>("Prefabs/Tower/HeavyTower") },
        { TowerId.ChainTower, Resources.Load<GameObject>("Prefabs/Tower/ChainTower") },
        { TowerId.SnipeTower, Resources.Load<GameObject>("Prefabs/Tower/SnipeTower") },
        { TowerId.FastBasicTower, Resources.Load<GameObject>("Prefabs/Tower/FastBasicTower") },
        { TowerId.LongRangeBasicTower, Resources.Load<GameObject>("Prefabs/Tower/LongRangeBasicTower") },
        { TowerId.DamageSlowTower, Resources.Load<GameObject>("Prefabs/Tower/DamageSlowTower") },
        { TowerId.SplashHeavyTower, Resources.Load<GameObject>("Prefabs/Tower/SplashHeavyTower") },
        { TowerId.SuperChainTower, Resources.Load<GameObject>("Prefabs/Tower/SuperChainTower") },
        { TowerId.SuperSnipeTower, Resources.Load<GameObject>("Prefabs/Tower/SuperSnipeTower") }
    };

    // Accessor method to get the prefab by TowerId
    public static GameObject GetTowerPrefab(TowerId towerId)
    {
        if (towerPrefabs.TryGetValue(towerId, out GameObject prefab))
        {
            return prefab;
        }
        else
        {
            Debug.LogWarning($"TowerId {towerId} not found!");
            return null;
        }
    }
}
