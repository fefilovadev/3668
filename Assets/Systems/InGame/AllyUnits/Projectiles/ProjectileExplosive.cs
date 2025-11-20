using UnityEngine;

public class ProjectileExplosive : ProjectileBase
{
    [Header("Explosion Settings")]
    [SerializeField] private float explosionRadius = 2.5f;
    [SerializeField] private LayerMask enemyMask;

    protected override void OnHit(GameObject hitTarget)
    {
        Vector2 explosionPoint = transform.position;

        Collider2D[] hits = Physics2D.OverlapCircleAll(explosionPoint, explosionRadius, enemyMask);
        

        
        foreach (var hit in hits)
        {
            var health = hit.GetComponent<EnemyHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.4f);
        Gizmos.DrawSphere(transform.position, explosionRadius);
    }
}
