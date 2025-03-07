using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SceneController : Singleton<SceneController> , IDataPersistance
{
    public float timeBetweenWaves;
    public float waveTimer;
    public List<Wave> waves;
    public List<EnemySpawn> spawnLocations;
    public int waveNumber = 0;
    public bool isWaveInProgress = false;
    public Slider waveTimerVisual;
    public int numberOfEnemiesAlive = 0;

    new private void Awake()
    {
        waveTimerVisual.maxValue = timeBetweenWaves;
    }

    private void Update()
    {
        if (!isWaveInProgress)
        {
            waveTimer += Time.deltaTime;
            if (waveTimer >= timeBetweenWaves)
            {
                StartWave();
            }
            waveTimerVisual.value = timeBetweenWaves - waveTimer;
        }
        else
        {
            if (numberOfEnemiesAlive <= 0)
            {
                WaveCompleted();
            }
        }
    }

    public void SkipTimer()
    {
        waveTimer = timeBetweenWaves;
    }

    public void StartWave()
    {
        // Stop if out of waves
        if (waveNumber >= waves.Count) return;

        waveTimer = 0;
        isWaveInProgress = true;
        Wave currentWave = waves[waveNumber];
        spawnLocations[currentWave.spawnLocationNumber].SpawnWave(currentWave);
    }

    public void WaveCompleted()
    {
        waveNumber++;
        isWaveInProgress = false;
        DataPersistanceManager.Instance.SaveGame();
        Debug.Log("Wave completed, Autosaving game");
    }

    public void LoadData(GameData data)
    {
        waveNumber = data.waveNumber;
    }

    public void SaveData(ref GameData data)
    {
        data.waveNumber = waveNumber;
    }
}
