using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class UI02_SkillSlots : MonoBehaviour
{
    [SerializeField] private List<Sprite> skillIcons;

    [SerializeField] private Image[] skillSlots; //°ø°£

    public void UpdateSkillsSlots()
    {
        if (skillIcons.Count == 0) return;
        if(skillSlots.Length == 0) return;
        
        for(int i = 0; i < skillIcons.Count && i < skillSlots.Length; i++)
        {
            skillSlots[i].sprite = skillIcons[i];
        }
    }

}
