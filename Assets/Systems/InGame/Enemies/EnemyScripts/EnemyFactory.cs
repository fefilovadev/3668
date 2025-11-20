using UnityEngine;

public class EnemyFactory
{
    public EnemyController CreateEnemy(GameObject prefab)
    {
        GameObject obj = Object.Instantiate(prefab);
        obj.SetActive(false);

        var enemy = obj.GetComponent<EnemyController>();
        if (enemy == null)
            Debug.LogError($"Prefab {prefab.name} must have EnemyController component!");

        return enemy;
    }
}
