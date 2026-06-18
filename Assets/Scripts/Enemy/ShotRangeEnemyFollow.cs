using UnityEngine;
using System.Collections;

public class ShotRangeEnemyFollow : MonoBehaviour
{
    [Header("몬스터 스텟")]
    [SerializeField] private int Level = 1;
    [SerializeField] private int EnemyHP = 3;
    [SerializeField] private int EnemyATK = 1;
    [SerializeField] private float EnemySpeed = 2.0f;
    public float knockbackForce = 20.0f;

    private Transform playerTransform;
    private Rigidbody2D rb;
    private bool isKnockedBack = false;
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
            EnemyHP--;
            StartCoroutine(KnockbackRoutine(collision.transform.position));
            Debug.Log($"남은 체력 : {EnemyHP}");
            if (EnemyHP <= 0)
            {
                Destroy(gameObject);
                Debug.Log("몬스터가 죽었다....");
            }
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
