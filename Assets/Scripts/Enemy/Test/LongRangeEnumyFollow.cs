using SP1Assets.MonsterPack2D;
using System.Collections;
using UnityEngine;

public class LongRangeEnemyFollowTest : MonoBehaviour, IDamageable
{
    [Header("몬스터 스텟")]
    [SerializeField] private float enemyMaxHP = 3;
    private float enemyCurrentHP;
    [SerializeField] private int enemyRange = 5;
    [SerializeField] private int enemyATK = 1;
    [SerializeField] private float enemyFireInterval = 1;
    [SerializeField] private float enemySpeed = 2.0f;
    [SerializeField] private int enemyPoint = 0;

    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private EnemyHPUI enemyUI;
    private PlayerStat playerStat;

    public float knockbackForce = 20.0f;

    public LayerMask playerLayer;
    private Transform playerTransform;
    private Rigidbody2D rb;
    private bool isAttack = false;
    private bool isAttacking = false;
    private bool isKnockedBack = false;
    private bool timerCheck = false;
    private float fireTimer=0;

    MonsterPrefabController monster;
    private bool isDead = false;
    private bool isHit = false;

    private string currentAnim = "";

    private void PlayAnim(string animName)
    {
        if (currentAnim == animName)
            return;

        currentAnim = animName;
        monster.PlayAnimation(animName, 0.1f);
    }
    void Start()
    {
        enemyCurrentHP = enemyMaxHP;
        rb = GetComponent<Rigidbody2D>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
            playerStat = playerObj.GetComponent<PlayerStat>();
        }
        monster = GetComponent<MonsterPrefabController>();

        monster.Init();
        PlayAnim("walk");
    }

    private void FacePlayer()
    {
        if (playerTransform == null) return;

        Vector3 scale = transform.localScale;

        if (playerTransform.position.x > transform.position.x)
            scale.x = -Mathf.Abs(scale.x);
        else
            scale.x = Mathf.Abs(scale.x);

        transform.localScale = scale;
    }
    void FixedUpdate()
    {
        if (!timerCheck)
        {
            CheckForPlayer();
        }
        if (isKnockedBack || playerTransform == null) return;
        if (isAttack)
        {
            rb.linearVelocity = Vector2.zero;
            Fire();
            return;
        }
        FacePlayer();
        Vector2 direction = (playerTransform.position - transform.position).normalized;

        rb.AddForce(direction * enemySpeed * 10f);
        if (rb.linearVelocity.magnitude > enemySpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * enemySpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (playerStat != null)
            {
                playerStat.DamagePlayer(enemyATK);
            }
        }
        if (collision.CompareTag("Skill"))
        {
            enemyCurrentHP--;
            enemyUI.UpdateHealthBar(enemyCurrentHP, enemyMaxHP);
            if (enemyCurrentHP <= 0)
            {
                if (coinPrefab != null && enemyPoint > 0)
                {
                    for (int i = 0; i < enemyPoint; i++)
                    {
                        Instantiate(coinPrefab, transform.position, Quaternion.identity);
                    }

                }
                StartCoroutine(Die());
                return;
            }
            StartCoroutine(HitRoutine(collision.transform.position));
        }

    }
    public void TakeDamage(float damage)
    {
        enemyCurrentHP -= damage;
        enemyUI.UpdateHealthBar(enemyCurrentHP, enemyMaxHP);
        if (enemyCurrentHP <= 0)
        {
            if (coinPrefab != null && enemyPoint > 0)
            {
                for (int i = 0; i < enemyPoint; i++)
                {
                    Instantiate(coinPrefab, transform.position, Quaternion.identity);
                }
            }
            Destroy(gameObject);
        }

    }

    private IEnumerator Die()
    {
        if (isDead) yield break;

        isDead = true;

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        PlayAnim("die");

        yield return new WaitForSeconds(0.5f);

        Destroy(gameObject);
    }

    private IEnumerator HitRoutine(Vector3 attackerPos)
    {
        isHit = true;

        PlayAnim("hit");

        StartCoroutine(KnockbackRoutine(attackerPos));

        yield return new WaitForSeconds(1f); // hit 애니메이션 길이

        PlayAnim("walk");

        isHit = false;
    }
    void CheckForPlayer()
    {
        float dist = Vector2.Distance(transform.position, playerTransform.position);

        
            if (dist <= enemyRange)
            {
                isAttack = true;
                if (!isAttacking)
                {
                    PlayAnim("idle");
                }
                
             }
        
            else if (dist > enemyRange * 1.2f)
            {
                isAttack = false;
                 fireTimer = 0f;
            }
        
    }

    private void Fire()
    {
        fireTimer += Time.fixedDeltaTime;
        timerCheck = true;
        if (fireTimer >= enemyFireInterval)
        {
            StartCoroutine(ReturnToIdle());
            Vector2 direction = (playerTransform.position - firePoint.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0, 0, angle));
            fireTimer = 0f;
            timerCheck = false;
        }
    }
    private IEnumerator ReturnToIdle()
    {
        isAttacking = true;
        PlayAnim("attack");
        yield return new WaitForSeconds(0.8f);
        
        isAttacking = false;
    }

    private IEnumerator KnockbackRoutine(Vector3 attackerPos)
    {
        isKnockedBack = true;

        Vector2 knockbackDir = (transform.position - attackerPos).normalized;
        rb.linearVelocity = knockbackDir * knockbackForce;

        yield return new WaitForSeconds(0.2f);

        isKnockedBack = false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyRange);
    }
}
