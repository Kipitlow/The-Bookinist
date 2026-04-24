using UnityEngine;

public class SaveService : MonoBehaviour
{
    public static SaveService instance;

    private PlayerProfile profile => SaveSystem.instance.profile;
    private PlayerCurrency currency => SaveSystem.instance.currency;


    public void Awake()
    {
        instance = this;
    }

    public void ChangeName(string newName)
    {
        profile.ChangePlayerName(newName);

        SaveSystem.instance.Save();
    }

    public void ChangeEnergy(int newVal)
    {
        currency.ChangeEnergyVal(newVal);

        SaveSystem.instance.Save();
    }
    public void ModifyEnergy(int modifVal)
    {
        currency.ChangeEnergyVal(currency.playerCurrencyEnergy + modifVal);

        SaveSystem.instance.Save();
    }
    public void ChangeLastLogin(long lastLogin)
    {
        currency.ChangeLastLogin(lastLogin);

        SaveSystem.instance.Save();
    }

    public void ChangeHard(int newVal)
    {
        currency.ChangeHardVal(newVal);

        SaveSystem.instance.Save();
    }

    public void ChangeSoft(int newVal)
    {
        currency.ChangeSoftVal(newVal);

        SaveSystem.instance.Save();
    }
}
