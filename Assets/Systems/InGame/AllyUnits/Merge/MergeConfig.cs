using UnityEngine;

[CreateAssetMenu(fileName = "UnitConfig", menuName = "Configs/UnitConfig", order = 1)]

[System.Serializable]
public class UnitConfig : ScriptableObject
{
    [Header("Main Info")]
    public EUnitTypes Type;
    public int Level = 1;
    public int PowerLevel = 1;
    public int SpeedLevel = 1;
    public int RadiusLevel = 1;
    public int MaxStatLevel = 6;

    [Header("Base Stats")]
    public float Damage = 10f;
    public float AttackRate = 1f;
    public float ProjectileSpeed = 10f;
    public float AttackRadius = 3f;

    [Header("Merge Bonuses (%)")]
    [Range(0f, 1f)] public float DamageBonusPercent = 0.2f;
    [Range(0f, 1f)] public float SpeedBonusPercent = 0.1f;
    [Range(0f, 1f)] public float RadiusBonusPercent = 0.05f;

    [Header("Upgrade Bonuses (%)")]
    [Range(0f, 1f)] public float DamagePercent = 0.2f;
    [Range(0f, 1f)] public float SpeedPercent = 0.1f;
    [Range(0f, 1f)] public float RadiusPercent = 0.05f;
}
