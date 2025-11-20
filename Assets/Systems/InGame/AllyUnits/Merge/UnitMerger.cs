using UnityEngine;

[RequireComponent(typeof(AllyUnit))]
public class UnitMerger : MonoBehaviour
{
    private AllyUnit _unit;
    private const int MAX_LEVEL = 6;

    private void Awake()
    {
        _unit = GetComponent<AllyUnit>();
    }

    public AllyUnit TryMerge(UnitMerger other)
    {
        if (other == null || _unit == null || other._unit == null)
            return null;

        var myCfg = _unit.Config;
        var otherCfg = other._unit.Config;

        if (myCfg == null || otherCfg == null)
            return null;

        if (myCfg.Type != otherCfg.Type)
            return null;

        int mergedLevel = myCfg.Level + otherCfg.Level;
        if (mergedLevel > MAX_LEVEL)
            return null;

        AllyUnit survivor = myCfg.Level >= otherCfg.Level ? _unit : other._unit;
        AllyUnit toDestroy = survivor == _unit ? other._unit : _unit;

        PlayMergeEffect(survivor);

        UnitConfig baseCfg = survivor.Config;
        UnitConfig newConfig = Instantiate(baseCfg);

        int levelDiff = mergedLevel - baseCfg.Level;
        newConfig.Level = mergedLevel;

        newConfig.Damage = baseCfg.Damage * (1f + baseCfg.DamageBonusPercent * levelDiff);
        newConfig.ProjectileSpeed = baseCfg.ProjectileSpeed * (1f + baseCfg.SpeedBonusPercent * levelDiff);
        newConfig.AttackRadius = baseCfg.AttackRadius * (1f + baseCfg.RadiusBonusPercent * levelDiff);

        survivor.ApplyConfig(newConfig);

        if (survivor.TryGetComponent<UnitMergeUI>(out var mergeUI))
            mergeUI.UpdateUI(newConfig.Level);

        Destroy(toDestroy.gameObject);
        AchievementObserver.Instance.Trigger(2, 1); //first merge (2)
        if (survivor.Config.Level == 6) AchievementObserver.Instance.Trigger(6, 1); //golden rooster (6)
        return survivor;
    }
    private void PlayMergeEffect(AllyUnit survivor)
    {
        if (survivor == null)
            return;

        var ps = survivor.GetComponentInChildren<ParticleSystem>(includeInactive: true);
        if (ps == null)
            return;

        ps.gameObject.SetActive(true);
        ps.transform.position = survivor.transform.position;
        ps.Play();

        var audio = ps.GetComponent<AudioSource>();
        if (audio != null)
            audio.Play();
    }
}
