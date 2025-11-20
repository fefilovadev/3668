using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SafeAreaFitter : MonoBehaviour
{
    private RectTransform _panel;
    private Rect _lastSafeArea = Rect.zero;

    private void Awake()
    {
        _panel = GetComponent<RectTransform>();
    }

    private void Start()
    {
        ApplySafeAreaDelayed();
    }

    private void ApplySafeArea(Rect safeArea)
    {
        _lastSafeArea = safeArea;

        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        _panel.anchorMin = anchorMin;
        _panel.anchorMax = anchorMax;

        _panel.offsetMin = Vector2.zero;
        _panel.offsetMax = Vector2.zero;
    }

    private void ApplySafeAreaDelayed()
    {
        StartCoroutine(WaitAndApply());
    }

    private System.Collections.IEnumerator WaitAndApply()
    {
        yield return null;
        ApplySafeArea(Screen.safeArea);
    }
}
