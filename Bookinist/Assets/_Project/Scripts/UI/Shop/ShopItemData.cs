using UnityEngine;

[CreateAssetMenu(fileName = "NewShopItem", menuName = "Shop/ShopItem")]
public class ShopItemData : ScriptableObject
{
    [Header("Item Info")]
    public string itemName;
    public Sprite icon;
    public int price;

    [Header("Shop Display")]
    public bool isFurniture;

    [Header("Customisation")]
    [Range(0, 5)]
    public int viewIndex;
    public GameObject mesh;
}