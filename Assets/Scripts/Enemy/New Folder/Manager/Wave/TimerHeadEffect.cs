using UnityEngine;
using DG.Tweening;

public class TimerHeadEffect : MonoBehaviour
{
    [SerializeField] private RectTransform timerImage;
    [SerializeField] private RectTransform bossImage;
    private RectTransform target;
    [SerializeField] TimerAndMonsterUI timerUI;
    private Tween beatTween;
    private Sequence hitSeq;
    void Start()
    {
        timerUI.OnTimerStateChanged += OnTimerChanged;
        target = timerImage;
        StartBeat();
    }

    void OnTimerChanged(bool isTimer)
    {
        if (isTimer)
        {
            if(bossImage!=null)
            {
                bossImage.DOScale(0.2f, 0.15f).OnComplete(() => bossImage.gameObject.SetActive(false));
            }
            timerImage.gameObject.SetActive(true);
            timerImage.localScale = Vector3.one * 0.2f;

            timerImage.DOScale(1f, 0.2f);

            target = timerImage;

        }
        else
        {
            if (timerImage!=null)
            {
                timerImage.DOScale(0.2f, 0.15f)
           .OnComplete(() => timerImage.gameObject.SetActive(false));
            }
            bossImage.gameObject.SetActive(true);
            bossImage.localScale = Vector3.one * 0.2f;

            bossImage.DOScale(1f, 0.2f);

            target = bossImage;
        }
        StartBeat();
    }

    void StartBeat()
    {
        StopAllTweens();
        target.localScale = Vector3.one;

        target.DOScale(1.05f, 0.6f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    public void OnBossHit()
    {

        StopAllTweens();

        Sequence seq = DOTween.Sequence();

        for (int i = 0; i < 3; i++)
        {
            seq.Append(target.DOScale(2f, 0.08f));
            seq.Append(target.DOScale(1f, 0.08f));
        }
        seq.OnComplete(() =>
        {
            StartBeat();
        });
    }
    void StopAllTweens()
    {
        beatTween?.Kill();
        hitSeq?.Kill();

        if (timerImage != null) timerImage.DOKill();
        if (bossImage != null) bossImage.DOKill();
    }
}