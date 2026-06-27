using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SP1Assets.MonsterPack2D;

public class FourBoss : BossBase
{
    [Header("보스 추가 스텟")]
    [SerializeField] private ChangeModel changeModel;
    [SerializeField] protected GameObject eggPrefab;
    [SerializeField] protected int eggCount = 4;
    [SerializeField] protected GameObject warningPrefab;
    [SerializeField] protected GameObject powerWarningPrefab;
    [SerializeField] protected GameObject damagePrefab;
    [SerializeField] protected GameObject powerDamagePrefab;
    [SerializeField] protected float warningCount = 2.0f;
    [SerializeField] protected float powerWarningCount = 1.0f;
    [SerializeField] protected float damageCount = 0.5f;

    

    public List<GameObject> eggList = new List<GameObject>();
    private bool isInvincible = true;
    private bool isEscape = false;
    private bool shieldShowing = false;

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
        base.Start();
        for (int i = 0; i < eggCount; i++)
        {
            Vector3 randomPos = GetRandomPositionInCamera();
            GameObject egg = Instantiate(eggPrefab, randomPos, Quaternion.identity);
            eggList.Add(egg);
        }
        monster = GetComponent<MonsterPrefabController>();

        monster.Init();
        monster.SetAnimationSpeed(0.5f);
        PlayAnim("walk");

    }
    private void FacePlayer()
    {
        if (playerTransform == null) return;

        Vector3 scale = transform.localScale;

        if (!isEscape)
        {
            if (playerTransform.position.x > transform.position.x)
                scale.x = -Mathf.Abs(scale.x);
            else
                scale.x = Mathf.Abs(scale.x);

            transform.localScale = scale;
        }
        else if (isEscape)
        {
            if (playerTransform.position.x > transform.position.x)
                scale.x = Mathf.Abs(scale.x);
            else
                scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }


    }

    private void Update()
    {
        if (Time.frameCount % 30 == 0)
            eggList.RemoveAll(egg => egg == null);

        if (isInvincible && eggList.Count == 0)
        {
            changeModel.ChangeForm();
            isInvincible = false;
            isEscape = false;
            Debug.Log("무적 해제!");
        }
    }

    protected override void FixedUpdate()
    {
        if (isDead || isHit || isAttacking)
            return;
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
        Vector2 moveDirection = isEscape ? -direction : direction;
        float speedMultiplier = isInvincible ? 1.0f : 3.0f;
        rb.AddForce(moveDirection * enemySpeed * speedMultiplier * 10f);
        if (rb.linearVelocity.magnitude > enemySpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * enemySpeed;
        }
        MoveLimit();
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
                PlayAnim("walk");
            }
            else if (dist <= enemyRange)
            {
                isEscape = false;
                isAttack = true;
                PlayAnim("idle");
            }
            else if (dist > enemyRange * 1.2f)
            {
                isEscape = false;
                isAttack = false;
                fireTimer = 0f;
                PlayAnim("walk");
            }
        }
        else
        {
            isEscape = false;
            if (dist <= enemyRange*0.5)
            {
                isAttack = true;
                
                PlayAnim("idle");
            }
            else if (dist > enemyRange)
            {
                isEscape = false;
                isAttack = false;
                fireTimer = 0f;
                PlayAnim("walk");
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
                StartCoroutine(ReturnToIdle());
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
                StartCoroutine(ReturnToIdle());
                Vector3 playerPos = playerTransform.position;
                StartCoroutine(AttackRoutine(playerPos));
                fireTimer = 0f;
                timerCheck = false;
            }
        }

    }
    private IEnumerator AttackRoutine(Vector3 targetPos)
    {
        if (isInvincible)
        {
            GameObject warning = Instantiate(warningPrefab, targetPos, Quaternion.identity);

            yield return new WaitForSeconds(warningCount);

            Destroy(warning);
            GameObject poison = Instantiate(damagePrefab, targetPos, Quaternion.identity);

            yield return new WaitForSeconds(damageCount);
            Destroy(poison);
        }
        if (!isInvincible)
        {
            GameObject warning = Instantiate(powerWarningPrefab, targetPos, Quaternion.identity);

            yield return new WaitForSeconds(powerWarningCount);

            Destroy(warning);
            GameObject poison = Instantiate(powerDamagePrefab, targetPos, Quaternion.identity);

            yield return new WaitForSeconds(damageCount);
            Destroy(poison);
        }
    }

    private Vector3 GetRandomPositionInCamera()
    {

        float x = Random.Range(posXMin, posXMax);
        float y = Random.Range(posYMin, posYMax);

        return new Vector3(x, y, 0f);
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
            
            if (!isInvincible)
            {
                StartCoroutine(KnockbackRoutine(collision.transform.position));
            }
        }
    }
   
    public override void TakeDamage(float damage)
    {
        if (!isInvincible)
        {
            enemyCurrentHP -= damage;
            StartCoroutine(HitRoutine());
            if (enemyCurrentHP <= 0)
            {
                if (coinPrefab != null && enemyPoint > 0)
                {
                    for (int i = 0; i < enemyPoint; i++)
                    {
                        Instantiate(coinPrefab, transform.position, Quaternion.identity);
                    }
                }
                RaiseBossDeath();
                StartCoroutine(Die());
            }
        }
        

    }

    private IEnumerator ReturnToIdle()
    {
        isAttacking = true;
        PlayAnim("attack");
        yield return new WaitForSeconds(0.2f);
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
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        PlayAnim("die");
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

}
