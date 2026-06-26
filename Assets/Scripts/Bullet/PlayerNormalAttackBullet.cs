using UnityEngine;

public class PlayerNormalAttackBullet : MonoBehaviour
{
    private static Sprite fallbackBulletSprite;

    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private int damage;
    [SerializeField] private float moveSpeed = 10f;

    [SerializeField] private PlayerStat pStat;

    private void Start()
    {
        pStat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStat>();
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        transform.Translate(Vector3.down *moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((targetLayer.value & (1 << collision.gameObject.layer)) == 0) return;

        float totalDamage = damage *
            PlayerStatDictionary.PlayerDamageIncrease[pStat.GetStatLvl(PlayerStatEnum.DamageIncrease)];
        
        if (collision.gameObject.GetComponent<IDamageable>() is IDamageable damageable)
        {
            damageable.TakeDamage(totalDamage);
        }

        Destroy(gameObject);
    }
}
