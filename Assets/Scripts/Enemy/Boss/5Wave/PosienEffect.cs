using UnityEngine;

public class PosienEffect : MonoBehaviour
{
    void Start()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
    }
}
