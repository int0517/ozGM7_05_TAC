using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    // 플레이어 스탯
    private int pLevel = 1;
    private int pMaxHP = 3;
    private int pCurrentHP;
    private int pAttackBonus = 1;
    private float pSpeedBonus = 1f;

    // 플레이어 스탯 프로퍼티
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

    // 플레이어 스탯 레벨
    private int pMoveSpeedLevel = 0;
    private int pAttackSpeedLevel = 0;
    private int pMaxHPLevel = 0;
    private int pDamageIncreaseLevel = 0;
    private int pMagnetLevel = 0;

    // 플레이어 스탯 레벨 프로퍼티
    public int PMoveSpeedLevel
    {  
        get => pMoveSpeedLevel;
        set => pMoveSpeedLevel = Mathf.Clamp(value, 0, 2);
    }

    public int PAttackSpeedLevel
    {
        get => pAttackSpeedLevel;
        set => pAttackSpeedLevel = Mathf.Clamp(value, 0, 2);
    }

    public int PMaxHPLevel
    {
        get => pAttackSpeedLevel;
        set => pAttackSpeedLevel = Mathf.Clamp(value, 0, 2);
    }

    public int PDamageIncreaseLevel
    {
        get => pDamageIncreaseLevel;
        set => pDamageIncreaseLevel = Mathf.Clamp(value, 0, 2);
    }

    public int PMagnetLevel
    {
        get => pMagnetLevel;
        set => pMagnetLevel = Mathf.Clamp(value, 0, 2);
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

    // 플레이어 스탯 레벨업 메서드
    public void PlayerMoveSpeedLevelUp()
    {
        if (pMoveSpeedLevel >= 2) return;
        pMoveSpeedLevel++;
    }

    public void PlayerAttackSpeedLevelUp()
    {
        if (pAttackSpeedLevel >= 2) return;
        pAttackSpeedLevel++;
    }

    public void PlayerMaxHPLevelUp()
    {
        if (pMaxHPLevel >= 2) return;
        pMaxHPLevel++;
    }

    public void PlayerDamageIncreaseLevelUp()
    {
        if (pDamageIncreaseLevel >= 2) return;
        pDamageIncreaseLevel++;
    }

    public void PlayerMagnetLevelUp()
    {
        if (pMagnetLevel >= 2) return;
        pMagnetLevel++;
    }

    /*
    public void SetPlayerAttackBonus(int amount)
    {
        pAttackBonus = amount;

        if (pAttackBonus < 0) pAttackBonus = 0;
    }

    public void SetPlayerSpeedBonus(float amount)
    {
        pSpeedBonus = amount;
    }
    */
}
