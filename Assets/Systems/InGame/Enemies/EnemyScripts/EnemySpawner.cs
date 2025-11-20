using UnityEngine;
using System.Collections.Generic;
using System;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private List<PathData> pathDatasets;
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private int poolSizePerPrefab;
    [SerializeField] private int poolExpandStep;


    private EnemyConfig normalEnemyConfig;
    private EnemyConfig bossConfig; private EnemyFactory _factory;
    private EnemyPool _pool;

    public event Action<Transform> OnEnemyDied;

    private void Awake()
    {
        _factory = new EnemyFactory();
        _pool = new EnemyPool(_factory, poolSizePerPrefab, poolExpandStep);
        _pool.EnemyDespawned += (Transform transform) => OnEnemyDied.Invoke(transform);

        PathData[] childrenPaths = GetComponentsInChildren<PathData>(true);
        if (childrenPaths.Length > 0)
        {
            pathDatasets.InsertRange(0, childrenPaths);
        }
    }

    public void SetConfigs(EnemyConfig enemyCfg, EnemyConfig bossCfg)
    {
        normalEnemyConfig = enemyCfg;
        bossConfig = bossCfg;
    }

    public void SpawnEnemy()
    {
        if (enemyPrefabs.Count == 0 || spawnPoint == null) return;

        var prefab = enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Count)];
        var enemy = _pool.Get(prefab);
        if (enemy != null)
        {
            enemy.transform.position = spawnPoint.position;
            enemy.transform.SetParent(transform);
            enemy.Init(_pool, prefab, pathDatasets, normalEnemyConfig);
        }
    }

    public void SpawnBoss()
    {
        if (enemyPrefabs.Count == 0 || spawnPoint == null) return;

        var prefab = enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Count)];
        var enemy = _pool.Get(prefab);
        if (enemy != null)
        {
            enemy.transform.position = spawnPoint.position;
            enemy.transform.SetParent(transform);
            enemy.Init(_pool, prefab, pathDatasets, bossConfig, true);
        }
    }
}
