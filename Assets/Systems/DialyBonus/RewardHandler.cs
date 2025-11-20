using UnityEngine;

public class RewardHandler : MonoBehaviour
{
    [SerializeField] private DialyBonusPopUp popUp;
    public void ApplyReward(WheelSegment seg)
    {
        Debug.Log($"Reward: {seg.rewardId}, +{seg.amount}");

        if (seg.rewardId == "coin")
        {
            PlayerInventory.AddCoins(seg.amount);
        }
         if (seg.rewardId == "bomb")
        {
            PlayerInventory.AddBombs(seg.amount);
        }
         if (seg.rewardId == "heal")
        {
            PlayerInventory.AddHeals(seg.amount);
        }  
        popUp.ShowPopUp(seg);
        Debug.Log(321321);
    }
}
