using UnityEngine;

public enum FurnitureType
{
    Lamp,
    Stairs,
    Sofa,
    Rug,
    Chair,
}

[CreateAssetMenu(fileName = "NewShopItem", menuName = "Shop/ShopItem")]
public class ShopItemData : ScriptableObject
{
    [Header("Item Info")]
    public string itemName;
    public Sprite icon;
    public int price;
    public string id;

    [Header("Shop Display")]
    public bool isFurniture;

    [Header("Customisation")]
    [Range(0, 5)]
    public int viewIndex;
    public FurnitureType furnitureType;
    public GameObject prefab;

    [Tooltip("Optionnel : second prefab à instancier en même temps")]
    public GameObject additionalPrefab;

    [Tooltip("Prefab avec la scale correct pour la preview dans le shop")]
    public GameObject previewPrefab;
}