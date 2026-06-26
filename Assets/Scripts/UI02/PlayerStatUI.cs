using TMPro;
using UnityEngine;

public class PlayerStatUI : MonoBehaviour
{
    [Header("Stat UI")]
    [SerializeField] protected TMP_Text scoreText;
    [SerializeField] protected TMP_Text levelText;
    [SerializeField] protected TMP_Text attackText;
    [SerializeField] protected TMP_Text attackSpeedText;
    [SerializeField] protected TMP_Text maxHpText;
    [SerializeField] protected TMP_Text moveSpeedText;
    [SerializeField] protected TMP_Text magnetismText;

    public void UpdateUI(PlayerStat playerStat)
    {
        if (playerStat == null)
        {
            Debug.LogWarning("PlayerStat이 없습니다.");
            return;
        }

        scoreText.text = $"SCORE : ";
        levelText.text = $"Lv. ";

        attackText.text = $"공격력 : {playerStat.PAttackBonus}";
        attackSpeedText.text = "-";
        moveSpeedText.text = $"이동속도 : {playerStat.PSpeedBonus:F1}";
        maxHpText.text = $"HP : {playerStat.PCurrentHP}/{playerStat.PMaxHP}";
        magnetismText.text = "-";
    }
}
