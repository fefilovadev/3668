using UnityEngine;

public class LevelUnit : MonoBehaviour
{
    public Sprite LevelSprite;
    public int LevelId;

    private LevelUnitUI levelUnitUI;
    private MenuSceneController sceneController;

    private ELevelStates _levelState;
    public ELevelStates LevelState
    {
        get => _levelState;
        set
        {
            _levelState = value;
            levelUnitUI.UpdateLevelUI(value);
        }
    }

    private bool _inFocus;
    public bool InFocus
    {
        get => _inFocus;
        set
        {
            _inFocus = value;
            if (value == true) SetLevelToLoad();
        }
    }

    public void Init(int _levelId, Sprite _levelSprite, MenuSceneController _sceneController)
    {
        LevelId = _levelId;
        LevelSprite = _levelSprite;
        levelUnitUI = GetComponent<LevelUnitUI>();
        levelUnitUI.InitLevelUI(LevelSprite, LevelId, LevelState);
        sceneController = _sceneController;
    }
    private void SetLevelToLoad()
    {
        sceneController.LevelId = LevelId;
    }
    public void InitializePosition(Vector2 startPos, float startScale)
    {
        var rt = GetComponent<RectTransform>();
        rt.anchoredPosition = startPos;
        transform.localScale = Vector3.one * startScale;
    }
}
