using UnityEngine;

public class Slot : MonoBehaviour
{
    [SerializeField] private InventoryController _inventoryController;

    private bool _isEmpty = true;
    private GameObject _currentObject;

    public bool IsEmpty() { return _isEmpty; }

    public void Fill(GameObject prefab)
    {
        if (prefab == null) return;
        _currentObject = Instantiate(prefab, this.transform.position,
                                     this.transform.rotation, this.transform);
        _isEmpty = false;
    }

    public void Clear()
    {
        if (_isEmpty || _currentObject == null) return;
        Destroy(_currentObject);
        _isEmpty = true;
    }
}