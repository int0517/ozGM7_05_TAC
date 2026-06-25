using UnityEngine;

public class pSkill4_SatellitesHit : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private int damage;
    [SerializeField] private PlayerStat pStat;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((targetLayer.value & (1 << collision.gameObject.layer)) == 0) return;

        float totalDamage = damage *
            PlayerStatDictionary.PlayerDamageIncrease[pStat.GetStatLvl(PlayerStatEnum.DamageIncrease)];
        // 적 피격
    }
}
