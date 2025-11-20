using UnityEngine;

public class FrostDebuff : MonoBehaviour, IDebuff
{
    [SerializeField] private float slowPercent = 0.3f;
    private float effectDuration;
    private float duration;
    private bool isStackable;

    private PathFollower follower;
    private float originalSpeed;
    private bool applied;

    private void Start()
    {
        follower = GetComponent<PathFollower>();
        if (follower != null && !applied && duration > 0f)
            ApplySlow();
    }

    private void OnDisable()
    {
        EndDebuff();
    }

    private void Update()
    {
        if (!applied) return;

        if (duration <= 0f)
        {
            EndDebuff();
            return;
        }

        duration -= Time.deltaTime;
    }
    public void AddDebuff(float newDuration, bool stackable, float baseValue)
    {
        isStackable = stackable;
        effectDuration = newDuration;
        duration = newDuration;

        if (follower == null)
            follower = GetComponent<PathFollower>();

        if (follower != null && !applied)
            ApplySlow();
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
        slowPercent += slowPercent * 0.2f;

        if (!applied && follower == null)
            follower = GetComponent<PathFollower>();
        if (!applied && follower != null)
            ApplySlow();
        else if (applied)
            UpdateAppliedSlow();
    }

    public void ResetDebuff()
    {
        duration = effectDuration;
    }

    public void EndDebuff()
    {
        if (!applied) return;

        if (follower != null)
            follower.moveSpeed = originalSpeed;

        applied = false;
        Destroy(this);
    }

    private void ApplySlow()
    {
        if (follower == null) return;

        originalSpeed = follower.moveSpeed;
        follower.moveSpeed *= Mathf.Max(0f, 1f - slowPercent);
        applied = true;
    }

    private void UpdateAppliedSlow()
    {
        if (follower == null) return;
        follower.moveSpeed = originalSpeed;
        follower.moveSpeed *= Mathf.Max(0f, 1f - slowPercent);
    }
}
