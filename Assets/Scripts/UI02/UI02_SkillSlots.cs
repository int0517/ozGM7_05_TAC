using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.Runtime.CompilerServices;

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

    //캐싱용 배열 추가
    private UI02_SkillSlotInfo[] slotInfos;

    //Awake에서 캐싱 
    private void Awake()
    {
        slotInfos = new UI02_SkillSlotInfo[skillSlots.Length];

        for(int i = 0; i <skillSlots.Length; i++)
        {
            slotInfos[i] = skillSlots[i].GetComponent<UI02_SkillSlotInfo>();
        }
    }

    public void UpdateSkillsSlots()
    {
        if (skills.Count == 0) return;
        if(skillSlots.Length == 0) return;
        
        for(int i = 0; i < skills.Count && i < skillSlots.Length; i++)
        {
            skillSlots[i].sprite = skills[i].icon;

            //UI02_SkillSlotInfo slot = skillSlots[i].GetComponent<UI02_SkillSlotInfo>(); //겟컴포넌트 비용이들기때문에 캐싱 어웨이크에서 캐싱 배열로 호출하기

            if (slotInfos[i] != null)
            {
                slotInfos[i].SetSkillData(skills[i]);
            }
        }
    }

    public void SetSkills(List<SkillData> newSkills)
    {
        skills = newSkills;

        UpdateSkillsSlots();
    }

}
