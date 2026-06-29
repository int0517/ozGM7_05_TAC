using UnityEngine;

public class PlayerNormalAttack : MonoBehaviour
{
    [SerializeField] private PlayerNormalAttackBullet bulletPrefab;
    [SerializeField] private Transform[] firePoint;

    [Header("스킬 레벨")]
    [SerializeField] private int normalAttackLevel;
    public int NormalAttackLevel { get { return normalAttackLevel; } }

    [Header("공격 쿨타임")]
    [SerializeField] private float attackTimerMax = 1.0f;
    [SerializeField] private float attackTimer = 0f;

    [SerializeField] private PlayerStat pStat;

    void Update()
    {
        if(!pStat.IsDead)
        {
            attackTimer += Time.deltaTime
             * PlayerStatDictionary.PlayerAttackSpeed[pStat.GetStatLvl(PlayerStatEnum.AttackSpeed)];

            Attack();
        }
        
    }

    private void Attack()
    {
        if (attackTimer <= attackTimerMax) return;

        switch(normalAttackLevel)
        {
            case 0:
                break;
            case 1:
                Instantiate(bulletPrefab, firePoint[0].position, transform.rotation).Init(pStat);
                break;
            case 2:
                Instantiate(bulletPrefab, firePoint[1].position, transform.rotation).Init(pStat);
                Instantiate(bulletPrefab, firePoint[2].position, transform.rotation).Init(pStat);
                break;
            case 3:
                Instantiate(bulletPrefab, firePoint[0].position, transform.rotation).Init(pStat);
                Instantiate(bulletPrefab, firePoint[3].position, transform.rotation).Init(pStat);
                Instantiate(bulletPrefab, firePoint[4].position, transform.rotation).Init(pStat);
                break;
        }
        
        attackTimer = 0f;
    }

    public void SkillLevelUp()
    {
        if (normalAttackLevel < 3) normalAttackLevel++;
    }
}
