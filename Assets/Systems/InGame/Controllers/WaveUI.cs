using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
    [Header("Wave UI")]
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private Slider waveProgressSlider;
    [SerializeField] private TextMeshProUGUI waveMessageText;
    [SerializeField] private float countdownInterval = 1f;
    [SerializeField] private float messageDisplayTime = 2f;

    private int totalEnemiesThisWave;

    public void Initialize(int totalWaves)
    {
        waveText.text = $"0/{totalWaves}";
        waveProgressSlider.value = 0f;
        waveProgressSlider.maxValue = 1f;
        waveMessageText.text = "";
    }

    public void OnWaveStarted(int waveNumber, int totalWaves = 20, bool isBossWave = false)
    {
        waveText.text = $"{waveNumber}/{totalWaves}";
        waveProgressSlider.value = 0f;
        totalEnemiesThisWave = 0;

        if (isBossWave)
            ShowMessage("BOSS WAVE!");
        else
            ShowMessage("WAVE STARTED");
    }

    public void OnWaveEnded(bool isBossWave = false)
    {
        waveProgressSlider.value = 1f;

        if (isBossWave)
            ShowMessage("BOSS DEFEATED");
        else
            ShowMessage("WAVE ENDED");
    }

    public void OnEnemySpawnedInWave(int spawnedCount, int totalEnemies)
    {
        totalEnemiesThisWave = totalEnemies;
        waveProgressSlider.value = Mathf.Clamp01((float)spawnedCount / totalEnemiesThisWave);
    }

    private void ShowMessage(string message)
    {
        StopAllCoroutines();
        StartCoroutine(ShowMessageCoroutine(message));
    }

    private IEnumerator ShowMessageCoroutine(string message)
    {
        waveMessageText.text = message;
        yield return new WaitForSeconds(messageDisplayTime);
        waveMessageText.text = "";
    }
    public IEnumerator StartCountdownCoroutine()
    {
        int countdown = 3;
        while (countdown > 0)
        {
            waveMessageText.text = countdown.ToString();
            yield return new WaitForSeconds(countdownInterval);
            countdown--;
        }

        waveMessageText.text = "START!";
        yield return new WaitForSeconds(messageDisplayTime);
        waveMessageText.text = "";
    }
}
