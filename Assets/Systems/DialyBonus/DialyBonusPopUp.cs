using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class DialyBonusPopUp : MonoBehaviour
{
    [SerializeField] private Sprite heartSprite;
    [SerializeField] private Sprite bombSprite;
    [SerializeField] private Sprite coinSprite;

    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI amountText;

    public void ShowPopUp(WheelSegment segment)
    {
        gameObject.SetActive(true);
        switch (segment.rewardId)
        {
            case "coin":
                itemImage.sprite = coinSprite;
                break;
            case "heal":
                itemImage.sprite = heartSprite;
                break;
            case "bomb":
                itemImage.sprite = bombSprite;
                break;
            default:
                break;
        }
        amountText.text = "+ " + segment.amount.ToString("F0");
    }
}
