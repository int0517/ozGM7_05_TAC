
using Assets.Scripts.Enemy.Manager;
using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;
    public float waveTime = 60.0f;
    public float bossTime = 30.0f;
    private float currentWaveTimer = 0.0f;
    public SpawnManager spawnManager;
    [SerializeField] private TimerAndBossHpUI UI;
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
        bool isCurrentlyBossWave = spawnManager.IsCurrentWaveBoss();
        currentWaveTimer = 0f;
        StartCoroutine(spawnManager.StartCurrentWave());
        if (isCurrentlyBossWave)
        {
            while(currentWaveTimer<bossTime)
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
                if (isBossDead)
                {
                    Clean();
                    yield break;
                }
                yield return null;
            }
        }
        else if(!isCurrentlyBossWave)
        {
            while (currentWaveTimer < waveTime)
            {
                currentWaveTimer += Time.deltaTime;
                UI.UpdateTimerBar(currentWaveTimer,waveTime);
                yield return null;
            }
            Clean();
        }
    } 
    
    private void Clean()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
        foreach (GameObject bullet in bullets)
        {
            Destroy(bullet);
        }
    }
}
