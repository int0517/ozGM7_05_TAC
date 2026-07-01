using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    private Vector3 desPos;

    [SerializeField] float desPosXMax, desPosXMin, desPosYMax, desPosYMin;

    void Update()
    {
        desPos = new Vector3(target.position.x, target.position.y, target.position.z - 10f);

        if (desPos.x < desPosXMin) desPos.x = desPosXMin;
        if (desPos.x > desPosXMax) desPos.x = desPosXMax;
        if (desPos.y < desPosYMin) desPos.y = desPosYMin;
        if (desPos.y > desPosYMax) desPos.y = desPosYMax;

        transform.position = desPos;
    }
}
