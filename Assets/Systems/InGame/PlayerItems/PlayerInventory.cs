using UnityEngine;

public static class PlayerInventory
{
    private const string KEY_BOMBS = "Inventory_Bombs";
    private const string KEY_HEALS = "Inventory_Heals";
    private const string KEY_COINS = "Inventory_Coins";
    private const string KEY_CHARGES = "Inventory_Charges";
    
    public static int MaxCharges = 10;
    public static int Bombs
    {
        get => PlayerPrefs.GetInt(KEY_BOMBS, 2);
        set
        {
            PlayerPrefs.SetInt(KEY_BOMBS, Mathf.Max(0, value));
            PlayerPrefs.Save();
        }
    }

    public static int Heals
    {
        get => PlayerPrefs.GetInt(KEY_HEALS, 2);
        set
        {
            PlayerPrefs.SetInt(KEY_HEALS, Mathf.Max(0, value));
            PlayerPrefs.Save();
        }
    }

    public static int Coins
    {
        get => PlayerPrefs.GetInt(KEY_COINS, 0);
        set
        {
            PlayerPrefs.SetInt(KEY_COINS, Mathf.Max(0, value));
            PlayerPrefs.Save();
        }
    }

    public static int Charges
    {
        get => PlayerPrefs.GetInt(KEY_CHARGES, 10);
        set
        {
            PlayerPrefs.SetInt(KEY_CHARGES, Mathf.Max(0, value));
            PlayerPrefs.Save();
        }
    }

    public static void AddBombs(int amount) => Bombs += Mathf.Max(0, amount);
    public static void AddHeals(int amount) => Heals += Mathf.Max(0, amount);
    public static void AddCoins(int amount) => Coins += Mathf.Max(0, amount);
    public static void AddCharges(int amount)
    {
        Charges = Mathf.Min(Charges + Mathf.Max(0, amount), MaxCharges);
    }
}
