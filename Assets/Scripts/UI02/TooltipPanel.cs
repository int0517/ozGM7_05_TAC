using TMPro;
using UnityEngine;

public class TooltipPanel : UIPanel
{
    [SerializeField] private TMP_Text skillNameText;
    [SerializeField] private TMP_Text descriptionText;

    private void Update()
    {
        if (!IsOpen)
            return;

        //마우스 위치 기준으로 오프셋
        transform.position =
            Input.mousePosition + new Vector3(20, -20, 0); //살짝 옆으로 빼둔다
    }

    public void ShowTooltip(UI02_SkillSlots.SkillData skillData, int skillLevel)
    {
        if (skillData == null)
            return;
        //스킬표시 
        skillNameText.text = skillData.skillName;
        descriptionText.text =
            $"{skillData.description}\n\nLevel: {skillLevel} / {skillData.maxLevel}";
        
        Open(); 
    }
}
