using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI02_TestPlayerStats : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text attackText;
    [SerializeField] private TMP_Text attackSpeed;
    [SerializeField] private TMP_Text maxHpText;
    [SerializeField] private TMP_Text moveSpeed;
    [SerializeField] private TMP_Text magemtism;

    [Header("Test Data")]
    [SerializeField] private int testScore = 12345;
    [SerializeField] private int testAttack = 25;
    [SerializeField] private int testAttackSpeed = 7;
    [SerializeField] private int testMaxHp = 100;
    [SerializeField] private int testMoveSpeed = 7;
    [SerializeField] private int testMagemtism = 30;

    [Header("보유 스킬")]
    [SerializeField]
    private List<UI02_SkillSlots.SkillData> ownedSkills =
    new List<UI02_SkillSlots.SkillData>();

    [SerializeField]
    private UI02_SkillSlots skillSlotsUI;


    private void Start()
    {
        ShowStatUI();
    }

    private void ShowStatUI()
    {
        scoreText.text = $"SCORE : {testScore}";

        attackText.text = $"ATK : {testAttack}";
        attackSpeed.text = $"ATK SPD : {testAttackSpeed}";
        maxHpText.text = $"MAX HP : {testMaxHp}";
        moveSpeed.text = $"MOVE SPD : {testMoveSpeed}";
        magemtism.text = $"MAGNETISM : {testMagemtism}";
    }

    public void AddSkill(UI02_SkillSlots.SkillData skill)
    {
        for (int i = 0; i < ownedSkills.Count; i++)
        {
            if (ownedSkills[i].skillId == skill.skillId)
            {
                // 같은 계열 스킬이면 교체
                ownedSkills[i] = skill;

                Debug.Log($"{skill.skillName} 획득");
                UpdateOwnedSkillsUI();
                return;
            }
        }

        // 처음 얻는 경우
        ownedSkills.Add(skill);

        Debug.Log($"{skill.skillName} 최초 획득");
        UpdateOwnedSkillsUI();

    }
    private void UpdateOwnedSkillsUI()
    {
        skillSlotsUI.SetSkills(ownedSkills);
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
} 