using UnityEngine;

public class pSkill3_LaserHit : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private int damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((targetLayer.value & (1 << collision.gameObject.layer)) == 0) return;

        // 적 피격
    }
}
