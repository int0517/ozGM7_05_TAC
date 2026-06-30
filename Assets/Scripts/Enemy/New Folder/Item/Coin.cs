using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{
    private Rigidbody2D rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Init()
    {
        StopAllCoroutines();

        rb.gravityScale = 1f;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        Vector2 randomForce = new Vector2(
            Random.Range(-2f, 2f),
            Random.Range(2f, 4f));

        rb.AddForce(randomForce, ForceMode2D.Impulse);

        StartCoroutine(StopMovement());
        StartCoroutine(ReturnPoolRoutine());
    }
    private IEnumerator StopMovement()
    {
        yield return new WaitForSeconds(0.7f);
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;
    }
    private IEnumerator ReturnPoolRoutine()
    {
        yield return new WaitForSeconds(10f);
        StopAllCoroutines();
        CManagers.Pool.ReturnPool(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)//임시 방편 포인트 올려줘요!!!!!
    {
        if (collision.CompareTag("Player"))
        {
            StopAllCoroutines();
            CManagers.Pool.ReturnPool(this);
        }
    }
}
