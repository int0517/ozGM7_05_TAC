using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    void Update()
    {
        transform.position = new Vector3(target.position.x, target.position.y, target.position.z - 10f);
    }
}
