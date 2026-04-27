using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInventory
{
    public List<string> playerInventoryOwnedItemIDs = new List<string> { "2", "5", "13", "8", "14", "20", "15" };

    public void AddCleanID(string IDToAdd)
    {
        if (playerInventoryOwnedItemIDs.Contains(IDToAdd))
        {
            Debug.Log("Item Already added");
            return;
        }
        else
        {
            playerInventoryOwnedItemIDs.Add(IDToAdd);
        }
    }
}
