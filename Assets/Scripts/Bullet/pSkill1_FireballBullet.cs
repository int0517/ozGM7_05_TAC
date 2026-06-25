using UnityEngine;

public class pSkill1_FireballBullet : MonoBehaviour
{
    private static Sprite fallbackFireballSprite;

    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float damage;
    private float damageBonus;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float explosionRadius = 1f;
    [SerializeField] private float searchLength = 8f;

    private bool isExploded = false;
    private Vector2 moveDirection;

    public void Init(PlayerStat pStat)
    {
        damageBonus = PlayerStatDictionary.PlayerDamageIncrease[pStat.GetStatLvl(PlayerStatEnum.DamageIncrease)];
        SearchEnemy();
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        transform.position += (Vector3)moveDirection * moveSpeed * Time.deltaTime;
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

        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius, targetLayer);

        float totalDamage = damage * damageBonus;

        foreach (Collider2D enemy in enemies)
        {
            // 적 피격 메서드
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.orange;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
