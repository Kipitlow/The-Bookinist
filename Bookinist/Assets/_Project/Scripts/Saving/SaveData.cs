using UnityEngine;

[System.Serializable]
public class SaveData
{
    public PlayerProfile profile;
    public PlayerSettings settings;
    public PlayerCurrency currency;
    public PlayerInventory inventory;
    public PlayerProgression progression;
    public PlayerCustomShop customShop;
    public PlayerGacha gacha;
    public PlayerBP bp;
}