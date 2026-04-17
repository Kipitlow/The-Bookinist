using System.Collections.Generic;
using UnityEngine;

public class Machine1ItemsDict : MonoBehaviour
{
    private int _fileIncrementer = 0;
    [SerializeField] private List<ShopItemData> _itemList;

    public Dictionary<string, ShopItemData> _machine1ItemsDict;

    public static Machine1ItemsDict instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _machine1ItemsDict = new Dictionary<string, ShopItemData>();
        foreach (var item in _itemList)
        {
            _fileIncrementer++;
            _machine1ItemsDict.Add(("M1-1" + _fileIncrementer), item);
        }
    }
}
 