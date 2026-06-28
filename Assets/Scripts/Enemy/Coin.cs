using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector2 randomForce = new Vector2(Random.Range(-2f, 2f), Random.Range(2f, 4f));
        rb.AddForce(randomForce, ForceMode2D.Impulse);
        StartCoroutine(StopMovement());
        Destroy(gameObject, 10f);
    }
    private IEnumerator StopMovement()
    {
        yield return new WaitForSeconds(0.7f);
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;
        Debug.Log("코인 정지!");
    }

    private void OnTriggerEnter2D(Collider2D collision)//임시 방편 포인트 올려줘요!!!!!
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
