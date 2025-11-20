using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float damage = 10f;
    [SerializeField] private float attackRange = 1.5f;

    public void Attack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);
        foreach (var hit in hits)
        {
            FarmScript farm = hit.GetComponent<FarmScript>();
            if (farm != null)
            {
                farm.TakeDamage(damage);
                break;
            }
        }

        gameObject.GetComponent<EnemyController>().ReturnToPool();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
