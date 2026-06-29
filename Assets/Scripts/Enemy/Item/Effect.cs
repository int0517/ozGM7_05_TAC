using UnityEngine;

public class Effect : MonoBehaviour
{
    void LateUpdate()
    {
        transform.localPosition = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }
}
