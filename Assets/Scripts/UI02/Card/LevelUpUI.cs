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

    //!! 이번에 뽑힌 카드
    private List<UI02_SkillSlots.SkillData> currentCards;

    //!! 플레이어 연결
    [SerializeField]
    private UI02_TestPlayerStats playerStats;

    //현재 웨이브
    private int currentWave = 1;
    public int CurrentWave => currentWave;
    public void NextWave()
    {
        currentWave++;
    }

    [Header("액티브 스킬")]
    [SerializeField]
    private List<SkillData> activeSkills;

    [Header("패시브 스킬")]
    [SerializeField]
    private List<SkillData> passiveSkills;

    //액티브 + 패시브 스킬을 하나로 관리하기 위한 리스트
    private List<SkillData> allSkills = new();


    //남은 레벨업 횟수
    private int remainLevelUpCount;

    private void Start()
    {
        // 액티브/패시브 스킬을 하나의 리스트로 합침
        allSkills.AddRange(activeSkills);
        allSkills.AddRange(passiveSkills);

        Debug.Log($"전체 스킬 개수 : {allSkills.Count}");

        GiveDefaultSkills(); //기본 보유 스킬 지급
        CloseInstant(); // 레벨업 UI 초기화
    }

    public void Open()
    {
        if (isOpen) return;
        Debug.Log($"allSkills: {allSkills.Count}");
        Debug.Log($"active: {activeSkills.Count}, passive: {passiveSkills.Count}");
        Debug.Log($"stage: {currentWave}");

        if (playerStats == null)
        {
            Debug.LogError("playerStats 연결 안 됨" );
            return;
        }

        //!!
        GenerateCards();

        if (currentCards.Count == 0) //남은 스킬 0개면 리턴
        {
            Debug.Log("선택 가능한 스킬이 없습니다!");
            return;
        }


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

        //레벨업 타이틀 등장 애니메이션
        PlayTitleTween();
        //스킬 카드 3개 동작 애니메이션
        PlayCardOpenTween();
    }

    //레벨업 시작 함수 / 일반 웨이브는 1, 보스 웨이브는 2를 전달받는다.
    public void StartLevelUp(int count)
    {
        remainLevelUpCount = count;
        Open();
    }

    //웨이브 종료 이벤트 구독 
    private void OnEnable()
    {
        WaveManager.OnWaveEnded += HandleWaveEnded;
    }

    //오브젝트 꺼질 때 구독 해제 (중복 호출 방지)
    private void OnDisable()
    {
        WaveManager.OnWaveEnded -= HandleWaveEnded;
    }

    //웨이브 끝났다는 신호 받으면 실행
    private void HandleWaveEnded(bool wasBossWave)
    {
        NextWave();
        StartLevelUp(wasBossWave ? 2 : 1);
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
                skillCards[i].gameObject.SetActive(true);
                skillCards[i].SetSkillData(currentCards[i]);
            }
            else
            {
                skillCards[i].gameObject.SetActive(false);
            }
        }

        //카드 등장 순서를 제어하기 위한 시퀀스를 만든다.
        //시퀀스를 사용하면 여러 트윈이나 콜백을 순서대로 실행할 수 있음.
        Sequence sequence = DOTween.Sequence().SetUpdate(true);

        for (int i = 0; i < currentCards.Count; i++)
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
        //이미 카드 선택했으면 추가 입력 방지
        if (isSelected) return;

        // 실제 스킬 저장
        playerStats.AddSkill(selectCard.GetSkillData());

        isSelected = true;
        
        //카드 선택 / 나머지 카드 숨기는 애니메이션 처리
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
            //레벨업 1회 완료
            remainLevelUpCount--;

            //아직 남은 레벨업이 있다면 
            if (remainLevelUpCount > 0)
            {
                //UI를 다지 않고 카드만 새로 생성
                RefreshCard();
            }
            //남은 레벨업이 없다면 
            else
            {
                //UI 종료
                Close();
            }
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

        if (allSkills == null || allSkills.Count == 0)
        {
            Debug.LogError("allSkills 비어있음");
            return;
        }

        List<SkillData> tempPool = allSkills.FindAll(CanShowSkill);

        for (int i = 0; i < skillCards.Length; i++)
        {
            if (tempPool.Count == 0)
                break;

            int randomIndex = Random.Range(0, tempPool.Count);

            SkillData selected = tempPool[randomIndex];

            currentCards.Add(selected);

            tempPool.RemoveAt(randomIndex);
        }
    }

    //!!! 카드 등장 조건 bool
    private bool CanShowSkill(SkillData skill)
    {
        //기본 보유 스킬은 카드에 나오지 않게 함
        if (skill.skillType == SkillType.Default)
            return false;

        //스테이지 제한
        if (currentWave < skill.unlockStage)
            return false;

        // 이미 최대 레벨이면 제외
        int currentLevel = playerStats.GetSkillLevel(skill.skillId);

        return currentLevel < skill.maxLevel;
    }

    //기본 보유 스킬 지급 -> ownedSkills로 들어가게 하기
    private void GiveDefaultSkills()
    {
        foreach (SkillData skill in allSkills)
        {
            if (skill.skillType == SkillType.Default)
            {
                playerStats.AddSkill(skill);
            }
        }
    }
 
    //보스 웨이브 때 연속 레벨업을 위해 레벨업UI 유지하여 새로운 카드만 생성
    private void RefreshCard()
    {
        //새로운 카드 3개 다시 생성
        GenerateCards();
        //뽑을 카드가 없다면 UI종료
        if(currentCards.Count== 0)
        {
            Close();
            return;
        }
        //카드 등장 애니메이션 다시 실행
        PlayCardOpenTween();
    }
}
