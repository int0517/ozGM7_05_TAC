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

            Clean(isCurrentlyBossWave); //일반웨이브 종료
        }
    }

    private void Clean(bool wasBossWave)
    {
        EManagers.Pool.ReturnAll();
        EBManagers.Pool.ReturnAll();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var e in enemies)
            Destroy(e);
        OnWaveEnded?.Invoke(wasBossWave);
    }
}