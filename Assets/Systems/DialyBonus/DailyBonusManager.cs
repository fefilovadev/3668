using System;
using UnityEngine;

public class DailyBonusManager : MonoBehaviour
{
    private const string LastSpinKey = "LastDailySpin";

    [Header("Debug")]
    public bool debugInfiniteSpins = false;

    public bool CanSpin()
    {
        if (debugInfiniteSpins)
            return true;

        if (!PlayerPrefs.HasKey(LastSpinKey))
            return true;

        long binary = Convert.ToInt64(PlayerPrefs.GetString(LastSpinKey));
        DateTime lastSpin = DateTime.FromBinary(binary);

        return (DateTime.UtcNow - lastSpin).TotalHours >= 24;
    }

    public TimeSpan GetRemainingTime()
    {
        if (debugInfiniteSpins)
            return TimeSpan.Zero;

        if (!PlayerPrefs.HasKey(LastSpinKey))
            return TimeSpan.Zero;

        long binary = Convert.ToInt64(PlayerPrefs.GetString(LastSpinKey));
        DateTime lastSpin = DateTime.FromBinary(binary);

        TimeSpan passed = DateTime.UtcNow - lastSpin;
        TimeSpan duration = TimeSpan.FromHours(24);

        return duration - passed;
    }

    public void SaveSpinTime()
    {
        if (debugInfiniteSpins)
            return;

        long binary = DateTime.UtcNow.ToBinary();
        PlayerPrefs.SetString(LastSpinKey, binary.ToString());
        PlayerPrefs.Save();
    }
}
