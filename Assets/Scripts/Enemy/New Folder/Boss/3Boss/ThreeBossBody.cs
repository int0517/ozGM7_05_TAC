using UnityEngine;

public class ThreeBossBody : BossBase
{
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
            if (playerStat != null)
            {
                playerStat.DamagePlayer(1);
            }
        }
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
