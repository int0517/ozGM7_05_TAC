using UnityEngine;

public class EnemyBullets : MonoBehaviour
{
    [Header("투사체 스텟")]
    [SerializeField] private float bulletSpeed = 3f;
    [SerializeField] private float bulletLifeTime = 3f;
    [Header("회전 설정")]
    [SerializeField] private float rotationSpeed = 500f;
    private PlayerStat playerStat;
    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * bulletSpeed;
        rb.angularVelocity = rotationSpeed;
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerStat = playerObj.GetComponent<PlayerStat>();
        }

        Destroy(gameObject, bulletLifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (playerStat != null)
            {
                playerStat.DamagePlayer(1);
            }
            Destroy(gameObject);
        }
    }
}
