using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerController : MonoBehaviour
{
    private static Sprite fallbackSquareSprite;

    [SerializeField] private float moveSpeed = 5.0f;

    private InputAction moveAction;
    private Rigidbody2D rb;
    private PlayerStat pStat;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pStat = GetComponent<PlayerStat>();

        EnsureVisibleFallbackSprite();

        moveAction = InputSystem.actions?.FindAction("Move");
        moveAction?.Enable();
    }

    private void Update()
    {
        Rotate();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void EnsureVisibleFallbackSprite()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            return;
        }

        if (fallbackSquareSprite == null)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.filterMode = FilterMode.Point;
            texture.SetPixel(0, 0, new Color(0.15f, 0.45f, 1f, 1f));
            texture.Apply();

            fallbackSquareSprite = Sprite.Create(
                texture,
                new Rect(0f, 0f, 1f, 1f),
                new Vector2(0.5f, 0.5f),
                1f
            );
            fallbackSquareSprite.name = "PlayerFallbackSquare";
        }

        spriteRenderer.sprite = fallbackSquareSprite;
        spriteRenderer.sharedMaterial = null;
        spriteRenderer.color = new Color(0.15f, 0.45f, 1f, 1f);
        spriteRenderer.sortingOrder = Mathf.Max(spriteRenderer.sortingOrder, 10);
    }

    private void Move()
    {
        if (rb == null)
        {
            return;
        }

        Vector2 movement = ReadMovement();
        float speedBonus = pStat != null ? pStat.PSpeedBonus : 1f;
        rb.linearVelocity = new Vector2(movement.x * moveSpeed, movement.y * moveSpeed * speedBonus);
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
        if (Camera.main == null || Mouse.current == null)
        {
            return;
        }

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePos.z = 0f;

        Vector2 direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle + 90f);
    }
}
