using UnityEngine;

public class QuitPanel : UIPanel
{
    public void CancelExit()
    {
        UIManager.Instance.CloseQuit();
    }
}