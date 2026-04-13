using UnityEngine;

public class SelectedItemIsWanted : MonoBehaviour
{
    [SerializeField] private InventoryController _inventoryController;
    public bool IsCorrectObject(Item objName)
    {
        Debug.Log("[CONDITION] Item 1 : " + objName.itemName + ", and : " + _inventoryController.activeItem.itemName + " are equal to : " + (objName.itemName == _inventoryController.activeItem.itemName).ToString());
        if (objName == null) return false;
        //if (DragContext.DraggedItem == null) return false; // pas de drag en cours
        //Debug.Log($"[IsCorrectObject] Item dragged : ' {DragContext.DraggedItem.itemName}'");
        //return DragContext.DraggedItem.itemName == objName.itemName;
        return objName.itemName == _inventoryController.activeItem.itemName;
    }
}
