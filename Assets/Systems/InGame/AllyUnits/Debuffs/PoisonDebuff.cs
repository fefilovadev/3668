using UnityEngine;

public class PoisonDebuff : MonoBehaviour, IDebuff
{
    [SerializeField] private float tickDamage = 5f;
    [SerializeField] private float tickRate = 1f;

    private float effectDuration;
    private float duration;
    private float tickTimer;
    private bool isStackable;

    private EnemyHealth targetHealth;

    private void Start()
    {
        targetHealth = GetComponent<EnemyHealth>();
    }
    private void OnDisable()
    {
         EndDebuff();
    }
    private void Update()
    {
        if (duration <= 0f)
        {
            EndDebuff();
            return;
        }

        duration -= Time.deltaTime;
        tickTimer -= Time.deltaTime;

        if (tickTimer <= 0f)
        {
            tickTimer = tickRate;
            if (targetHealth != null)
                targetHealth.TakeDamage(tickDamage);
        }
    }

    public void AddDebuff(float newDuration, bool stackable, float baseValue)
    {
        isStackable = stackable;
        effectDuration = newDuration;
        duration = newDuration;
        tickTimer = tickRate;
        tickDamage = baseValue / 10f;
    }

    public void ProlongDebuff()
    {
        if (isStackable)
            StackDebuff();
        else
            ResetDebuff();
    }

    public void StackDebuff()
    {
        duration += effectDuration * 0.5f;
        tickDamage += tickDamage * 0.2f;
    }

    public void ResetDebuff()
    {
        duration = effectDuration;
    }

    public void EndDebuff()
    {
        Destroy(this);
    }
}

