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
        public void UpdateBossHPBar(int currentHP, int MAXHP)
        {
            fillImage.color = Color.red;
            float fill = (float)currentHP / MAXHP;
            if (fillImage != null)
            {
                fillImage.fillAmount = fill;
            }
        }
    }
}