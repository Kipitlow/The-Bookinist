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
        if (_inventoryContents.Count < _inventorySlots)
            _inventoryContents.Add(itemToAdd);
    }

    public void RemoveItem(Item itemToRemove)
    {
        int index = GetItemIndex(itemToRemove);
        if (index >= 0)
            _inventoryContents.RemoveAt(index);
    }

    public int GetItemIndex(Item checkItem)
    {
        for (int i = 0; i < _inventoryContents.Count; i++)
            if (_inventoryContents[i] == checkItem)
                return i;
        return -1;
    }

    public List<Item> GetInventoryContent() => _inventoryContents;

    #endregion
}
