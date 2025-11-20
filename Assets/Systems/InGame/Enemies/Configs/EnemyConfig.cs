using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "Configs/EnemyConfig", order = 1)]
public class EnemyConfig : ScriptableObject
{
    [Header("Multipliers")]
    public float HealthMultiplier = 1f;
    public float DamageMultiplier = 1f;
    public float SpeedMultiplier = 1f;
    public float SizeMultiplier = 1f;
}
