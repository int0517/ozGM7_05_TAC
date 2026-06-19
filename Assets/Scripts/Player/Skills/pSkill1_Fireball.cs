using UnityEngine;

public class pSkill1_FireBall : MonoBehaviour
{
    [SerializeField] private pSkill1_FireballBullet bulletPrefab;
    [SerializeField] private Transform firePoint;

    [Header("스킬 레벨")]
    [SerializeField] private int skill1Level = 0;

    [Header("공격 쿨타임")]
    [SerializeField] private float attackTimerMax = 1.0f;
    [SerializeField] private float attackTimer = 0f;

    void Update()
    {
        attackTimer += Time.deltaTime;

        Attack();
    }

    private void Attack()
    {
        if (attackTimer <= attackTimerMax) return;

        Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        attackTimer = 0f;
    }
}
