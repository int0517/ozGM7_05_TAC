using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Enemy.Manager
{
	public class TimerAndBossHpUI : MonoBehaviour
	{

        [SerializeField] private Image fillImage;

        public void UpdateTimerBar(int currentTimer, int timer)
        {
            fillImage.color = Color.white;
            float fill = (float)currentTimer / timer;
            Debug.Log($"전체 시간 : {timer}, 남은 시간 : {currentTimer}");
            if (fillImage != null)
            {
                fillImage.fillAmount = fill;
            }
        }
        public void UpdateBossHPBar(int currentHP, int MAXHP)
        {
            fillImage.color = Color.red;
            float fill = (float)currentHP / MAXHP;
            Debug.Log($"전체 체력 : {MAXHP}, 남은 체력 : {currentHP}");
            if (fillImage != null)
            {
                fillImage.fillAmount = fill;
            }
        }
    }
}