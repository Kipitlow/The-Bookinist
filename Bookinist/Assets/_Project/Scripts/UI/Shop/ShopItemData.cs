using UnityEngine;

[CreateAssetMenu(fileName = "NewShopItem", menuName = "Shop/ShopItem")]
public class ShopItemData : ScriptableObject
{
    [Header("Item Info")]
    public string itemName;
    public string id;
    public Sprite icon;
    public int price;

    [Header("Shop Display")]
    public bool isFurniture;

    [Header("Customisation")]
    [Range(0, 5)]
    public int viewIndex;
    public GameObject prefab;

    [Tooltip("Optionnel : second prefab à instancier en même temps")]
    public GameObject additionalPrefab;

    [Tooltip("Prefab avec la scale correct pour la preview dans le shop")]
    public GameObject previewPrefab;
}