using UnityEngine;
using System.Collections;
using System;


public class BossBase : MonoBehaviour, IDamageable
{
    [Header("보스 기본 스텟")]
    [SerializeField] protected float enemyMaxHP = 20;
    protected float enemyCurrentHP;
    [SerializeField] protected int enemyRange = 5;
    [SerializeField] protected float enemyFireInterval = 1;
    [SerializeField] protected float enemySpeed = 1.0f;
    [SerializeField] protected int enemyPoint = 0;
    [SerializeField] protected GameObject coinPrefab;
    [SerializeField] protected GameObject bossPotionPrefab;
    [SerializeField] protected float knockbackForce = 5.0f;

    protected Transform playerTransform;
    protected PlayerStat playerStat;
    protected Rigidbody2D rb;
    protected bool isKnockedBack = false;
    protected bool isAttack = false;
    protected bool timerCheck = false;
    protected float fireTimer = 0;
    public static event Action OnBossDeath;

    [Header("보스 이동 제한")]
    protected float posX, posY;
    [SerializeField] protected float posXMax = 27.25f;
    [SerializeField] protected float posXMin = -28.25f;
    [SerializeField] protected float posYMax = 17.75f;
    [SerializeField] protected float posYMin = -17.75f;


    protected virtual void Start()
    {
        enemyCurrentHP = enemyMaxHP;
        rb = GetComponent<Rigidbody2D>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
            playerStat = playerObj.GetComponent<PlayerStat>();
        }
        if (WaveManager.Instance != null)
        {
            WaveManager.Instance.RegisterBoss(this);
        }
    }


    protected virtual void FixedUpdate()
    {
        if (!timerCheck)
        {
            CheckForPlayer();
        }
        if (isKnockedBack || playerTransform == null) return;
        if (isAttack)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = (playerTransform.position - transform.position).normalized;

        rb.AddForce(direction * enemySpeed * 10f);
        if (rb.linearVelocity.magnitude > enemySpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * enemySpeed;
        }
        MoveLimit();
    }
    protected virtual void CheckForPlayer()
    {
        float dist = Vector2.Distance(transform.position, playerTransform.position);


        if (dist <= enemyRange)
        {
            isAttack = true;
        }

        else if (dist > enemyRange * 1.2f)
        {
            isAttack = false;
            fireTimer = 0f;
        }

    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (playerStat != null)
            {
                playerStat.DamagePlayer(1);
            }
        }
        if (collision.CompareTag("Skill"))
        {
            StartCoroutine(KnockbackRoutine(collision.transform.position));
        }
    }
    protected void RaiseBossDeath()
    {
        OnBossDeath?.Invoke();
    }
    public virtual void TakeDamage(float damage)
    {
        enemyCurrentHP -= damage;
        if (enemyCurrentHP <= 0)
        {
            RaiseBossDeath();
            if (coinPrefab != null && enemyPoint > 0)
            {
                for (int i = 0; i < enemyPoint; i++)
                {
                    Instantiate(coinPrefab, transform.position, Quaternion.identity);
                }
            }
            Instantiate(bossPotionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

    }

    public float GetCurrentHp() => enemyCurrentHP;
    public float GetMaxHp() => enemyMaxHP;

    protected IEnumerator KnockbackRoutine(Vector3 attackerPos)
    {
        isKnockedBack = true;

        Vector2 knockbackDir = (transform.position - attackerPos).normalized;
        rb.linearVelocity = knockbackDir * knockbackForce;

        yield return new WaitForSeconds(0.2f);

        isKnockedBack = false;
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyRange);
    }
    protected void MoveLimit()
    {
        Vector2 pos = rb.position;

        pos.x = Mathf.Clamp(pos.x, posXMin, posXMax);
        pos.y = Mathf.Clamp(pos.y, posYMin, posYMax);

        rb.position = pos;
    }
}
