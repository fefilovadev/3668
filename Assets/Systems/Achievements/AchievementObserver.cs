using UnityEngine;

public class AchievementObserver : MonoBehaviour
{
    public static AchievementObserver Instance { get; private set; }

    [Header("JSON Asset Reference")]
    public TextAsset achievementsJsonAsset;

    private AchievementSaveService service;

    public Achievement[] AllAchievements => service.GetAll();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);

        service = new AchievementSaveService(achievementsJsonAsset);
    }

    public void Trigger(int id, int amount = 1)
    {
        service.Increment(id, amount);
    }
}
