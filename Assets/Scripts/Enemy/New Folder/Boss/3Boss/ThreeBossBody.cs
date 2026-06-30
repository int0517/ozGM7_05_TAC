using UnityEngine;

public class ThreeBossBody : BossBase
{
    [SerializeField] private ParticleSystem runEnemyEffect;
    [SerializeField] private ParticleSystem runPlayerEffect;
    private ThreeBoss boss;
    private bool isDead = false;
    protected override void Start()
    {
        enemyCurrentHP = enemyMaxHP;
        boss = GetComponentInParent<ThreeBoss>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (WaveManager.Instance != null)
        {
            WaveManager.Instance.RegisterBoss(this);
            playerStat = playerObj.GetComponent<PlayerStat>();
        }
        rb = GetComponent<Rigidbody2D>();
    }
    protected override void FixedUpdate()
    { }
    protected override void CheckForPlayer()
    { }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            
            if (boss.IsCharging())
            {
                PlayerCargingEffect();
            }
            if (playerStat != null)
            {
                playerStat.DamagePlayer(1);
            }
        }
        if (collision.CompareTag("Enemy"))
        {
            if (boss.IsCharging())
            {
                EnemyCargingEffect();
                ShortRangeEnemy shortEnemy = collision.GetComponent<ShortRangeEnemy>();
                if (shortEnemy != null)
                {
                    Debug.Log("근접!");
                    shortEnemy.ApplyKnockback(transform.position);
                }

                LongRangeEnemy longEnemy = collision.GetComponent<LongRangeEnemy>();
                if (longEnemy != null)
                {
                    Debug.Log("원거리!");
                    longEnemy.ApplyKnockback(transform.position);
                }
            }
        }
    }

    private void EnemyCargingEffect()
    {
        runEnemyEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        runEnemyEffect.Play();
    }
    private void PlayerCargingEffect()
    {
        runPlayerEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        runPlayerEffect.Play();
    }

    public override void TakeDamage(float damage)
    {

        if (!boss.IsCharging())
        {
            enemyCurrentHP -= damage;
            boss.OnHeadDamaged(transform.position);
            if (enemyCurrentHP <= 0)
            {
                if (!isDead)
                {

                    RaiseBossDeath();
                    boss.Die();
                    isDead = true;

                }
            }
        }


    }
}
