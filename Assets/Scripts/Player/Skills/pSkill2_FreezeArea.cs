using System.Collections;
using UnityEngine;

public class pSkill2_FreezeArea : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float freezeRadius = 1f;
    [SerializeField] private float freezeTime = 2f;

    [Header("스킬 레벨")]
    [SerializeField] private int skill2Level = 0;

    [Header("공격 쿨타임")]
    [SerializeField] private float attackTimerMax = 5.0f;
    [SerializeField] private float attackTimer = 0f;

    void Update()
    {
        attackTimer += Time.deltaTime;

        StartCoroutine(Freeze());
    }

    IEnumerator Freeze()
    {
        if (attackTimer <= attackTimerMax) yield break;

        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, freezeRadius, targetLayer);

        foreach (Collider2D enemy in enemies)
        {
            if (enemy.TryGetComponent<Rigidbody2D>(out var enemyRB))
            {
                enemyRB.simulated = false;
            }
        }

        yield return new WaitForSeconds(freezeTime);

        foreach (Collider2D enemy in enemies)
        {
            if (enemy != null && enemy.TryGetComponent<Rigidbody2D>(out var enemyRB))
            {
                enemyRB.simulated = true;
            }
        }
        attackTimer = 0f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, freezeRadius);
    }
}
