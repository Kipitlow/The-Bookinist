using System.Collections.Generic;
using UnityEngine;

public class MasterDict : MonoBehaviour
{
    [SerializeField] private string _shopItem;

    public void GetShopItem()
    {
        Debug.Log(ItemDatabase.Instance.Get(_shopItem).itemName);
    }
}
