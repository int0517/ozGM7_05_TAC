using UnityEngine;

public class PlayerMagnet : MonoBehaviour
{
    [SerializeField] private float magnetRadius = 3.0f;
    [SerializeField] private float magnetMoveSpeed = 8.0f;
    [SerializeField] private LayerMask itemLayer;

    void Update()
    {
        MoveEnemies();
    }

    private void MoveEnemies()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            magnetRadius,
            itemLayer);

        foreach (Collider2D hit in hits)
        {
            hit.transform.position = Vector2.MoveTowards(
                hit.transform.position,
                transform.position,
                magnetMoveSpeed * Time.deltaTime);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, magnetRadius);
    }
}
