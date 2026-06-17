using UnityEngine;
using UnityEngine.InputSystem;

public class InputManger : MonoBehaviour
{
    public static Vector2 Movement { get; private set; } = Vector2.zero;
    public static bool IsFire { get; private set; } = false;

    private InputAction moveAction;
    private InputAction fireAction;

    private void Awake()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        fireAction = InputSystem.actions.FindAction("Jump");
    }

    void Update()
    {
        Movement = moveAction.ReadValue<Vector2>();
        IsFire = fireAction.WasPressedThisFrame();
    }
}
