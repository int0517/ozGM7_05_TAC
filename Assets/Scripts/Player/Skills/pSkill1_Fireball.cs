using UnityEngine;

public class pSkill1_FireBall : MonoBehaviour
{
    [SerializeField] private pSkill1_FireballBullet[] bulletPrefabs;
    [SerializeField] private Transform firePoint;

    [Header("스킬 레벨")]
    [SerializeField] private int skill1Level = 0;
    public int Skill1Level { get { return skill1Level; } }

    [Header("공격 쿨타임")]
    [SerializeField] private float attackTimerMax = 1.0f;
    [SerializeField] private float attackTimer = 0f;

    [SerializeField] private PlayerStat pStat;

    void Update()
    {
        if(!pStat.IsDead)
        {
            if (attackTimer < attackTimerMax) attackTimer += Time.deltaTime
                 * PlayerStatDictionary.PlayerAttackSpeed[pStat.GetStatLvl(PlayerStatEnum.AttackSpeed)];

            Attack();
        }
    }

    private void Attack()
    {
        if (attackTimer <= attackTimerMax) return;

        if (firePoint != null)
        {
            switch(skill1Level)
            {
                case 0:
                    break;
                case 1:
                    pSkill1_FireballBullet bullet1 = Managers.Pool.GetPool(bulletPrefabs[0]);
                    bullet1.transform.position = firePoint.position;
                    bullet1.transform.rotation = transform.rotation;
                    bullet1.Init(pStat);
                    break;
                case 2:
                    pSkill1_FireballBullet bullet2 = Managers.Pool.GetPool(bulletPrefabs[1]);
                    bullet2.transform.position = firePoint.position;
                    bullet2.transform.rotation = transform.rotation;
                    bullet2.Init(pStat);
                    break;
                case 3:
                    pSkill1_FireballBullet bullet3 = Managers.Pool.GetPool(bulletPrefabs[2]);
                    bullet3.transform.position = firePoint.position;
                    bullet3.transform.rotation = transform.rotation;
                    bullet3.Init(pStat);
                    break;
                default:
                    break;
            }
        }

        attackTimer = 0f;
    }

    public void Skill1LevelUp()
    {
        if (skill1Level < 3) skill1Level++;
    }
}
