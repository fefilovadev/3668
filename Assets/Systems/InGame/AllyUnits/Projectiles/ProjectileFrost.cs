using UnityEngine;

public class ProjectileFrost : ProjectileBase
{
    [SerializeField] private float effectDuration = 5f;
    [SerializeField] private bool effectStackable = true;
    protected override void OnHit(GameObject target)
    {
        if (target != null)
        {
            target.GetComponent<EnemyHealth>().TakeDamage(damage);
        }
        if (target.TryGetComponent(out FrostDebuff debuff))
        {
            debuff.ProlongDebuff();
        }
        else
        {
            var frost = target.AddComponent<FrostDebuff>();
            frost.AddDebuff(effectDuration, effectStackable, damage);
            AchievementObserver.Instance.Trigger(10, 1); //hit 100 enemies with ice (10)
        }
    }
}