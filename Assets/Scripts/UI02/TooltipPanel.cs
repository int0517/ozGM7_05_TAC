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

        transform.position =
            Input.mousePosition + new Vector3(20, -20, 0); //»ìÂ¦ ¿·À¸·Î »©µÐ´Ù
    }

    public void ShowTooltip(UI02_SkillSlots.SkillData skillData)
    {
        if (skillData == null)
            return;

        skillNameText.text = skillData.skillName;
        descriptionText.text = skillData.description;

        Open(); 
    }
}
