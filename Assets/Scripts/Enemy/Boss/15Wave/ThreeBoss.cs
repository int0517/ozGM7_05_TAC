using UnityEngine;
using System.Collections;
using SP1Assets.MonsterPack2D;

public class NewThreeBoss : MonoBehaviour
{
    [Header("보스 기본 스텟")]
    [SerializeField] protected int enemyRange = 5;
    [SerializeField] protected float enemyFireInterval = 1;
    [SerializeField] protected float enemySpeed = 1.0f;
    [SerializeField] protected int enemyPoint = 0;
    [SerializeField] protected GameObject coinPrefab;
    [SerializeField] protected float knockbackForce = 5.0f;
    [Header("보스 추가 스텟")]
    [SerializeField] public float runningSpeed = 15f;
    [SerializeField] private LineRenderer warningLine;
    [SerializeField] private float chargeDuration = 2f;

    [Header("보스 이동 제한")]
    private float posX, posY;
    [SerializeField] private float posXMax = 27.25f;
    [SerializeField] private float posXMin = -28.25f;
    [SerializeField] private float posYMax = 17.75f;
    [SerializeField] private float posYMin = -17.75f;
    private SpriteRenderer spriteRenderer;

    protected Transform playerTransform;
    protected Rigidbody2D rb;
    protected bool isAttack = false;
    protected bool timerCheck = false;
    protected float fireTimer = 0;
    protected bool isKnockedBack = false;
    private bool isCharging = false;
    MonsterPrefabController monster;
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
        rb = GetComponent<Rigidbody2D>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (warningLine != null) warningLine.enabled = false;
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
        if (timerCheck) return;


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
        FacePlayer();
        Vector2 direction = (playerTransform.position - transform.position).normalized;

        rb.AddForce(direction * enemySpeed * 10f);
        if (rb.linearVelocity.magnitude > enemySpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * enemySpeed;
        }
        MoveLimit();
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
        if (isAttack && !timerCheck)
        {
            StartCoroutine(ChargeSequence());
        }
    }
    public bool IsCharging()
    {
        return isCharging;
    }
    public void OnHeadDamaged(Vector3 hitPosition)
    {
        if (!isCharging)
        {
            StartCoroutine(KnockbackRoutine(hitPosition));

            StartCoroutine(hitRoutine());
        }
        
    }
    private IEnumerator hitRoutine()
    {
        PlayAnim("hit");
        yield return new WaitForSeconds(1.5f);
        PlayAnim("walk");
    }

    public void Die()
    {
        if (coinPrefab != null && enemyPoint > 0)
        {
            for (int i = 0; i < enemyPoint; i++)
            {
                Instantiate(coinPrefab, transform.position, Quaternion.identity);
            }
        }

        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        
        PlayAnim("die");
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        yield return new WaitForSeconds(0.5f);

        Destroy(gameObject);
    }

    private IEnumerator KnockbackRoutine(Vector3 attackerPos)
    {
        isKnockedBack = true;

        Vector2 knockbackDir = (transform.position - attackerPos).normalized;
        rb.linearVelocity = knockbackDir * knockbackForce;

        yield return new WaitForSeconds(0.2f);

        isKnockedBack = false;
    }

    private IEnumerator ChargeSequence()
    {
        timerCheck = true;

        yield return StartCoroutine(ShowWarning());    // 경고 단계
        yield return StartCoroutine(PerformCharge());  // 돌진 단계
        yield return StartCoroutine(ChargeCooldown()); // 대기 단계

        timerCheck = false;
        isAttack = false;
    }

    private IEnumerator ShowWarning()
    {
        isCharging= true;
        PlayAnim("idle");
        warningLine.enabled = true;
        float elapsed = 0f;

        while (elapsed < 2.0f)
        {
            warningLine.SetPosition(0, transform.position);
            warningLine.SetPosition(1, playerTransform.position);
            elapsed += Time.deltaTime;
            yield return null;
        }
        warningLine.enabled = false;
    }

    private IEnumerator PerformCharge()
    {
        Vector2 chargeDir = (playerTransform.position - transform.position).normalized;

        rb.linearVelocity = chargeDir * runningSpeed;

        monster.SetAnimationSpeed(2f);
        PlayAnim("walk");
        yield return new WaitForSeconds(chargeDuration);
        monster.SetAnimationSpeed(1f);
        isCharging= false;
    }

    private IEnumerator ChargeCooldown()
    {
        PlayAnim("idle");
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(0.5f);
        PlayAnim("walk");
    }
    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyRange);
    }

    private void MoveLimit()
    {
        rb.position = new Vector2(
    Mathf.Clamp(rb.position.x, posXMin, posXMax),
    Mathf.Clamp(rb.position.y, posYMin, posYMax));
    }
}
