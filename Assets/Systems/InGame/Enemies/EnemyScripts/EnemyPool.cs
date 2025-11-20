using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool
{
    private readonly Dictionary<GameObject, Queue<EnemyController>> _pools = new();
    private readonly EnemyFactory _factory;
    private readonly int _defaultPrewarmCount;
    private readonly int _expandStep;

    public event Action<Transform> EnemyDespawned;

    public EnemyPool(EnemyFactory factory, int defaultPrewarmCount = 5, int expandStep = 3)
    {
        _factory = factory;
        _defaultPrewarmCount = defaultPrewarmCount;
        _expandStep = expandStep;
    }
    public void Prewarm(GameObject prefab, int count = -1)
    {
        if (!_pools.ContainsKey(prefab))
            _pools[prefab] = new Queue<EnemyController>();

        int prewarmCount = count > 0 ? count : _defaultPrewarmCount;

        for (int i = 0; i < prewarmCount; i++)
        {
            var enemy = _factory.CreateEnemy(prefab);
            enemy.gameObject.SetActive(false);
            _pools[prefab].Enqueue(enemy);
        }
    }
    public EnemyController Get(GameObject prefab)
    {
        if (!_pools.ContainsKey(prefab))
            Prewarm(prefab, _defaultPrewarmCount);

        var queue = _pools[prefab];

        if (queue.Count == 0)
            ExpandPool(prefab);

        var enemy = queue.Dequeue();
        enemy.gameObject.SetActive(true);
        return enemy;
    }
    public void Return(GameObject prefab, EnemyController enemy)
    {
        if (!_pools.ContainsKey(prefab)) _pools[prefab] = new Queue<EnemyController>();

        enemy.gameObject.SetActive(false);
        _pools[prefab].Enqueue(enemy);
        EnemyDespawned?.Invoke(enemy.transform);
    }
    private void ExpandPool(GameObject prefab)
    {
        var queue = _pools[prefab];

        for (int i = 0; i < _expandStep; i++)
        {
            var enemy = _factory.CreateEnemy(prefab);
            enemy.gameObject.SetActive(false);
            queue.Enqueue(enemy);
        }
    }
    public void PrewarmAll(params GameObject[] prefabs)
    {
        foreach (var prefab in prefabs)
            Prewarm(prefab);
    }
    public void RemoveFromPool(GameObject prefab, EnemyController enemy)
    {
        if (_pools.ContainsKey(prefab))
        {
            var queue = _pools[prefab];
            var newQueue = new Queue<EnemyController>();
            foreach (var e in queue)
            {
                if (e != enemy) newQueue.Enqueue(e);
            }
            _pools[prefab] = newQueue;
        }
    }
}

