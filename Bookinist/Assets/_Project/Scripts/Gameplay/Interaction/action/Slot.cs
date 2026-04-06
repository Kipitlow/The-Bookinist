using UnityEngine;

/// <summary>
/// Représente un emplacement (slot) pouvant être rempli ou vidé par des items.
/// </summary>
public class Slot : MonoBehaviour
{
    #region Variables

    [SerializeField] private InventoryController _inventoryController;

    private bool _isEmpty;
    private Sprite _currentObject;

    #endregion

    #region Methods

    public bool IsEmpty() { return _isEmpty; }

    public void Fill()
    {
        if (gameObject == null) return;
        _currentObject = Instantiate(_inventoryController.activeItem.itemSprite);
        _isEmpty = true;
    }

    public void Clear()
    {
        if (_isEmpty || _currentObject == null) return;
        Destroy(_currentObject);
        _isEmpty = false;
    }

    #endregion
}
