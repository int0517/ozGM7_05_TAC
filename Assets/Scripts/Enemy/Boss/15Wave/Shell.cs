using UnityEngine;

public class Shell : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 15f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Skill"))
        {
            ShootInRandomDirection();
            Destroy(collision.gameObject);
        }
    }
    public void ShootInRandomDirection()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;

        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        // 3. 탄환 이동 (Rigidbody2D를 사용한다고 가정)
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = randomDirection * bulletSpeed;
        }
    }
}
