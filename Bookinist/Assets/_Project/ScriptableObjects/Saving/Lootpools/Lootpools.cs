using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Loot Pool")]
public class LootPools : ScriptableObject
{
    public List<ShopItemData> items;
}
