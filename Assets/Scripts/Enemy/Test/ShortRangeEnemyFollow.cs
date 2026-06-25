using UnityEngine;
using System.Collections;

public class ShortRangeEnemyFollowTest : MonoBehaviour
{
    [Header("몬스터 스텟")]
    [SerializeField] private float enemyMaxHP = 3;
    [SerializeField] private int enemyATK = 1;
    [SerializeField] private float enemySpeed = 2.0f;
    [SerializeField] private int enemyPoint = 0;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private EnemyHPUI enemyUI;
    public float knockbackForce = 20.0f;

    private float enemyCurrentHP;
    private Animator anim;
    private Rigidbody2D rb;
    private PlayerStat playerStat;
    private Transform playerTransform;

    private bool isDead = false;
    private bool isKnockedBack = false;
    private int currentDebugState = -1; // 디버깅용 현재 상태

    void Start()
    {
        enemyCurrentHP = enemyMaxHP;
        rb = GetComponent<Rigidbody2D>();

        // 중요: 자식 오브젝트에서 Animator를 찾음
        anim = GetComponentInChildren<Animator>();
        if (anim == null) Debug.LogError($"{gameObject.name}의 자식에서 Animator를 찾을 수 없습니다!");

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
            playerStat = playerObj.GetComponent<PlayerStat>();
        }
    }

    void FixedUpdate()
    {

        if (isDead || anim == null || isKnockedBack) return;

        // 이동 속도에 따라 상태 결정 (0: Idle, 1: Walk)
        int targetState = (rb.linearVelocity.magnitude > 0.1f) ? 1 : 0;
        SetState(targetState);

        // 추적 로직
        if (playerTransform != null)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            rb.AddForce(direction * enemySpeed * 10f);

            // --- 여기서부터 추가된 부분 ---
            SpriteRenderer sr = anim.GetComponent<SpriteRenderer>();
            if (direction.x > 0) sr.flipX = false; // 오른쪽 이동
            else if (direction.x < 0) sr.flipX = true; // 왼쪽 이동
                                                       // ---------------------------

            if (rb.linearVelocity.magnitude > enemySpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * enemySpeed;
            }
        }
    }

    // 상태 변경 및 디버그 출력
    private void SetState(int newState)
    {
        if (currentDebugState != newState)
        {
            Debug.Log($"[애니메이션 상태 변경] {currentDebugState} -> {newState}");
            anim.SetInteger("State", newState);
            currentDebugState = newState;
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;
        enemyCurrentHP -= damage;
        enemyUI.UpdateHealthBar(enemyCurrentHP, enemyMaxHP);

        if (enemyCurrentHP <= 0) StartCoroutine(DieRoutine());
        else StartCoroutine(HitRoutine());
    }

    private IEnumerator HitRoutine()
    {
        Debug.Log("[애니메이션] 맞음 상태 진입 (2)");
        anim.SetInteger("State", 2);
        currentDebugState = 2;

        yield return new WaitForSeconds(0.3f);

        currentDebugState = -1; // 다음 FixedUpdate에서 상태 재계산 강제
    }

    private IEnumerator DieRoutine()
    {
        isDead = true;
        Debug.Log("[애니메이션] 죽음 상태 진입 (3)");
        anim.SetInteger("State", 3);
        currentDebugState = 3;

        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }

    private IEnumerator KnockbackRoutine(Vector3 attackerPos)
    {
        isKnockedBack = true;
        Vector2 knockbackDir = (transform.position - attackerPos).normalized;
        rb.linearVelocity = knockbackDir * knockbackForce;

        yield return new WaitForSeconds(0.2f);

        isKnockedBack = false;
        currentDebugState = -1; // 복귀 시 상태 재계산
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (playerStat != null) playerStat.DamagePlayer(enemyATK);
        }
        if (collision.CompareTag("Skill"))
        {
            StartCoroutine(KnockbackRoutine(collision.transform.position));
            TakeDamage(1);
        }
    }
}