using UnityEngine;
using System;

[DisallowMultipleComponent]
public class EnemyDetector : MonoBehaviour
{
    [Header("Detection")]
    [Tooltip("Radius in world units to search for enemies.")]
    public float detectionRadius = 3f;

    [Tooltip("How often (seconds) to check for enemies. 0 = only on demand.")]
    public float detectionInterval = 0.5f;

    [Tooltip("Optional tag to filter candidates. Leave empty to ignore tag filtering.")]
    public string targetTag = "Enemy";

    [Header("Behavior")]
    [Tooltip("If true, detector will start automatically on Enable.")]
    public bool autoStart = true;

    [Tooltip("If true, invoke event only when the detected target changed.")]
    public bool onlyInvokeOnChange = true;
    public event Action<GameObject> OnEnemyDetected;

    private GameObject _lastDetected;
    private float _timer;
    private bool _running;

    private void OnEnable()
    {
        if (autoStart)
            StartDetect();
    }

    private void OnDisable()
    {
        StopDetect();
    }

    private void Update()
    {
        if (!_running) return;

        if (detectionInterval <= 0f) return;

        _timer += Time.deltaTime;
        if (_timer >= detectionInterval)
        {
            _timer = 0f;
            DetectAndNotify();
        }
    }
    public void DetectNow()
    {
        DetectAndNotify();
    }
    public void StartDetect()
    {
        _running = true;
        _timer = detectionInterval;
        DetectAndNotify();
    }

    public void StopDetect()
    {
        _running = false;
    }

    private void DetectAndNotify()
    {
        GameObject nearest = FindNearestTarget();

        if (onlyInvokeOnChange)
        {
            if (nearest != _lastDetected)
            {
                _lastDetected = nearest;
                OnEnemyDetected?.Invoke(nearest);
            }
        }
        else
        {
            _lastDetected = nearest;
            OnEnemyDetected?.Invoke(nearest);
        }
    }
    private GameObject FindNearestTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius, Physics2D.AllLayers);

        GameObject nearest = null;
        float minSqr = float.MaxValue;
        bool useTag = !string.IsNullOrEmpty(targetTag);

        for (int i = 0; i < hits.Length; i++)
        {
            var c = hits[i];
            if (c == null) continue;

            if (useTag && c.gameObject.tag != targetTag) continue;

            float sqr = (c.transform.position - transform.position).sqrMagnitude;
            if (sqr < minSqr)
            {
                minSqr = sqr;
                nearest = c.gameObject;
            }
        }

        return nearest;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
        Gizmos.DrawSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
