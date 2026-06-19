using UnityEngine;

public class SpiderWebVisual : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private int webPoints = 10; // 점을 늘리면 더 부드러운 곡선이 됩니다
    [SerializeField] private float waveAmplitude = 0.2f; // 흔들리는 폭
    [SerializeField] private float waveSpeed = 5f; // 흔들리는 속도

    private Transform playerTransform;

    void Start()
    {
        // 플레이어 찾기
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) playerTransform = playerObj.transform;

        // 라인 설정
        lineRenderer.positionCount = webPoints;
        lineRenderer.useWorldSpace = true; // 중요!
    }

    void Update()
    {
        if (playerTransform == null) return;

        UpdateWebVisuals();
    }

    void UpdateWebVisuals()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = playerTransform.position;

        for (int i = 0; i < webPoints; i++)
        {
            float t = (float)i / (webPoints - 1);

            // 시작점(보스)과 끝점(플레이어) 사이를 보간
            Vector3 pos = Vector3.Lerp(startPos, endPos, t);

            // 중간 점들만 사인파로 흔들기 (양 끝점은 0.0이라서 안 흔들림)
            float sineOffset = Mathf.Sin(Time.time * waveSpeed + i) * waveAmplitude * (t * (1 - t) * 4);
            pos.y += sineOffset;
            pos.z = 0; // Z축 고정

            lineRenderer.SetPosition(i, pos);
        }
    }
}