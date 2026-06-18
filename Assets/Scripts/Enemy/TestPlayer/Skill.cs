using UnityEngine;

public class Skill : MonoBehaviour
{
    public float speed = 10.0f;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Enumy"))
        {
            Destroy(gameObject);
            Debug.Log("탕!");
        }
        
    }
}
