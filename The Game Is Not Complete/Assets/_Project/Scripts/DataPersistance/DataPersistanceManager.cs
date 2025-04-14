using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Collections;

public class DataPersistanceManager : Singleton<DataPersistanceManager>
{
    //Data file configuration
    [SerializeField] private string fileName = "SaveFile";
    private FileHandler fileHandler;

    //Game data is saved and loaded from this
    private GameData gameData;

    //All objects that will be saved or loaded
    private List<IDataPersistance> dataPersistanceObjects;

    private void Start()
    {
        //Create the file handler
        fileHandler = new(Application.persistentDataPath, fileName);
        //Find all objects that need to be saved or loaded
        dataPersistanceObjects = FindAllDataPersistanceObjects();
    }

    private List<IDataPersistance> FindAllDataPersistanceObjects()
    {
        // Find all MonoBehaviour objects that implement IDataPersistance
        IEnumerable<IDataPersistance> dataPersistanceMonoBehaviours = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IDataPersistance>();

        return new List<IDataPersistance>(dataPersistanceMonoBehaviours);
    }

    public void NewGame(int levelIndex)
    {
        //Create new data file and save it then change to first level
        gameData = new GameData();
        SaveGame();
        SceneManager.LoadScene(levelIndex);
        PlayerResources.Instance.RestoreHealth();
    }

    public void SaveGame()
    {
        //Find all objects to save, important here because scene changes
        dataPersistanceObjects = FindAllDataPersistanceObjects();

        //Save variables from each object
        foreach (IDataPersistance dataPersistanceObject in dataPersistanceObjects)
        {
            dataPersistanceObject.SaveData(ref gameData);
        }

        //Save the current level, done from here for simplicity
        gameData.currentLevelIndex = SceneManager.GetActiveScene().buildIndex;

        //Send the data to the file handler for saving
        fileHandler.Save(gameData);
    }

    public void LoadGame()
    {
        //Loads the game data from the file handler
        gameData = fileHandler.Load();

        //If no there is no game data, start a new game
        if (gameData == null)
        {
            Debug.Log("No savefile found, starting a new game");
            NewGame(1);
        }
        //Load data if no scene change is needed
        if (gameData.currentLevelIndex == SceneManager.GetActiveScene().buildIndex)
        {
            LoadData();
        }
        else
        {
            StartCoroutine(WaitForSceneChange(gameData.currentLevelIndex));
        }
    }

    private IEnumerator WaitForSceneChange(int targetLevelIndex)
    {
        // Wait until the scene is loaded
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(targetLevelIndex);
        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        // After scene has finished loading, load the game data
        SceneController.Instance.ResetTimer();
        LoadData();
    }

    public void LoadData()
    {
        //Find all objects to load, important here because scene changes
        dataPersistanceObjects = FindAllDataPersistanceObjects();
        //Load variables to each object
        foreach (IDataPersistance dataPersistanceObject in dataPersistanceObjects)
        {
            dataPersistanceObject.LoadData(gameData);
        }
    }
}
