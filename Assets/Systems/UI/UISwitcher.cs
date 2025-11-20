using System.Collections;
using UnityEngine;

public class UISwitcher : MonoBehaviour
{
    [Header("Fade settings")]
    [SerializeField] private float fadeDuration = 0.3f;
    [SerializeField] private CanvasGroup defaultPanel;
    private CanvasGroup currentPanel;

    private void Awake()
    {
        currentPanel = defaultPanel;
    }
    public void OpenPanel(CanvasGroup target)
    {
        StartCoroutine(FadeOut(currentPanel));
        StartCoroutine(FadeIn(target, true));
    }

    public void FadeInPanel(CanvasGroup target)
    {
        StartCoroutine(FadeIn(target, false));
    }
    public void FadeOutPanel(CanvasGroup target)
    {
        StartCoroutine(FadeOut(target));
    }

    private IEnumerator FadeIn(CanvasGroup cg, bool setCurrent)
    {
        cg.gameObject.SetActive(true);
        cg.interactable = false;
        cg.blocksRaycasts = false;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            cg.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }

        cg.alpha = 1f;
        cg.interactable = true;
        cg.blocksRaycasts = true;

        if (setCurrent) currentPanel = cg;
    }

    private IEnumerator FadeOut(CanvasGroup cg)
    {
        cg.interactable = false;
        cg.blocksRaycasts = false;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            cg.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }

        cg.alpha = 0f;
        cg.gameObject.SetActive(false);
    }
}
