using UnityEngine;

[CreateAssetMenu(fileName = "WaveMultConfig", menuName = "Configs/WaveMultConfig")]
public class WaveMultConfig : ScriptableObject
{
    [Header("Wave Scaling")]
    public float EnemyHealthMultiplierPerWave = 0.1f;
    public float EnemyDamageMultiplierPerWave = 0.1f;
    public float EnemySpeedMultiplierPerWave = 0.05f;

    [Header("Boss Wave Scaling")]
    public float BossHealthMultiplierPerWave = 0.1f;
    public float BossDamageMultiplierPerWave = 0.1f;
    public float BossSpeedMultiplierPerWave = 0.05f;
}