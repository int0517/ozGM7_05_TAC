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

    [SerializeField] private UI02_TestPlayerStats testPlayerStats;

    public void UpdateUI(PlayerStat playerStat)
    {
        if (playerStat == null)
        {
            Debug.LogWarning("PlayerStat이 없습니다."); //노란색 경고출력
            return;
        }

        scoreText.text = $"SCORE : {testPlayerStats.Score}";
        levelText.text = $"Lv. {playerStat.PLevel}";

        //현재 레벨 가져오기
        int moveLevel = playerStat.GetStatLvl(PlayerStatEnum.MoveSpeed);
        int attackSpeedLevel = playerStat.GetStatLvl(PlayerStatEnum.AttackSpeed);
        int damageLevel = playerStat.GetStatLvl(PlayerStatEnum.DamageIncrease);
        int magnetLevel = playerStat.GetStatLvl(PlayerStatEnum.MagnetRadius);

        // Dictionary에서 실제 값 가져오기
        float moveSpeed = PlayerStatDictionary.PlayerMoveSpeed[moveLevel];
        float attackSpeed = PlayerStatDictionary.PlayerAttackSpeed[attackSpeedLevel];
        float damage = PlayerStatDictionary.PlayerDamageIncrease[damageLevel];
        float magnet = PlayerStatDictionary.PlayerMagnetRadius[magnetLevel];

        attackText.text = $"공격력 : {damage}";
        attackSpeedText.text = $"공격속도 : {attackSpeed}";
        moveSpeedText.text = $"이동속도 : {moveSpeed}";
        maxHpText.text = $"HP : {playerStat.PCurrentHP}/{playerStat.PMaxHP}";
        magnetismText.text = $"자력 : {magnet}";
    }
}
