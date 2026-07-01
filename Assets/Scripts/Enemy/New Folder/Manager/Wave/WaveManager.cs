using System;
using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    //웨이브 종료 시 발행하는 이벤트 (bool: 보스웨이브였는지 여부)
    public static event Action<bool> OnWaveEnded;

    public float waveTime = 60.0f;
    public float bossTime = 30.0f;
    private float currentWaveTimer = 0.0f;
    public SpawnManager spawnManager;
    [SerializeField] private TimerAndMonsterUI UI;
    private BossBase currentBoss;
    private bool isBossDead;

    private void Awake() => Instance = this;

    private void OnEnable()
    {
        BossBase.OnBossDeath += SetBossDead;
    }

    private void OnDisable()
    {
        BossBase.OnBossDeath -= SetBossDead;
    }

    private void SetBossDead()
    {
        isBossDead = true;
    }

    public void RegisterBoss(BossBase boss)
    {
        currentBoss = boss;
    }

    void Start()
    {
        StartCoroutine(WaveStart());
    }

    public void OnNextWaveButtonClick()
    {
        StartCoroutine(WaveStart());
    }

    public IEnumerator WaveStart()
    {
        spawnManager.ResumeSpawn();
        isBossDead = false;

        //웨이브 시작 시점에 보스웨이브 여부를 미리 저장
        //Clean() 시점에 다시 조회하면 이미 다음 웨이브 인덱스로 넘어가 있어서 틀린 값이 나옴
        bool isCurrentlyBossWave = spawnManager.IsCurrentWaveBoss();

        currentWaveTimer = 0f;
        StartCoroutine(spawnManager.StartCurrentWave());

        if (isCurrentlyBossWave)
        {
            while (currentWaveTimer < bossTime)
            {
                currentWaveTimer += Time.deltaTime;
                UI.UpdateTimerBar(currentWaveTimer, bossTime);
                yield return null;
            }

            yield return new WaitUntil(() => currentBoss != null);

            while (true)
            {
                Debug.Log("보스출현");
                if (currentBoss != null)
                {
                    UI.UpdateBossHPBar(currentBoss.GetCurrentHp(), currentBoss.GetMaxHp());
                }

                //보스 처치 시 웨이브 종료
                if (isBossDead)
                {
                    spawnManager.StopSpawn();
                    Clean(isCurrentlyBossWave); //보스웨이브 종료
                    yield break;
                }
                yield return null;
            }
        }
        else if (!isCurrentlyBossWave)
        {
            while (currentWaveTimer < waveTime)
            {
                currentWaveTimer += Time.deltaTime;
                UI.UpdateTimerBar(currentWaveTimer, waveTime);
                yield return null;
            }
            while (currentWaveTimer > 0)
            {
                currentWaveTimer -= Time.deltaTime * 90f;

                if (currentWaveTimer < 0)
                    currentWaveTimer = 0;

                UI.UpdateTimerBar(currentWaveTimer, waveTime);

                yield return null;
            }
            spawnManager.StopSpawn();
            Clean(isCurrentlyBossWave);
        }
    }

    private void Clean(bool wasBossWave) //파라미터 추가: 보스웨이브 여부를 전달받음
    {
        Debug.Log("Clean 호출!");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Debug.Log("Enemy 개수 : " + enemies.Length);
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("EnemyBullet");

        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
        foreach (GameObject bullet in bullets)
        {
            Destroy(bullet);
        }

        //적/총알 정리 완료 후 웨이브 종료 이벤트 발행
        //구독 중인 스크립트(LevelUpUI 등)가 자동으로 반응함
        OnWaveEnded?.Invoke(wasBossWave);
    }
}