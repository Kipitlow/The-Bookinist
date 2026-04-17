using UnityEngine;

[CreateAssetMenu(fileName = "NewShopItem", menuName = "Shop/ShopItem")]
public class ShopItemData : ScriptableObject
{
    [Header("Item Info")]
    public string itemName;
    public string id;
    public Sprite icon;
    public int price;
    public bool isFurniture;
    public GameObject mesh;
}