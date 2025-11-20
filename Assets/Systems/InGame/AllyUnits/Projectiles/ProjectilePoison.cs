using UnityEngine;

public class ProjectilePoison : ProjectileBase
{
    [SerializeField] private float effectDuration = 5f;
    [SerializeField] private bool effectStackable = true;

    protected override void OnHit(GameObject target)
    {
        if (target.TryGetComponent(out IDebuff debuff))
        {
            debuff.ProlongDebuff();
        }
        else
        {
            var poison = target.AddComponent<PoisonDebuff>();
            poison.AddDebuff(effectDuration, effectStackable, damage);
            AchievementObserver.Instance.Trigger(11, 1); //hit 200 enemies with poison (11)
        }
    }
}
