using UnityEngine;

public class OrcPowerAttack : MonoBehaviour
{
    [SerializeField] private int posienATK = 1;
    private PlayerStat playerStat;
    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerStat = playerObj.GetComponent<PlayerStat>();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (playerStat != null)
            {
                playerStat.DamagePlayer(posienATK);
            }
        }
    }

}
