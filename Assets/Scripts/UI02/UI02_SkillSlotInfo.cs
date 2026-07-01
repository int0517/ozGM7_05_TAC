using UnityEngine;
using UnityEngine.EventSystems;

public class UI02_SkillSlotInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //��ų �⺻ ����
    private UI02_SkillSlots.SkillData skillData;

    //���� ���� ��
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

    //�ܺο��� SkillData�� ���� �������� �ʰ�, �Լ��� ���ؼ��� ���޹޾� ����(ĸ��ȭ)
    public void SetSkillData(UI02_SkillSlots.SkillData data, int level)
    {
        skillData = data; //��ų �⺻ ���� ����
        skillLevel = level; //���� ���� �� ����
    }
}
