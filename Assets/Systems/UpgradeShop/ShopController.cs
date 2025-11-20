using UnityEngine;
using System.Collections.Generic;

public class ShopController : MonoBehaviour
{
    [SerializeField] private List<UpgradableUnit> items;
    [SerializeField] private UpgradeShopPopUp popUp;

    [Header("UI")]
    [SerializeField] private Transform container;
    [SerializeField] private ShopUnitCard cardPrefab;

    private void Awake()
    {
        GenerateCards();
    }

    private void GenerateCards()
    {
        foreach (var item in items)
        {
            var card = Instantiate(cardPrefab, container);
            card.Setup(item.unitSprite, item.config, popUp);
        }
    }
}
