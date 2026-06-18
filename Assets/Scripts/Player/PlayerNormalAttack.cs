using UnityEngine;

public class PlayerNormalAttack : MonoBehaviour
{
    [SerializeField] private PlayerNormalAttackBullet bulletPrefab;
    [SerializeField] private Transform playerTransform;

    [Header("공격 쿨타임")]
    [SerializeField] private float attackTimerMax = 1.0f;
    private float attackTimer = 0f;

    private void Awake()
    {
        
    }

    void Update()
    {
        attackTimer += Time.deltaTime;

        Attack();
    }

    private void Attack()
    {
        if (attackTimer <= attackTimerMax) return;

        Instantiate(bulletPrefab, playerTransform.position, playerTransform.rotation);
        attackTimer = 0f;
    }
}
