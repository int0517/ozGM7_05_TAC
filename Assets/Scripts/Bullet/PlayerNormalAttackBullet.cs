using UnityEngine;

public class PlayerNormalAttackBullet : MonoBehaviour
{
    private static Sprite fallbackBulletSprite;

    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float damage;
    private float damageBonus;
    [SerializeField] private float moveSpeed = 10f;

    public void Init(PlayerStat pStat)
    {
        damageBonus = PlayerStatDictionary.PlayerDamageIncrease[pStat.GetStatLvl(PlayerStatEnum.DamageIncrease)];
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        transform.Translate(Vector3.down *moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((targetLayer.value & (1 << collision.gameObject.layer)) == 0) return;

        float totalDamage = damage * damageBonus;
        
        if (collision.gameObject.GetComponent<IDamageable>() is IDamageable damageable)
        {
            damageable.TakeDamage(totalDamage);
        }

        Destroy(gameObject);
    }
}
