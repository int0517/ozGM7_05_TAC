using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using static UI02_SkillSlots;

public class LevelUpUI : MonoBehaviour
{
    [Header("패널")]
    //레벨업 UI전체 패널오브젝트
    [SerializeField] private GameObject levelUpPanel;
    [Header("투명도")]
    [SerializeField] private CanvasGroup panleCanvasGroup;
    [Header("타이틀")]
    [SerializeField] private RectTransform levelUpText;
    [Header("스킬카드")]
    [SerializeField] private SkillCardUI[] skillCards;

    //패널이 열려있냐?
    private bool isOpen;

    //카드 선택용
    private bool isSelected;

    //!! 전체 스킬 풀
    [SerializeField] private List<UI02_SkillSlots.SkillData> allSkills;

    //!! 이번에 뽑힌 카드
    private List<UI02_SkillSlots.SkillData> currentCards;

    //!! 플레이어 연결
    [SerializeField]
    private UI02_TestPlayerStats playerStats;

    //!!!
    [SerializeField] private int currentStage;
    void Start()
    {
        CloseInstant();
    }

    public void Open()
    {
        if (isOpen) return;


        isOpen = true;//열린 상태로 변경
        isSelected = false; //아직 어떤 카드도 선택하지 않은 상태임.
        levelUpPanel.SetActive(true);//레벨업 패널 오브젝트 활성화
        panleCanvasGroup.alpha = 0.0f;
        panleCanvasGroup.blocksRaycasts = true;
        panleCanvasGroup.interactable = true;

        //패널에 전체 투명도로 0.25초동안 1로 만든다
        //화면이 자연스럽게 나타나는 페이드 인 효과임.
        panleCanvasGroup.DOFade(1.0f, 0.25f)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                //Time.timeScale = 0f; // 게임만 멈춤
            });

        //!!
        GenerateCards();

        //레벨업 타이틀 등장 애니메이션
        PlayTitleTween();
        //스킬 카드 3개 동작 애니메이션
        PlayCardOpenTween();
    }
    private void PlayTitleTween()
    {
        levelUpText.localScale = Vector3.zero;
        //타이틀 크기를 0.4초 동안 원래 크기로 키운다.
        //Ease.OutBack는 살짝 튕기듯 커지는 느낌을 준다.
        levelUpText.DOScale(Vector3.one, 0.4f).SetEase(Ease.OutBack).SetUpdate(true);
    }
    private void PlayCardOpenTween()
    {
        //우선 모든 카드를 먼저 숨기자.

        for (int i = 0; i < skillCards.Length; i++)
        {
            skillCards[i].HideInstant();

            if (i < currentCards.Count)
            {
                skillCards[i].SetSkillData(currentCards[i]);
            }
        }

        //카드 등장 순서를 제어하기 위한 시퀀스를 만든다.
        //시퀀스를 사용하면 여러 트윈이나 콜백을 순서대로 실행할 수 있음.
        Sequence sequence = DOTween.Sequence().SetUpdate(true);

        for (int i = 0; i < skillCards.Length; i++)
        {
            int index = i;

            //현재 순서의 카드 등장 애니메이션을 실행한다.
            //AppendCallback 트윈은 아니지만 시퀀스 중간에 함수를 실행할 수 있게 해준다.
            sequence.AppendCallback(() =>
            {
                skillCards[index].PlayOpenTween();
            });

            //다음 카드가 등하기 전까지 0.12초 기다린다.
            //이 간격때문에 카드가 하나씩 순서대로 등장하는 느낌을 볼 수 있음
            sequence.AppendInterval(0.12f);
        }
        //!
        sequence.OnComplete(() =>
        {
            DOVirtual.DelayedCall(0.4f, () =>
            {
                Time.timeScale = 0f;
            }).SetUpdate(true);
        });
    }
    //카드가 클릭 되었을 때
    public void SelectCard(SkillCardUI selectCard)
    {
        if (isSelected) return;

        //!! 실제 저장
        playerStats.AddSkill(selectCard.GetSkillData());

        isSelected = true;
        Sequence sequence = DOTween.Sequence().SetUpdate(true);

        for (int i = 0; i < skillCards.Length; i++)
        {
            SkillCardUI card = skillCards[i];//배열에서 현재 카드를 가져오고

            if (card == selectCard)
            {
                sequence.Join(card.PlaySelectTween());
            }
            else
            {
                sequence.Join(card.PlayHideTween());
            }
        }
        sequence.AppendInterval(0.5f);

        sequence.OnComplete(() =>
        {
            Close();
        });
    }
    public void Close()
    {
        Time.timeScale = 1.0f;

        panleCanvasGroup.DOFade(0.0f, 0.25f)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                CloseInstant();
            });
    }
    private void CloseInstant()
    {
        isOpen = false;//패널이 닫힌 상태
        isSelected = false;//카드 선택 상태 초기화
        levelUpPanel.SetActive(false);//오브젝트 비활성화
        panleCanvasGroup.alpha = 0.0f;//패널 투명도를 0으로
        panleCanvasGroup.blocksRaycasts = false;//패널이 마우스 입력을 받지 않게 하자.
        panleCanvasGroup.interactable = false;//패널안의 UI상호작용을 막자.

        //여기서 카드도 숨긴 상태로 초기화하자
    }


    //!! 카드 랜덤 생성 함수 추가
    private void GenerateCards()
    {
        currentCards = new List<SkillData>();

        List<SkillData> tempPool = allSkills.FindAll(CanShowSkill);

        for (int i = 0; i < skillCards.Length; i++)
        {
            if (tempPool.Count == 0) break;

            int randomIndex = Random.Range(0, tempPool.Count);

            SkillData selected = tempPool[randomIndex];

            currentCards.Add(selected);
            tempPool.RemoveAt(randomIndex);
        }
    }

    //!!! 카드 등장 조건
    private bool CanShowSkill(SkillData skill)
    {
        if (currentStage < skill.unlockStage)
            return false;

        if (skill.skillLevel == 1)
        {
            return !playerStats.HasSkill(skill.skillId, 1);
        }

        return playerStats.HasSkill(skill.skillId, skill.skillLevel - 1)
            && !playerStats.HasSkill(skill.skillId, skill.skillLevel);
    }
}
