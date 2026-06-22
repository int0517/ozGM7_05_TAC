using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillCardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("������ UI")]
    [SerializeField] private LevelUpUI levelUpUI;
    [Header("ī�� ������")]
    [SerializeField] private CanvasGroup canvasGroup;
    [Header("���� ���� ��ġ")]
    [SerializeField] private float startOffset = -80.0f; 
    [Header("���콺 ���� ũ��")]
    [SerializeField] private float hoveScale = 1.09f;

    private RectTransform rectTransform;

    private Vector2 originalPosition;
    private Vector3 originalScale;
    private bool isClickable;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;

        originalScale = transform.localScale;
    }

    public void HideInstant()
    {
        isClickable = false;
        transform.DOKill();
        canvasGroup.DOKill();
        rectTransform.DOKill();
        canvasGroup.alpha = 0.0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        rectTransform.anchoredPosition = originalPosition + new Vector2(0.0f, startOffset);

        transform.localScale = Vector3.zero;
    }

    public void PlayOpenTween()
    {
        isClickable = true;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;

        Sequence sequence = DOTween.Sequence();

        sequence.Join(canvasGroup.DOFade(1.0f, 0.25f));
        sequence.Join(rectTransform.DOAnchorPos(originalPosition, 0.35f).SetEase(Ease.OutCubic));
        sequence.Join(transform.DOScale(originalScale, 0.35f).SetEase(Ease.OutBack));
    }
    public Tween PlaySelectTween()
    {
        isClickable = false;
        transform.DOKill();

        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.DOScale(originalScale * 1.2f, 0.2f).SetEase(Ease.OutBack));
        sequence.Append(transform.DOScale(originalScale, 0.15f));

        return sequence;
    }
    public Tween PlayHideTween()
    {
        isClickable = false;
        transform.DOKill();
        canvasGroup.DOKill();

        Sequence sequence = DOTween.Sequence();

        sequence.Join(canvasGroup.DOFade(0.0f, 0.2f));
        sequence.Join(transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack));

        return sequence;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isClickable) return;

        transform.DOKill();

        transform.DOScale(originalScale * hoveScale, 0.15f).SetEase(Ease.OutQuad);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isClickable) return;

        transform.DOKill();

        transform.DOScale(originalScale, 0.15f).SetEase(Ease.OutQuad);
    }

    public void OnClickCard()
    {
        if (!isClickable) return;
        levelUpUI.SelectCard(this);
    }
}
