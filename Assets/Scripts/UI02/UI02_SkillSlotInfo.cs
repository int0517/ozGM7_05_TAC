using UnityEngine;
using UnityEngine.EventSystems;

public class UI02_SkillSlotInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UI02_SkillSlots.SkillData skillData;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (skillData == null) return;
        if (UIManager.Instance == null) return;
        UIManager.Instance.ShowTooltip(skillData);
    }
    public void OnPointerExit(PointerEventData eventData) 
    {
        UIManager.Instance.HideTooltip();
    }

    //�ܺο��� SkillData�� ���� �������� �ʰ�, �Լ��� ���ؼ��� ���޹޾� ����(ĸ��ȭ)
    public void SetSkillData(UI02_SkillSlots.SkillData data)
    {
        skillData = data;
    }
}
