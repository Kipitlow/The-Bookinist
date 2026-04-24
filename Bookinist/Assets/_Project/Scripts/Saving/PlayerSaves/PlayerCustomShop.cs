using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerCustomShop
{
    public List<string> placedItems = new List<string>();

    public void AddCleanID(Dictionary<FurnitureType, ShopItemData>[] activeItemList)
    {
        foreach (var dictionary in activeItemList)
        {
            foreach (var kvp in dictionary)
            {
                ShopItemData itemData = kvp.Value;

                if (placedItems.Contains(itemData.id))
                {
                    Debug.Log("Item Already added");
                    return;
                }
                placedItems.Add(itemData.id);
            }
        }
    }
}
