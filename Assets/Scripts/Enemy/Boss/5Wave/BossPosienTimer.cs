using UnityEngine;
using TMPro;

public class BossPosienTimer : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public float timeRemaining = 3.0f; // 경고 시간과 동일하게 설정

    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            // 소수점 1자리까지 표시 (예: 2.9, 2.8...)
            countdownText.text = timeRemaining.ToString("F1");
        }
        else
        {
            countdownText.text = "0.0";
        }
    }
}
