using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    private Vector2 movement = Vector2.zero;
    [SerializeField] private float moveSpeed = 5.0f;

    private InputAction moveAction;
    private Rigidbody2D rb;
    private PlayerStat pStat;

    private void Awake()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        rb = GetComponent<Rigidbody2D>();
        pStat = GetComponent<PlayerStat>();
    }

    void Update()
    {
        Rotate();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        movement = moveAction.ReadValue<Vector2>();
        float moveX = movement.x;
        float moveY = movement.y;

        rb.linearVelocity = new Vector2(moveX * moveSpeed, moveY * moveSpeed * pStat.PSpeedBonus);
    }

    private void Rotate()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePos.z = 0f;

        Vector2 direction = mousePos - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle + 90f);
    }
}
