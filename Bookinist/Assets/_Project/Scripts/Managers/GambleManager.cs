using UnityEngine;
using System.Collections.Generic;

public class GambleManager : MonoBehaviour
{
    [SerializeField] private LootPools _itemsUnlockable;
    [SerializeField] private GameObject _displayObtained;

    public void Gamble()
    {
        ShopItemData pulledItem = PullItem();
        CustomShopManager.Instance.AddObject(pulledItem);
        SaveSystem.instance.inventory.ownedItemIDs.Add(pulledItem.id);
        SaveSystem.instance.Save();
        DisplayScriptable(pulledItem);
    }

    public ShopItemData PullItem()
    {
        RandomService rng = new RandomService();
        return _itemsUnlockable.items[rng.Range(0, _itemsUnlockable.items.Count)];
    }

    public void DisplayScriptable(ShopItemData itemObtained)
    {
        Debug.Log(itemObtained.name);
    }

}
