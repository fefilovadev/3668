using System;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
public class DropEggAnimator : MonoBehaviour
{
    [Header("Despawn Animation")]
    [SerializeField] private float rotationDegrees = 360f;
    [SerializeField] private float duration = 0.6f;
    [SerializeField] private float fadeTo = 0f;
    [SerializeField] private Ease ease = Ease.InOutQuad;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void PlayDespawnAnimation(Action onComplete)
    {
        transform.DOKill();
        spriteRenderer.DOKill();

        Sequence seq = DOTween.Sequence();

        seq.Join(transform.DORotate(Vector3.forward * rotationDegrees, duration, RotateMode.LocalAxisAdd)
            .SetEase(ease));
        seq.Join(spriteRenderer.DOFade(fadeTo, duration));

        seq.OnComplete(() =>
        {
            Color c = spriteRenderer.color;
            c.a = 1f;
            spriteRenderer.color = c;
            onComplete?.Invoke();
        });

        seq.Play();
    }
}
