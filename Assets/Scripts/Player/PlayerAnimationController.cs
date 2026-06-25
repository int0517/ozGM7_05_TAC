using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Transform playerTrans;
    private bool isFacingRight;

    private void Update()
    {
        bool shouldFaceRight = Input.mousePosition.x >= Screen.width * 0.5f;

        if (shouldFaceRight != isFacingRight)
        {
            isFacingRight = shouldFaceRight;

            transform.rotation = isFacingRight
                ? Quaternion.Euler(0, 180, 0)
                : Quaternion.Euler(0, 0, 0);
        }

        float posX = playerTrans.position.x + 0.055f;
        float posY = playerTrans.position.y - 0.7f;
        transform.position = new Vector3(posX, posY, 0f);
    }
}
