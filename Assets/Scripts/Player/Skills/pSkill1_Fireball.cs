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

    void Update()
    {
        if (attackTimer < attackTimerMax) attackTimer += Time.deltaTime;

        Attack();
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
                    Instantiate(bulletPrefabs[0], firePoint.position, Quaternion.identity);
                    break;
                case 2:
                    Instantiate(bulletPrefabs[1], firePoint.position, Quaternion.identity);
                    break;
                case 3:
                    Instantiate(bulletPrefabs[2], firePoint.position, Quaternion.identity);
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
