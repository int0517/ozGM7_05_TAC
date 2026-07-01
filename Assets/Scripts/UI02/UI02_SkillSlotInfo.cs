using UnityEngine;
using UnityEngine.EventSystems;

public class UI02_SkillSlotInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //스킬 기본 데이터
    private UI02_SkillSlots.SkillData skillData;
    //현재 스킬 레벨
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
    //외부에서 SkillData를 직접 노출하지 않고, 함수를 통해서만 전달받아 저장(캡슐화)
    public void SetSkillData(UI02_SkillSlots.SkillData data, int level)
    {
        Debug.Log($"SetSkillData 호출됨: {data?.skillName}, 레벨: {level}"); // 테스트로그
        skillData = data; //스킬 기본 데이터 저장
        skillLevel = level; //현재 스킬 레벨 저장
    }
}