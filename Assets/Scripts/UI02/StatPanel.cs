using TMPro;
using UnityEngine;

public class StatPanel : UIPanel
{
    [SerializeField] protected UI02_SkillSlots skillSlots;
    [SerializeField] protected PlayerStatUI playerStatUI;

    protected PlayerStat playerStat;

    protected virtual void Awake()
    {
        playerStat = FindFirstObjectByType<PlayerStat>();
    }
    // 스킬 슬롯과 플레이어 스탯 UI를 최신 정보로 갱신
    protected void RefreshUI()
    {
        skillSlots.UpdateSkillsSlots(); 
        playerStatUI.UpdateUI(playerStat); 
    }
}
