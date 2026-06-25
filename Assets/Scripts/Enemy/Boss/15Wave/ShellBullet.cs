using UnityEngine;

public class ShellBullet : MonoBehaviour
{
    [Header("투사체 스텟")]
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private int bulletATK = 1;
    [SerializeField] private float bulletLifeTime = 3f;
    [SerializeField] private float rotationSpeed = 300f;

    private PlayerStat playerStat;
    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        rb.linearVelocity = transform.right * bulletSpeed;
        rb.angularVelocity = rotationSpeed;
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
                playerStat.DamagePlayer(bulletATK);
            }
        }
        if (collision.CompareTag("Player"))
        {
            Debug.Log("플레이어 피격!");
            Destroy(gameObject);
        }
    }
}
