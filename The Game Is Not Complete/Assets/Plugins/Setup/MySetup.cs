using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class MySetup
{
#if UNITY_EDITOR
    [MenuItem("Setup/Create Folders")]
    public static void CreateMyFolders()
    {
        Folder.CreateFolders("_Project", "Animations", "Art", "Audio", "Fonts", "Materials", "Prefabs", "ScriptableObjects", "Scripts", "Settings");
        AssetDatabase.Refresh();
    }
#endif
    static class Folder
    {
        public static void CreateFolders(string root, params string[] folders)
        {
            string fullPath = Path.Combine(Application.dataPath, root);

            foreach (string folder in folders)
            {
                string folderPath = Path.Combine(fullPath, folder);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
            }
        }
    }
}
