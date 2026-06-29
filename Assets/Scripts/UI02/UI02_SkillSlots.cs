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

        public SkillType skillType;
    }

    public enum SkillType
    {
        Default, //시작부터 보유 중
        Upgrade // 레벨업으로 획득
    }

    [SerializeField] private List<SkillData> skills;

    public void UpdateSkillsSlots()
    {
        if (skills.Count == 0) return;
        if(skillSlots.Length == 0) return;
        
        for(int i = 0; i < skills.Count && i < skillSlots.Length; i++)
        {
            skillSlots[i].sprite = skills[i].icon;

            UI02_SkillSlotInfo slot = skillSlots[i].GetComponent<UI02_SkillSlotInfo>(); //겟컴포넌트 비용이들기때문에 캐싱 어웨이크에서 캐싱 배열로 호출하고

            if (slot != null)
            {
                //SkillData를 직접 접근하지 않기 위해 SkillSlotInfo의 SetSkillData()를 통해 전달.
                slot.SetSkillData(skills[i]);
            }
        }
    }

    public void SetSkills(List<SkillData> newSkills)
    {
        skills = newSkills;

        UpdateSkillsSlots();
    }

}
