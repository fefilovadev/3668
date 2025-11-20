[System.Serializable]
public class PlayerItems
{
    public int Bombs;
    public int Heals;

    public PlayerItems(int bombs = 0, int heals = 0)
    {
        Bombs = bombs;
        Heals = heals;
    }
}
