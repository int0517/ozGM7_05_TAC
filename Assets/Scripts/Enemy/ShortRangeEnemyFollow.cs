using UnityEngine;
using System.Collections;

public class ShortRangeEnemyFollow : MonoBehaviour
{
    [Header("몬스터 스텟")]
    [SerializeField] private int enemyMaxHP = 3;
    private int enemyCurrentHP;
    [SerializeField] private int enemyATK = 1;
    [SerializeField] private float enemySpeed = 2.0f;
    [SerializeField] private int enemyPoint = 0;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private EnemyHPUI enemyUI;
    public float knockbackForce = 20.0f;

    private Transform playerTransform;
    private Rigidbody2D rb;
    private bool isKnockedBack = false;
    void Start()
    {
        enemyCurrentHP = enemyMaxHP;
        rb = GetComponent<Rigidbody2D>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
    }

    void FixedUpdate()
    {
       
        if (isKnockedBack || playerTransform == null) return;

        Vector2 direction = (playerTransform.position - transform.position).normalized;

        rb.AddForce(direction * enemySpeed * 10f);
        if (rb.linearVelocity.magnitude > enemySpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * enemySpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            //Destroy(collision.gameObject);
            //Debug.Log("죽어랏!");
        }
        if (collision.CompareTag("Skill"))
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
            enemyUI.UpdateHealthBar(enemyCurrentHP, enemyMaxHP);
            StartCoroutine(KnockbackRoutine(collision.transform.position));
        }

    }
    private IEnumerator KnockbackRoutine(Vector3 attackerPos)
    {
        isKnockedBack = true;

        Vector2 knockbackDir = (transform.position - attackerPos).normalized;
        rb.linearVelocity = knockbackDir * knockbackForce;

        yield return new WaitForSeconds(0.2f);

        isKnockedBack = false;
    }
}
