using UnityEngine;
using UnityEngine.UI;

public class ShopUnitCard : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Slider progressSlider;

    private UnitConfig unitConfig;
    private UpgradeShopPopUp popUp;

    public void Setup(Sprite sprite, UnitConfig config, UpgradeShopPopUp _popUp)
    {
        icon.sprite = sprite;
        unitConfig = config;
        popUp = _popUp;
        popUp.Upgraded += () => UpdateSlider();
        UpdateSlider();
    }

    public void ShowPopUp()
    {
        popUp.Show(unitConfig, icon.sprite);
    }

    private void UpdateSlider()
    {
        unitConfig = Instantiate(unitConfig);
        ApplyLevelsToConfig();
        int minSum = 3;
        int maxSum = unitConfig.MaxStatLevel * 3;

        int currentSum =
            unitConfig.PowerLevel +
            unitConfig.RadiusLevel +
            unitConfig.SpeedLevel;

        float progress = (float)(currentSum - minSum) / (maxSum - minSum);
        progressSlider.value = Mathf.Clamp01(progress);
    }
     private void ApplyLevelsToConfig()
    {
        StatsSaverService statsSaver = new StatsSaverService();
        UnitStatsLevels lvl = statsSaver.LoadUnitLevels(unitConfig.Type);
        unitConfig.PowerLevel = lvl.AttackLevel;
        unitConfig.RadiusLevel = lvl.RadiusLevel;
        unitConfig.SpeedLevel = lvl.SpeedLevel;

        unitConfig.Damage = unitConfig.Damage * (1f + unitConfig.DamagePercent * (lvl.AttackLevel - 1));
        unitConfig.AttackRadius = unitConfig.AttackRadius * (1f + unitConfig.RadiusPercent * (lvl.RadiusLevel - 1));
        unitConfig.ProjectileSpeed = unitConfig.ProjectileSpeed * (1f + unitConfig.SpeedPercent * (lvl.SpeedLevel - 1));
    }
}
