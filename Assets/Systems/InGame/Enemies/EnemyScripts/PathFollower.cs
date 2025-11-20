using System;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    [Header("Paths")]
    public List<PathData> Paths;

    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float arriveThreshold = 0.01f;

    private List<Transform> currentPathPoints;
    private int currentPointIndex;
    private Vector3 targetPoint;
    private bool isMoving;
    public event Action OnPathCompleted;

    public void StartRandomPath()
    {
        ResetState();
        if (Paths == null || Paths.Count == 0)
        {
            isMoving = false;
            return;
        }

        PathData pathData = Paths[UnityEngine.Random.Range(0, Paths.Count)];
        if (pathData == null || pathData.points == null || pathData.points.Count == 0)
        {
            isMoving = false;
            return;
        }

        currentPathPoints = pathData.points;
        currentPointIndex = 0;
        targetPoint = currentPathPoints[0].position;
        isMoving = true;
    }

    private void ResetState()
    {
        currentPathPoints = null;
        currentPointIndex = 0;
        targetPoint = Vector3.zero;
        isMoving = false;
    }
    public void SetTarget(Transform target)
    {
        currentPathPoints = new List<Transform> { target };
        currentPointIndex = 0;
        targetPoint = target.position;
        isMoving = true;
    }

    private void FixedUpdate()
    {
        if (!isMoving || currentPathPoints == null || currentPathPoints.Count == 0) return;

        Vector3 currentPos = transform.position;
        transform.position = Vector3.MoveTowards(currentPos, targetPoint, moveSpeed * Time.fixedDeltaTime);
        if (Vector3.Distance(transform.position, targetPoint) <= arriveThreshold)
        {
            SetNextTarget();
        }
    }

    private void SetNextTarget()
    {
        currentPointIndex++;

        if (currentPointIndex >= currentPathPoints.Count)
        {
            isMoving = false;
            OnPathCompleted?.Invoke();
            return;
        }

        targetPoint = currentPathPoints[currentPointIndex].position;
    }
}
