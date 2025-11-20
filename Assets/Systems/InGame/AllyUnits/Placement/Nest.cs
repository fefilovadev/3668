using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class Nest : MonoBehaviour
{
    private AllyUnit _unit;
    private SpriteRenderer _spriteRenderer;
    
    private const float CheckInterval = 0.1f;
    private Coroutine _checkCoroutine;

    public bool IsOccupied => _unit != null && _unit.gameObject.activeInHierarchy;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateVisual();
        
        _checkCoroutine = StartCoroutine(SelfCheckRoutine());
    }

    private IEnumerator SelfCheckRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(CheckInterval);

            if (!IsOccupied)
            {
                var draggable = GetComponentInChildren<DraggableUnit>(includeInactive: false);
                
                if (draggable != null)
                {
                    PlaceUnit(draggable);
                }
            }
        }
    }

    private void UpdateVisual()
    {
        if (_spriteRenderer != null)
            _spriteRenderer.enabled = !IsOccupied;
    }

    public bool PlaceUnit(DraggableUnit draggable)
    {
        if (draggable == null) return false;
        var newUnit = draggable.GetComponent<AllyUnit>();
        if (newUnit == null) return false;

        if (IsOccupied)
        {
            var existingMerger = _unit.GetComponent<UnitMerger>();
            var newMerger = newUnit.GetComponent<UnitMerger>();
            if (existingMerger != null && newMerger != null)
            {
                AllyUnit survivor = existingMerger.TryMerge(newMerger);
                if (survivor == null) return false;

                _unit = survivor;
                _unit.transform.position = transform.position;
                _unit.transform.SetParent(this.transform);
                UpdateVisual();
                NotifyManager();
                return true;
            }
            return false;
        }

        _unit = newUnit;
        _unit.transform.position = transform.position;
        draggable.transform.SetParent(this.transform); 
        UpdateVisual();
        NotifyManager();
        return true;
    }

    public void ClearNest()
    {
        _unit = null;
        UpdateVisual();
        NotifyManager();
    }

    private void NotifyManager()
    {
        NestManager.Instance?.UpdateEmptyNests();
    }
}