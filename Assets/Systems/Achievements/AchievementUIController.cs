using UnityEngine;

public class AchievementUIController : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private GameObject cardPrefab;

    private AchievementSaveService saveService;
    public TextAsset achievementsJsonAsset;

    private void Start()
    {
        saveService = new AchievementSaveService(achievementsJsonAsset);
        var achievements = saveService.GetAll();

        foreach (var ach in achievements)
        {
            CreateCard(ach);
        }
    }

    private void CreateCard(Achievement ach)
    {
        var card = Instantiate(cardPrefab, container);
        var view = card.GetComponent<AchievementCard>();
        view.Setup(ach);
    }
}
