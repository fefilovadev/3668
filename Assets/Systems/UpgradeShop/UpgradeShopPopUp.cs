using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UpgradeShopPopUp : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image unitImage;
    [SerializeField] private TMP_Text damageLevelText;
    [SerializeField] private TMP_Text speedLevelText;
    [SerializeField] private TMP_Text radiusLevelText;

    [SerializeField] private Button damageUpgradeButton;
    [SerializeField] private Button speedUpgradeButton;
    [SerializeField] private Button radiusUpgradeButton;

    [SerializeField] private TMP_Text damageCostText;
    [SerializeField] private TMP_Text speedCostText;
    [SerializeField] private TMP_Text radiusCostText;

    [Header("Settings")]
    [SerializeField] private int baseCost = 100;

    private UnitConfig baseConfig;
    private UnitStatsLevels currentLevels;
    private EUnitTypes unitType;
    private StatsSaverService statsSaver;
    
    public event Action Upgraded;
     

    public void Show(UnitConfig config, Sprite unitSprite)
    {
        gameObject.GetComponent<UIPanel>().FadeIn();
        statsSaver = new StatsSaverService();
        unitType = config.Type;
        baseConfig = config;
        currentLevels = statsSaver.LoadUnitLevels(unitType);
        unitImage.sprite = unitSprite;

        RefreshUI();
    }

    private void RefreshUI()
    {
        int max = baseConfig.MaxStatLevel;

        damageLevelText.text = $"{currentLevels.AttackLevel}/{max}";
        speedLevelText.text = $"{currentLevels.SpeedLevel}/{max}";
        radiusLevelText.text = $"{currentLevels.RadiusLevel}/{max}";

        damageCostText.text = currentLevels.AttackLevel >= max ? "MAX" : GetCost(currentLevels.AttackLevel).ToString();
        speedCostText.text = currentLevels.SpeedLevel >= max ? "MAX" : GetCost(currentLevels.SpeedLevel).ToString();
        radiusCostText.text = currentLevels.RadiusLevel >= max ? "MAX" : GetCost(currentLevels.RadiusLevel).ToString();

        damageUpgradeButton.interactable = currentLevels.AttackLevel < max && PlayerInventory.Coins >= GetCost(currentLevels.AttackLevel);
        speedUpgradeButton.interactable = currentLevels.SpeedLevel < max && PlayerInventory.Coins >= GetCost(currentLevels.SpeedLevel);
        radiusUpgradeButton.interactable = currentLevels.RadiusLevel < max && PlayerInventory.Coins >= GetCost(currentLevels.RadiusLevel);

        damageUpgradeButton.onClick.RemoveAllListeners();
        damageUpgradeButton.onClick.AddListener(() => UpgradeStat(ref currentLevels.AttackLevel));

        speedUpgradeButton.onClick.RemoveAllListeners();
        speedUpgradeButton.onClick.AddListener(() => UpgradeStat(ref currentLevels.SpeedLevel));

        radiusUpgradeButton.onClick.RemoveAllListeners();
        radiusUpgradeButton.onClick.AddListener(() => UpgradeStat(ref currentLevels.RadiusLevel));
    }

    private int GetCost(int lvl) => Mathf.RoundToInt(baseCost);

    private void UpgradeStat(ref int levelField)
    {
        int cost = GetCost(levelField);

        if (PlayerInventory.Coins < cost)
            return;

        PlayerInventory.Coins -= cost;
        levelField++;
        statsSaver.SaveUnitLevels(unitType, currentLevels);

        RefreshUI();
        Upgraded?.Invoke();
    }
}
