using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryModel : MonoBehaviour
{
    [SerializeField] private int _maxSlots = 3;
    private Dictionary<GameObject, SlotData> _slots = new Dictionary<GameObject, SlotData>();

    [SerializeField] private GameObject _slotPrefab;
    [SerializeField] private Transform _slotContainer;

    public event Action OnInventoryChanged;

    public int MaxSlots => _maxSlots;
    public Dictionary<GameObject, SlotData> Slots => _slots;
    public GameObject SlotPrefab => _slotPrefab;
    public Transform SlotContainer => _slotContainer;


    public void Initialize(GameObject newSlot)
    {
        _slots[newSlot] = new SlotData();
    }
    public void AddSlot(GameObject newSlot)
    {
        _slots[newSlot] = new SlotData();
    }

    public GameObject RemoveSlot()
    {
        foreach (var slot in _slots.Reverse())
        {
            if (!slot.Value.IsFilled)
            {
                _slots.Remove(slot.Key);
                return slot.Key;
            }
        }
        return null;
    }

    public GameObject GetEmptySlot()
    {
        foreach (var slot in _slots)
        {
            if (!slot.Value.IsFilled)
            {
                return slot.Key;
            }
        }
        return null;
    }

    public Transform AddItem(ItemModel item)
    {
        foreach (var slot in _slots)
        {
            if (!slot.Value.IsFilled)
            {
                slot.Value.SetItem(item);
                return slot.Key.transform;
            }
        }
        return null;
    }

    public void RemoveItem(ItemModel item)
    {
        if (item == null)
        {
            Debug.Log("Trying to remove null item");
            return;
        }

        foreach (var slot in _slots)
        {
            if (slot.Value.Item == item)
            {
                Debug.Log($"Removed item from slot {slot.Key}");
                slot.Value.ClearItem();
                break;
            } else
            {
                Debug.Log($"Nah from slot {slot.Key}");
            }
        }
    }
}
