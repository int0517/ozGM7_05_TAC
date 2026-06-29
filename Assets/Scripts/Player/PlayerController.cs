using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using static PlayerAnimationController;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerAnimationController playerAnimationController;

    [SerializeField] private float moveSpeed = 5.0f;
    private Vector2 movement;
    private InputAction moveAction;
    private Rigidbody2D rb;
    private PlayerStat pStat;

    [Header("플레이어 이동 제한")]
    [SerializeField] private float posXMax = 27.25f;
    [SerializeField] private float posXMin = -28.25f;
    [SerializeField] private float posYMax = 17.75f;
    [SerializeField] private float posYMin = -17.75f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pStat = GetComponent<PlayerStat>();

        moveAction = InputSystem.actions?.FindAction("Move");
        moveAction?.Enable();
    }

    private void Update()
    {
        if (pStat.IsDead)
        {
            GetComponent<Collider2D>().enabled = false;
            GetComponent<PlayerMagnet>().enabled = false;
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Rotate();
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        if (pStat.IsDead) return;

        Move();
        MoveLimit();
    }

    private void Move()
    {
        if (rb == null)
        {
            return;
        }

        movement = ReadMovement();
        moveSpeed = PlayerStatDictionary.PlayerMoveSpeed[pStat.GetStatLvl(PlayerStatEnum.MoveSpeed)];
        rb.linearVelocity = new Vector2(movement.x * moveSpeed, movement.y * moveSpeed);
    }

    private void MoveLimit()
    {

        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, posXMin, posXMax);
        pos.y = Mathf.Clamp(pos.y, posYMin, posYMax);

        transform.position = pos;
    }

    private Vector2 ReadMovement()
    {
        if (moveAction != null)
        {
            Vector2 actionMovement = moveAction.ReadValue<Vector2>();
            if (actionMovement.sqrMagnitude > 0f)
            {
                return Vector2.ClampMagnitude(actionMovement, 1f);
            }
        }

        Keyboard keyboard = Keyboard.current;
        if (keyboard == null)
        {
            return Vector2.zero;
        }

        Vector2 keyboardMovement = Vector2.zero;
        keyboardMovement.x = ReadAxis(keyboard.aKey, keyboard.leftArrowKey, keyboard.dKey, keyboard.rightArrowKey);
        keyboardMovement.y = ReadAxis(keyboard.sKey, keyboard.downArrowKey, keyboard.wKey, keyboard.upArrowKey);

        return Vector2.ClampMagnitude(keyboardMovement, 1f);
    }

    private float ReadAxis(KeyControl negativeKey, KeyControl negativeAltKey, KeyControl positiveKey, KeyControl positiveAltKey)
    {
        float value = 0f;

        if (negativeKey.isPressed || negativeAltKey.isPressed)
        {
            value -= 1f;
        }

        if (positiveKey.isPressed || positiveAltKey.isPressed)
        {
            value += 1f;
        }

        return value;
    }

    private void Rotate()
    {
        if (Camera.main == null || Mouse.current == null) return;
        if (CombatHudController.isPaused || pStat.IsDead) return;


        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePos.z = 0f;

        Vector2 direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle + 90f);
    }

    private void UpdateAnimation()
    {
        if (CombatHudController.isPaused || pStat.IsDead) return;
        if (playerAnimationController.GetCurrentState() == PlayerAnimState.Hit) return;

        PlayerAnimationController.PlayerAnimState nextState;

        if (Mathf.Abs(movement.x) > 0.01f || Mathf.Abs(movement.y) > 0.01f)
        {
            nextState = PlayerAnimationController.PlayerAnimState.Walk;
        }
        else
        {
            nextState = PlayerAnimationController.PlayerAnimState.Idle;
        }
        
        playerAnimationController.SetState(nextState);
    }
}
