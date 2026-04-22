using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BattlePassData
{
    public float confirmedXp;
    public float waitingXp;
    public bool isPremiumActive;
    public List<bool> freeRewardsTaken = new List<bool>(); 
    public List<bool> premiumRewardsTaken = new List<bool>();
}
