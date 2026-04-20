using UnityEngine;
using System.Collections.Generic;

public class GambleManager : MonoBehaviour
{
    [SerializeField] private LootPools _itemsUnlockable;
    [SerializeField] private List<ShopItemData> _filteredUnlockables;
    [SerializeField] private GameObject _displayObtained;

    private void Start()
    {
        _filteredUnlockables = new List<ShopItemData>();
        CleanFilteredList();
    }

    private void CleanFilteredList()
    {
        _filteredUnlockables.Clear();

        foreach (var item in _itemsUnlockable.items)
        {
            if (!SaveSystem.instance.inventory.ownedItemIDs.Contains(item.id.ToString()))
            {
                _filteredUnlockables.Add(item);
            }
        }
    }

    public void Gamble()
    {
        ShopItemData pulledItem = PullItem();

        if (pulledItem == null)
            return;

        CustomShopManager.Instance.AddObject(pulledItem);
        SaveSystem.instance.inventory.ownedItemIDs.Add(pulledItem.id.ToString());
        SaveSystem.instance.Save();
        DisplayScriptable(pulledItem);
        
    }

    public ShopItemData PullItem()
    {
        if (_filteredUnlockables.Count == 0)
        {
            Debug.Log("No more items to pull.");
            return null;
        }

        RandomService rng = new RandomService();
        int index = rng.Range(0, _filteredUnlockables.Count);

        ShopItemData selectedItem = _filteredUnlockables[index];

        _filteredUnlockables.RemoveAt(index);

        return selectedItem;
    }

    public void DisplayScriptable(ShopItemData itemObtained)
    {
        Debug.Log(itemObtained.name);
    }

}
