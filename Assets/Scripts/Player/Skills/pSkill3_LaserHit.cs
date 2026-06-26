using UnityEngine;

public class pSkill3_LaserHit : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float damage = 1;
    [SerializeField] private PlayerStat pStat;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((targetLayer.value & (1 << collision.gameObject.layer)) == 0) return;

        float totalDamage = damage *
            PlayerStatDictionary.PlayerDamageIncrease[pStat.GetStatLvl(PlayerStatEnum.DamageIncrease)];

        if (collision.gameObject.GetComponent<IDamageable>() is IDamageable damageable)
        {
            damageable.TakeDamage(totalDamage);
        }
    }
}
