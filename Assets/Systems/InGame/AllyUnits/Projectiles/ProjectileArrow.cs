using UnityEngine;

public class ProjectileArrow : ProjectileBase
{
    protected override void OnHit(GameObject target)
    {
        if (target != null)
        {
            target.GetComponent<EnemyHealth>().TakeDamage(damage, unitType);
        }
    }
}
