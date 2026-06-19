using UnityEngine;
using UnityEngine.UI;

public class EnemyHPUI : MonoBehaviour
{

    [SerializeField] private Image hpFillImage;
   
    public void UpdateHealthBar(int currentHp, int maxHP)
    {
        float fill = (float)currentHp / maxHP;
        Debug.Log($"현재 체력: {currentHp}, 최대 체력: {maxHP}, 결과값: {fill}"); // <--- 이 로그가 뜨는지 확인!

        if (hpFillImage != null)
        {
            hpFillImage.fillAmount = fill;
        }
    }
}
