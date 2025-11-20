using UnityEngine;
using UnityEngine.Audio;

public abstract class ProjectileBase : MonoBehaviour
{
    protected Transform target;

    [SerializeField] protected float speed = 5f;
    [SerializeField] protected float damage = 5f;
    [SerializeField] protected AudioClip hitSound;

    private ProjectilePool pool;
    private ParticlePool particlePool;
    public EUnitTypes unitType;

    public void SetPool(ProjectilePool pool)
    {
        this.pool = pool;
    }

    public virtual void Init(Transform target, float baseDamage, float baseSpeed, ParticlePool _particlePool = null, EUnitTypes unitType = EUnitTypes.Base)
    {
        this.target = target;
        speed = baseSpeed;
        gameObject.SetActive(true);
        particlePool = _particlePool;
        this.unitType = unitType;
    }

    protected virtual void Update()
    {
        if (target == null || !target.gameObject.activeSelf)
        {
            ReturnToPool();
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        Vector3 direction = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        if (transform.position == target.position)
        {
            OnHit(target.gameObject);

            AudioSource audioSource = target.GetComponent<AudioSource>();
            if (audioSource != null && hitSound != null && audioSource.gameObject.activeSelf)
                audioSource.PlayOneShot(hitSound);

            if (particlePool != null) particlePool.PlayEffect(transform.position);

            ReturnToPool();
        }
    }


    protected abstract void OnHit(GameObject target);

    protected void ReturnToPool()
    {
        target = null;
        pool?.ReturnToPool(this);
    }
}
