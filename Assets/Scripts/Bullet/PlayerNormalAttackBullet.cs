using UnityEngine;

public class PlayerNormalAttackBullet : MonoBehaviour
{
    private static Sprite fallbackBulletSprite;

    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private int damage;
    [SerializeField] private float moveSpeed = 10f;

    [SerializeField] private PlayerStat pStat;

    private void Start()
    {
        EnsureVisibleFallbackSprite();
        Destroy(gameObject, 5f);
    }

    private void EnsureVisibleFallbackSprite()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            return;
        }

        if (fallbackBulletSprite == null)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.filterMode = FilterMode.Point;
            texture.SetPixel(0, 0, new Color(1f, 0.9f, 0.1f, 1f));
            texture.Apply();

            fallbackBulletSprite = Sprite.Create(
                texture,
                new Rect(0f, 0f, 1f, 1f),
                new Vector2(0.5f, 0.5f),
                1f
            );
            fallbackBulletSprite.name = "PlayerBulletFallbackSquare";
        }

        spriteRenderer.sprite = fallbackBulletSprite;
        spriteRenderer.sharedMaterial = null;
        spriteRenderer.color = new Color(1f, 0.9f, 0.1f, 1f);
        spriteRenderer.sortingOrder = Mathf.Max(spriteRenderer.sortingOrder, 20);
    }

    void Update()
    {
        transform.Translate(Vector3.down *moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((targetLayer.value & (1 << collision.gameObject.layer)) == 0) return;

        float totalDamage = damage *
            PlayerStatDictionary.PlayerDamageIncrease[pStat.GetStatLvl(PlayerStatEnum.DamageIncrease)];
        // 적 피격 메서드

        Destroy(gameObject);
    }
}
