using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStopUIManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject quitPanel;

    //��ų ����
    [SerializeField] private UI02_SkillSlots skillSlots;


    //플레이어 스탯 받기
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
        pausePanel.SetActive(false);
        quitPanel.SetActive(false);

        playerStat = FindFirstObjectByType<PlayerStat>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (quitPanel.activeSelf)
                return;
            if (pausePanel.activeSelf)
                GoResume(); //activeSelf : ������Ʈ�� SetActive�� ���� �ִ°�?
            else
                GameStop();
        }
    }
    private void GameStop()
    {
        pausePanel.SetActive(true);
        skillSlots.UpdateSkillsSlots();

        UpdatePlayerStatUI();

        Time.timeScale = 0f;

    }
    public void GoResume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }
    public void GoTitle()
    {
        SceneManager.LoadScene("TitleScene");
        Time.timeScale = 1f;
    }
    public void GoExit()
    {
        quitPanel.SetActive(true);
    }
    public void CancelExit()
    {
        quitPanel.SetActive(false);
    }
    public void ConfirmExit()
    {
        Application.Quit();
        Debug.Log("���� ����");
    }

    //�÷��̾� ���� �߰�
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
}
