using UnityEngine;

public class QuitPanel : UIPanel
{
    public void CancelExit()
    {
        UIManager.Instance.CloseQuit();
    }

    public void ConfirmExit()
    {
        Application.Quit();

        Debug.Log("게임 종료");
    }
}