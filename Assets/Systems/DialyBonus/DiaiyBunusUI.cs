using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class DailyBonusUI : MonoBehaviour
{
    [SerializeField] private DailyBonusManager bonusManager;
    [SerializeField] private WheelSpinner spinner;
    [SerializeField] private RewardHandler rewardHandler;

    [SerializeField] private Button spinButton;
    [SerializeField] private TMP_Text spinButtonText;

    private void Start()
    {
        spinner.OnSpinComplete += OnSpinFinished;
    }

    private void Update()
    {
        if (spinner.IsSpinning)
        {
            spinButton.interactable = false;
            spinButtonText.text = "SPIN...";
            return;
        }

        if (bonusManager.CanSpin())
        {
            spinButton.interactable = true;
            spinButtonText.text = "SPIN";
        }
        else
        {
            spinButton.interactable = false;
            TimeSpan t = bonusManager.GetRemainingTime();
            spinButtonText.text = $"{t.Hours:D2}:{t.Minutes:D2}:{t.Seconds:D2}";
        }
    }


    public void OnClickSpin()
    {
        if (!bonusManager.CanSpin()) return;

        bonusManager.SaveSpinTime();
        spinner.Spin();
    }

    private void OnSpinFinished(WheelSegment segment)
    {
        rewardHandler.ApplyReward(segment);
        Debug.Log("Spin Complete!");
    }
}
