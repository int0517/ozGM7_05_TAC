using UnityEngine;

public class ShellBullet : MonoBehaviour
{
    [Header("투사체 스텟")]
    //[SerializeField] private float bulletSpeed = 3f;
    [SerializeField] private float bulletLifeTime = 3f;
    void Start()
    {
        Destroy(gameObject, bulletLifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("플레이어 피격!");
            Destroy(gameObject);
        }
    }
}
