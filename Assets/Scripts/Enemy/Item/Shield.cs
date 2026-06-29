using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private GameObject shieldEffect;
    private PlayerStat playerStat;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector2 randomForce = new Vector2(Random.Range(-2f, 8f), Random.Range(2f, 8f));
        rb.AddForce(randomForce, ForceMode2D.Impulse);
        StartCoroutine(StopMovement());
        Destroy(gameObject, 10f);
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerStat = playerObj.GetComponent<PlayerStat>();
        }
    }
    private IEnumerator StopMovement()
    {
        yield return new WaitForSeconds(0.7f);
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)//임시 방편 포인트 올려줘요!!!!!
    {
        if (collision.CompareTag("Player"))
        {
            if (playerStat != null)
            {
                //플레이어 함수 넣기
            }
            shieldEffect.SetActive(true);
            shieldEffect.transform.SetParent(collision.transform);
            Destroy(shieldEffect, 10f);
            Destroy(gameObject);
        }
    }
}
