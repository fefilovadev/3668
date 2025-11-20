using UnityEngine;

[RequireComponent(typeof(UnitMerger))]
public class AllyUnit : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private UnitConfig _config;
    [SerializeField] private AudioClip shootSound;

    [Header("Projectile Settings")]
    [SerializeField] private ProjectileBase projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;

    private ProjectilePool projectilePool;
    private float attackCooldown;
    private GameObject priorityTarget;
    private AudioSource audioSource;
    private ParticlePool particlePool;

    public UnitConfig Config => _config;

    private void Awake()
    {
        
        _config = Instantiate(_config);

        StatsSaverService statsSaverService = new StatsSaverService();
        UnitStatsLevels levels = statsSaverService.LoadUnitLevels(_config.Type);

        ApplyLevelsToConfig(levels);

        projectilePool = new ProjectilePool(projectilePrefab, 10, transform);

        audioSource = GetComponent<AudioSource>();
        if (TryGetComponent(out ParticlePool _particlePool))
            particlePool = _particlePool;
        else
            particlePool = null;
    }

    private void Update()
    {
        if (_config == null) return;

        attackCooldown -= Time.deltaTime;

        GameObject target = FindNearestEnemy();
        if (priorityTarget != null && InRange(priorityTarget))
            target = priorityTarget;

        if (target != null && attackCooldown <= 0f)
        {
            Attack(target);
            attackCooldown = 1f / _config.AttackRate;
        }
    }

    private void Attack(GameObject target)
    {
        if (projectilePool == null || projectilePrefab == null) return;

        var proj = projectilePool.Spawn(projectileSpawnPoint.position, Quaternion.identity);
        proj.Init(target.transform, _config.Damage, _config.ProjectileSpeed, particlePool);

        audioSource.PlayOneShot(shootSound);
    }

    private bool InRange(GameObject target)
    {
        return Vector2.Distance(transform.position, target.transform.position) <= _config.AttackRadius;
    }

    private GameObject FindNearestEnemy()
    {
        var hits = Physics2D.OverlapCircleAll(transform.position, _config.AttackRadius);
        GameObject nearest = null;
        float minDist = float.MaxValue;

        foreach (var h in hits)
        {
            if (h.CompareTag("Enemy"))
            {
                float dist = Vector2.Distance(transform.position, h.transform.position);
                if (dist < minDist)
                {
                    nearest = h.gameObject;
                    minDist = dist;
                }
            }
        }

        return nearest;
    }
    public void ApplyConfig(UnitConfig newConfig)
    {
        _config = newConfig;
    }

    private void ApplyLevelsToConfig(UnitStatsLevels lvl)
    {
        _config.PowerLevel = lvl.AttackLevel;
        _config.RadiusLevel = lvl.RadiusLevel;
        _config.SpeedLevel = lvl.SpeedLevel;

        _config.Damage = _config.Damage * (1f + _config.DamagePercent * (lvl.AttackLevel - 1));
        _config.AttackRadius = _config.AttackRadius * (1f + _config.RadiusPercent * (lvl.RadiusLevel - 1));
        _config.ProjectileSpeed = _config.ProjectileSpeed * (1f + _config.SpeedPercent * (lvl.SpeedLevel - 1));
    }

    private void OnDrawGizmosSelected()
    {
        if (_config == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _config.AttackRadius);
    }
}
