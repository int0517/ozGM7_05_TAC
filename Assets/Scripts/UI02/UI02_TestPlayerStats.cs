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

    [Header("보유 스킬 (테스트)")] //보유 여부만
    [SerializeField]
    private List<UI02_SkillSlots.SkillData> ownedSkills =
    new List<UI02_SkillSlots.SkillData>();

    //플레이어 스킬 레벨 가져오기
    [SerializeField] private PlayerSkillLevel playerSkillLevel;

    //안전하게 가져가기 위한 함수
    public int GetSkillLevel(int skillId)
    {
        // PlayerSkillLevel 내부 enum으로 변환해서 실제 레벨 반환
        return playerSkillLevel.GetSkillLvl((PlayerStatEnum)skillId);
    }

    //스킬 획득
    public void AddSkill(UI02_SkillSlots.SkillData skill)
    {
        //이미 같은 계열의 스킬을 가지고 있는지 확인
        for (int i = 0; i < ownedSkills.Count; i++)
        {
            if (ownedSkills[i].skillId == skill.skillId)
            {
                // 같은 계열 스킬이면 더 높은 레벨로 교체
                //ownedSkills[i] = skill; 
                //이미 보유 중이면 추가x
                Debug.Log($"{skill.skillName} 이미 보유 중");

                return;
            }
        }

        // 처음 얻는 경우만 리스트에 추가
        ownedSkills.Add(skill);

        Debug.Log($"{skill.skillName} 최초 획득");
    }
   
    //보유중인 스킬 레벨 확인
    public bool HasSkill(int skillId, int level)
    {
        foreach (var skill in ownedSkills)
        {
            if (skill.skillId == skillId)
            {
                //PlayerSkillLevel에서 실제 레벨 가져오기
                int currentLevel =
                    playerSkillLevel.GetSkillLvl((PlayerStatEnum)skill.skillId);

                //요구 레벨 이상인지 확인
                return currentLevel >= level;
            }
        }

        return false;
    }

    //보유 스킬 목록 반환 및 갱신
    public List<UI02_SkillSlots.SkillData> GetOwnedSkills()
    {
        return ownedSkills;
    }

    //테스트용! 스코어 찾기 플레이어 위치 기반 코인 감지
    private int score = 0;
    public int Score => score;

    private Transform player;

    private void Start()
    {
        //플레이어 태그 찾기
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void AddScore(int value)
    {
        score += value;
        Debug.Log("Score: " + score);
    }
    private void Update()
    {
        //테스트용! P 누르면 점수 증가
        if (Input.GetKeyDown(KeyCode.P))
        {
            AddScore(1);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Coin 충돌 시 점수 증가
        if (other.CompareTag("Coin"))
        {
            AddScore(1);
        }
    }
} 