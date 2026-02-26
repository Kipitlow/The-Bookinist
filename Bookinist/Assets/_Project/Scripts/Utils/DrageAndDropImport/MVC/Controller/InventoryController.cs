using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private InventoryModel _model;

    public InventoryModel Model => _model;

    private void Start()
    {
        for (int i = 0; i < _model.MaxSlots; i++)
        {
            AddSlot();
        }
    }

    public void AddSlot()
    {
        GameObject slotObj = Instantiate(_model.SlotPrefab, _model.SlotContainer);
        _model.AddSlot(slotObj);
    }

    public void RemoveSlot()
    {
        GameObject slotToRemove = _model.RemoveSlot();
        if (slotToRemove)
        {
            Destroy(slotToRemove);
        }
    }

    public void AddItem(GameObject item)
    {
        GameObject slotObj = _model.GetEmptySlot();

        if (slotObj != null)
        {
            GameObject newItem = Instantiate(item, slotObj.transform);
            ItemModel itemModel = newItem.GetComponent<ItemModel>();
            _model.Slots[slotObj].SetItem(itemModel);
        }
    }

    public void RemoveItem(ItemModel item)
    {
        _model.RemoveItem(item);
    }
}