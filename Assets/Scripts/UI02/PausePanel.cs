using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanel : StatPanel
{
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
}
