using UnityEngine;

public class pSkill1_FireballBullet : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float damage;
    private float damageBonus;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float explosionRadius = 1f;
    [SerializeField] private float searchLength = 8f;

    private bool isExploded = false;
    private Vector2 moveDirection;

    [SerializeField] private float lifeTime = 5f;
    private float timer;

    [SerializeField] private GameObject fireballVFX;
    [SerializeField] private GameObject explosionVFX;

    private GameObject originPrefab;

    public void Init(PlayerStat pStat, GameObject prefab)
    {
        damageBonus = PlayerStatDictionary.PlayerDamageIncrease[pStat.GetStatLvl(PlayerStatEnum.DamageIncrease)];
        SearchEnemy();
        timer = lifeTime;
        originPrefab = prefab;
    }

    void Update()
    {
        transform.position += (Vector3)moveDirection * moveSpeed * Time.deltaTime;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Managers.Pool.ReturnPool(originPrefab, this);
        }
    }

    private void SearchEnemy()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, searchLength, targetLayer);

        if(enemies.Length == 0 )
        {
            moveDirection = Vector2.down;
            return;
        }

        Transform nearestEnemy = null;
        float nearestDistance = float.MaxValue;

        foreach (Collider2D enemy in enemies)
        {
            float distance =
                (enemy.transform.position - transform.position).sqrMagnitude;

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy.transform;
            }
        }

        moveDirection = (nearestEnemy.position - transform.position).normalized;
        
        transform.up = moveDirection;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isExploded) return;
        if ((targetLayer.value & (1 << collision.gameObject.layer)) == 0) return;

        isExploded = true;
        Explode();
    }

    private void Explode()
    {
        // 폭발 이펙트 추가
        fireballVFX.SetActive(false);
        explosionVFX.SetActive(true);

        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius, targetLayer);

        float totalDamage = damage * damageBonus;

        foreach (Collider2D enemy in enemies)
        {
            if (enemy.gameObject.GetComponent<IDamageable>() is IDamageable damageable)
            {
                moveSpeed = 0f;
                damageable.TakeDamage(totalDamage);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.orange;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
