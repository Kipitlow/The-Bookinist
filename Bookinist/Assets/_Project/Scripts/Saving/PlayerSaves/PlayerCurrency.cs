using System;
using UnityEngine;

[System.Serializable]
public class PlayerCurrency
{
    public long playerCurrencyLastLogin = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    public int playerCurrencyEnergy = 60;
    public int playerCurrencyHard = 0;
    public int playerCurrencySoft = 10000;

    public void ChangeLastLogin(long newVal)
    {
        playerCurrencyLastLogin = newVal;
    }


    public void ChangeEnergyVal(int newVal)
    {
        playerCurrencyEnergy = Mathf.Clamp(newVal, 0, 60);
    }

    public void ChangeHardVal(int newVal)
    {
        playerCurrencyHard = Mathf.Max(0, newVal);
    }

    public void ChangeSoftVal(int newVal)
    {
        playerCurrencySoft = Mathf.Max(0, newVal);
    }
}

