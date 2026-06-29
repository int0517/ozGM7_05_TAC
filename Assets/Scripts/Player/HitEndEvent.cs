using UnityEngine;
using static PlayerAnimationController;

public class HitEndEvent : MonoBehaviour
{
    [SerializeField] private PlayerAnimationController playerAnimationController;

    public void HitEnd()
    {
        playerAnimationController.SetState(PlayerAnimState.Idle);
    }
}
