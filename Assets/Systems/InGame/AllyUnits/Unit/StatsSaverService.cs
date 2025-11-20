using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class StatsSaverService
{
    private readonly string SavePath;
    private Wrapper data;

    [System.Serializable]
    private class Wrapper
    {
        public List<UnitEntry> units = new List<UnitEntry>();
        [System.NonSerialized] public Dictionary<string, UnitStatsLevels> dict = new Dictionary<string, UnitStatsLevels>();
    }

    public StatsSaverService()
    {
        SavePath = Path.Combine(Application.persistentDataPath, "GameData/unit_stats.json");

        string folder = Path.GetDirectoryName(SavePath);
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        Load();
    }

    private void Load()
    {
        if (!File.Exists(SavePath))
        {
            data = new Wrapper();
            data.dict = new Dictionary<string, UnitStatsLevels>();
            Save();
            return;
        }

        string json = File.ReadAllText(SavePath);
        data = JsonUtility.FromJson<Wrapper>(json);
        if (data.units == null) data.units = new List<UnitEntry>();

        data.dict = new Dictionary<string, UnitStatsLevels>();
        foreach (var entry in data.units)
            data.dict[entry.key] = entry.value;
    }

    private void Save()
    {
        data.units = new List<UnitEntry>();
        foreach (var kvp in data.dict)
            data.units.Add(new UnitEntry { key = kvp.Key, value = kvp.Value });

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
    }

    public UnitStatsLevels LoadUnitLevels(EUnitTypes type)
    {
        string key = type.ToString();
        if (!data.dict.ContainsKey(key))
        {
            data.dict[key] = new UnitStatsLevels();
            Save();
        }

        return data.dict[key];
    }

    public void SaveUnitLevels(EUnitTypes type, UnitStatsLevels lvl)
    {
        string key = type.ToString();
        data.dict[key] = lvl;
        Save();
    }
}