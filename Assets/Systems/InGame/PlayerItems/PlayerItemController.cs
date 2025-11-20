using UnityEngine;

[RequireComponent(typeof(PlayerItemsUI))]
public class PlayerItemController : MonoBehaviour
{
    [SerializeField] private int cooldown;
    [SerializeField] private FarmScript Farm;
    [SerializeField] private BombScript Bomb;
    [SerializeField] private WaveController waveController;
    [SerializeField] private ChickenSpawner spawner;

    private PlayerItemsUI itemsUI;

    private void Awake()
    {
        itemsUI = GetComponent<PlayerItemsUI>();
    }

    private void Start()
    {
        // Подписка на волны
        if (waveController != null)
            waveController.OnWaveStateChanged += HandleWaveStateChanged;

        // Подписка на изменения здоровья
        if (Farm != null)
            Farm.HealthChanged += ToggleHealButton;

        // Подписка на события спавнера
        if (spawner != null)
        {
            NestManager.Instance.NestsUpdated += ToggleSpawnButton;
        }

        // Начальное обновление кнопок
        ToggleHealButton();
        ToggleBombButton();
        ToggleSpawnButton();
        UpdateUI();
    }

    private void HandleWaveStateChanged(bool isWaveActive)
    {
        // Бомба доступна только во время волны и если есть бомбы
        itemsUI.UpdateBombButton(PlayerInventory.Bombs > 0 && isWaveActive);
    }

    public void ToggleHealButton()
    {
        bool canHeal = Farm != null && Farm.Health < Farm.DefaultHealth && PlayerInventory.Heals > 0;
        itemsUI.UpdateHealButton(canHeal);
    }

    public void ToggleBombButton()
    {
        bool canUseBomb = PlayerInventory.Bombs > 0;
        itemsUI.UpdateBombButton(canUseBomb);
    }

    public void ToggleSpawnButton(bool noNests = false)
    {
        Debug.Log(noNests);
        bool canSpawn = spawner != null && spawner.eggs > 0 && noNests == false;
        itemsUI.UpdateSpawnButton(canSpawn);
    }

    public void UseHeal()
    {
        if (PlayerInventory.Heals <= 0 || Farm == null) return;

        PlayerInventory.Heals--;
        Farm.Heal();
        ToggleHealButton();

        if (PlayerInventory.Heals > 0)
            itemsUI.CooldownHealButton(cooldown); // пример cooldown

        UpdateUI();
    }

    public void UseBomb()
    {
        if (PlayerInventory.Bombs <= 0 || Bomb == null) return;

        PlayerInventory.Bombs--;
        Bomb.SummonBomb();
        ToggleBombButton();

        if (PlayerInventory.Bombs > 0)
            itemsUI.CooldownBombButton(cooldown);

        UpdateUI();
    }

    public void UseSpawn()
    {
        if (spawner == null) return;

        spawner.SpawnChicken(false);

        UpdateUI();
    }

    private void UpdateUI()
    {
        itemsUI.UpdateText(PlayerInventory.Heals, PlayerInventory.Bombs);
    }
}
