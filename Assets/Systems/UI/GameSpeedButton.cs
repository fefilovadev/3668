using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSpeedButton : MonoBehaviour
{
    private TextMeshProUGUI ButtonText;

    private void Start()
    {
        ButtonText = GetComponentInChildren<TextMeshProUGUI>();
        GetComponent<Button>().onClick.AddListener(SetText);
        SetText();
    }
    private void SetText()
    {
        if (Time.timeScale == 1) ButtonText.text = "X" + 2;
        else if (Time.timeScale == 2) ButtonText.text = "X" + 1;
        else return;
    }
}
