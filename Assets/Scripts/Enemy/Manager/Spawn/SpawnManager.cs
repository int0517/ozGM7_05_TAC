using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    public GameLevelData levelData;
    public EnemySpawn spawner;
    private int currentWaveIndex = 0;

    void Start()
    {
        // 게임이 시작될 때 첫 번째 웨이브를 자동으로 실행합니다.
        StartCoroutine(StartCurrentWave());
    }
    public IEnumerator StartCurrentWave()
    {
        if (currentWaveIndex >= levelData.waveList.Count) yield break;

        WaveInfo currentWave = levelData.waveList[currentWaveIndex];
        Debug.Log($"{currentWave.waveNumber} 웨이브 시작!");

        // 모든 몬스터 종류에 대해 스폰 실행
        foreach (var info in currentWave.spawnList)
        {
            StartCoroutine(spawner.SpawnEnemyRoutine(info));
        }

        // 웨이브 순서 증가
        currentWaveIndex++;
    }
}
