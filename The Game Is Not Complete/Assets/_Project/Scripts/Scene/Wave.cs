using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWave", menuName = "Wave")]
public class Wave : ScriptableObject
{
    public List<Enemy> enemyList;
    public int spawnLocationNumber;
}
