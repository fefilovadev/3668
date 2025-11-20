using System.IO;
using UnityEngine;

public class PlayerItemSaver
{
    private readonly string _folderPath;
    private readonly string _filePath;

    public PlayerItemSaver()
    {
        _folderPath = Path.Combine(Application.persistentDataPath, "GameData");
        _filePath = Path.Combine(_folderPath, "player_items.json");

        if (!Directory.Exists(_folderPath))
            Directory.CreateDirectory(_folderPath);
    }

    public void SaveItems(PlayerItems items)
    {
        string json = JsonUtility.ToJson(items, true);
        File.WriteAllText(_filePath, json);
    }

    public PlayerItems LoadItems()
    {
        if (!File.Exists(_filePath))
        {
            var defaultItems = new PlayerItems(5, 5);
            SaveItems(defaultItems);
            return defaultItems;
        }

        string json = File.ReadAllText(_filePath);
        var data = JsonUtility.FromJson<PlayerItems>(json);
        return data;
    }
}
