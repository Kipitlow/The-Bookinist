using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase Instance;

    [SerializeField] private List<ShopItemData> allItems;

    private Dictionary<string, ShopItemData> _dict;

    private void Awake()
    {
        Instance = this;

        _dict = new Dictionary<string, ShopItemData>();

        foreach (var item in allItems)
        {
            if (!_dict.ContainsKey(item.id))
                _dict.Add(item.id, item);
            else
                Debug.LogWarning("Duplicate ID: " + item.id);
        }
    }

    public ShopItemData Get(string id)
    {
        if (_dict.TryGetValue(id, out var item))
            return item;

        Debug.LogWarning("Missing ID: " + id);
        return null;
    }
}