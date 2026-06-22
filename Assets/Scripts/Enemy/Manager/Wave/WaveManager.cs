
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
    private BossHpBase currentBoss;

    private void Awake()
    {
        Instance = this;

    }
    public void RegisterBoss(BossHpBase boss)
    {
        currentBoss = boss;
    }
    void Start()
    {
        StartCoroutine(WaveStart());
    }

    public IEnumerator WaveStart()
    {
        bool isCurrentlyBossWave = spawnManager.IsCurrentWaveBoss();
        currentWaveTimer = 0f;
        StartCoroutine(spawnManager.StartCurrentWave());
        if (isCurrentlyBossWave)
        {
            while(currentWaveTimer<bossTime)
            {
                currentWaveTimer += Time.deltaTime;
                UI.UpdateTimerBar((int)currentWaveTimer, (int)bossTime);
                yield return null;
            }
            yield return new WaitUntil(() => currentBoss != null);



            while (true)
            {
                if (currentBoss != null)
                {
                    UI.UpdateBossHPBar(currentBoss.GetCurrentHp(), currentBoss.GetMaxHp());

                    if (currentBoss.GetCurrentHp() <= 0)
                    {
                        
                        yield break;
                    }
                }
                yield return null;
            }
        }
        else if(!isCurrentlyBossWave)
        {
            while (currentWaveTimer < waveTime)
            {
                currentWaveTimer += Time.deltaTime;
                UI.UpdateTimerBar((int)currentWaveTimer, (int)waveTime);
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
