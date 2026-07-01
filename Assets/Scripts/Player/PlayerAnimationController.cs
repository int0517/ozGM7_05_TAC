using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public enum PlayerAnimState
    {
        Idle, Walk, Hit, Die
    }
    private static readonly int StateHash = Animator.StringToHash("State");

    [SerializeField] private Transform playerTrans;
    [SerializeField] private PlayerStat pStat;
    [SerializeField] private Animator playerAnimator;
    private PlayerAnimState currentState = PlayerAnimState.Idle;
    private bool isFacingRight;

    private void Update()
    {
        SpriteRotate();

        AnimatorStateInfo info = playerAnimator.GetCurrentAnimatorStateInfo(0);

        //Debug.Log($"State:{info.shortNameHash} Time:{info.normalizedTime} Speed:{playerAnimator.speed}");

        Debug.Log(Animator.StringToHash("hit"));
    }

    private void SpriteRotate()
    {
        if (CombatHudController.isPaused || pStat.IsDead) return;
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

    public void SetState(PlayerAnimState newState)
    {
        if (currentState == newState) return;

        currentState = newState;
        playerAnimator.SetInteger(StateHash, (int)newState);
    }

    public void SetTrigger(string triggername)
    {
        playerAnimator.SetTrigger(triggername);
    }

    public PlayerAnimState GetCurrentState()
    {
        return (PlayerAnimState)playerAnimator.GetInteger(StateHash);
    }
}