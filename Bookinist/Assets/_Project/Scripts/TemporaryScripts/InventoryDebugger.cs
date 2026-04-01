using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryDebugger : MonoBehaviour
{
    [SerializeField] private InventoryController _inventoryController;
    [SerializeField] private Item CoolRock;
    [SerializeField] InputActionReference DebugAction;
    [SerializeField] InputActionReference DebugAction1;
    [SerializeField] private bool canDo = true;

    void OnEnable()
    {
        DebugAction.action.Enable();
        DebugAction1.action.Enable();
    }

    void OnDisable()
    {
        DebugAction.action.Disable();
        DebugAction1.action.Disable();
    }

    void Update()
    {
       
        if (DebugAction.action.IsPressed())
        {
            if (canDo)
            {
                _inventoryController.AddInventoryItem(CoolRock);
                StartCoroutine(retriggerCD());
            }
        }
        if (DebugAction1.action.IsPressed())
        {
            if (canDo)
            {
                _inventoryController.RemoveInventoryItem(CoolRock);
                StartCoroutine(retriggerCD());
            }
        }
    }

    private IEnumerator retriggerCD()
    {
        canDo = false;
        yield return new WaitForSeconds(0.1f);
        canDo = true;
    }
}
