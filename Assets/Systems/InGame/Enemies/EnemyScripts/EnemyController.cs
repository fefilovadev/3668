using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(PathFollower), typeof(EnemyAttack), typeof(EnemyHealth))]
public class EnemyController : MonoBehaviour
{
    private PathFollower pathFollower;
    private EnemyAttack enemyAttack;
    private EnemyHealth enemyHealth;
    private EnemyPool pool;
    private GameObject prefab;

    private EnemyConfig currentConfig;
    private bool isBoss;

    private void Awake()
    {
        pathFollower = GetComponent<PathFollower>();
        enemyAttack = GetComponent<EnemyAttack>();
        enemyHealth = GetComponent<EnemyHealth>();
        enemyHealth.EnemyHealthZero += ReturnToPool;
    }

    private void OnEnable()
    {
        pathFollower.OnPathCompleted += HandlePathCompleted;
    }

    private void OnDisable()
    {
        if (pathFollower != null)
            pathFollower.OnPathCompleted -= HandlePathCompleted;
    }

    private void HandlePathCompleted()
    {
        enemyAttack?.Attack();
    }
    public void Init(EnemyPool objectPool, GameObject prefabObj, List<PathData> paths, EnemyConfig config, bool isBoss = false)
    {
        pool = objectPool;
        prefab = prefabObj;
        currentConfig = config;
        pathFollower.Paths = paths;

        transform.localScale = transform.localScale * currentConfig.SizeMultiplier;

        if (enemyHealth != null)
            enemyHealth.DefaultHealth *= currentConfig.HealthMultiplier;
        enemyHealth.Health *= currentConfig.HealthMultiplier;

        if (enemyAttack != null)
            enemyAttack.damage *= currentConfig.DamageMultiplier;

        if (pathFollower != null)
            pathFollower.moveSpeed *= currentConfig.SpeedMultiplier;

        gameObject.SetActive(true);
        pathFollower.StartRandomPath();
        this.isBoss = isBoss;
    }

    public void ReturnToPool()
    {
        if (isBoss)
        {
            AchievementObserver.Instance.Trigger(5, 1); // first boss (5)

            pool?.RemoveFromPool(prefab, this);
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
            pool?.Return(prefab, this);
        }
    }

}
