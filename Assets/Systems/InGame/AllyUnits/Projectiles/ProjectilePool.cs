using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool
{
    private readonly ProjectileBase prefab;
    private readonly Queue<ProjectileBase> pool = new Queue<ProjectileBase>();
    private readonly Transform parent;
    public ProjectilePool(ProjectileBase prefab, int initialCount = 10, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;

        for (int i = 0; i < initialCount; i++)
        {
            var proj = Object.Instantiate(prefab, parent);
            proj.gameObject.SetActive(false);
            pool.Enqueue(proj);
        }
    }

    public ProjectileBase Spawn(Vector3 position, Quaternion rotation)
    {
        ProjectileBase instance;

        if (pool.Count > 0)
        {
            instance = pool.Dequeue();
        }
        else
        {
            instance = Object.Instantiate(prefab, parent);
        }

        instance.transform.SetPositionAndRotation(position, rotation);
        instance.gameObject.SetActive(true);

        instance.SetPool(this);

        return instance;
    }

    public void ReturnToPool(ProjectileBase projectile)
    {
        projectile.gameObject.SetActive(false);
        if (parent != null)
            projectile.transform.SetParent(parent);

        pool.Enqueue(projectile);
    }
}
