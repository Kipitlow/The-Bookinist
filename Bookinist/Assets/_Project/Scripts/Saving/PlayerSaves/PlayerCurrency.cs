using UnityEngine;

[System.Serializable]
public class PlayerCurrency
{
    public int playerCurrencyEnergy = 5;
    public int playerCurrencyHard = 0;
    public int playerCurrencySoft = 0;

    public void ChangeEnergyVal(int newVal)
    {
        playerCurrencyEnergy = Mathf.Clamp(newVal, 0, 5);
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

