using UnityEngine;
using UnityEngine.UI;

public class SpinCheatMultiClick : MonoBehaviour
{
    [Header("Настройка")]
    [SerializeField] private DailyBonusManager spinner;
    [SerializeField] private Button button;
    [SerializeField] private int requiredClicks = 5;

    private int currentClicks = 0;

    private void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();

        if (button != null)
            button.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        currentClicks++;
        if (currentClicks >= requiredClicks)
        {
            spinner.debugInfiniteSpins = true;
            Debug.Log("Cheat activated: " + spinner.debugInfiniteSpins);
            currentClicks = 0;
        }
    }

    private void OnDisable()
    {
        spinner.debugInfiniteSpins = false;
        currentClicks = 0;
        Debug.Log("Cheat deactivated: " + spinner.debugInfiniteSpins);
    }
}

