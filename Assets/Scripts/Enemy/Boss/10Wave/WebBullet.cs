using UnityEngine;

public class WebBullet : MonoBehaviour
{
    [Header("투사체 스텟")]
    [SerializeField] private float bulletSpeed = 6f;
    [SerializeField] private float bulletLifeTime = 3f;
    [SerializeField] private float homingRange = 5f;
    [SerializeField] private float turnSpeed = 200f;

    private Rigidbody2D rb;
    private Transform player;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        Destroy(gameObject, bulletLifeTime);
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > homingRange)
        {
            rb.angularVelocity = 0f;
            rb.linearVelocity = transform.right * bulletSpeed;
            return;
        }
        Vector2 direction = ((Vector2)player.position - rb.position).normalized;

        float rotateAmount = Vector3.Cross(direction, transform.right).z;

        rb.angularVelocity = -rotateAmount * turnSpeed;
        rb.linearVelocity = transform.right * bulletSpeed;
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, homingRange);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            TwoBoss boss = Object.FindAnyObjectByType<TwoBoss>();

            // 보스가 살아있고 active일 때만 실행
            if (boss != null && boss.gameObject.activeInHierarchy)
            {
                boss.StartPulling();
            }

            Destroy(gameObject);
        }
    }
}
