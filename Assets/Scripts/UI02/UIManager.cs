using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour //UI 열기/닫기 및 입력 처리 담당
{
    public static UIManager Instance { get; private set; }

    [Header("Panels")]
    [SerializeField] private PausePanel pausePanel;
    [SerializeField] private GameOverPanel gameOverPanel;
    [SerializeField] private QuitPanel quitPanel;
    [SerializeField] private TooltipPanel tooltipPanel;

    //게임오버를 위해 플레이어 스탯 받아오기
    private PlayerStat playerStat;
    private bool isGameOver;

    public enum GameState //규모 커지면 게임스테이지매니저 따로 만들기
    {
        Title,
        Playing,
        Paused,
        GameOver
    }

    public GameState CurrentState { get; private set; }

    public void SetState(GameState state)
    {
        CurrentState = state;
    }

    public bool IsGameplay => CurrentState == GameState.Playing;

    private void Awake()
    {
        CurrentState = GameState.Playing; //게임 씬 시작 시 Playing으로 고정

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
        if (!IsGameplay) return;

        CurrentState = GameState.Paused; //게임 상태 일시정지
        pausePanel.Open();
    }

    public void ClosePause()
    {
        CurrentState = GameState.Playing;
        pausePanel.Close();
        HideTooltip();
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
        CurrentState = GameState.GameOver;

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
    public void ShowTooltip(UI02_SkillSlots.SkillData skillData, int level)
    {
        tooltipPanel.ShowTooltip(skillData, level);
    }

    public void HideTooltip()
    {
        tooltipPanel.Close();
    }
}
