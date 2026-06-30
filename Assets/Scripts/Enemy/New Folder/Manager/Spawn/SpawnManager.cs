using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    public GameLevelData levelData;
    public SpawnEnemy spawner;
    private int currentWaveIndex = 0;
    public IEnumerator StartCurrentWave()
    {
        if (currentWaveIndex >= levelData.waveList.Count) yield break;

        WaveInfo currentWave = levelData.waveList[currentWaveIndex];
        if (currentWave.bossWave)
        {
            Debug.Log($"{currentWave.waveNumber / 5}번째  보스웨이브 시작!");
        }
        else
        {
            Debug.Log($"{currentWave.waveNumber} 웨이브 시작!");
        }

        if (currentWave.bossWave)
        {
            StartCoroutine(spawner.SpawnBossRoutine(currentWave));
        }
        // 모든 몬스터 종류에 대해 스폰 실행
        foreach (var info in currentWave.spawnList)
        {
            StartCoroutine(spawner.SpawnEnemyRoutine(info));
        }

        // 웨이브 순서 증가
        currentWaveIndex++;
    }
    public bool IsCurrentWaveBoss()
    {
        if (levelData.waveList == null || currentWaveIndex >= levelData.waveList.Count)
            return false;

        return levelData.waveList[currentWaveIndex].bossWave;
    }
}
