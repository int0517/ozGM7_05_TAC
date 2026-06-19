using UnityEngine;
using System.Collections;

public class HitFlash : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

   
    public void Flash()
    {
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        spriteRenderer.color = new Color(1f, 0.5f, 0.5f, 1f);
        yield return new WaitForSeconds(0.1f); 
        spriteRenderer.color = originalColor;
    }
}