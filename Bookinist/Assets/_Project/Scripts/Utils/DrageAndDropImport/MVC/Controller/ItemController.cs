using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class ItemController : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private ItemModel _model;
    [SerializeField] private InventoryController _inventoryController;

    public void Start()
    {
        _inventoryController = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryController>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_model.IsDragging)
        {
            var itemModel = GetComponent<ItemModel>();
            _inventoryController.RemoveItem(itemModel);
            Destroy(gameObject);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _model.SetDragging(true);
        _model.SetOriginalParent(transform.parent);

    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Mouse.current.position.ReadValue();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _model.SetDragging(false);

        GameObject closestSlot = null;
        float minDistance = 35f;

        foreach (var slot in _inventoryController.Model.Slots)
        {
            float distance = Vector3.Distance(transform.position, slot.Key.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                closestSlot = slot.Key;
            }
        }

        if (closestSlot != null)
        {
            if (!_inventoryController.Model.Slots[closestSlot].IsFilled)
            {
                _inventoryController.Model.Slots[_model.OriginalParent.gameObject].ClearItem();
                transform.SetParent(closestSlot.transform);
                transform.localPosition = Vector3.zero;
                _inventoryController.Model.Slots[closestSlot].SetItem(_model);
            }
            else
            {
                ItemModel selectedItem = _model;
                ItemModel otherItem = _inventoryController.Model.Slots[closestSlot].Item;

                transform.SetParent(closestSlot.transform);
                transform.localPosition = Vector3.zero;

                otherItem.gameObject.transform.SetParent(_model.OriginalParent);
                otherItem.gameObject.transform.localPosition = Vector3.zero;

                _inventoryController.Model.Slots[closestSlot].SetItem(selectedItem);
                _inventoryController.Model.Slots[_model.OriginalParent.gameObject].SetItem(otherItem);
            }
        } else
        {
            transform.SetParent(_model.OriginalParent);
            transform.localPosition = Vector3.zero;
        }
    }
}