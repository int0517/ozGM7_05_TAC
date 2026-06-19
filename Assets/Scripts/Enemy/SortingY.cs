using UnityEngine;

public class SortingY : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        int precision = 100;
        spriteRenderer.sortingOrder = -(int)(transform.position.y * precision);
    }
}