using System;

[System.Serializable]
public class Achievement
{
    public int Id;
    public string Title;
    public string Description;
    public int CurrentProgress;
    public int TargetProgress;
    public bool IsCompleted;
    public bool IsRewarded;
    public int RewardCoins;

    public event Action Rewarded;

    public void TryUnlock()
    {
        if (IsCompleted && !IsRewarded)
        {
            IsRewarded = true;
            PlayerInventory.AddCoins(RewardCoins);
        }
        Rewarded?.Invoke();
    }
    public void Increment(int amount = 1)
    {
        CurrentProgress += amount;
    }
    public void Reset()
    {
        CurrentProgress = 0;
    }
}
