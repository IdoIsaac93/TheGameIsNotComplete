using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SceneController : Singleton<SceneController>
{
    public float timeBetweenWaves;
    public float waveTimer;
    public List<Wave> waves;
    public List<EnemySpawn> spawnLocations;
    public int waveNumber;
    public bool isWaveInProgress = false;
    public Slider waveTimerVisual;

    public static SceneController Instance { get; private set; }

    private void Awake()
    {
        //Singleton pattern to ensure only one SceneController
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
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
    }
}
