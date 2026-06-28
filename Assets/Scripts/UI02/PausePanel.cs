using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanel : StatPanel
{
    public bool IsOpen => gameObject.activeSelf; //식 본문 프로퍼티(Expression-bodied Property) -> 한 줄짜리 getter

    public override void Open()
    {
        base.Open();

        RefreshUI();

        Time.timeScale = 0f;
    }

    public override void Close()
    {
        base.Close();

        Time.timeScale = 1f;
    }

    public void GoResume()
    {
        Close();
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
