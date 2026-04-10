using TMPro;
using UnityEngine;

public enum DataDisplay
{
    PlayerName,
    PlayerNameNoPre,
    PlayerLevel,
    PlayerLevelNoPre,
    PlayerXP,
    PlayerBooksUnlocked
}

public class ReactiveText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textVal;
    [SerializeField] private DataDisplay textType;

    private void Start()
    {
        textUpdate();
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
        }  
    }
}
