using UnityEngine;

public class UIPanel : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private UISwitcher switcher;

    public void Open()
    {
        Init();
        switcher.OpenPanel(canvasGroup);
    }

    public void FadeIn()
    {
        Init();
        switcher.FadeInPanel(canvasGroup);
    }

    public void FadeOut()
    {
        Init();
        switcher.FadeOutPanel(canvasGroup);
    }

    private void Init()
    {
        if (canvasGroup == null || switcher == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
            switcher = FindAnyObjectByType<UISwitcher>();
        }
    }
}
