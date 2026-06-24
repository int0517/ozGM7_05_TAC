using UnityEngine;

public class ThreeBossBody : BossBase
{
    protected override void Start()
    {
        enemyCurrentHP = enemyMaxHP;
        if (WaveManager.Instance != null)
        {
            WaveManager.Instance.RegisterBoss(this);
        }
        rb = GetComponent<Rigidbody2D>();
    }
    protected override void FixedUpdate()
    { }
    protected override void CheckForPlayer()
    { }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Skill"))
        {
            TakeDamage();
        }
    }

}
