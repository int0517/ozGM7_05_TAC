using UnityEngine;

public class ShellBullet : MonoBehaviour
{
    [Header("투사체 스텟")]
    [SerializeField] private int bulletATK = 1;
    [SerializeField] private float bulletLifeTime = 3f;
    private PlayerStat playerStat;
    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerStat = playerObj.GetComponent<PlayerStat>();
        }
        Destroy(gameObject, bulletLifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (playerStat != null)
            {
                playerStat.DamagePlayer(bulletATK);
            }
        }
        if (collision.CompareTag("Player"))
        {
            Debug.Log("플레이어 피격!");
            Destroy(gameObject);
        }
    }
}
