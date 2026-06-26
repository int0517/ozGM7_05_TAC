using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Panels")]
    [SerializeField] private PausePanel pausePanel;
    [SerializeField] private GameOverPanel gameOverPanel;
    [SerializeField] private QuitPanel quitPanel;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (pausePanel.IsOpen) 
                ClosePause();
            else
                OpenPause();
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
}
