using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class GambleManager : MonoBehaviour
{
    [SerializeField] private LootPools _itemsUnlockable;
    [SerializeField] private List<ShopItemData> _filteredUnlockables;
    [SerializeField] private Image _displayObtained;
    [SerializeField] private Sprite _defaultIcon;
    private float _transparency;


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
            if (!SaveSystem.instance.inventory.playerInventoryOwnedItemIDs.Contains(item.id.ToString()))
            {
                _filteredUnlockables.Add(item);
            }
        }
    }

    public void Gamble()
    {
        ShopItemData pulledItem = PullItem();

        DisplayScriptable(pulledItem);

        if (pulledItem == null)
            return;

        CustomShopManager.Instance.AddObject(pulledItem);
        SaveSystem.instance.inventory.playerInventoryOwnedItemIDs.Add(pulledItem.id.ToString());
        SaveSystem.instance.Save();
    }

    private ShopItemData PullItem()
    {
        if (_filteredUnlockables.Count == 0)
        {
            Debug.Log("No more items to pull.");
            _transparency = 0f;
            return null;
        }

        RandomService rng = new RandomService();
        int index = rng.Range(0, _filteredUnlockables.Count);

        ShopItemData selectedItem = _filteredUnlockables[index];

        _filteredUnlockables.RemoveAt(index);

        _transparency = 1f;

        return selectedItem;
    }

    private void DisplayScriptable(ShopItemData itemObtained)
    {
        var tempColor = _displayObtained.color;
        if (itemObtained == null)
            _displayObtained.sprite = _defaultIcon;
        else
            _displayObtained.sprite = itemObtained.icon;
        
        tempColor.a = _transparency;
        _displayObtained.color = tempColor;
    }

}
