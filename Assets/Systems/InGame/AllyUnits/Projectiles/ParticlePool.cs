using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticlePool : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] private ParticleSystem particlePrefab;
    [SerializeField] private int initialSize = 1;

    [Header("Effect Settings")]
    [SerializeField] private Vector3 defaultScale = Vector3.one;
    [SerializeField] private float defaultZ = -5f;

    private readonly List<ParticleSystem> pool = new List<ParticleSystem>();

    private void Awake()
    {
        for (int i = 0; i < initialSize; i++)
            AddNewParticleToPool();
    }
    public void PlayEffect(Vector3 position)
    {
        PlayEffect(position, defaultScale);
    }
    public void PlayEffect(Vector3 position, Vector3 scale)
    {
        ParticleSystem ps = GetFreeParticle();
        position.z = defaultZ;
        ps.transform.position = position;

        ps.gameObject.SetActive(true);
        ps.Play();
        StartCoroutine(ReturnWhenStopped(ps));
    }

    private ParticleSystem GetFreeParticle()
    {
        foreach (var ps in pool)
        {
            if (!ps.gameObject.activeInHierarchy)
                return ps;
        }

        return AddNewParticleToPool();
    }

    private ParticleSystem AddNewParticleToPool()
    {
        var newPs = Instantiate(particlePrefab);
        newPs.gameObject.SetActive(false);

        newPs.transform.position = new Vector3(transform.position.x, transform.position.y, -6f);

        pool.Add(newPs);

        return newPs;
    }





    private IEnumerator ReturnWhenStopped(ParticleSystem ps)
    {
        while (ps.IsAlive(true))
            yield return null;

        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        yield return null;

        ps.gameObject.SetActive(false);
    }
}

