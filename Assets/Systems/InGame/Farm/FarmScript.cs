using System;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class FarmScript : MonoBehaviour
{
    [Header("Health Settings")]
    public float DefaultHealth = 100f;
    public float Health;
    public UnityEvent HealthZero;
    public event Action HealthChanged;

    public bool beenDamaged = false;

    [Header("UI")]
    [SerializeField] private TMP_Text healthText;

    [Header("Effects")]
    [SerializeField] private ParticleSystem damageEffect;
    [SerializeField] private ParticleSystem healEffect;

    private void Awake()
    {
        Health = DefaultHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(float damage)
    {
        if (Health <= 0)
            return;

        Health -= damage;
        if (Health < 0)
            Health = 0;

        PlayDamageEffect();
        UpdateHealthUI();
        HealthChanged.Invoke();

        if (Health == 0)
            HealthZero?.Invoke();

        beenDamaged = true;
    }

    public void Heal(float amount = 1)
    {
        if (Health >= DefaultHealth)
            return;

        Health += amount;
        if (Health > DefaultHealth)
            Health = DefaultHealth;

        PlayHealEffect();
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
            healthText.text = Mathf.CeilToInt(Health).ToString();
    }

    private void PlayDamageEffect()
    {
        if (damageEffect != null)
        {
            damageEffect.transform.position = transform.position;
            damageEffect.gameObject.SetActive(true);
            damageEffect.Play();

            var audio = damageEffect.GetComponent<AudioSource>();
            if (audio != null)
                audio.Play();
        }
    }

    private void PlayHealEffect()
    {
        if (healEffect != null)
        {
            healEffect.transform.position = transform.position;
            healEffect.gameObject.SetActive(true);
            healEffect.Play();

            var audio = healEffect.GetComponent<AudioSource>();
            if (audio != null)
                audio.Play();
        }
    }
}
