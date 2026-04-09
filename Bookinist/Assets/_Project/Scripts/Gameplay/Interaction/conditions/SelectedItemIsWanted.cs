using UnityEngine;

public class SelectedItemIsWanted : MonoBehaviour
{
    [SerializeField] private InventoryController _inventoryController;
    public bool IsCorrectObject(Item objName)
    {
        if (objName == null) return false;
        //if (_inventoryController.activeItem.itemName == objName.itemName)
        //{
        //    return true;
        //}
        if (DragContext.DraggedItem.itemName == objName.itemName)
        {
            return true;
        }
        return false;
    }
}
