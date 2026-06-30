using UnityEngine;
using System.Collections;

public class EnemyBullets : MonoBehaviour, IPoolable
{
    [Header("투사체 스텟")]
    [SerializeField] private float bulletSpeed = 3f;
    [SerializeField] private float bulletLifeTime = 3f;
    [Header("회전 설정")]
    [SerializeField] private float rotationSpeed = 500f;
    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(Vector2 direction)
    {
        StopAllCoroutines();

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = rotationSpeed;

        rb.AddForce(direction.normalized * bulletSpeed, ForceMode2D.Impulse);

        StartCoroutine(LifeRoutine());
    }
    private IEnumerator LifeRoutine()
    {
        yield return new WaitForSeconds(bulletLifeTime);

        EBManagers.Pool.ReturnPool(this);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerStat>()?.DamagePlayer(1);
            EBManagers.Pool.ReturnPool(this);
        }
    }

    public void ReturnToPool()
    {
        EBManagers.Pool.ReturnPool(this);
    }
}
