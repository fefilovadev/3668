using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private LevelUnit levelPrefab;
    [SerializeField] private LevelSpritesSO spritesSO;
    [SerializeField] private MenuSceneController sceneController;
    [SerializeField] private int levelCount = 9;

    private LevelSaveService saveService;
    private List<LevelUnit> units = new();
    private LevelSwitcher levelSwitcher;

    public void Start()
    {
        saveService = new LevelSaveService(levelCount);
        var savedStates = saveService.LoadAll();

        if (savedStates.Count > 0)
            savedStates[0] = ELevelStates.Opened;

        SpawnLevelUnits(savedStates);
        TryOpenNextLevel();

        levelSwitcher = GetComponent<LevelSwitcher>();
        StartCoroutine(DelayedSet());
        
        saveService.SaveAll(savedStates);
    }

    private void SpawnLevelUnits(List<ELevelStates> savedStates)
    {
        units.Clear();

        for (int i = 0; i < levelCount; i++)
        {
            LevelUnit unit = Instantiate(levelPrefab, transform);
            unit.gameObject.SetActive(false);

            int levelNumber = i + 1;
            Sprite levelSprite = GetSprite(i);

            unit.Init(levelNumber, levelSprite, sceneController);
            unit.LevelState = savedStates[i];

            units.Add(unit);
        }
    }

    private void TryOpenNextLevel()
    {
        int lastOpenIndex = -1;
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i].LevelState != ELevelStates.Closed)
                lastOpenIndex = i;
        }

        if (lastOpenIndex == -1 || lastOpenIndex >= units.Count - 1)
            return;

        LevelUnit lastOpenLevel = units[lastOpenIndex];

        if (lastOpenLevel.LevelState == ELevelStates.ThreeStars)
        {
            LevelUnit nextLevel = units[lastOpenIndex + 1];
            if (nextLevel.LevelState == ELevelStates.Closed)
            {
                nextLevel.LevelState = ELevelStates.Opened;
                var savedStates = saveService.LoadAll();
                savedStates[lastOpenIndex + 1] = ELevelStates.Opened;
                saveService.SaveAll(savedStates);
            }
        }
    }

    private Sprite GetSprite(int index)
    {
        if (spritesSO.sprites.Count == 0)
            return null;

        if (index < spritesSO.sprites.Count)
            return spritesSO.sprites[index];

        return spritesSO.sprites[^1];
    }

    private IEnumerator DelayedSet()
    {
        yield return null; // Ждём конца кадра
        levelSwitcher.SetLevels(units);
    }
}
