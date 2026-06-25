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
        // 이미 같은 레벨을 가지고 있으면 추가 안함
        if (HasSkill(skill.skillId, skill.skillLevel))
            return;

        ownedSkills.Add(skill);

        Debug.Log($"{skill.skillName} 획득");

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