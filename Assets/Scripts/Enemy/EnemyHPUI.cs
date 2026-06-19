using UnityEngine;
using UnityEngine.UI;

public class EnemyHPUI : MonoBehaviour
{

    [SerializeField] private Image hpFillImage;
   
    public void UpdateHealthBar(int currentHp, int maxHP)
    {
        float fill = (float)currentHp / maxHP;

        if (hpFillImage != null)
        {
            hpFillImage.fillAmount = fill;
        }
    }
}
