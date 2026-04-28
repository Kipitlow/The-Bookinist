using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerQuests
{
    public List<int> playerQuestsDaily = new List<int> {};
    public List<int> playerQuestsHebdo = new List<int> {};
    public List<int> playerQuestsSeason = new List<int> {};

    /*
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
    */
}
