using UnityEngine;
using UnityEngine.SceneManagement;

public class UI02_ButtonsClickManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject quitPanel;

    private void Start()
    {
        pausePanel.SetActive(false);
        quitPanel.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (quitPanel.activeSelf)
                return;
            if (pausePanel.activeSelf)
                GoResume(); //activeSelf : 오브젝트가 SetActive로 켜져 있는가?
            else
                GameStop();
        }
    }
    private void GameStop()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0.0f;

    }
    public void GoResume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1.0f;
    }
    public void GoTitle()
    {
        SceneManager.LoadScene("UI02TestTitleScene");
        Time.timeScale = 1.0f;
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


}
