using UnityEngine;
using System.Collections;

public class OneBossEnemyFollow : MonoBehaviour
{
    [Header("몬스터 스텟")]
    [SerializeField] private int enemyMaxHP = 3;
    private int enemyCurrentHP;
    [SerializeField] private int enemyRange = 5;
    [SerializeField] private float enemyFireInterval = 1;
    [SerializeField] private float enemySpeed = 1.0f;
    [SerializeField] private int enemyPoint = 0;
    [SerializeField] private GameObject coinPrefab;
    [Header("보스 추가 스텟")]
    [SerializeField] public float warningTime = 3f;
    [SerializeField] public float poisonDuration = 5f;
    [SerializeField] private GameObject warningPrefab;
    [SerializeField] private GameObject poisonPrefab;
    [Header("보스 체력바는 따로")]
    [SerializeField] private EnemyHPUI enemyUI;


    public float knockbackForce = 20.0f;

    public LayerMask playerLayer;
    private Transform playerTransform;
    private Rigidbody2D rb;
    private bool isAttack = false;
    private bool isKnockedBack = false;
    private bool timerCheck = false;
    private float fireTimer = 0;
    void Start()
    {
        enemyCurrentHP = enemyMaxHP;
        rb = GetComponent<Rigidbody2D>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
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

        Vector2 direction = (playerTransform.position - transform.position).normalized;

        rb.AddForce(direction * enemySpeed * 10f);
        if (rb.linearVelocity.magnitude > enemySpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * enemySpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Skill"))
        {
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
            enemyCurrentHP--;
            enemyUI.UpdateHealthBar(enemyCurrentHP, enemyMaxHP);
            StartCoroutine(KnockbackRoutine(collision.transform.position));
        }

    }
    void CheckForPlayer()
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

    private void Fire()
    {
        fireTimer += Time.fixedDeltaTime;
        timerCheck = true;
        if (fireTimer >= enemyFireInterval)
        {

            Vector3 playerPos = playerTransform.position;
            StartCoroutine(PoisonAttackRoutine(playerPos));

            // 2. 랜덤 위치 2개 계산 및 실행
            for (int i = 0; i < 2; i++)
            {
                Vector3 randomPos = GetRandomPositionInCamera();
                StartCoroutine(PoisonAttackRoutine(randomPos));
            }

            fireTimer = 0f;
            timerCheck = false;
        }
    }

    private IEnumerator PoisonAttackRoutine(Vector3 targetPos)
    {

        GameObject warning = Instantiate(warningPrefab, targetPos, Quaternion.identity);

        yield return new WaitForSeconds(warningTime);

        Destroy(warning);
        GameObject poison = Instantiate(poisonPrefab, targetPos, Quaternion.identity);

        yield return new WaitForSeconds(poisonDuration);
        Destroy(poison);
    }

    private Vector3 GetRandomPositionInCamera()
    {

        float randomX = Random.Range(0.1f, 0.9f);
        float randomY = Random.Range(0.1f, 0.9f);

        return Camera.main.ViewportToWorldPoint(new Vector3(randomX, randomY, 10f));
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
