using TMPro;
using UnityEngine;

public class PausePanel : UIPanel
{
    [SerializeField] private UI02_SkillSlots skillSlots;

    private PlayerStat playerStat;

    [Header("스탯 UI")]
    [SerializeField] private TMP_Text scoreText;

    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text attackText;
    [SerializeField] private TMP_Text attackSpeedText;
    [SerializeField] private TMP_Text maxHpText;
    [SerializeField] private TMP_Text moveSpeedText;
    [SerializeField] private TMP_Text magnetismText;

    private void Start()
    {
        playerStat = FindFirstObjectByType<PlayerStat>();
    }

    private void UpdatePlayerStatUI()
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

    public override void Open()
    {
        base.Open();

        skillSlots.UpdateSkillsSlots();

        UpdatePlayerStatUI();

        Time.timeScale = 0f;
    }

    public override void Close()
    {
        base.Close();

        Time.timeScale = 1f;
    }
}
