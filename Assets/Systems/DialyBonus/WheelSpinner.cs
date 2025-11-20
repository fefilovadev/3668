using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
public class WheelSpinner : MonoBehaviour
{
    [Header("Wheel Settings")]
    public RectTransform wheel;
    public WheelSegment[] segments;

    [Header("Spin Settings")]
    public float spinDuration = 4f;
    public int fullRotations = 6;
    public AnimationCurve easeCurve;

    [Header("Effects")]
    public WheelEffects effects;

    [HideInInspector]
    public bool IsSpinning { get; private set; }

    private float segmentAngle;
    private float lastTickAngle;

    public event Action<WheelSegment> OnSpinComplete;

    public void Spin()
    {
        if (IsSpinning || segments.Length == 0) return;

        if (easeCurve == null || easeCurve.keys.Length == 0)
            easeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        IsSpinning = true;
        segmentAngle = 360f / segments.Length;
        lastTickAngle = wheel.localEulerAngles.z;

        int targetIndex = UnityEngine.Random.Range(0, segments.Length);

        float targetSectorAngle = targetIndex * segmentAngle + segmentAngle / 2f;

        float currentZ = wheel.localEulerAngles.z;

        float deltaAngleCW = (targetSectorAngle - currentZ + 360f) % 360f;

        float finalAngle = currentZ + fullRotations * 360f + deltaAngleCW;

        wheel
            .DORotate(new Vector3(0, 0, finalAngle), spinDuration, RotateMode.FastBeyond360)
            .SetEase(easeCurve)
            .OnComplete(() =>
            {
                wheel.localEulerAngles = new Vector3(0, 0, targetSectorAngle);

                IsSpinning = false;
                effects?.PlayWin();
                OnSpinComplete?.Invoke(segments[targetIndex]);
            });

        StartCoroutine(TickRoutine());
    }

    private IEnumerator TickRoutine()
    {
        while (IsSpinning)
        {
            float currentAngle = wheel.localEulerAngles.z;
            float delta = Mathf.DeltaAngle(lastTickAngle, currentAngle);

            if (Mathf.Abs(delta) >= segmentAngle)
            {
                effects?.PlayTick();
                lastTickAngle = currentAngle;
            }

            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        if (wheel == null || segments.Length == 0) return;

        segmentAngle = 360f / segments.Length;
        Vector3 center = wheel.position;
        float radius = 100f;

        for (int i = 0; i < segments.Length; i++)
        {
            float angle = i * segmentAngle;
            float rad = Mathf.Deg2Rad * angle;

            Vector3 dir = new Vector3(Mathf.Sin(rad), Mathf.Cos(rad), 0);
            Vector3 end = center + dir * radius;

            Gizmos.color = (i == 0) ? Color.red : Color.white;
            Gizmos.DrawLine(center, end);

#if UNITY_EDITOR
            Handles.color = Color.yellow;
            Vector3 textPos = center + dir * (radius + 15f);
            Handles.Label(textPos, i.ToString());
#endif
        }
    }
}
