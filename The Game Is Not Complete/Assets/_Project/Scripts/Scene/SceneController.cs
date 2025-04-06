using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour, IDataPersistance
{
    [Header("Waves")]
    [SerializeField] private List<Wave> waves;
    [SerializeField] private int waveNumber = 0;

    [Header("Timer")]
    [SerializeField] private float timeBetweenWaves;
    [SerializeField] private float waveTimer;
    [SerializeField] private bool isWaveInProgress = false;

    [Header("Visuals")]
    [SerializeField] private Slider waveTimerVisual;
    [SerializeField] private Button skipTimerButton;

    [Header("Enemies")]
    [SerializeField] private List<EnemySpawn> spawnLocations;
    public int numberOfEnemiesAlive = 0;
    public static SceneController Instance { get; private set; }
    public static event System.Action<int> OnWaveCompleted;


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

        //Comment this out when we have another visual
        waveTimerVisual.maxValue = timeBetweenWaves;
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
            //Comment this out when we have another visual
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

    public void ResetTimer()
    {
        waveTimer = 0;
    }
    public void SkipTimer()
    {
        if (!isWaveInProgress)
        {
            waveTimer = timeBetweenWaves;
        }
    }

    public void StartWave()
    {
        // Stop if out of waves
        if (waveNumber >= waves.Count) return;

        waveTimer = 0;
        isWaveInProgress = true;
        Wave currentWave = waves[waveNumber];
        spawnLocations[currentWave.spawnLocationNumber].SpawnWave(currentWave);
        //Deactivate skip timer button
        skipTimerButton.interactable = false;
    }

    public void WaveCompleted()
    {
        waveNumber++;
        isWaveInProgress = false;
        DataPersistanceManager.Instance.SaveGame();
        Debug.Log("Wave completed, Autosaving game");
        //Reactivate skip timer button
        skipTimerButton.interactable = true;
        //Activate the event for achievements
        OnWaveCompleted?.Invoke(waveNumber);
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
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
