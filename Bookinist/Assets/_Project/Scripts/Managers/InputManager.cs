using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private TouchDetection touchDetection;

    private InputSystem_Actions _inputActions;

    private void Awake()
    {
        _inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        _inputActions.LevelNav.DragPress.performed += OnPress;
        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.LevelNav.DragPress.performed -= OnPress;
        _inputActions.Disable();
    }

    private void OnPress(InputAction.CallbackContext context)
    {
        Vector2 screenPosition = _inputActions.LevelNav.Position.ReadValue<Vector2>();
        touchDetection.OnTouch(screenPosition);
    }
}