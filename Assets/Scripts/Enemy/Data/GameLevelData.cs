using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnInfo
{
    [Header("몬스터 프리랩")]
    public GameObject enemyPrefab; // 몬스터 종류
    [Header("한 웨이브 총 마리")]
    public int count;              // 마리 수
    [Header("한번에 스폰되는 수")]
    public int batchSize;          // 한번에 스폰되는 수
    [Header("스폰간격")]
    public float spawnInterval;    // 스폰 간격

}

[System.Serializable]
public class WaveInfo
{
    [Header("웨이브 번호")]
    public int waveNumber;
    [Header("보스전")]
    public bool bossWave;
    [Header("--- 보스 정보 (보스 웨이브 전용) ---")]
    public GameObject bossPrefab;
    public float bossSpawnDelay;
    [Header("이 웨이브에 등장할 몬스터들")]
    public List<EnemySpawnInfo> spawnList;

    public void ValidateBossData()
    {
        if (!bossWave)
        {
            bossPrefab = null;
            bossSpawnDelay = 0f;
            Debug.Log("보스 데이터가 초기화되었습니다.");
        }
    }

}


[CreateAssetMenu(fileName = "GameLevelData", menuName = "Spawn/GameLevelData")]
public class GameLevelData : ScriptableObject
{

    public List<WaveInfo> waveList;
    private void OnValidate()
    {
        if (waveList == null) return;

        foreach (var wave in waveList)
        {
            // 5의 배수 체크
            if (wave.waveNumber > 0 && wave.waveNumber % 5 == 0)
            {
                wave.bossWave = true;
            }
            else
            {
                wave.bossWave = false;
            }
        }
    }
}