using UnityEngine;

public class WebBullet : MonoBehaviour
{
    [Header("투사체 스텟")]
    [SerializeField] private float bulletSpeed = 6f;
    [SerializeField] private float bulletLifeTime = 3f;
    void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocity = transform.right * bulletSpeed;
        Destroy(gameObject, bulletLifeTime);
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
