using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class UI02_SkillSlots : MonoBehaviour
{
    //[SerializeField] private List<Sprite> skillIcons;

    [SerializeField] private Image[] skillSlots; //공간

    //
    [Serializable]
    public class SkillData
    {
        public string skillName;
        public string description;
        public Sprite icon;

        public int skillLevel;
        public int skillId;
        public int maxLevel = 5;

        public int unlockStage;
    }

    [SerializeField] private List<SkillData> skills;

    public void UpdateSkillsSlots()
    {
        if (skills.Count == 0) return;
        if(skillSlots.Length == 0) return;
        
        for(int i = 0; i < skills.Count && i < skillSlots.Length; i++)
        {
            skillSlots[i].sprite = skills[i].icon;

            UI02_SkillSlotInfo slot = skillSlots[i].GetComponent<UI02_SkillSlotInfo>();

            slot.skillData = skills[i];
        }
    }

    public void SetSkills(List<SkillData> newSkills)
    {
        skills = newSkills;

        UpdateSkillsSlots();
    }

}
