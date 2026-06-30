using UnityEngine;
using System.Collections;

public class Potion : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private GameObject HealEffect;
    [SerializeField] private int upHp = 1;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (playerStat != null)
            {
                playerStat.HealPlayer(upHp);
            }
            Destroy(gameObject);
        }


    }

}
