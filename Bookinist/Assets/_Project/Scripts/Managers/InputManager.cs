using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Pont entre le nouvel Input System et TouchDetection.
/// </summary>
public class InputManager : MonoBehaviour
{
    #region Variables

    [SerializeField] private TouchDetection _touchDetection;
    private InputSystem_Actions _inputActions;

    #endregion

    #region Unity Methods

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

    #endregion

    #region Methods

    private void OnPress(InputAction.CallbackContext context)
    {
        Vector2 screenPosition = _inputActions.LevelNav.Position.ReadValue<Vector2>();
        _touchDetection.OnTouch(screenPosition);
    }

    #endregion
}