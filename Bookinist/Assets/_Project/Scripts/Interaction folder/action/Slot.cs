using Unity.VisualScripting;
using UnityEngine;

public class Slot : MonoBehaviour
{
    [SerializeField] private InventoryController _inventoryController;

    private bool _isEmpty;
    private GameObject _currentObject;


    public bool IsEmpty() { return _isEmpty; }

    public void Fill(GameObject prefab)
    {
        if (gameObject == null) return;
        _currentObject = Instantiate(prefab);
        _isEmpty = true;
    }

    public void Clear()
    {
        if (_isEmpty || _currentObject == null) return;
        Destroy(_currentObject);
        _isEmpty = false;
    }

}
