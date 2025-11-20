using System.Collections.Generic;
using UnityEngine;

public class DropPool
{
    private readonly GameObject prefab;
    private readonly Transform parent;
    private readonly bool canExpand;
    private readonly Queue<GameObject> pool = new Queue<GameObject>();

    public DropPool(GameObject prefab, int initialSize, bool canExpand, Transform parent = null)
    {
        this.prefab = prefab;
        this.canExpand = canExpand;
        this.parent = parent;

        for (int i = 0; i < initialSize; i++)
            AddNewObjectToPool();
    }

    private GameObject AddNewObjectToPool()
    {
        var obj = Object.Instantiate(prefab, parent);
        obj.SetActive(false);
        pool.Enqueue(obj);
        return obj;
    }

    public GameObject Get()
    {
        if (pool.Count == 0)
        {
            if (canExpand)
                AddNewObjectToPool();
            else
                return null;
        }

        var obj = pool.Dequeue();
        obj.SetActive(true);
        return obj;
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}

