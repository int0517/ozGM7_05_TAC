using UnityEngine;

public class SubtleCameraDrift : MonoBehaviour
{
    [SerializeField] private Vector2 driftAmount = new Vector2(0.8f, 0.45f);
    [SerializeField] private float driftSpeed = 0.45f;

    private Vector3 startPosition;

    private void Awake()
    {
        startPosition = transform.position;
    }

    private void OnEnable()
    {
        startPosition = transform.position;
    }

    private void LateUpdate()
    {
        float time = Time.time * driftSpeed;
        Vector3 offset = new Vector3(
            Mathf.Sin(time) * driftAmount.x,
            Mathf.Sin(time * 0.73f + 1.4f) * driftAmount.y,
            0f
        );

        transform.position = startPosition + offset;
    }
}
