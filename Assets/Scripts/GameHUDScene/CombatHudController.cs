using UnityEngine;
using UnityEngine.UI;

public class CombatHudController : MonoBehaviour
{
    [SerializeField] private Text waveText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text pauseButtonText;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button pauseButton;

    private int wave = 1;
    private int score;
    public static bool isPaused;

    private void Awake()
    {
        waveText ??= GameObject.Find("WaveText")?.GetComponent<Text>();
        scoreText ??= GameObject.Find("ScoreText")?.GetComponent<Text>();
        pauseButtonText ??= GameObject.Find("PauseButtonText")?.GetComponent<Text>();
        pausePanel ??= GameObject.Find("PausePanel");
        pauseButton ??= GameObject.Find("PauseButton")?.GetComponent<Button>();

        if (pauseButton != null)
        {
            pauseButton.onClick.RemoveListener(TogglePause);
            pauseButton.onClick.AddListener(TogglePause);
        }

        SetPaused(false);
        Refresh();
    }

    public void SetWave(int newWave)
    {
        wave = Mathf.Max(1, newWave);
        Refresh();
    }

    public void SetScore(int newScore)
    {
        score = Mathf.Max(0, newScore);
        Refresh();
    }

    public void AddScore(int amount)
    {
        SetScore(score + amount);
    }

    public void TogglePause()
    {
        SetPaused(!isPaused);
    }

    private void SetPaused(bool paused)
    {
        isPaused = paused;
        Time.timeScale = isPaused ? 0f : 1f;

        if (pausePanel != null)
        {
            pausePanel.SetActive(isPaused);
        }

        if (pauseButtonText != null)
        {
            pauseButtonText.text = isPaused ? ">" : "||";
        }
    }

    private void Refresh()
    {
        if (waveText != null)
        {
            waveText.text = $"{wave} Wave";
        }

        if (scoreText != null)
        {
            scoreText.text = $"Score {score}";
        }
    }
}
