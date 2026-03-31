using UnityEngine;
using System.Collections.Generic;

public class InventoryModel : MonoBehaviour
{
    #region Variables

    [SerializeField] private List<Item> inventoryContents;
    [SerializeField] private int inventorySlots;

    #endregion

    #region Unity Methods

    #endregion

    #region Methods

    public void AddItem(Item itemToAdd)
    {
        if (inventoryContents.Count < inventorySlots)
            inventoryContents.Add(itemToAdd);
    }

    public void RemoveItem(Item itemToRemove)
    {
        foreach (var item in inventoryContents)
        {
            if (item == itemToRemove)
            {
                inventoryContents.RemoveAt(GetItemIndex(itemToRemove));
                return;
            }
        }
    }

    public int GetItemIndex(Item checkItem)
    {
        int itemIndex = -1;
        foreach (var item in inventoryContents)
        {
            itemIndex++;
            if (item == checkItem)
                return itemIndex;
        }
        return -1;
    }

    public List<Item> GetInventoryContent()
    {
        return inventoryContents;
    }
    #endregion
}
