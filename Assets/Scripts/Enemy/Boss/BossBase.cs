using UnityEngine;
using System.Collections;

public class BossBase : MonoBehaviour
{
    [Header("보스 기본 스텟")]
    [SerializeField] private int enemyMaxHP = 20;
    private int enemyCurrentHP;
    [SerializeField] private int enemyRange = 5;
    [SerializeField] private float enemyFireInterval = 1;
    [SerializeField] private float enemySpeed = 1.0f;
    [SerializeField] private int enemyPoint = 0;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] public float knockbackForce = 5.0f;

    public LayerMask playerLayer;
    private Transform playerTransform;
    private Rigidbody2D rb;
    private bool isKnockedBack = false;
    private bool isAttack = false;
    private float fireTimer = 0;
    protected virtual void Start()
    {
        enemyCurrentHP = enemyMaxHP;
        rb = GetComponent<Rigidbody2D>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
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

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Skill"))
        {
            TakeDamage();
            StartCoroutine(KnockbackRoutine(collision.transform.position));
        }
    }
    protected virtual void TakeDamage()//상대 공격력 넣어줘야함
    {
        if (enemyCurrentHP <= 0)
        {
            if (coinPrefab != null && enemyPoint > 0)
            {
                for (int i = 0; i < enemyPoint; i++)
                {
                    Instantiate(coinPrefab, transform.position, Quaternion.identity);
                }
            }
            Destroy(gameObject);
        }
        enemyCurrentHP--;
    }

    protected IEnumerator KnockbackRoutine(Vector3 attackerPos)
    {
        isKnockedBack = true;

        Vector2 knockbackDir = (transform.position - attackerPos).normalized;
        rb.linearVelocity = knockbackDir * knockbackForce;

        yield return new WaitForSeconds(0.2f);

        isKnockedBack = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyRange);
    }

}
