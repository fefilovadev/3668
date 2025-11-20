using System.Collections;
using UnityEngine;

public class DropEgg : MonoBehaviour
{
    [Header("Lifetime Settings")]
    [SerializeField] private float lifeTime = 1.5f;

    [Header("Effects")]
    [SerializeField] private ParticleSystem particleSystemEffect;
    [SerializeField] private AudioClip dropSound;

    private AudioSource audioSource;
    private DropPool pool;
    private DropEggAnimator animator;
    private Coroutine lifeRoutine;

    public void SetPool(DropPool pool)
    {
        this.pool = pool;
    }

    private void Awake()
    {
        if (particleSystemEffect == null)
            particleSystemEffect = GetComponentInChildren<ParticleSystem>();

        animator = GetComponentInChildren<DropEggAnimator>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    private void OnEnable()
    {
        if (particleSystemEffect != null)
            particleSystemEffect.Play();

        if (dropSound != null)
            audioSource.PlayOneShot(dropSound);

        if (lifeRoutine != null)
            StopCoroutine(lifeRoutine);
        lifeRoutine = StartCoroutine(LifeTimer());
    }

    private IEnumerator LifeTimer()
    {
        yield return new WaitForSeconds(lifeTime);

        var chickenSpawner = FindAnyObjectByType<ChickenSpawner>();
        if (chickenSpawner != null)
            chickenSpawner.RestoreEgg();
        if (animator != null)
        {
            bool done = false;
            animator.PlayDespawnAnimation(() => done = true);
            yield return new WaitUntil(() => done);
        }

        if (pool != null)
            pool.ReturnToPool(gameObject);
        else
            Destroy(gameObject);
    }

    private void OnDisable()
    {
        if (lifeRoutine != null)
            StopCoroutine(lifeRoutine);
    }
}
