using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;

public class ChickenSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public List<GameObject> Chickens = new List<GameObject>();

    [Header("Egg Settings")]
    [SerializeField] private int maxEggs = 5;
    public int eggs = 3;

    [Header("Start Settings")]
    [SerializeField] private int initialChickensCount = 3;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI eggsText;

    public event Action EggsRestored;

    private void Start()
    {
        UpdateEggsUI();
        StartCoroutine(SpawnInitialChickensWithDelay());
    }

    private IEnumerator SpawnInitialChickensWithDelay()
    {
        yield return null;

        for (int i = 0; i < initialChickensCount; i++)
        {
            SpawnChicken(true);
            yield return null;
        }
    }

    public void SpawnChicken(bool forFree)
    {
        if (eggs <= 0 && !forFree) return;

        Nest emptyNest = NestManager.Instance.GetRandomEmptyNest();
        if (emptyNest == null) return; // нет пустых гнёзд

        if (Chickens.Count == 0) return;

        GameObject prefab = Chickens[UnityEngine.Random.Range(0, Chickens.Count)];
        GameObject chickenObj = Instantiate(prefab, emptyNest.transform.position, Quaternion.identity);

        if (chickenObj.TryGetComponent<DraggableUnit>(out var draggable))
        {
            bool placed = emptyNest.PlaceUnit(draggable);
            if (!placed) Destroy(chickenObj); // защита на случай, если не удалось поставить
        }

        if (!forFree)
        {
            eggs--;
            AchievementObserver.Instance.Trigger(1, 1); // first hatch
        }
        NestManager.Instance.UpdateEmptyNests();
        UpdateEggsUI();
    }

    public void RestoreEgg()
    {
        if (eggs < maxEggs)
        {
            eggs++;
            UpdateEggsUI();
        }
        NestManager.Instance.UpdateEmptyNests();
        AchievementObserver.Instance.Trigger(7, 1); // golden rooster
    }

    private void UpdateEggsUI()
    {
        if (eggsText != null)
            eggsText.text = eggs.ToString();
    }
}
