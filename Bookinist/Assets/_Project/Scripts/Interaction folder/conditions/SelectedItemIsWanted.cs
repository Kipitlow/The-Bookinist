using UnityEngine;

public class SelectedItemIsWanted : MonoBehaviour
{
    [SerializeField] private InventoryController _inventoryController;
    public bool IsCorrectObject(Item objName)
    {
        if (_inventoryController.activeItem.itemName == objName.itemName)
        {
            return true;
        }
        return false;
    }
}
