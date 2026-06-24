using UnityEngine;
using System.Collections;

public class NewThreeBoss : MonoBehaviour
{
    [Header("보스 기본 스텟")]
    [SerializeField] protected int enemyRange = 5;
    [SerializeField] protected float enemyFireInterval = 1;
    [SerializeField] protected float enemySpeed = 1.0f;
    [SerializeField] protected int enemyPoint = 0;
    [SerializeField] protected GameObject coinPrefab;
    [Header("보스 추가 스텟")]
    [SerializeField] public float runningSpeed = 15f;
    [SerializeField] private LineRenderer warningLine;
    [SerializeField] private float chargeDuration = 2f;
    [Header("조개 오브젝트")]
    [SerializeField] private GameObject leftShell;  // 왼쪽 조개
    [SerializeField] private GameObject rightShell; // 오른쪽 조개
    [SerializeField] private Shell leftShellTrigger;
    [SerializeField] private Shell rightShellTrigger;
    private SpriteRenderer spriteRenderer;

    public LayerMask playerLayer;
    protected Transform playerTransform;
    protected Rigidbody2D rb;
    protected bool isAttack = false;
    protected bool timerCheck = false;
    protected float fireTimer = 0;

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
    }

    void FixedUpdate()
    {
        if (timerCheck) return;

        if (rb.linearVelocity.magnitude > 0.1f)
        {
            UpdateShells(rb.linearVelocity.x < 0);
        }

        if (!timerCheck)
        {
            CheckForPlayer();
        }
        if (playerTransform == null) return;
        if (isAttack)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = (playerTransform.position - transform.position).normalized;

        rb.AddForce(direction * enemySpeed * 10f);
        if (rb.linearVelocity.magnitude > enemySpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * enemySpeed;
        }
    }

    private void UpdateShells(bool isLeft)
    {
        if (leftShell == null || rightShell == null) return;

        leftShell.SetActive(isLeft);
        rightShell.SetActive(!isLeft);
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

        UpdateShells(chargeDir.x < 0);

        rb.linearVelocity = chargeDir * runningSpeed;
        yield return new WaitForSeconds(chargeDuration);
    }

    private IEnumerator ChargeCooldown()
    {
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(0.5f);
    }
    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyRange);
    }
}
