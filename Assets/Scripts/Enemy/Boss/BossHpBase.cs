using UnityEngine;

public class BossHpBase : MonoBehaviour
{
    [Header("보스 체력")]
    [SerializeField] private int enemyMaxHP;
    [SerializeField] private int enemyCurrentHP;

    void Start()
    {
        enemyMaxHP = enemyCurrentHP;
        if (WaveManager.Instance != null)
        {
            WaveManager.Instance.RegisterBoss(this);
        }
    }
    public int GetCurrentHp() => enemyCurrentHP;
    public int GetMaxHp() => enemyMaxHP;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Skill"))
        {
            enemyCurrentHP--;
        }

    }
}