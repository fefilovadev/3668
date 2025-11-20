using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(AllyUnit))]
public class DraggableUnit : MonoBehaviour
{
    [Header("Drag Settings")]
    [SerializeField] private Sprite dragSprite;

    private Sprite normalSprite;
    private SpriteRenderer spriteRenderer;
    private Collider2D _collider;
    private AllyUnit _allyUnit;

    private bool _isDragging;
    private Vector3 _offset;
    private Vector3 _originalPosition;
    private static DraggableUnit currentDragging;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        _allyUnit = GetComponent<AllyUnit>();
        normalSprite = spriteRenderer.sprite;
    }

    private void Update()
    {
        Vector3 inputPos;
        bool inputDown = false;
        bool inputUp = false;

        if (Input.mousePresent)
        {
            inputPos = Input.mousePosition;
            inputDown = Input.GetMouseButtonDown(0);
            inputUp = Input.GetMouseButtonUp(0);
        }
        else if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            inputPos = touch.position;
            inputDown = touch.phase == TouchPhase.Began;
            inputUp = touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled;
        }
        else return;

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(inputPos);
        worldPos.z = transform.position.z;

        if (inputDown) TryStartDrag(worldPos);
        if (_isDragging) Drag(worldPos);
        if (inputUp && _isDragging) EndDrag();
    }

    private void TryStartDrag(Vector3 worldPos)
    {
        if (_isDragging || currentDragging != null) return;
        if (!_collider.OverlapPoint(worldPos)) return;

        _isDragging = true;
        currentDragging = this;

        _originalPosition = transform.position;
        _offset = transform.position - worldPos;

        _allyUnit.enabled = false;
        spriteRenderer.sprite = dragSprite;

        // очищаем Nest, если был установлен
        if (transform.parent != null && transform.parent.TryGetComponent<Nest>(out var nest))
            nest.ClearNest();

        transform.SetParent(null);
    }

    private void Drag(Vector3 worldPos)
    {
        transform.position = worldPos + _offset;
    }

    private void EndDrag()
    {
        _isDragging = false;
        currentDragging = null;

        bool placed = false;

        Collider2D[] hits = Physics2D.OverlapPointAll(transform.position);
        Nest closestNest = null;
        float minDist = float.MaxValue;

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<Nest>(out var nest))
            {
                float dist = (nest.transform.position - transform.position).sqrMagnitude;
                if (dist < minDist)
                {
                    minDist = dist;
                    closestNest = nest;
                }
            }
        }

        if (closestNest != null)
        {
            placed = closestNest.PlaceUnit(this);
        }

        if (!placed)
        {
            transform.position = _originalPosition;
        }

        _allyUnit.enabled = true;
        spriteRenderer.sprite = normalSprite;
    }
}
