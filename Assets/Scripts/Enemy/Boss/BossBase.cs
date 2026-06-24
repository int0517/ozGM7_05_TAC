using UnityEngine;
using System.Collections;
using System;

public class BossBase : MonoBehaviour
{
    [Header("보스 기본 스텟")]
    [SerializeField] protected int enemyMaxHP = 20;
    protected int enemyCurrentHP;
    [SerializeField] protected int enemyRange = 5;
    [SerializeField] protected float enemyFireInterval = 1;
    [SerializeField] protected float enemySpeed = 1.0f;
    [SerializeField] protected int enemyPoint = 0;
    [SerializeField] protected GameObject coinPrefab;
    [SerializeField] protected float knockbackForce = 5.0f;

    

    public LayerMask playerLayer;
    protected Transform playerTransform;
    protected Rigidbody2D rb;
    protected bool isKnockedBack = false;
    protected bool isAttack = false;
    protected bool timerCheck = false;
    protected float fireTimer = 0;
    public static event Action OnBossDeath;

    protected virtual void Start()
    {
        enemyCurrentHP = enemyMaxHP;
        rb = GetComponent<Rigidbody2D>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
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
        if (collision.CompareTag("Skill"))
        {
            TakeDamage();
            StartCoroutine(KnockbackRoutine(collision.transform.position));
        }
    }
    protected virtual void TakeDamage()//상대 공격력 넣어줘야함
    {
        if (enemyCurrentHP <= 0)
        {
            Debug.Log("보스 사망! 이벤트 발사 직전");
            OnBossDeath?.Invoke();
            Debug.Log("보스 사망! 이벤트 발사 완료");
            if (coinPrefab != null && enemyPoint > 0)
            {
                for (int i = 0; i < enemyPoint; i++)
                {
                    Instantiate(coinPrefab, transform.position, Quaternion.identity);
                }
            }
            Destroy(gameObject);
        }
        enemyCurrentHP--;
    }
    public int GetCurrentHp() => enemyCurrentHP;
    public int GetMaxHp() => enemyMaxHP;

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

}
