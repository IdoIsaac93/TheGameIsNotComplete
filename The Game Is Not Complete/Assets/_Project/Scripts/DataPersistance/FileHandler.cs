using UnityEngine;
using System;
using System.IO;

public class FileHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public FileHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load()
    {
        //Combines the directory path and the filename to create a full path
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                //Deserialize the data
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load from " + fullPath + "\n" + e);

            }
        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        //Combines the directory path and the filename to create a full path
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            //Creates a folder for the save file if it doesn't exist already
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            //serialize the object into json, the boolean makes it PrettyPrint
            string dataToSave = JsonUtility.ToJson(data, true);

            //Using makes it open and close the stream, create means it creates new or overwrites existing
            using (FileStream stream = new FileStream(fullPath,FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToSave);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save to " + fullPath + "\n" + e);
        }
    }
}
