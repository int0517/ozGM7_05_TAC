using UnityEngine;
using System.Collections;
using SP1Assets.MonsterPack2D;

public class ShortRangeEnemyFollow : MonoBehaviour, IDamageable
{
    [Header("몬스터 스텟")]
    [SerializeField] private float enemyMaxHP = 3;
    private float enemyCurrentHP;
    [SerializeField] private int enemyATK = 1;
    [SerializeField] private float enemySpeed = 2.0f;
    [SerializeField] private int enemyPoint = 0;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject pothionPrefab;
    private PlayerStat playerStat;
    public float knockbackForce = 20.0f;

    private Transform playerTransform;
    private Rigidbody2D rb;
    private bool isKnockedBack = false;
    MonsterPrefabController monster;

    private bool isDead = false;
    private bool isHit = false;

    private string currentAnim = "";

    [Header("이동 제한")]
    private float posX, posY;
    [SerializeField] private float posXMax = 27.25f;
    [SerializeField] private float posXMin = -28.25f;
    [SerializeField] private float posYMax = 17.75f;
    [SerializeField] private float posYMin = -17.75f;

    private void PlayAnim(string animName)
    {
        if (currentAnim == animName)
            return;

        currentAnim = animName;
        monster.PlayAnimation(animName, 0.1f);
    }
    void Start()
    {
        enemyCurrentHP = enemyMaxHP;
        rb = GetComponent<Rigidbody2D>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
            playerStat = playerObj.GetComponent<PlayerStat>();
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

    void FixedUpdate()
    {
       
        if (isKnockedBack || playerTransform == null) return;

        FacePlayer();

        Vector2 direction = (playerTransform.position - transform.position).normalized;
        
        rb.AddForce(direction * enemySpeed * 10f);
        if (rb.linearVelocity.magnitude > enemySpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * enemySpeed;
        }
        MoveLimit();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;
        if (collision.CompareTag("Player"))
        {
            if (playerStat != null)
            {
                playerStat.DamagePlayer(enemyATK);
            }
        }
        if (collision.CompareTag("Skill"))
        {
            StartCoroutine(KnockbackRoutine(collision.transform.position));
        }

    }
    public void TakeDamage(float damage)
    {
        if (isDead) return;
        enemyCurrentHP -= damage;
        StartCoroutine(HitRoutine());
        if (enemyCurrentHP <= 0)
        {
            if (coinPrefab != null && enemyPoint > 0)
            {
                Instantiate(pothionPrefab, transform.position, Quaternion.identity);
                for (int i = 0; i < enemyPoint; i++)
                {
                    Instantiate(coinPrefab, transform.position, Quaternion.identity);
                }
            }
            StartCoroutine(Die());
        }

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

    private IEnumerator HitRoutine()
    {
        isHit = true;

        PlayAnim("hit");

        yield return new WaitForSeconds(1f);

        PlayAnim("walk");

        isHit = false;
    }
    private IEnumerator KnockbackRoutine(Vector3 attackerPos)
    {
        isKnockedBack = true;

        Vector2 knockbackDir = (transform.position - attackerPos).normalized;
        rb.linearVelocity = knockbackDir * knockbackForce;

        yield return new WaitForSeconds(0.2f);

        isKnockedBack = false;
    }
    private void MoveLimit()
    {
        Vector2 pos = rb.position;

        pos.x = Mathf.Clamp(pos.x, posXMin, posXMax);
        pos.y = Mathf.Clamp(pos.y, posYMin, posYMax);

        rb.position = pos;
    }
}
