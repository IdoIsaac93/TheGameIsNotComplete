using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour, IDataPersistance
{
    [SerializeField] private float timeBetweenWaves;
    [SerializeField] private float waveTimer;
    [SerializeField] private List<Wave> waves;
    [SerializeField] private List<EnemySpawn> spawnLocations;
    [SerializeField] private int waveNumber = 0;
    [SerializeField] private bool isWaveInProgress = false;
    [SerializeField] private Slider waveTimerVisual;
    public int numberOfEnemiesAlive = 0;
    public static SceneController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Duplicate SceneController destroyed");
           Destroy(gameObject);
            
        }
        else
        {
            Instance = this;
            Debug.Log("SceneController initialized");
        }

        // Register the scene loaded listener
        SceneManager.sceneLoaded += OnSceneLoaded;

        //Commented out by Raz
        //waveTimerVisual.maxValue = timeBetweenWaves;
    }

    private void Update()
    {
        if (!isWaveInProgress)
        {
            if (waveTimer < timeBetweenWaves)
            {
                waveTimer += Time.deltaTime;
            }
            else
            {
                StartWave();
            }
            //Commented out by Raz
            //waveTimerVisual.value = timeBetweenWaves - waveTimer;
        }
        else
        {
            if (numberOfEnemiesAlive <= 0)
            {
                WaveCompleted();
            }
        }
    }

    public void ResetTimer()
    {
        waveTimer = 0;
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetTimer();

        // Clear the current spawn locations
        spawnLocations.Clear();

        // Find all GameObjects with the tag "Spawner" and add them to the spawnLocations list
        GameObject[] spawners = GameObject.FindGameObjectsWithTag("Spawner");
        foreach (GameObject spawner in spawners)
        {
            EnemySpawn spawnComponent = spawner.GetComponent<EnemySpawn>();
            if (spawnComponent != null)
            {
                spawnLocations.Add(spawnComponent);
            }
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from the event when this object is destroyed
        Debug.LogError("SceneController was destroyed! Stack Trace:\n" + System.Environment.StackTrace);

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
