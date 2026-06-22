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
        yield return new WaitForSeconds(0.7f); // 1초 대기
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;
        Debug.Log("코인 정지!");
    }
}
