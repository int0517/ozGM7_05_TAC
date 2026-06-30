using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOverPanel : StatPanel
{
    [SerializeField] private TMP_Text waveText;

    public override void Open()
    {
        base.Open();

        RefreshUI();

        Time.timeScale = 0f;

        //현재 진행 중인 웨이브 표시
        if (WaveTracker.Instance != null)
        {
            waveText.text = $"Wave {WaveTracker.Instance.CurrentWave}";

            //테스트!!
            Debug.Log(WaveTracker.Instance.CurrentWave);
        }
    }

    public void GoRetry()
    {
        //웨이브 초기화
        WaveTracker.Instance.ResetWave();

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
