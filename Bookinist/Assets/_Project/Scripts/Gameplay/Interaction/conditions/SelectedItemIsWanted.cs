using UnityEngine;

/// <summary>
/// Vérifie si l'item sélectionné dans l'inventaire correspond au ScriptableObject attendu.
/// </summary>
public class SelectedItemIsWanted : MonoBehaviour
{
    #region Variables

    [SerializeField] private InventoryController _inventoryController;

    #endregion

    #region Methods

    public bool IsCorrectObject(Item objName)
    {
        if (objName == null) return false;
        if (_inventoryController == null) return false;
        return _inventoryController.activeItem.itemName == objName.itemName;
    }

    #endregion
}
