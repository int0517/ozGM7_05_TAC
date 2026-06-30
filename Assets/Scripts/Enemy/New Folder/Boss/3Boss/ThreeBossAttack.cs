using UnityEngine;

public class ThreeBossAttack : MonoBehaviour
{
    private PlayerStat playerStat;
    [SerializeField] private ParticleSystem runPlayerEffect;
    private ThreeBoss boss;

    void Start()
    {
        boss = GetComponentInParent<ThreeBoss>();
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
            if (boss.IsCharging())
            {
                PlayerCargingEffect();
            }
            if (playerStat != null)
            {
                playerStat.DamagePlayer(1);
            }
        }
    }
    private void PlayerCargingEffect()
    {
        runPlayerEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        runPlayerEffect.Play();
    }
}
