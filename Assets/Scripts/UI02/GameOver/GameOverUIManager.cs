using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject quitPanel;

    //스킬 슬롯
    [SerializeField] private UI02_SkillSlots skillSlots;

    //플레이어 정보
    [SerializeField] private PlayerStat playerStat;

    [Header("스탯 UI")]
    [SerializeField] private TMP_Text scoreText;

    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text attackText;
    [SerializeField] private TMP_Text attackSpeedText;
    [SerializeField] private TMP_Text maxHpText;
    [SerializeField] private TMP_Text moveSpeedText;
    [SerializeField] private TMP_Text magnetismText;

    private bool isGameOver = false;

    private void Awake()
    {
        gameOverPanel.SetActive(false);
        quitPanel.SetActive(false);

        playerStat = FindFirstObjectByType<PlayerStat>();

    }

    private void Update()
    {
        if (playerStat == null) return;

        if (Input.GetKeyDown(KeyCode.G)) //테스트
        {
            StartCoroutine(GameOver());
        }
        //if (!isGameOver && playerStat.PCurrentHP <= 0)
        //{
        //    isGameOver = true;
        //    StartCoroutine(GameOver());
        //}
    }

    private IEnumerator GameOver() //게임오버 느낌을 주고 싶어서 느리게 연출 
    {
        Time.timeScale = 0.2f;

        yield return new WaitForSecondsRealtime(1f);

        skillSlots.UpdateSkillsSlots(); 

        UpdatePlayerStatUI(); 

        gameOverPanel.SetActive(true);

        Time.timeScale = 0f;

        //버튼 애니메이션 같은 걸 넣을 예정이면 Animator Update Mode = Unscaled Time 설정이 필요할 수 있다.

    }

    public void GoRetry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //지금 플레이 중인 씬의 빌드 인덱스를 받아 다시 시작한다.
    }

    public void GoTitle()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("TitleScene");
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
        Debug.Log("게임 종료");
    }

    //플레이어 스탯 추가
    private void UpdatePlayerStatUI()
    {
        if (!isGameOver && Input.GetKeyDown(KeyCode.G))
        {
            isGameOver = true;
            StartCoroutine(GameOver());
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
