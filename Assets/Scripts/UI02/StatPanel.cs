using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StatPanel : UIPanel //플레이어 스탯 / 스킬 UI 갱신 담당
{
    [SerializeField] protected UI02_SkillSlots skillSlots;
    [SerializeField] protected PlayerStatUI playerStatUI;

    [SerializeField] protected PlayerStat playerStat;

    //플레이어 테스트 스탯
    protected UI02_TestPlayerStats testPlayerStats;

    [SerializeField] private string titleSceneName = "TitleScene";

    protected override void Awake()
    {
        base.Awake();

        playerStat = FindFirstObjectByType<PlayerStat>();
        testPlayerStats = FindFirstObjectByType<UI02_TestPlayerStats>();
    }

    // 스킬 슬롯과 플레이어 스탯 UI를 최신 정보로 갱신
    protected void RefreshUI()
    {
        // 플레이어가 보유한 스킬을 슬롯에 전달
        if (testPlayerStats != null && skillSlots != null)
        {
            skillSlots.SetSkills(testPlayerStats.GetOwnedSkills());
        }

        // 플레이어 스탯 UI 갱신
        if (playerStatUI != null && playerStat != null)
        {
            playerStatUI.UpdateUI(playerStat);
        }
    }
    public void GoTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(titleSceneName);
    }

    public void GoExit()
    {
        Time.timeScale = 0f;

        UIManager.Instance.OpenQuit();
    }
}
