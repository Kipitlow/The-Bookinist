using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerBP
{
    public float playerBPConfirmedXp;
    public float playerBPWaitingXp;
    public bool playerBPIsPremiumActive;
    public List<bool> playerBPFreeRewardsTaken = new List<bool>();
    public List<bool> playerBPPremiumRewardsTaken = new List<bool>();
}
