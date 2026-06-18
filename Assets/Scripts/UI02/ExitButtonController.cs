using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButtonController : MonoBehaviour
{
   [SerializeField] private GameObject pausePanel;

    private bool isEsc = false;

    private void Start()
    {
        pausePanel.SetActive(false);
    }
    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            if (pausePanel.activeSelf) Resume(); //activeSelf : 오브젝트가 SetActive로 켜져 있는가?
        }
    }
    public void GameStop()
    {
        
       
    }
    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
