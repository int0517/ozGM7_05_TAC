using UnityEngine;
using System.Collections;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] private float posXMax = 27.25f;
    [SerializeField] private float posXMin = -28.25f;
    [SerializeField] private float posYMax = 17.75f;
    [SerializeField] private float posYMin = -17.75f;

    public IEnumerator SpawnEnemyRoutine(EnemySpawnInfo info)
    {
        int spawnedCount = 0;
        while (spawnedCount < info.count)
        {
            for (int i = 0; i < info.batchSize; i++)
            {
                if (spawnedCount >= info.count) break;

                SpawnEnemeis(info.enemyPrefab);
                spawnedCount++;
            }
            yield return new WaitForSeconds(info.spawnInterval);
        }
    }
    public IEnumerator SpawnBossRoutine(WaveInfo info)
    {
        yield return new WaitForSeconds(info.bossSpawnDelay);

        SpawnEnemeis(info.bossPrefab);
    }

    void SpawnEnemeis(GameObject prefab)
    {
        Vector2 spawnPos = GetRandomSpawnPosition();

        ShortRangeEnemy shortEnemy = prefab.GetComponent<ShortRangeEnemy>();

        if (shortEnemy != null)
        {
            ShortRangeEnemy enemy = EManagers.Pool.GetPool(shortEnemy);

            enemy.transform.position = spawnPos;
            enemy.transform.rotation = Quaternion.identity;
            enemy.Init();
            return;
        }

        LongRangeEnemy longEnemy = prefab.GetComponent<LongRangeEnemy>();

        if (longEnemy != null)
        {
            LongRangeEnemy enemy = EManagers.Pool.GetPool(longEnemy);

            enemy.transform.position = spawnPos;
            enemy.transform.rotation = Quaternion.identity;
            enemy.Init();
            return;
        }

    
        Instantiate(prefab, spawnPos, Quaternion.identity);
    }

    Vector2 GetRandomSpawnPosition()
    {
        int side = Random.Range(0, 4);

        Vector3 viewportPos = Vector3.zero;

        switch (side)
        {
            case 0: viewportPos = new Vector3(-0.1f, Random.value, 10f); break; // 왼쪽
            case 1: viewportPos = new Vector3(1.1f, Random.value, 10f); break;  // 오른쪽
            case 2: viewportPos = new Vector3(Random.value, -0.1f, 10f); break; // 아래
            case 3: viewportPos = new Vector3(Random.value, 1.1f, 10f); break;  // 위
        }

        Vector3 worldPos = Camera.main.ViewportToWorldPoint(viewportPos);
        worldPos.z = 0f;

        worldPos.x = Mathf.Clamp(worldPos.x, posXMin, posXMax);
        worldPos.y = Mathf.Clamp(worldPos.y, posYMin, posYMax);

        return worldPos;
    }
}
