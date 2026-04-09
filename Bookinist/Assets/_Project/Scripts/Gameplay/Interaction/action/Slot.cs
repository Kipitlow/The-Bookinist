using UnityEngine;

public class Slot : MonoBehaviour
{
    [SerializeField] private InventoryController _inventoryController;
    [SerializeField] private GameObject _prefabSprite;

    private bool _isEmpty = true;
    private GameObject _currentObject;

    public bool IsEmpty() { return _isEmpty; }

    private void Start()
    {
        //_prefabSprite = new GameObject("prefabsprite", typeof(SpriteRenderer));
    }

    public void Fill(GameObject prefab)
    {
        if (prefab == null) return;
        _currentObject = Instantiate(prefab, this.transform.position, this.transform.rotation, this.transform);
        _isEmpty = false;
    }

     public void FillWithSprite (Item item)
    {
        if (item == null) return;

        _currentObject = Instantiate(_prefabSprite, this.transform.position, this.transform.rotation, this.transform);
        _currentObject.GetComponent<SpriteRenderer>().sprite = item.itemSprite;
        _isEmpty = false;
    }

    public void Clear()
    {
        if (_isEmpty || _currentObject == null) return;
        Destroy(_currentObject);
        _isEmpty = true;
    }
}