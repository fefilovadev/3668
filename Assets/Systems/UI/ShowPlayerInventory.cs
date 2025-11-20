using TMPro;
using UnityEngine;

public class ShowPlayerInventory : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI chargesText;
    void Update()
    {
        coinsText.text = PlayerInventory.Coins.ToString("F0");
        chargesText.text = PlayerInventory.Charges.ToString("F0") + "/" + PlayerInventory.MaxCharges.ToString("F0");
    }
}
