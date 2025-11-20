using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private TMP_Text statName;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private Button button;

    public void Setup(
        string name,
        int level,
        int maxLevel,
        int cost,
        System.Action onClick)
    {
        statName.text = name;
        levelText.text = $"{level}/{maxLevel}";
        costText.text = level >= maxLevel ? "MAX" : cost.ToString();

        button.interactable = level < maxLevel;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClick());
    }
}
