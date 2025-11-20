using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveScenarioConfig", menuName = "Configs/WaveScenarioConfig")]
public class WaveScenarioConfig : ScriptableObject
{
    [Header("Scenario Settings")]
    public List<int> BossWaves;
    public int WavesCount = 20;
    public float TimeBetweenWaves;
    public int EnemiesPerFirstWave;
    public float SpawnInterval;

    [Header("Scanerio Settings")]
    public float EnemiesCountScaling;
}