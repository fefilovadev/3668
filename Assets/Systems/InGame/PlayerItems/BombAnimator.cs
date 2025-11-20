using System;
using UnityEngine;
using DG.Tweening;

public class BombAnimator : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private Vector2 startPosition = new Vector2(-10f, -2f);
    [SerializeField] private Vector2 targetPosition = new Vector2(0f, -2f);
    [SerializeField] private float moveDuration = 1.5f;
    [SerializeField] private Ease moveEase = Ease.OutSine;

    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 360f;
    [SerializeField] private bool rotateClockwise = true;

    private Tween moveTween;
    private Tween rotateTween;
    private float initialZ;

    private void Awake()
    {
        initialZ = transform.position.z;
    }

    public void PlayMoveAnimation(Action onArrived)
    {
        transform.position = new Vector3(startPosition.x, startPosition.y, initialZ);

        moveTween?.Kill();
        rotateTween?.Kill();

        float finalAngle = (rotateClockwise ? -1f : 1f) * rotationSpeed * moveDuration;
        rotateTween = transform.DORotate(new Vector3(0, 0, finalAngle), moveDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear);

        moveTween = transform.DOMove(new Vector3(targetPosition.x, targetPosition.y, initialZ), moveDuration)
            .SetEase(moveEase)
            .OnComplete(() =>
            {
                onArrived?.Invoke();
            });
    }
    public void ResetPosition()
    {
        moveTween?.Kill();
        rotateTween?.Kill();
        transform.position = new Vector3(startPosition.x, startPosition.y, initialZ);
        transform.rotation = Quaternion.identity;
    }
}
