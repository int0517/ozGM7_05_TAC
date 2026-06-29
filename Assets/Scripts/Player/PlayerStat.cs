using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    // 플레이어 스탯
    private int pLevel = 1;
    private int pMaxHP = 3;
    [SerializeField] private int pCurrentHP;
    
    private float pAttackBonus = 1f;
    private float pSpeedBonus = 1f;
    

    // 플레이어 스탯 프로퍼티
    public int PLevel { get { return pLevel; } }
    public int PMaxHP { get { return pMaxHP; } }
    public int PCurrentHP { get { return pCurrentHP; } }
    
    public float PAttackBonus
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

    private float playerNonhitTimerMax = 1f;
    private float playerNonhitTimer = 0f;
    private bool canHit;
    public bool CanHit {  get { return canHit; } }

    [SerializeField] private PlayerAnimationController playerAnimationController;
    public bool isDead;

    private void Start()
    {
        pCurrentHP = pMaxHP;
        isDead = false;
        canHit = true;
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
        if (playerNonhitTimer < playerNonhitTimerMax || !canHit) return;

        pCurrentHP -= amount;
        playerNonhitTimer = 0f;

        if(pCurrentHP > 0)
        {
            playerAnimationController.SetState(PlayerAnimationController.PlayerAnimState.Hit);
        }
        else if (pCurrentHP <= 0)
        {
            pCurrentHP = 0;

            // 플레이어 사망 처리, 종료 화면
            if (!isDead)
            {
                playerAnimationController.SetState(PlayerAnimationController.PlayerAnimState.Die);
                playerAnimationController.SetTrigger("Dead");
                isDead = true;
            }
        }
    }

    public void HealPlayer(int amount)
    {
        pCurrentHP += amount;

        if (pCurrentHP > pMaxHP)
            pCurrentHP = pMaxHP;

        Debug.Log($"회복! 현재 체력 : {pCurrentHP}");
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

    public int GetStatLvl(PlayerStatEnum statEnum)
    {
        switch(statEnum)
        {
            case PlayerStatEnum.MoveSpeed:
                return pMoveSpeedLevel;
            case PlayerStatEnum.AttackSpeed:
                return pAttackSpeedLevel;
            case PlayerStatEnum.MaxHP:
                return pMaxHPLevel;
            case PlayerStatEnum.DamageIncrease:
                return pDamageIncreaseLevel;
            case PlayerStatEnum.MagnetRadius:
                return pMagnetLevel;
            default:
                return -1;
        }
    }
}
