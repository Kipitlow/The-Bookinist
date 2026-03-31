using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;

public class Inventory : MonoBehaviour
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
        foreach (var item in inventoryContents)
        {
            if (item == itemToAdd)
            {
                getItemIndex(itemToAdd);
                inventoryContents[0].itemQuantity++;
            }
            if (inventoryContents.Count < inventorySlots)
                inventoryContents.Add(itemToAdd);
        }
    }

    public int getItemIndex(Item checkItem)
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
    #endregion
}
