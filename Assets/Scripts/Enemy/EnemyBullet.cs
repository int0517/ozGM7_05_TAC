using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [Header("투사체 스텟")]
    [SerializeField] private float bulletSpeed = 3f;
    [SerializeField] private float bulletLifeTime = 3f;
    private PlayerStat playerStat;
    void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocity = transform.right * bulletSpeed;
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
