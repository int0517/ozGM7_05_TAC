using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourBoss : BossBase
{
    [Header("보스 추가 스텟")]
    [SerializeField] protected GameObject eggPrefab;
    [SerializeField] protected int eggCount = 4;
    [SerializeField] protected GameObject warningPrefab;
    [SerializeField] protected GameObject damagePrefab;
    [SerializeField] protected GameObject TwoDamagePrefab;
    [SerializeField] protected float warningCount = 2.0f;
    [SerializeField] protected float damageCount = 0.5f;
    public List<GameObject> eggList = new List<GameObject>();
    public bool isInvincible = true;
    public bool isEscape = false;

    protected override void Start()
    {
        base.Start();
        for (int i = 0; i < eggCount; i++)
        {
            Vector3 randomPos = GetRandomPositionInCamera();
            GameObject egg = Instantiate(eggPrefab, randomPos, Quaternion.identity);
            eggList.Add(egg);
        }
        
    }
    private void Update()
    {
        eggList.RemoveAll(egg => egg == null);

        if (isInvincible && eggList.Count == 0)
        {
            isInvincible = false;
            Debug.Log("무적 해제! 보스 공격 가능!");
        }
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

        Vector2 direction = (playerTransform.position - transform.position).normalized;
        Vector2 moveDirection = isEscape ? -direction : direction;
        float speedMultiplier = isInvincible ? 1.0f : 2.0f;
        rb.AddForce(moveDirection * enemySpeed * speedMultiplier * 10f);
        if (rb.linearVelocity.magnitude > enemySpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * enemySpeed;
        }
    }
    protected override void CheckForPlayer()
    {
        float dist = Vector2.Distance(transform.position, playerTransform.position);
        if (isInvincible)
        {
            if (dist <= enemyRange * 0.8)
            {
                isEscape = true;
                isAttack = false;
            }
            else if (dist <= enemyRange)
            {
                isEscape = false;
                isAttack = true;
            }
            else if (dist > enemyRange * 1.2f)
            {
                isEscape = false;
                isAttack = false;
                fireTimer = 0f;
            }
        }
        else
        {
            isEscape = false;
            if (dist <= enemyRange*0.5)
            {
                isAttack = true;
            }
            else if (dist > enemyRange)
            {
                isEscape = false;
                isAttack = false;
                fireTimer = 0f;
            }
        }

    }

    private void Fire()
    {
        fireTimer += Time.fixedDeltaTime;
        timerCheck = true;
        if (isInvincible)
        {
            if (fireTimer >= enemyFireInterval)
            {
                Vector3 playerPos = playerTransform.position;
                StartCoroutine(AttackRoutine(playerPos));
                fireTimer = 0f;
                timerCheck = false;
            }
        }
        else if (!isInvincible)
        {
            if (fireTimer >= enemyFireInterval/2)
            {
                Vector3 playerPos = playerTransform.position;
                StartCoroutine(AttackRoutine(playerPos));
                fireTimer = 0f;
                timerCheck = false;
            }
        }

    }
    private IEnumerator AttackRoutine(Vector3 targetPos)
    {

        GameObject warning = Instantiate(warningPrefab, targetPos, Quaternion.identity);

        yield return new WaitForSeconds(warningCount);

        Destroy(warning);
        GameObject poison = Instantiate(damagePrefab, targetPos, Quaternion.identity);

        yield return new WaitForSeconds(damageCount);
        Destroy(poison);
    }

    private Vector3 GetRandomPositionInCamera()
    {

        float randomX = Random.Range(0.1f, 0.9f);
        float randomY = Random.Range(0.1f, 0.9f);

        return Camera.main.ViewportToWorldPoint(new Vector3(randomX, randomY, 10f));
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
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
            Debug.Log("무적 상태라 데미지를 입지 않습니다!");
            StartCoroutine(KnockbackRoutine(collision.transform.position));
            if (!isInvincible)
            {
                Debug.Log("뚤렸네?");
                enemyCurrentHP--;
                if (enemyCurrentHP<=0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
