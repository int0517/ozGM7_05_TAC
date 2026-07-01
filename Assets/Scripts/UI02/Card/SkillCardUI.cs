using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillCardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("레벨업 UI")]
    [SerializeField] private LevelUpUI levelUpUI;
    [Header("카드 투명도")]
    [SerializeField] private CanvasGroup canvasGroup;
    [Header("등장 시작 위치")]
    [SerializeField] private float startOffset = -80.0f; //원래 위치보다 아래에서 나타난다
    [Header("마우스 오버 크기")]
    [SerializeField] private float hoveScale = 1.09f;

    [Header("FX")]
    [SerializeField] private ParticleSystem hoverFX;   // 마우스 올렸을 때
    [SerializeField] private ParticleSystem selectFX;  // 선택했을 때

    private RectTransform rectTransform;

    private Vector2 originalPosition;
    private Vector3 originalScale;
    private bool isClickable;

    //!! SkillCardUI에 SkillData 넣기 ====
    private UI02_SkillSlots.SkillData skillData;

    public void SetSkillData(UI02_SkillSlots.SkillData data)
    {
        skillData = data;

        if (skillData != null)
            UpdateUI();
    }

    [SerializeField] private TMP_Text skillNameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private UnityEngine.UI.Image icon;

    private void UpdateUI()
    {
        if (skillData == null) return;

        skillNameText.text = skillData.skillName;
        descriptionText.text = skillData.description;
        icon.sprite = skillData.icon;
    }
    //=======


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;

        originalScale = transform.localScale;
    }

    //카드를 숨기는 녀석
    public void HideInstant()
    {
        isClickable = false;
        //DOKill : 현재 오브젝트에서 실행중인 DoTween 애니메이션을 즉시 종료하는 녀석
        transform.DOKill();
        canvasGroup.DOKill();
        rectTransform.DOKill();
        canvasGroup.alpha = 0.0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        //카드를 원래 위치보다 아래쪽으로 이동시킨다.
        rectTransform.anchoredPosition = originalPosition + new Vector2(0.0f, startOffset);

        transform.localScale = Vector3.zero;
    }

    //카드 등장 애니메이션을 실행 
    public void PlayOpenTween()
    {
        isClickable = true;//카드 등장하면 클릭 가능한 상태
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;

        Sequence sequence = DOTween.Sequence();

        sequence.Join(canvasGroup.DOFade(1.0f, 0.25f).SetUpdate(true));//투명도를 0.25초동안 1로 만든다.
        sequence.Join(rectTransform.DOAnchorPos(originalPosition, 0.35f).SetEase(Ease.OutCubic).SetUpdate(true));
        sequence.Join(transform.DOScale(originalScale, 0.35f).SetEase(Ease.OutBack).SetUpdate(true));
    
    }
    //카드가 선택되었을때 실행할 녀석
    public Tween PlaySelectTween()
    {
        isClickable = false;
        transform.DOKill();

        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.DOScale(originalScale * 1.2f, 0.2f).SetEase(Ease.OutBack).SetUpdate(true));
        sequence.Append(transform.DOScale(originalScale, 0.15f).SetUpdate(true));

        return sequence;
    }
    //선택되지 않은 카드가 사라질 때
    public Tween PlayHideTween()
    {
        isClickable = false;
        transform.DOKill();
        canvasGroup.DOKill();

        Sequence sequence = DOTween.Sequence();

        sequence.Join(canvasGroup.DOFade(0.0f, 0.2f).SetUpdate(true));
        sequence.Join(transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack).SetUpdate(true));

        return sequence;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isClickable) return;

        transform.DOKill();

        transform.DOScale(originalScale * hoveScale, 0.15f).SetEase(Ease.OutQuad).SetUpdate(true);

        //호버 FX 재생
        if (hoverFX != null) hoverFX.Play();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isClickable) return;

        transform.DOKill();

        transform.DOScale(originalScale, 0.15f).SetEase(Ease.OutQuad).SetUpdate(true);

        // 호버 FX 끄기
        if (hoverFX != null) hoverFX.Stop();
    }

    //카드가 클릭되었을 때 Button의 OnClick에서 호출할 녀석
    public void OnClickCard()
    {
        if (!isClickable) return;
        levelUpUI.SelectCard(this);

        //선택 FX 재생
        if (selectFX != null) selectFX.Play();

        levelUpUI.SelectCard(this);
    }

    //!!
    public UI02_SkillSlots.SkillData GetSkillData()
    {
        return skillData;
    }
}
