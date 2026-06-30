using UnityEngine;
using UnityEngine.EventSystems;

public class UI02_SkillSlotInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //스킬 기본 정보
    private UI02_SkillSlots.SkillData skillData;

    //실제 레벨 값
    private int skillLevel;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (skillData == null) return;
        if (UIManager.Instance == null) return;
        
        UIManager.Instance.ShowTooltip(skillData, skillLevel);
    }
    public void OnPointerExit(PointerEventData eventData) 
    {
        UIManager.Instance.HideTooltip();
    }

    //외부에서 SkillData를 직접 수정하지 않고, 함수를 통해서만 전달받아 설정(캡슐화)
    public void SetSkillData(UI02_SkillSlots.SkillData data, int level)
    {
        skillData = data; //스킬 기본 정보 저장
        skillLevel = level; //실제 레벨 값 저장
    }
}
