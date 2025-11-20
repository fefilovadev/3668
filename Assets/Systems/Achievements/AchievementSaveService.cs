using System.IO;
using UnityEngine;

public class AchievementSaveService
{
    private readonly string savePath;
    private AchievementJsonWrapper data;

    private const string FOLDER_NAME = "GameData";
    private const string FILE_NAME = "achievements.json";

    public AchievementSaveService(TextAsset defaultJsonAsset)
    {
        string folder = Path.Combine(Application.persistentDataPath, FOLDER_NAME);
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        savePath = Path.Combine(folder, FILE_NAME);

        if (!File.Exists(savePath))
        {
            File.WriteAllText(savePath, defaultJsonAsset.text);
        }

        Load();

        foreach(var achievement in data.achievements) achievement.Rewarded += () => Save();
    }

    public void Load()
    {
        string json = File.ReadAllText(savePath);
        data = JsonUtility.FromJson<AchievementJsonWrapper>(json);

        if (data.achievements == null)
            data.achievements = new Achievement[0];
    }

    public Achievement[] GetAll() =>
        data.achievements;

    public void Increment(int id, int amount)
    {
        Achievement ach = Find(id);
        if (ach == null) return;
        if (ach.IsCompleted) return;

        ach.Increment(amount);

        if (ach.CurrentProgress >= ach.TargetProgress)
        {
            ach.IsCompleted = true;
            ach.TryUnlock();
        }

        Save();
    }

    public Achievement Find(int id)
    {
        foreach (var a in data.achievements)
            if (a.Id == id)
                return a;
        return null;
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
    }
}
