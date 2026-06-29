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
    [SerializeField] protected GameObject damagePrefab;
    [SerializeField] protected float warningCount = 2.0f;
    [SerializeField] protected float powerWarningCount = 1.0f;
    [SerializeField] protected float damageCount = 0.5f;



    public List<GameObject> eggList = new List<GameObject>();
    private bool isInvincible = true;

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
        PlayAnim("walk");
    }
    private void FacePlayer()
    {
        if (playerTransform == null) return;

        Vector3 scale = transform.localScale;

        if (!isInvincible)
        {
            if (playerTransform.position.x > transform.position.x)
                scale.x = -Mathf.Abs(scale.x);
            else
                scale.x = Mathf.Abs(scale.x);

            transform.localScale = scale;
        }
        else if (isInvincible)
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
            knockbackForce = knockbackForce / 5;
            isInvincible = false;
            timerCheck = false;
            fireTimer = 0;
            monster.SetAnimationSpeed(4f);
            PlayAnim("walk");
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
        Vector2 moveDirection = isInvincible ? -direction : direction;

        float speedMultiplier = isInvincible ? 1f : 10f;

        rb.linearVelocity = moveDirection * enemySpeed * speedMultiplier;
        MoveLimit();
    }
    protected override void CheckForPlayer()
    {
        float dist = Vector2.Distance(transform.position, playerTransform.position);
        if (isInvincible)
        {
            if (dist <= enemyRange * 0.8)
            {
                isAttack = false;
                PlayAnim("walk");
            }
            else if (dist <= enemyRange)
            {
                isAttack = true;
                PlayAnim("idle");
            }
            else if (dist > enemyRange * 1.2f)
            {
                isAttack = false;
                fireTimer = 0f;
                PlayAnim("walk");
            }
        }
        else
        {
            isAttack = false;
            if (currentAnim != "walk")
            {
                monster.SetAnimationSpeed(4f);
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
            if (!isInvincible)
            {
                if (playerStat != null)
                {
                    playerStat.DamagePlayer(1);
                    StartCoroutine(ReturnToIdle());
                }
            }

        }
        if (collision.CompareTag("Skill"))
        {
            StartCoroutine(KnockbackRoutine(collision.transform.position));
            if (isInvincible)
            {
                StartCoroutine(HitRoutine());
            }

        }
    }



    public override void TakeDamage(float damage)
    {
        if (!isInvincible)
        {
            enemyCurrentHP -= damage;
            if (enemyCurrentHP <= 0)
            {
                Instantiate(bossPotionPrefab, transform.position, Quaternion.identity);
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
        monster.SetAnimationSpeed(1f);
        isAttacking = true;
        PlayAnim("attack");
        yield return new WaitForSeconds(0.2f);
        PlayAnim("idle");
        isAttacking = false;
    }
    private IEnumerator HitRoutine()
    {
        monster.SetAnimationSpeed(1f);
        isHit = true;
        PlayAnim("hit");
        yield return new WaitForSeconds(2f);
        PlayAnim("walk");
        isHit = false;
    }
    private IEnumerator Die()
    {
        monster.SetAnimationSpeed(1f);
        if (isDead) yield break;
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        PlayAnim("die");
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
