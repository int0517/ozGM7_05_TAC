using UnityEngine;

public class PlayerNormalAttack : MonoBehaviour
{
    [SerializeField] private PlayerNormalAttackBullet bulletPrefab;
    [SerializeField] private Transform firePoint;

    [Header("스킬 레벨")]
    [SerializeField] private int normalAttackLevel;

    [Header("공격 쿨타임")]
    [SerializeField] private float attackTimerMax = 1.0f;
    private float attackTimer = 0f;

    void Update()
    {
        attackTimer += Time.deltaTime;

        Attack();
    }

    private void Attack()
    {
        if (attackTimer <= attackTimerMax) return;

        Instantiate(bulletPrefab, firePoint.position, transform.rotation);
        attackTimer = 0f;
    }

    public void SkillLevelUp()
    {
        normalAttackLevel++;
    }
}
