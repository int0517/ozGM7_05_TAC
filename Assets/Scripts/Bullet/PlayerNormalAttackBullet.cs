using UnityEngine;

public class PlayerNormalAttackBullet : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private int damage;
    [SerializeField] private float moveSpeed = 10f;

    private void Start()
    {
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        transform.Translate(Vector3.down *moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((targetLayer.value & (1 << collision.gameObject.layer)) == 0) return;

        // 적 피격 메서드

        Destroy(gameObject);
    }
}
