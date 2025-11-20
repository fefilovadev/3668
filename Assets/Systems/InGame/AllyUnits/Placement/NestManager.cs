using UnityEngine;
using System;
using System.Collections.Generic;

public class NestManager : MonoBehaviour
{
    public static NestManager Instance;

    public List<Nest> nests = new List<Nest>();
    private List<Nest> emptyNests = new List<Nest>();
    public bool HasEmptyNest => emptyNests.Count > 0;
    public event Action<bool> NestsUpdated;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        nests.Clear();
        nests.AddRange(FindObjectsByType<Nest>(FindObjectsSortMode.InstanceID));
        UpdateEmptyNests();
    }

    public void UpdateEmptyNests()
    {
        emptyNests.Clear();
        foreach (var nest in nests)
        {
            if (!nest.IsOccupied)
                emptyNests.Add(nest);
        }
        NestsUpdated?.Invoke(emptyNests.Count == 0);
        Debug.Log($"Empty Nests: {emptyNests.Count}");
    }

    public Nest GetRandomEmptyNest()
    {
        if (emptyNests.Count == 0) return null;
        return emptyNests[UnityEngine.Random.Range(0, emptyNests.Count)];
    }

    public Nest GetClosestEmptyNest(Vector3 position)
    {
        UpdateEmptyNests();
        Nest closest = null;
        float minDist = float.MaxValue;
        foreach (var nest in emptyNests)
        {
            float dist = (nest.transform.position - position).sqrMagnitude;
            if (dist < minDist)
            {
                minDist = dist;
                closest = nest;
            }
        }
        return closest;
    }
}
