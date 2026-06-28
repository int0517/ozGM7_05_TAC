using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class GameOverPanel : StatPanel
{
    public override void Open()
    {
        base.Open();

        RefreshUI();

        Time.timeScale = 0f;
    }

    public void GoRetry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScene");
    }

    public void GoExit()
    {
        UIManager.Instance.OpenQuit();
    }
}
