using UnityEngine;
using UnityEngine.UI;

public class MultiClickButton : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private int clickCountTarget = 5;
    [SerializeField] private int callTimesPerClick = 1;

    private int currentClicks = 0;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        currentClicks++;
        if (currentClicks >= clickCountTarget)
        {
            for (int i = 0; i < callTimesPerClick; i++)
            {
                ExecuteCheat();
            }
            currentClicks = 0;
        }
    }

    private void ExecuteCheat()
    {
        PlayerInventory.AddCharges(10);
    }

    private void OnDisable()
    {
        currentClicks = 0;
    }
}
