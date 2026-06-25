using TMPro;
using UnityEngine;

public class BossPosienTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private float currentTime = 3.0f;

    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;

            if (currentTime < 0) currentTime = 0;

            timerText.text = currentTime.ToString("F1");
        }
    }
}
