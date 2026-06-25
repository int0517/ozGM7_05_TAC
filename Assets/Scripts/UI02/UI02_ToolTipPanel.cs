using TMPro;
using UnityEngine;

public class UI02_ToolTipPanel : MonoBehaviour
{
    [SerializeField] private GameObject tooltipPanel;

    [SerializeField] private TMP_Text skillNameText;
    [SerializeField] private TMP_Text descriptionText;

    private void Update()
    {
        tooltipPanel.transform.position =
            Input.mousePosition + new Vector3(20, -20, 0); //»ìÂ¦ ¿·À¸·Î »©µÐ´Ù
    }

    public void ShowTooltip(UI02_SkillSlots.SkillData skillData)
    {
        tooltipPanel.SetActive(true);

        skillNameText.text = skillData.skillName;
        descriptionText.text = skillData.description;
    }
    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }
}
