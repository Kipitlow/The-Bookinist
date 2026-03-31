using UnityEngine;

[CreateAssetMenu(menuName = "Game/Item Data")]
public class ItemData : ScriptableObject
{
    public string id;
    public string displayName;
    public Sprite icon;

    [Header("Prefabs")]
    public GameObject pagePrefab;

    [Header("Inventory")]
    public bool stackable;
    public int maxStack = 1;

    [Header("Placement")]
    public bool canBePlacedOnPage = true;
    public bool canBeTakenBack = true;
}