using UnityEngine;
using System.Collections;

public class TwoBossEnemyFollow : MonoBehaviour
{
    [Header("몬스터 스텟")]
    [SerializeField] private int enemyMaxHP = 20;
    private int enemyCurrentHP;
    [SerializeField] private int enemyRange = 15;
    [SerializeField] private int enemyATK = 5;
    [SerializeField] private float enemyFireInterval = 5;
    private float fireTimer = 0;
    [SerializeField] private float enemySpeed = 0.5f;
    [SerializeField] private int enemyPoint = 0;
    [SerializeField] private GameObject coinPrefab;
    [Header("보스 스텟")]
    [SerializeField] private Transform MouthPoint;
    [SerializeField] private GameObject WebBulletPrefab;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private int webPoints = 10; // 점을 늘리면 더 부드러운 곡선이 됩니다
    [SerializeField] private float waveAmplitude = 0.2f; // 흔들리는 폭
    [SerializeField] private float waveSpeed = 10f; // 흔들리는 속도
    [SerializeField] private float pullingSpeed = 5f;
    [SerializeField] private float pullingInterval = 3f;
    private float pullingTimer = 0;


    public float knockbackForce = 20.0f;

    public LayerMask playerLayer;
    private Transform playerTransform;
    private Rigidbody2D rb;
    private bool isAttack = false;
    private bool isKnockedBack = false;
    private bool timerCheck = false;
    private bool isBeingPulled = false;

    void Start()
    {
        enemyCurrentHP = enemyMaxHP;
        rb = GetComponent<Rigidbody2D>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }

        lineRenderer.positionCount = webPoints;
    }

    void Update()
    {
        if (playerTransform == null) return;

        if (isBeingPulled)
        {
            lineRenderer.enabled = true;
            UpdateWebVisuals();
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    void FixedUpdate()
    {
        if (!timerCheck)
        {
            CheckForPlayer();
        }
        if (isKnockedBack || playerTransform == null) return;
        if (isAttack&&!isBeingPulled)
        {
            Fire();
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
            Vector2 direction = (playerTransform.position - MouthPoint.position).normalized;
            float baseAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // -30도, 0도, +30도 방향으로 3발 발사
            for (int i = -1; i <= 1; i++)
            {
                float angle = baseAngle + (i * 30f);
                GameObject bullet = Instantiate(WebBulletPrefab, MouthPoint.position, Quaternion.Euler(0, 0, angle));
                bullet.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * 5f;
            }
            fireTimer = 0f;
            timerCheck = false;
        }
    }
    public void StartPulling()
    {
        if (!isBeingPulled)
        {
            StartCoroutine(PullPlayerRoutine());
        }
    }

    private IEnumerator PullPlayerRoutine()
    {
        isBeingPulled = true;
        pullingTimer = 0f;


        while (pullingTimer < pullingInterval)
        {
            pullingTimer += Time.deltaTime;

            // 플레이어를 보스 방향으로 강제 이동
            Vector2 dir = (transform.position - playerTransform.position).normalized;
            playerTransform.position += (Vector3)dir * pullingSpeed * Time.deltaTime;

            yield return null; // 다음 프레임까지 대기
        }

        isBeingPulled = false;
    }

    void UpdateWebVisuals()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = playerTransform.position;

        for (int i = 0; i < webPoints; i++)
        {
            float t = (float)i / (webPoints - 1);

            // 시작점(보스)과 끝점(플레이어) 사이를 보간
            Vector3 pos = Vector3.Lerp(startPos, endPos, t);

            // 중간 점들만 사인파로 흔들기 (양 끝점은 0.0이라서 안 흔들림)
            float sineOffset = Mathf.Sin(Time.time * waveSpeed + i) * waveAmplitude * (t * (1 - t) * 4);
            pos.y += sineOffset;
            pos.z = 0; // Z축 고정

            lineRenderer.SetPosition(i, pos);
        }
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