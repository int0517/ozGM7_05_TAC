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
    private UI02_SkillSlots skillSlotsUI; //스킬 슬롯 연결


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
        Debug.Log($"추가 시도 : {skill.skillName}");

        ownedSkills.Add(skill);

        Debug.Log($"{skill.skillName} 획득!");
        Debug.Log($"현재 개수 : {ownedSkills.Count}");

        UpdateOwnedSkillsUI();
    }
    private void UpdateOwnedSkillsUI()
    {
        skillSlotsUI.SetSkills(ownedSkills);
    }

}