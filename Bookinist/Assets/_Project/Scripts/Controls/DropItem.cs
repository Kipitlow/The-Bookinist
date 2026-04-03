using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField] private InventoryController _inventoryController;
    [SerializeField] private Item _itemSelf;

    private void Start()
    {
        _inventoryController = FindFirstObjectByType<InventoryController>();
        _itemSelf = GetComponentInParent<ItemModel>().itemScriptable;
    }

    public void DropSelf()
    {
        _inventoryController.RemoveInventoryItem(_itemSelf);
    }
}
