using UnityEngine;
using System.Collections;

public class TwoBoss : BossBase
{
    [Header("보스 스텟")]
    [SerializeField] private Transform MouthPoint;
    [SerializeField] private GameObject WebBulletPrefab;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private int enemyATK=5;
    [SerializeField] private int webPoints = 10; // 점을 늘리면 더 부드러운 곡선이 됩니다
    [SerializeField] private float waveAmplitude = 0.2f; // 흔들리는 폭
    [SerializeField] private float waveSpeed = 10f; // 흔들리는 속도
    [SerializeField] private float pullingSpeed = 5f;
    [SerializeField] private float pullingInterval = 3f;

    private float pullingTimer = 0;
    private bool isBeingPulled = false;

    protected override void Start()
    {
        enemyCurrentHP = enemyMaxHP;
        rb = GetComponent<Rigidbody2D>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }

        lineRenderer.positionCount = webPoints;
        base.Start();
 
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
    protected virtual void FixedUpdate()
    {
        if (!timerCheck)
        {
            CheckForPlayer();
        }
        if (isKnockedBack || playerTransform == null) return;
        if (isAttack && !isBeingPulled)
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

}
