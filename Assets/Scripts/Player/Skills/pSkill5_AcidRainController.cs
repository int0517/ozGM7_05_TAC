using UnityEngine;

public class pSkill5_AcidRainController: MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private int damage;
    [SerializeField] private float destroyTime = 3f;

    [Header("피격 쿨타임")]
    [SerializeField] private float hitTimer = 0f;
    [SerializeField] private float hitTimerMax = 1f;

    [SerializeField] PlayerStat pStat;

    public void Init(PlayerStat pStat)
    {
        // 이펙트
        float damageBonus = PlayerStatDictionary.PlayerDamageIncrease[pStat.GetStatLvl(PlayerStatEnum.DamageIncrease)];
        Destroy(gameObject, destroyTime);
    }

    private void Update()
    {
        hitTimer += Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((targetLayer.value & (1 << collision.gameObject.layer)) == 0) return;

        float totalDamage = damage *
            PlayerStatDictionary.PlayerDamageIncrease[pStat.GetStatLvl(PlayerStatEnum.DamageIncrease)];

        if (hitTimer >= hitTimerMax)
        {
            // 적 피격

            hitTimer = 0f;
        }
    }
}
