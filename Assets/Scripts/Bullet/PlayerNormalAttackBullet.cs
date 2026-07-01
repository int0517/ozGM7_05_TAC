using UnityEngine;

public class PlayerNormalAttackBullet : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float damage;
    private float damageBonus;
    [SerializeField] private float moveSpeed = 10f;

    [SerializeField] private float lifeTime = 5f;
    private float timer;

    private GameObject originPrefab;

    public void Init(PlayerStat pStat, GameObject prefab)
    {
        damageBonus = PlayerStatDictionary.PlayerDamageIncrease[pStat.GetStatLvl(PlayerStatEnum.DamageIncrease)];
        timer = lifeTime;
        originPrefab = prefab;
    }

    void Update()
    {
        transform.Translate(Vector3.down *moveSpeed * Time.deltaTime);

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Managers.Pool.ReturnPool(originPrefab, this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((targetLayer.value & (1 << collision.gameObject.layer)) == 0) return;

        float totalDamage = damage * damageBonus;
        
        if (collision.gameObject.GetComponent<IDamageable>() is IDamageable damageable)
        {
            damageable.TakeDamage(totalDamage);
        }

        Managers.Pool.ReturnPool(originPrefab, this);
    }
}
