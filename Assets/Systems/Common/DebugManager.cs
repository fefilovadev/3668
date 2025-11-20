using UnityEngine;
using System.IO;

public class DebugManager : MonoBehaviour
{
    public string SaveFolderName = "GameSaves";

    public string SaveFolderPath => Path.Combine(Application.persistentDataPath, SaveFolderName);

    public void ResetGame()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs cleared.");

        if (Directory.Exists(SaveFolderPath))
        {
            string[] files = Directory.GetFiles(SaveFolderPath, "*.json", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                File.Delete(file);
                Debug.Log("Deleted JSON file: " + file);
            }
        }
        else
        {
            Debug.Log("Save folder not found: " + SaveFolderPath);
        }
    }
}