using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BombAnimator))]
public class BombScript : MonoBehaviour
{
    [Header("Explosion")]
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private float postExplosionDelay = 0.5f;

    [Header("VFX / SFX (optional)")]
    [SerializeField] private ParticleSystem explosionVfxPrefab;
    [SerializeField] private AudioClip explosionSfx;
    [SerializeField] private float vfxDestroyDelay = 2f;
    [SerializeField] private float vfxZPosition = 0f;
    [SerializeField] private Vector3 vfxScale = Vector3.one;

    private AudioSource audioSource;
    private BombAnimator bombAnimator;
    private bool isActive;

    private void Awake()
    {
        bombAnimator = GetComponent<BombAnimator>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        gameObject.SetActive(false);
    }

    public void SummonBomb()
    {
        if (isActive) return;

        isActive = true;
        gameObject.SetActive(true);
        bombAnimator.PlayMoveAnimation(OnArrivedToCenter);
    }

    private void OnArrivedToCenter()
    {
        Explode();
    }

    private void Explode()
    {
        int killed = 0;
        if (explosionVfxPrefab)
        {
            Vector3 spawnPos = transform.position;
            spawnPos.z = vfxZPosition;

            var vfx = Instantiate(explosionVfxPrefab, spawnPos, Quaternion.identity);
            vfx.transform.localScale = vfxScale; 
            vfx.Play();
            Destroy(vfx.gameObject, vfxDestroyDelay);
        }

        if (explosionSfx && audioSource)
            audioSource.PlayOneShot(explosionSfx);
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var c in hits)
        {
            if (c && c.CompareTag(enemyTag))
            {
                var health = c.GetComponent<EnemyHealth>();
                if (health != null)
                    health.InstaKill();
                killed ++;
            }
        }
        if (killed >= 5) AchievementObserver.Instance.Trigger(12, 1); //explosive tactics (12)
        StartCoroutine(DeactivateAfterDelay());
    }

    private IEnumerator DeactivateAfterDelay()
    {
        yield return new WaitForSeconds(postExplosionDelay);
        bombAnimator.ResetPosition();
        gameObject.SetActive(false);
        isActive = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
