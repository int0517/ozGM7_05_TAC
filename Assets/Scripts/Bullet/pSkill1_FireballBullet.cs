using UnityEngine;

public class pSkill1_FireballBullet : MonoBehaviour
{
    private static Sprite fallbackFireballSprite;

    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private int damage;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float explosionRadius = 1f;
    [SerializeField] private float searchLength = 8f;

    private bool isExploded = false;
    private Vector2 moveDirection;

    void Start()
    {
        EnsureVisibleFallbackSprite();
        SearchEnemy();
        Debug.Log($"방향 : {moveDirection}");
        Debug.Log($"속도 : {moveSpeed}");
        Destroy(gameObject, 5f);
    }

    private void EnsureVisibleFallbackSprite()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            return;
        }

        if (fallbackFireballSprite == null)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.filterMode = FilterMode.Point;
            texture.SetPixel(0, 0, new Color(1f, 0.35f, 0.05f, 1f));
            texture.Apply();

            fallbackFireballSprite = Sprite.Create(
                texture,
                new Rect(0f, 0f, 1f, 1f),
                new Vector2(0.5f, 0.5f),
                1f
            );
            fallbackFireballSprite.name = "FireballFallbackSquare";
        }

        spriteRenderer.sprite = fallbackFireballSprite;
        spriteRenderer.sharedMaterial = null;
        spriteRenderer.color = new Color(1f, 0.35f, 0.05f, 1f);
        spriteRenderer.sortingOrder = Mathf.Max(spriteRenderer.sortingOrder, 20);
    }

    void Update()
    {
        transform.position += (Vector3)moveDirection * moveSpeed * Time.deltaTime;
    }

    private void SearchEnemy()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, searchLength, targetLayer);

        if(enemies.Length == 0 )
        {
            moveDirection = Vector2.down;
            return;
        }

        Transform nearestEnemy = null;
        float nearestDistance = float.MaxValue;

        foreach (Collider2D enemy in enemies)
        {
            float distance =
                (enemy.transform.position - transform.position).sqrMagnitude;

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy.transform;
            }
        }

        moveDirection = (nearestEnemy.position - transform.position).normalized;
        
        transform.up = moveDirection;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isExploded) return;
        if ((targetLayer.value & (1 << collision.gameObject.layer)) == 0) return;

        isExploded = true;
        Explode();
    }

    private void Explode()
    {
        // 폭발 이펙트 추가

        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius, targetLayer);

        foreach (Collider2D enemy in enemies)
        {
            // 적 피격 메서드
        }

        Destroy(gameObject);
    }

    public void SetExplosionRadius(float amount)
    {
        explosionRadius = amount;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.orange;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
