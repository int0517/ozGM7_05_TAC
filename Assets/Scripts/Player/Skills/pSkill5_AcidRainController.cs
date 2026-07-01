using UnityEngine;

public class pSkill5_AcidRainController: MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private int damage;

    [Header("피격 쿨타임")]
    [SerializeField] private float hitTimer = 0f;
    [SerializeField] private float hitTimerMax = 1f;

    [SerializeField] PlayerStat pStat;

    [SerializeField] private float lifeTime = 3f;
    private float timer;

    private GameObject originPrefab;

    public void Init(PlayerStat pStat, GameObject prefab)
    {
        // 이펙트
        float damageBonus = PlayerStatDictionary.PlayerDamageIncrease[pStat.GetStatLvl(PlayerStatEnum.DamageIncrease)];
        timer = lifeTime;
        originPrefab = prefab;
    }

    private void Update()
    {
        hitTimer += Time.deltaTime;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Managers.Pool.ReturnPool(originPrefab, this);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((targetLayer.value & (1 << collision.gameObject.layer)) == 0) return;

        float totalDamage = damage *
            PlayerStatDictionary.PlayerDamageIncrease[pStat.GetStatLvl(PlayerStatEnum.DamageIncrease)];

        if (hitTimer >= hitTimerMax)
        {
            if (collision.gameObject.GetComponent<IDamageable>() is IDamageable damageable)
            {
                damageable.TakeDamage(totalDamage);
            }

            hitTimer = 0f;
        }
    }
}
