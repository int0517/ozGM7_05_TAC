using UnityEngine;
using System.Collections;
using SP1Assets.MonsterPack2D;


public class TwoBoss : BossBase
{
    [Header("º¸½º ½ºÅÝ")]
    [SerializeField] private Transform MouthPoint;
    [SerializeField] private GameObject WebBulletPrefab;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private int enemyATK = 5;
    [SerializeField] private int webPoints = 10; // Á¡À» ´Ã¸®¸é ´õ ºÎµå·¯¿î °î¼±ÀÌ µË´Ï´Ù
    [SerializeField] private float waveAmplitude = 0.2f; // Èçµé¸®´Â Æø
    [SerializeField] private float waveSpeed = 10f; // Èçµé¸®´Â ¼Óµµ
    [SerializeField] private float pullingSpeed = 5f;
    [SerializeField] private float pullingInterval = 3f;

    private float pullingTimer = 0;
    private bool isBeingPulled = false;
    MonsterPrefabController monster;
    private string currentAnim = "";

    private bool isDead = false;
    private bool isHit = false;
    private bool isAttacking = false;

    private void PlayAnim(string animName)
    {
        if (currentAnim == animName)
            return;

        currentAnim = animName;
        monster.PlayAnimation(animName, 0.1f);
    }

    protected override void Start()
    {
        enemyCurrentHP = enemyMaxHP;
        rb = GetComponent<Rigidbody2D>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
            playerStat = playerObj.GetComponent<PlayerStat>();
        }

        lineRenderer.positionCount = webPoints;
        base.Start();
        monster = GetComponent<MonsterPrefabController>();

        monster.Init();
        monster.SetAnimationSpeed(0.5f);
        PlayAnim("walk");
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
        FacePlayer();
        Vector2 direction = (playerTransform.position - transform.position).normalized;

        rb.AddForce(direction * enemySpeed * 10f);
        if (rb.linearVelocity.magnitude > enemySpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * enemySpeed;
        }
        MoveLimit();
    }
    private void Fire()
    {
        fireTimer += Time.fixedDeltaTime;
        timerCheck = true;
        if (fireTimer >= enemyFireInterval)
        {
            Vector2 direction = (playerTransform.position - MouthPoint.position).normalized;
            float baseAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            StartCoroutine(ReturnToIdle());
            for (int i = -1; i <= 1; i++)
            {
                float angle = baseAngle + (i * 35f);
                GameObject bullet = Instantiate(WebBulletPrefab, MouthPoint.position, Quaternion.Euler(0, 0, angle));
                bullet.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * 5f;
            }
            fireTimer = 0f;
            timerCheck = false;
        }
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (playerStat != null)
            {
                StartCoroutine(ReturnToIdle());
                playerStat.DamagePlayer(enemyATK);
            }
        }
        if (collision.CompareTag("Skill"))
        {
            StartCoroutine(KnockbackRoutine(collision.transform.position));
        }
    }
    public override void TakeDamage(float damage)
    {
        enemyCurrentHP -= damage;
        StartCoroutine(HitRoutine());
        if (enemyCurrentHP <= 0)
        {
            Instantiate(bossPotionPrefab, transform.position, Quaternion.identity);
            if (coinPrefab != null && enemyPoint > 0)
            {
                for (int i = 0; i < enemyPoint; i++)
                {
                    Coin coin = CManagers.Pool.GetPool(coinPrefab.GetComponent<Coin>());

                    coin.transform.position = transform.position;
                    coin.transform.rotation = Quaternion.identity;
                    coin.Init();
                }
            }
            RaiseBossDeath();
            StartCoroutine(Die());
            Destroy(gameObject);
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


            Vector2 dir = (MouthPoint.position - playerTransform.position).normalized;
            playerTransform.position += (Vector3)dir * pullingSpeed * Time.deltaTime;

            yield return null;
        }

        isBeingPulled = false;
    }

    void UpdateWebVisuals()
    {
        Vector3 startPos = MouthPoint.position;
        Vector3 endPos = playerTransform.position;

        for (int i = 0; i < webPoints; i++)
        {
            float t = (float)i / (webPoints - 1);


            Vector3 pos = Vector3.Lerp(startPos, endPos, t);


            float sineOffset = Mathf.Sin(Time.time * waveSpeed + i) * waveAmplitude * (t * (1 - t) * 4);
            pos.y += sineOffset;
            pos.z = 0;

            lineRenderer.SetPosition(i, pos);
        }
    }
    private IEnumerator ReturnToIdle()
    {
        isAttacking = true;
        monster.SetAnimationSpeed(1f);
        PlayAnim("attack");
        yield return new WaitForSeconds(0.2f);
        PlayAnim("walk");
        monster.SetAnimationSpeed(0.5f);
        isAttacking = false;
    }
    private IEnumerator HitRoutine()
    {
        isHit = true;
        monster.SetAnimationSpeed(1f);
        PlayAnim("hit");

        yield return new WaitForSeconds(1f);

        PlayAnim("walk");
        monster.SetAnimationSpeed(0.5f);
        isHit = false;
    }
    private IEnumerator Die()
    {
        if (isDead) yield break;

        isDead = true;

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        monster.SetAnimationSpeed(1f);
        PlayAnim("die");

        yield return new WaitForSeconds(0.5f);

        Destroy(gameObject);
    }
}
