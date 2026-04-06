using UnityEngine;

/// <summary>
/// Composant responsable du retrait d'un item de l'inventaire (Drop).
/// </summary>
public class DropItem : MonoBehaviour
{
    #region Variables

    [SerializeField] private InventoryController _inventoryController;
    [SerializeField] private Item _itemSelf;

    #endregion

    #region Unity Methods

    private void Start()
    {
        _inventoryController = FindFirstObjectByType<InventoryController>();
        _itemSelf = GetComponentInParent<ItemModel>().itemScriptable;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Supprime cet item de l'inventaire via l'InventoryController.
    /// </summary>
    public void DropSelf()
    {
        _inventoryController.RemoveInventoryItem(_itemSelf);
    }

    #endregion
}
