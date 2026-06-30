using UnityEngine;

public class WaveTracker : MonoBehaviour //현재 웨이브 저장
{
    public static WaveTracker Instance;

    public int CurrentWave { get; private set; } = 1;

    private void Awake()
    {
        Instance = this;
    }
    //테스트!!
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetWave(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SetWave(5);
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            SetWave(9);
        }
    }

    //게임 재시작 시 1웨이브로 초기화
    public void ResetWave()
    {
        CurrentWave = 1;
    }

    //특정 웨이브로 설정
    public void SetWave(int wave)
    {
        CurrentWave = wave;
    }
}
