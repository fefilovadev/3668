using System;
using System.Collections.Generic;
using UnityEngine;

public class DropController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private GameObject dropPrefab;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private WaveController waveController;

    [Header("Pool Settings")]
    [SerializeField] private int initialPoolSize = 10;
    [SerializeField] private bool canExpandPool = true;

    [Header("Drop Logic")]
    [SerializeField] private int startDropCount = 2;
    [SerializeField] private int dropIncreasePerWave = 1;

    private DropPool dropPool;

    private List<int> dropIndices;
    private int enemyDeathCounter;
    private int totalEnemiesThisWave; 
    private int dropThisWave;

    private void Awake()
    {
        dropPool = new DropPool(dropPrefab, initialPoolSize, canExpandPool, transform);

        if (waveController != null)
            waveController.OnWaveStartedForDrop += HandleWaveStarted;

        if (enemySpawner != null)
            enemySpawner.OnEnemyDied += HandleEnemyDied;
    }

    private void OnDestroy()
    {
        if (waveController != null)
            waveController.OnWaveStartedForDrop -= HandleWaveStarted;

        if (enemySpawner != null)
            enemySpawner.OnEnemyDied -= HandleEnemyDied;
    }

    private void HandleWaveStarted(int waveNumber, int enemiesCount)
    {
        totalEnemiesThisWave = enemiesCount;
        dropThisWave = startDropCount + (waveNumber - 1) * dropIncreasePerWave;
        enemyDeathCounter = 0;

        dropIndices = new List<int>();
        HashSet<int> used = new HashSet<int>();
        System.Random rand = new System.Random();

        while (dropIndices.Count < dropThisWave && dropIndices.Count < totalEnemiesThisWave)
        {
            int index = rand.Next(1, totalEnemiesThisWave + 1);
            if (!used.Contains(index))
            {
                used.Add(index);
                dropIndices.Add(index);
            }
        }
    }

    private void HandleEnemyDied(Transform enemyTransform)
    {
        enemyDeathCounter++;

        if (dropIndices.Contains(enemyDeathCounter))
        {
            var dropObj = dropPool.Get();
            dropObj.transform.position = enemyTransform.position;

            var drop = dropObj.GetComponent<DropEgg>();
            drop.SetPool(dropPool);
            Debug.Log(3213);
        }
    }
}
