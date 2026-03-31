using UnityEngine;
using System.Collections.Generic;

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
        if (inventoryContents.Count < inventorySlots)
            inventoryContents.Add(itemToAdd);
    }

    #endregion
}
