using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI02_TestPlayerStats : MonoBehaviour
{
    //[SerializeField] private TMP_Text scoreText;
    //[SerializeField] private TMP_Text attackText;
    //[SerializeField] private TMP_Text attackSpeed;
    //[SerializeField] private TMP_Text maxHpText;
    //[SerializeField] private TMP_Text moveSpeed;
    //[SerializeField] private TMP_Text magemtism;

    //[Header("Test Data")]
    //[SerializeField] private int testScore = 12345;
    //[SerializeField] private int testAttack = 25;
    //[SerializeField] private int testAttackSpeed = 7;
    //[SerializeField] private int testMaxHp = 100;
    //[SerializeField] private int testMoveSpeed = 7;
    //[SerializeField] private int testMagemtism = 30

    [Header("보유 스킬 (테스트)")]
    [SerializeField]
    private List<UI02_SkillSlots.SkillData> ownedSkills =
    new List<UI02_SkillSlots.SkillData>();

    //스킬 획득
    public void AddSkill(UI02_SkillSlots.SkillData skill)
    {
        //이미 같은 계열의 스킬을 가지고 있는지 확인
        for (int i = 0; i < ownedSkills.Count; i++)
        {
            if (ownedSkills[i].skillId == skill.skillId)
            {
                // 같은 계열 스킬이면 더 높은 레벨로 교체
                ownedSkills[i] = skill;

                Debug.Log($"{skill.skillName} 획득");
                return;
            }
        }

        // 처음 얻는 경우 추가
        ownedSkills.Add(skill);

        Debug.Log($"{skill.skillName} 최초 획득");
    }
   
    //보유중인 스킬 레벨 확인
    public bool HasSkill(int skillId, int level)
    {
        foreach (var skill in ownedSkills)
        {
            if (skill.skillId == skillId &&
                skill.skillLevel >= level)
            {
                return true;
            }
        }

        return false;
    }

    //내가 가지고 잇는 스킬 목록 반환 및 갱신
    public List<UI02_SkillSlots.SkillData> GetOwnedSkills()
    {
        return ownedSkills;
    }
} 