using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class MenuSceneController : MonoBehaviour
{
    public int LevelId;

    [Header("Daily Charges Settings")]
    [SerializeField] private int dailyChargesAmount = 4;
    [SerializeField] public Button PlayButton;
    private const string LAST_DAILY_KEY = "LastDailyReward";

    private void Start()
    {
        Time.timeScale = 1;
        AddDailyCharges();
        if (PlayerPrefs.GetInt("Onboarded", 0) == 0)
        {
            SceneManager.LoadScene("Onboarding1");
        }
    }

     public void Update()
    {
        PlayButton.interactable = PlayerInventory.Charges >= 2;
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene("Level" + LevelId.ToString());
        PlayerInventory.Charges -= 2;
    }

    private void AddDailyCharges()
    {
        string lastTimeStr = PlayerPrefs.GetString(LAST_DAILY_KEY, "");
        DateTime lastTime;

        if (string.IsNullOrEmpty(lastTimeStr))
        {
            lastTime = DateTime.UtcNow;
            PlayerPrefs.SetString(LAST_DAILY_KEY, lastTime.ToString());
            PlayerPrefs.Save();
            return;
        }

        if (!DateTime.TryParse(lastTimeStr, out lastTime))
        {
            lastTime = DateTime.UtcNow;
        }

        TimeSpan diff = DateTime.UtcNow - lastTime;
        int fullDays = Mathf.FloorToInt((float)diff.TotalDays);

        if (fullDays > 0)
        {
            int totalToAdd = fullDays * dailyChargesAmount;
            PlayerInventory.AddCharges(totalToAdd);

            DateTime newTime = lastTime.AddDays(fullDays);
            PlayerPrefs.SetString(LAST_DAILY_KEY, newTime.ToString());
            PlayerPrefs.Save();
        }
    }
}
