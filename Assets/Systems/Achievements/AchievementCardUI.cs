using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementCard : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text conditionText;
    [SerializeField] private TMP_Text rewardText;
    [SerializeField] private TMP_Text progressText;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private Image checkmarkImage;

    private void Awake()
    {
        if (checkmarkImage != null)
            checkmarkImage.gameObject.SetActive(false);
    }

    public void Setup(Achievement ach)
    {
        titleText.text = ach.Title;
        conditionText.text = ach.Description;
        rewardText.text = ach.RewardCoins.ToString();

        if (ach.IsCompleted)
        {
            progressText.text = "DONE";
            progressSlider.maxValue = 1f;
            progressSlider.value = 1f;
            if (checkmarkImage != null) checkmarkImage.gameObject.SetActive(true);
        }
        else
        {
            progressSlider.maxValue = 1f;
            float progress = ach.CurrentProgress / (float)ach.TargetProgress;
            progressSlider.value = Mathf.Clamp01(progress);
            progressText.text = $"{ach.CurrentProgress}/{ach.TargetProgress}";
            if (checkmarkImage != null) checkmarkImage.gameObject.SetActive(false);
        }
    }

}
