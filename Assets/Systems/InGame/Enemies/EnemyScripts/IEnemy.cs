public interface IEnemy
{
    void Init();
    void StartMoving();
    void TakeDamage(float damage); 
    void ReturnToPool();
}