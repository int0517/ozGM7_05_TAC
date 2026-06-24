using UnityEngine;

public class ThreeBossBody : BossBase
{
    protected override void Start()
    {
        enemyCurrentHP = enemyMaxHP;
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
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (playerStat != null)
            {
                playerStat.DamagePlayer(1);
            }
        }
    }

}
