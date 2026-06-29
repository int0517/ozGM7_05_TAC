using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Panels")]
    [SerializeField] private PausePanel pausePanel;
    [SerializeField] private GameOverPanel gameOverPanel;
    [SerializeField] private QuitPanel quitPanel;
    [SerializeField] private TooltipPanel tooltipPanel;

    //게임오버를 위해 플레이어 스탯 받아오기
    [SerializeField] private PlayerStat playerStat;
    private bool isGameOver;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        playerStat = FindFirstObjectByType<PlayerStat>();
    }
    
    //게임 시작 시 UI상태 끄기
    private void Start()
    {
        pausePanel.Close();
        gameOverPanel.Close();
        quitPanel.Close();
        tooltipPanel.Close();
    }
    private void Update()
    {
        if (playerStat == null) //나중으로 빼기
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (quitPanel.IsOpen)
            {
                CloseQuit();
            }
            else if (pausePanel.IsOpen)
            {
                ClosePause();
            }
            else
            {
                OpenPause();
            }
        }

        //게임오버테스트
        if (Input.GetKeyDown(KeyCode.G))
        {
            OpenGameOver();
        }

        if (playerStat != null && playerStat.PCurrentHP <= 0 && !isGameOver)
        {
            isGameOver = true;
            OpenGameOver();
        }
    }

    //Pause
    public void OpenPause()
    {
        pausePanel.Open();
    }

    public void ClosePause()
    {
        pausePanel.Close();
    }

    //GameOver
    public void OpenGameOver()
    {
        if (gameOverPanel.IsOpen) //여러 번 실행 방지
            return;

        StartCoroutine(GameOverRoutine());
    }
    private IEnumerator GameOverRoutine()
    {
        Time.timeScale = 0.2f;

        yield return new WaitForSecondsRealtime(1f); //현실 시간만큼 기다리기

        gameOverPanel.Open();
    }

    //Quit
    public void OpenQuit()
    {
        quitPanel.Open();
    }

    public void CloseQuit()
    {
        quitPanel.Close();
    }

    //ToolTip
    public void ShowTooltip(UI02_SkillSlots.SkillData skillData)
    {
        tooltipPanel.ShowTooltip(skillData);
    }

    public void HideTooltip()
    {
        tooltipPanel.Close();
    }
}
