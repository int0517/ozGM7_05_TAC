using UnityEngine;

public class Shell : MonoBehaviour
{
    public GameObject oneBulletPrefab;
    public GameObject twoBulletPrefab;
    public GameObject threeBulletPrefab;
    public float bulletSpeed = 15f;
    private PlayerStat playerStat;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerStat = playerObj.GetComponent<PlayerStat>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (playerStat != null)
            {
                playerStat.DamagePlayer(1);
            }
        }
            if (collision.CompareTag("Skill"))
        {
            ShootInRandomDirection();
            Destroy(collision.gameObject);
        }
    }
    public void ShootInRandomDirection()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;

        float angle = Mathf.Atan2(randomDirection.y, randomDirection.x) * Mathf.Rad2Deg;

        GameObject bullet = Instantiate(RandomBulletChoice(), transform.position, Quaternion.Euler(0, 0, angle));

        // 3. 탄환 이동 (Rigidbody2D를 사용한다고 가정)
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = randomDirection * bulletSpeed;
        }
    }

    private GameObject RandomBulletChoice()
    {
        switch (Random.Range(0, 3))
        {
            case 0:
                return oneBulletPrefab;

            case 1:
                return twoBulletPrefab;

            default:
                return threeBulletPrefab;
        }
    }
}
