using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class LevelSwitcher : MonoBehaviour
{
    [Header("Layout")]
    [SerializeField] private float sideOffset = 400f;
    [SerializeField] private float focusedScale = 1f;
    [SerializeField] private float sideScale = 0.8f;
    [SerializeField] private float animDuration = 0.3f;
    [SerializeField] private float swipeThreshold = 50f;
    [SerializeField] private Image focusedLevelImage;
    [SerializeField] private TextMeshProUGUI focusedLevelText;

    public UnityEvent InitComplete;

    private RectTransform container;
    private List<LevelUnit> allLevels;
    private int currentIndex = 0;
    private Vector2 dragStart;

    public void SetLevels(List<LevelUnit> levels)
    {
        if (levels == null || levels.Count == 0)
            return;

        DOTween.Init();
        container = GetComponent<RectTransform>();
        allLevels = levels;

        // Определяем текущий открытый уровень
        currentIndex = 0;
        for (int i = 0; i < allLevels.Count; i++)
        {
            if (allLevels[i].LevelState == ELevelStates.Opened)
                currentIndex = i;
        }

        // Если все уровни закрыты, открываем первый
        bool anyOpened = allLevels.Exists(l => l.LevelState != ELevelStates.Closed);
        if (!anyOpened)
        {
            allLevels[0].LevelState = ELevelStates.Opened;
            currentIndex = 0;
        }

        // Активируем все уровни и задаём стартовые позиции для корректной анимации
        for (int i = 0; i < allLevels.Count; i++)
        {
            var unit = allLevels[i];
            unit.transform.SetParent(container, worldPositionStays: false);
            unit.gameObject.SetActive(true);

            int relativeIndex = i - currentIndex;
            float targetX = relativeIndex * sideOffset;
            float targetScale = (relativeIndex == 0) ? focusedScale : sideScale;

            var rt = unit.GetComponent<RectTransform>();

            // Ставим стартовую позицию чуть смещённую, чтобы DOTween видел движение
            rt.anchoredPosition = new Vector2(targetX + Mathf.Sign(relativeIndex) * -200f, rt.anchoredPosition.y);
            unit.transform.localScale = Vector3.one * (targetScale * 0.8f);
        }

        UpdatePositions();
        InitComplete?.Invoke();
    }

    private void UpdatePositions()
    {
        if (allLevels == null || allLevels.Count == 0)
            return;

        for (int i = 0; i < allLevels.Count; i++)
        {
            var level = allLevels[i];
            int relativeIndex = i - currentIndex;

            float targetX = relativeIndex * sideOffset;
            float targetScale = (relativeIndex == 0) ? focusedScale : sideScale;

            var rt = level.GetComponent<RectTransform>();
            rt.DOKill();
            level.transform.DOKill();

            rt.DOAnchorPosX(targetX, animDuration).SetEase(Ease.OutQuad);
            level.transform.DOScale(targetScale, animDuration).SetEase(Ease.OutQuad);

            if (relativeIndex == 0)
                level.transform.SetAsLastSibling();

            level.InFocus = relativeIndex == 0;
        }

        DisplayFocusedLevel(allLevels[currentIndex]);
    }

    public void Next()
    {
        if (currentIndex >= allLevels.Count - 1) return;
        currentIndex++;
        UpdatePositions();
    }

    public void Previous()
    {
        if (currentIndex <= 0) return;
        currentIndex--;
        UpdatePositions();
    }

    private void Update()
    {
        HandleMouseSwipe();
    }

    private void HandleMouseSwipe()
    {
        if (Input.GetMouseButtonDown(0))
            dragStart = Input.mousePosition;

        if (Input.GetMouseButtonUp(0))
        {
            float deltaX = Input.mousePosition.x - dragStart.x;

            if (Mathf.Abs(deltaX) < swipeThreshold)
                return;

            if (deltaX > 0)
                Previous();
            else
                Next();
        }
    }

    private void DisplayFocusedLevel(LevelUnit focusedLevel)
    {
        if (focusedLevelImage != null)
            focusedLevelImage.sprite = focusedLevel.LevelSprite;

        if (focusedLevelText != null)
            focusedLevelText.text = "LEVEL " + focusedLevel.LevelId.ToString();
    }

    public void FocusLastOpenedLevel()
    {
        if (allLevels == null || allLevels.Count == 0)
            return;

        int lastOpenIndex = 0;
        for (int i = 0; i < allLevels.Count; i++)
        {
            if (allLevels[i].LevelState != ELevelStates.Closed)
                lastOpenIndex = i;
        }

        currentIndex = lastOpenIndex;
        UpdatePositions();
    }
}
