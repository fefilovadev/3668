using UnityEngine;
using UnityEngine.UI;
using System;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float DefaultHealth;
    public float Health;

    [Header("UI")]
    [SerializeField] private Image healthFill;

    public event Action EnemyHealthZero;

    private void OnEnable()
    {
        Health = DefaultHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(float damage, EUnitTypes unit = EUnitTypes.Base)
    {
        if (Health <= 0)
            return;

        Health -= damage;
        if (Health < 0)
            Health = 0;

        UpdateHealthUI();

        if (Health <= 0)
            EnemyHealthZero?.Invoke();

        if (unit == EUnitTypes.Sniper) AchievementObserver.Instance.Trigger(13, 1); //sniperShot (13)
    }

    private void UpdateHealthUI()
    {
        if (healthFill != null)
        {
            float fillValue = Health / DefaultHealth;
            healthFill.fillAmount = Mathf.Clamp01(fillValue);
        }
    }

    public void InstaKill()
    {
        Health = 0;
        EnemyHealthZero?.Invoke();
        UpdateHealthUI();
    }
}
