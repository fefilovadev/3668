
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerItemsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healText;
    [SerializeField] private TextMeshProUGUI bombText;
    [SerializeField] private Button healButton;
    [SerializeField] private Image healMask;
    [SerializeField] private Button bombButton;
    [SerializeField] private Image bombMask;
    [SerializeField] private Button spawnButton;
    [SerializeField] private Image spawnMask;
    
    public void UpdateText(int healCharges, int bombCharges)
    {
        healText.text = healCharges.ToString();
        bombText.text = bombCharges.ToString();
    }
    public void UpdateHealButton(bool isOn)
    {
        healButton.interactable = isOn;
    }

    public void UpdateSpawnButton(bool isOn)
    {
        spawnButton.interactable = isOn;
    }

    public void UpdateBombButton(bool isOn)
    {
        bombButton.interactable = isOn;
    }

    public void CooldownBombButton(float time)
    {
        StartCoroutine(CooldownTimer(bombButton, bombMask, time));
    }

    public void CooldownHealButton(float time)
    {
        StartCoroutine(CooldownTimer(healButton, healMask, time));
    }

    public void CooldownSpawnButton(float time)
    {
        StartCoroutine(CooldownTimer(spawnButton, spawnMask, time));
    }
    private IEnumerator CooldownTimer(Button button, Image mask, float time)
    {
        button.interactable = false;
        mask.gameObject.SetActive(true);

        float timer = 0f;
        mask.fillAmount = 1f;

        while (timer < time)
        {
            timer += Time.deltaTime;
            mask.fillAmount = 1f - (timer / time);
            button.interactable = false;
            yield return null;
        }

        mask.fillAmount = 0f;
        mask.gameObject.SetActive(false);
        button.interactable = true;
    }
}
