using UnityEditor;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    #region Variables
    [SerializeField] private InventoryModel _inventoryModel;
    [SerializeField] private InventoryView _inventoryView;
    [SerializeField] private GameObject _activeItem;

    #endregion

    #region Unity Methods
    private void OnEnable()
    {
        ItemController.onItemClicked += HandleItemClicked;
    }

    private void OnDisable()
    {
        ItemController.onItemClicked -= HandleItemClicked;
    }

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

    private void HandleItemClicked(GameObject item)
    {
        Debug.Log($"Clicked: {item.name}");
        _activeItem = item;
    }

    #endregion
}
