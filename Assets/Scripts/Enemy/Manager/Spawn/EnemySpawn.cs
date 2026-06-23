using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour
{
    public IEnumerator SpawnEnemyRoutine(EnemySpawnInfo info)
    {
        int spawnedCount = 0;
        while (spawnedCount < info.count)
        {
            for (int i = 0; i < info.batchSize; i++)
            {
                if (spawnedCount >= info.count) break;

                SpawnEnemy(info.enemyPrefab);
                spawnedCount++;
            }
            yield return new WaitForSeconds(info.spawnInterval);
        }
    }
    public IEnumerator SpawnBossRoutine(WaveInfo info)
    {
        yield return new WaitForSeconds(info.bossSpawnDelay);

        SpawnEnemy(info.bossPrefab);
    }

    void SpawnEnemy(GameObject prefab)
    {
        Vector2 spawnPos = GetRandomSpawnPosition();
        Instantiate(prefab, spawnPos, Quaternion.identity);
    }

    Vector2 GetRandomSpawnPosition()
    {
        // 화면 테두리 밖의 랜덤 위치 계산
        float x = Random.Range(0f, 1f);
        float y = Random.Range(0f, 1f);

        // 0~3까지 랜덤하게 뽑아서 상/하/좌/우 중 어디서 나올지 결정
        int side = Random.Range(0, 4);
        if (side == 0) x = -0.1f;      // 왼쪽
        else if (side == 1) x = 1.1f;  // 오른쪽
        else if (side == 2) y = -0.1f; // 아래
        else y = 1.1f;                 // 위

        return Camera.main.ViewportToWorldPoint(new Vector3(x, y, 10f));
    }
}
