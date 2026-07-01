using UnityEngine;
using UnityEngine.UI;

public class TimerAndMonsterUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private RectTransform Handle;
    [SerializeField] TimerHeadEffect timerHeadEffect;

    private bool isTimerBar;
    public System.Action<bool> OnTimerStateChanged;
    private float lastHP = -1f;

    public void UpdateTimerBar(float currentTimer, float timer)
    {
        SetTimerState(true);

        fillImage.color = Color.white;
        float fill = currentTimer / timer;
        if (fillImage != null)
        {
            fillImage.fillAmount = fill;
        }
        UpdateHandle();
    }
    public void UpdateBossHPBar(float currentHP, float MAXHP)
    {
        SetTimerState(false);
        fillImage.color = Color.red;
        float fill = currentHP / MAXHP;
        if (fillImage != null)
        {
            fillImage.fillAmount = fill;
        }

        if (lastHP > currentHP)
        {
            timerHeadEffect.OnBossHit();
        }
        lastHP = currentHP;
        UpdateHandle();
    }
    private void SetTimerState(bool value)
    {
        if (isTimerBar == value) return;

        isTimerBar = value;
        Debug.Log("state change: " + value);
        OnTimerStateChanged?.Invoke(value);
    }

    private void UpdateHandle()
    {
        float t = fillImage.fillAmount;

        RectTransform parent = Handle.parent as RectTransform;

        float left = -parent.rect.width * 0.5f+30;
        float right = parent.rect.width * 0.5f-30;

        Vector2 pos = Handle.anchoredPosition;
        pos.x = Mathf.Lerp(left, right, t);

        Handle.anchoredPosition = pos;
    }
}
