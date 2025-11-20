public interface IDebuff
{
    void AddDebuff(float duration, bool isStackable, float baseValue);
    void ProlongDebuff();
    void EndDebuff();
}
