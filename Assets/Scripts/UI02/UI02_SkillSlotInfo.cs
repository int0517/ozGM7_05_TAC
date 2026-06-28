using UnityEngine;
using UnityEngine.EventSystems;

public class UI02_SkillSlotInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UI02_SkillSlots.SkillData skillData;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.ShowTooltip(skillData);
    }
    public void OnPointerExit(PointerEventData eventData) 
    {
        UIManager.Instance.HideTooltip();
    }

    //외부에서 SkillData를 직접 수정하지 않고, 함수를 통해서만 전달받아 설정(캡슐화)
    public void SetSkillData(UI02_SkillSlots.SkillData data)
    {
        skillData = data;
    }
}
