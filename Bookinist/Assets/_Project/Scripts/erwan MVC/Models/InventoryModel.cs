using System.Collections.Generic;
using UnityEngine;
public class InventoryModel
{
    private readonly List<SlotModel> _slots = new();

    public List<SlotModel> Slots => _slots;

    public InventoryModel(int slotCount)
    {
        for (int i = 0; i < slotCount; i++)
            _slots.Add(new SlotModel());
    }

    public bool TryAddItem(ItemData data, int amount = 1)
    {
        if (data == null || amount <= 0)
            return false;

        // 1. remplir les stacks existants
        if (data.stackable)
        {
            foreach (var slot in _slots)
            {
                while (amount > 0 && slot.CanStack(data))
                {
                    slot.AddOne();
                    amount--;
                }
            }
        }

        // 2. utiliser les slots vides
        foreach (var slot in _slots)
        {
            if (!slot.IsEmpty)
                continue;

            int amountToPlace = data.stackable ? Mathf.Min(amount, data.maxStack) : 1;
            slot.Set(data, amountToPlace);
            amount -= amountToPlace;

            if (amount <= 0)
                return true;
        }

        return amount <= 0;
    }

    public bool TryTakeOneFromSlot(int slotIndex, out ItemData takenItem)
    {
        takenItem = null;

        if (slotIndex < 0 || slotIndex >= _slots.Count)
            return false;

        var slot = _slots[slotIndex];
        if (slot.IsEmpty)
            return false;

        takenItem = slot.ItemData;
        slot.RemoveOne();
        return true;
    }
}