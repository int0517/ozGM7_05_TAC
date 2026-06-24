using UnityEngine;
using UnityEngine.UI;

public class EnemyHPUI : MonoBehaviour
{

    [SerializeField] private Image hpFillImage;
   
    public void UpdateHealthBar(float currentHp, float maxHP)
    {
        float fill = currentHp / maxHP;

        if (hpFillImage != null)
        {
            hpFillImage.fillAmount = fill;
        }
    }
}
