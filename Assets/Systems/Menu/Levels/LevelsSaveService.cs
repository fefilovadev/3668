using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelSaveService
{
    private readonly string filePath;
    private readonly int levelCount;

    public LevelSaveService(int levelCount, string fileName = "levels.json")
    {
        this.levelCount = levelCount;

        string folderPath = Path.Combine(Application.persistentDataPath, "GameData");
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        filePath = Path.Combine(folderPath, fileName);
    }

    public void SaveAll(List<ELevelStates> levels)
    {
        string json = JsonUtility.ToJson(new EnumListWrapper(levels), true);
        File.WriteAllText(filePath, json);
    }

    public List<ELevelStates> LoadAll()
    {
        if (!File.Exists(filePath))
        {
            var defaultLevels = CreateDefault();
            SaveAll(defaultLevels);
            return defaultLevels;
        }

        string json = File.ReadAllText(filePath);

        if (string.IsNullOrEmpty(json))
        {
            var defaultLevels = CreateDefault();
            SaveAll(defaultLevels);
            return defaultLevels;
        }

        var wrapper = JsonUtility.FromJson<EnumListWrapper>(json);

        if (wrapper == null || wrapper.Levels == null)
        {
            var defaultLevels = CreateDefault();
            SaveAll(defaultLevels);
            return defaultLevels;
        }
        if (wrapper.Levels.Count < levelCount)
        {
            int missing = levelCount - wrapper.Levels.Count;
            for (int i = 0; i < missing; i++)
                wrapper.Levels.Add(ELevelStates.Closed);

            SaveAll(wrapper.Levels);
        }

        return wrapper.Levels;
    }

    public void UpdateLevel(int index, ELevelStates newState)
    {
        List<ELevelStates> levels = LoadAll();

        if (index < 0 || index >= levels.Count)
            return;

        levels[index] = newState;
        SaveAll(levels);
    }

    public ELevelStates? GetLevel(int index)
    {
        List<ELevelStates> levels = LoadAll();

        if (index < 0 || index >= levels.Count)
            return null;

        return levels[index];
    }
    private List<ELevelStates> CreateDefault()
    {
        var defaultLevels = new List<ELevelStates>();

        for (int i = 0; i < levelCount; i++)
            defaultLevels.Add(ELevelStates.Closed);

        return defaultLevels;
    }
}
