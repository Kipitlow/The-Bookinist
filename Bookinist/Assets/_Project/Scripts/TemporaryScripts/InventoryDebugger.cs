using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryDebugger : MonoBehaviour
{
    [SerializeField] private Inventory inventoryManager;
    [SerializeField] private Item CoolRock;

    void Update()
    {
        inventoryManager.AddItem(CoolRock);
    }
}
