using UnityEngine;
using System.Collections;

public class ShortRangeEnemyFollow : MonoBehaviour
{
    [Header("몬스터 스텟")]
    [SerializeField] private int Level = 1;
    [SerializeField] private int EnemyMaxHP = 3;
    private int enemyCurrentHP;
    [SerializeField] private int EnemyATK = 1;
    [SerializeField] private float EnemySpeed = 2.0f;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private EnemyHPUI enemyUI;
    public float knockbackForce = 20.0f;

    private Transform playerTransform;
    private Rigidbody2D rb;
    private bool isKnockedBack = false;
    void Start()
    {
        enemyCurrentHP = EnemyMaxHP;
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

        rb.AddForce(direction * EnemySpeed * 10f);
        if (rb.linearVelocity.magnitude > EnemySpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * EnemySpeed;
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
            enemyCurrentHP--;
            enemyUI.UpdateHealthBar(enemyCurrentHP, EnemyMaxHP);
            StartCoroutine(KnockbackRoutine(collision.transform.position));
            Debug.Log($"남은 체력 : {enemyCurrentHP}");
            if (enemyCurrentHP <= 0)
            {
                if (coinPrefab != null)
                {
                    Instantiate(coinPrefab, transform.position, Quaternion.identity);
                }
                Destroy(gameObject);
                Debug.Log("몬스터가 죽었다....");
            }
        }
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enumy"))
        {

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
