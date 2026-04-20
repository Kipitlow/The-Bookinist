using UnityEngine;

[System.Serializable]
public class SaveData
{
    public PlayerProfile profile;
    public PlayerSettings settings;
    public PlayerCurrency currency;
    public BattlePassData battlePass;
}

[System.Serializable]
public class BattlePassData
{
    public float confirmedXp;
    public float waitingXp;  
}