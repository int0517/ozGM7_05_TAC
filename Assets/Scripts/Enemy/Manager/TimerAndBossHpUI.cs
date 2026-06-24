using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Enemy.Manager
{
	public class TimerAndBossHpUI : MonoBehaviour
	{

        [SerializeField] private Image fillImage;

        public void UpdateTimerBar(float currentTimer, float timer)
        {
            fillImage.color = Color.white;
            float fill = currentTimer / timer;
            if (fillImage != null)
            {
                fillImage.fillAmount = fill;
            }
        }
        public void UpdateBossHPBar(float currentHP, float MAXHP)
        {
            fillImage.color = Color.red;
            float fill = currentHP / MAXHP;
            if (fillImage != null)
            {
                fillImage.fillAmount = fill;
            }
        }
    }
}