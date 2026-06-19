using UnityEngine;
using UnityEngine.EventSystems;

public class UI02_SkillSlotInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UI02_SkillSlots.SkillData skillData;
    [SerializeField] private UI02_ToolTipPanel tooltip;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.ShowTooltip(skillData);
    }
    public void OnPointerExit(PointerEventData eventData) 
    {
        tooltip.HideTooltip();
    }
}
