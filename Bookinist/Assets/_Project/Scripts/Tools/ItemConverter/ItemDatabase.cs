using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance;

    [SerializeField] private List<ShopItemData> _allItems;

    private Dictionary<string, ShopItemData> _dict;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        LoadAllItems();
    }

    private void LoadAllItems()
    {
        _dict = new Dictionary<string, ShopItemData>();

        ShopItemData[] items = Resources.LoadAll<ShopItemData>("ShopItems");

        foreach (var item in items)
        {
            if (string.IsNullOrEmpty(item.id))
            {
                Debug.LogError($"Item '{item.name}' has no ID!");
                continue;
            }

            if (_dict.ContainsKey(item.id))
            {
                Debug.LogError($"Duplicate ID detected: {item.id}");
                continue;
            }

            _dict.Add(item.id, item);
        }

        Debug.Log($"Loaded {_dict.Count} items into database.");
    }


    public ShopItemData Get(string id)
    {
        if (_dict.TryGetValue(id, out var item))
            return item;

        Debug.LogWarning($"Item ID not found: {id}");
        return null;
    }
}