using UnityEngine;
using System.Collections;
using SP1Assets.MonsterPack2D;

public class OneBoss : BossBase
{
    [Header("보스 추가 스텟")]
    [SerializeField] public float warningTime = 3f;
    [SerializeField] public float poisonDuration = 5f;
    [SerializeField] private GameObject warningPrefab;
    [SerializeField] private GameObject poisonPrefab;
    

    private bool isDead = false;
    private bool isHit = false;
    private bool isAttacking = false;
    MonsterPrefabController monster;
    private string currentAnim = "";

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
        if (WaveManager.Instance != null)
        {
            WaveManager.Instance.RegisterBoss(this);
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

    protected override void FixedUpdate()
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
        MoveLimit();
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
            if (!isDead)
            {
                StartCoroutine(Die());
            }
            RaiseBossDeath();
        }

    }
    protected override void CheckForPlayer()
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
            PlayAnim("walk");
        }
    }


    private void Fire()
    {
        if (isHit) return;
        fireTimer += Time.fixedDeltaTime;
        if (fireTimer >= enemyFireInterval)
        {

            Vector3 playerPos = playerTransform.position;
            StartCoroutine(ReturnToIdle());
            StartCoroutine(PoisonAttackRoutine(playerPos));
            for (int i = 0; i < 2; i++)
            {
                Vector3 randomPos = GetRandomPositionInCamera();
                StartCoroutine(PoisonAttackRoutine(randomPos));
            }

            fireTimer = 0f;
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

    private IEnumerator ReturnToIdle()
    {
        isAttacking = true;
        PlayAnim("attack");
        yield return new WaitForSeconds(1f);
        PlayAnim("idle");
        isAttacking = false;
    }
    private IEnumerator HitRoutine()
    {
        isHit = true;
        PlayAnim("hit");
        yield return new WaitForSeconds(1f);
        PlayAnim("walk");
        isHit = false;
    }
    private IEnumerator Die()
    {
        if (isDead) yield break;
        Debug.Log("죽음 시작");
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        PlayAnim("die");
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Destroy 실행");
        Destroy(gameObject);
    }
}
