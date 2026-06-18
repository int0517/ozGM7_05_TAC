using UnityEngine;
using System.Collections;

public class LongRangeEnemyFollow : MonoBehaviour
{
    [Header("몬스터 스텟")]
    [SerializeField] private int enumyLevel = 1;
    [SerializeField] private int enemyHP = 3;
    [SerializeField] private int enemyRange = 5;
    [SerializeField] private int enemyATK = 1;
    [SerializeField] private float enemyFireInterval = 1;
    [SerializeField] private float enemySpeed = 2.0f;

    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;


    public float knockbackForce = 20.0f;

    public LayerMask playerLayer;
    private Transform playerTransform;
    private Rigidbody2D rb;
    private bool isAttack = false;
    private bool isKnockedBack = false;
    private bool timerCheck = false;
    private float fireTimer=0;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
    }

    
    void FixedUpdate()
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

        rb.AddForce(direction * enemySpeed * 10f);
        if (rb.linearVelocity.magnitude > enemySpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * enemySpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Skill"))
        {
            enemyHP--;
            StartCoroutine(KnockbackRoutine(collision.transform.position));
            Debug.Log($"남은 체력 : {enemyHP}");
            if (enemyHP <= 0)
            {
                Destroy(gameObject);
                Debug.Log("몬스터가 죽었다....");
            }
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

    private void Fire()
    {
        fireTimer += Time.fixedDeltaTime;
        timerCheck = true;
        if (fireTimer >= enemyFireInterval)
        {
            Vector2 direction = (playerTransform.position - firePoint.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0, 0, angle));
            fireTimer = 0f;
            timerCheck = false;
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
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyRange);
    }
}
