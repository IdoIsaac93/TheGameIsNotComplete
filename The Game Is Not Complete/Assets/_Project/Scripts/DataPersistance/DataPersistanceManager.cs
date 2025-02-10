using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistanceManager : MonoBehaviour
{
    //Data file configuration
    [SerializeField] private string fileName;
    private FileHandler fileHandler;

    //Game data is saved and loaded from this
    private GameData gameData;

    //All objects that will be saved or loaded
    private List<IDataPersistance> dataPersistanceObjects;

    //The data persistance manager that is in the scene
    public static DataPersistanceManager instance { get; private set; }
    private void Awake()
    {
        //Singleton pattern to make sure only one manager in the game
        if (instance != null)
        {
            Debug.Log("More than one 'Data Persistance Manager' found!");
        }
        instance = this;
    }

    private void Start()
    {
        //Create the file handler
        fileHandler = new FileHandler(Application.persistentDataPath, fileName);
        //Find all objects that need to be saved or loaded
        dataPersistanceObjects = FindAllDataPersistanceObjects();
    }

    private List<IDataPersistance> FindAllDataPersistanceObjects()
    {
        // Find all MonoBehaviour objects that implement IDataPersistance

//FindObjectOfType is obsolete, this gets rid of the warning.
#pragma warning disable CS0618 // Type or member is obsolete
        IEnumerable<IDataPersistance> dataPersistanceMonoBehaviours = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistance>();
#pragma warning restore CS0618 // Type or member is obsolete

        return new List<IDataPersistance>(dataPersistanceMonoBehaviours);
    }

    public void NewGame()
    {
        //Create new data file and save it then change to first level
        gameData = new GameData();
        SaveGame();
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
        //gameData.currentLevelIndex = SceneManager.GetActiveScene().buildIndex;

        //Send the data to the file handler for saving
        fileHandler.Save(gameData);
    }

    public void LoadGame()
    {
        //Find all objects to load, important here because scene changes
        dataPersistanceObjects = FindAllDataPersistanceObjects();

        //Loads the game data from the file handler
        gameData = fileHandler.Load();

        //If no there is no game data, start a new game
        if (gameData == null)
        {
            Debug.Log("No savefile found, starting a new game");
            NewGame();
        }
        //Load variables to each object
        foreach (IDataPersistance dataPersistanceObject in dataPersistanceObjects)
        {
            dataPersistanceObject.LoadData(gameData);
        }
    }


    //I had this in last semesters class but I can't remember why so I dont want to delete it yet

    //Needs to be seperate from LoadGame! When LoadGame is called from the scenecontroller it needs to not change the scene
    //public void LoadLevel()
    //{
    //    //Makes sure the main menu isn't loaded, goes to first level instead
    //    if (gameData.currentLevelIndex == 0)
    //    {
    //        gameData.currentLevelIndex++;
    //    }

    //    //Loads the level that was saved
    //    sceneController.ChangeScene(gameData.currentLevelIndex);
    //}
}
