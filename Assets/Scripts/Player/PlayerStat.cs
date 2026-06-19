using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    private int pLevel = 1;
    private int pMaxHP = 3;
    private int pCurrentHP;
    private int pAttackBonus = 1;
    private float pSpeedBonus = 1f;

    public int PLevel { get { return pLevel; } }
    public int PMaxHP { get { return pMaxHP; } }
    public int PCurrentHP { get { return pCurrentHP; } }
    public int PAttackBonus
    {
        get => pAttackBonus;
        set => pAttackBonus = Mathf.Max(0, value);
    }
    
    public float PSpeedBonus
    {
        get =>pSpeedBonus;
        set => pSpeedBonus = Mathf.Max(0f, value);
    }

    private float playerNonhitTimerMax = 2f;
    private float playerNonhitTimer = 0f;

    private void Start()
    {
        pCurrentHP = pMaxHP;
    }

    private void Update()
    {
        if (playerNonhitTimer < playerNonhitTimerMax) playerNonhitTimer += Time.deltaTime;
    }

    public void IncreasePlayerLevel()
    {
        pLevel++;
    }

    public void IncreasePlayerMaxHP()
    {
        pMaxHP++;
        pCurrentHP++;
    }

    public void DamagePlayer(int amount)
    {
        if (playerNonhitTimer < playerNonhitTimerMax) return;

        pCurrentHP -= amount;

        if (pCurrentHP < 0) pCurrentHP = 0;
        // 플레이어 사망 처리, 종료 화면
    }

    public void SetPlayerAttackBonus(int amount)
    {
        pAttackBonus = amount;

        if (pAttackBonus < 0) pAttackBonus = 0;
    }

    public void SetPlayerSpeedBonus(float amount)
    {
        pSpeedBonus = amount;
    }
}
