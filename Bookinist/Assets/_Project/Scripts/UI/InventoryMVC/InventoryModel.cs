using UnityEngine;
using System.Collections.Generic;

public class InventoryModel : MonoBehaviour
{
    #region Variables

    [SerializeField] private List<Item> _inventoryContents;
    [SerializeField] private int _inventorySlots;

    #endregion

    #region Unity Methods

    #endregion

    #region Methods

    public void AddItem(Item itemToAdd)
    {
        if (IsInventoryFull())
            _inventoryContents.Add(itemToAdd);
    }

    public void RemoveItem(Item itemToRemove)
    {
        foreach (var item in _inventoryContents)
        {
            if (item == itemToRemove)
            {
                _inventoryContents.RemoveAt(GetItemIndex(itemToRemove));
                return;
            }
        }
    }

    public int GetItemIndex(Item checkItem)
    {
        int itemIndex = -1;
        foreach (var item in _inventoryContents)
        {
            itemIndex++;
            if (item == checkItem)
                return itemIndex;
        }
        return -1;
    }

    public List<Item> GetInventoryContent()
    {
        return _inventoryContents;
    }

    public bool IsInventoryHasAPlace()
    {
        return _inventoryContents.Count < _inventorySlots;

    }

    public bool IsInventoryFull()
    {
        return _inventorySlots >= _inventoryContents.Count ;
    }
    #endregion
}
