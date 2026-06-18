using UnityEngine;
using UnityEngine.SceneManagement;

public class UI02_ButtonsClickManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;

    private void Start()
    {
        pausePanel.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
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
        Application.Quit();
        Debug.Log("게임종료");
    }

    
}
