using TMPro;
using UnityEngine;

public enum DataDisplay
{
    PlayerName,
    PlayerNameNoPre,
    PlayerLevel,
    PlayerLevelNoPre,
    PlayerXP,
    PlayerBooksUnlocked,
    PlayerEnergy,
    PlayerEnergyNoPre,
    PlayerHardCurrency,
    PlayerHardCurrencyNoPre,
    PlayerSoftCurrency,
    PlayerSoftCurrencyNoPre,
}

public class ReactiveText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textVal;
    [SerializeField] private DataDisplay textType;

    private void Start()
    {
        textUpdate();
    }

    private void OnEnable()
    {
        SaveSystem.instance.OnDataUpdate += textUpdate;
    }

    private void OnDisable()
    {
        if (SaveSystem.instance != null)
            SaveSystem.instance.OnDataUpdate -= textUpdate;
    }

    public void textUpdate()
    {
        switch (textType)
        {
            case DataDisplay.PlayerName:
                textVal.text = "Name : " + SaveSystem.instance.profile.playerName;
                break;

            case DataDisplay.PlayerNameNoPre:
                textVal.text = SaveSystem.instance.profile.playerName;
                break;

            case DataDisplay.PlayerLevel:
                textVal.text = "LVL " + SaveSystem.instance.profile.playerLevel.ToString();
                break;

            case DataDisplay.PlayerLevelNoPre:
                textVal.text = SaveSystem.instance.profile.playerLevel.ToString();
                break;

            case DataDisplay.PlayerXP:
                textVal.text = SaveSystem.instance.profile.playerXP.ToString() + " / " + SaveSystem.instance.profile.LevelToMaxExp(SaveSystem.instance.profile.playerLevel).ToString();
                break;

            case DataDisplay.PlayerBooksUnlocked:
                textVal.text = "Books Unlocked : " + SaveSystem.instance.profile.playerBooksUnlocked.ToString();
                break;

            case DataDisplay.PlayerEnergy:
                textVal.text = SaveSystem.instance.currency.playerCurrencyEnergy.ToString() + "/60";
                break;

            case DataDisplay.PlayerEnergyNoPre:
                textVal.text = SaveSystem.instance.currency.playerCurrencyEnergy.ToString();
                break;

            case DataDisplay.PlayerHardCurrency:
                textVal.text = SaveSystem.instance.currency.playerCurrencyHard.ToString() + "$";
                break;

            case DataDisplay.PlayerHardCurrencyNoPre:
                textVal.text = SaveSystem.instance.currency.playerCurrencyHard.ToString();
                break;

            case DataDisplay.PlayerSoftCurrency:
                textVal.text = SaveSystem.instance.currency.playerCurrencySoft.ToString() + "$";
                break;

            case DataDisplay.PlayerSoftCurrencyNoPre:
                textVal.text = SaveSystem.instance.currency.playerCurrencySoft.ToString();
                break;

        }  
    }
}
