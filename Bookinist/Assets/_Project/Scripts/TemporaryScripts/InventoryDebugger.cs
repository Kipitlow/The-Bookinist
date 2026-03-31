using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryDebugger : MonoBehaviour
{
    [SerializeField] private Inventory inventoryManager;
    [SerializeField] private Item CoolRock;
    [SerializeField] InputActionReference DebugAction;

    void OnEnable()
    {
        DebugAction.action.Enable();
    }

    void OnDisable()
    {
        DebugAction.action.Disable();
    }

    void Update()
    {

        bool pressed = DebugAction.action.IsPressed();

        if (pressed)
        {
            Debug.Log("A pressed");
            inventoryManager.AddItem(CoolRock);
        }
    }
}
