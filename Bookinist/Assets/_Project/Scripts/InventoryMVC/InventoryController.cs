using UnityEngine;

public class InventoryController : MonoBehaviour
{
    #region Variables
    [SerializeField] private InventoryModel _inventoryModel;
    [SerializeField] private InventoryView _inventoryView;

    #endregion

    #region Unity Methods

    #endregion

    #region Methods

    public void AddInventoryItem(Item ItemToAdd)
    {
        _inventoryModel.AddItem(ItemToAdd);
        UpdateInventory();
    }
    public void RemoveInventoryItem(Item ItemToAdd)
    {
        _inventoryModel.RemoveItem(ItemToAdd);
        UpdateInventory();
    }

    public void UpdateInventory()
    {
        _inventoryView.UpdateInventory(_inventoryModel.GetInventoryContent());
    }

    #endregion
}
