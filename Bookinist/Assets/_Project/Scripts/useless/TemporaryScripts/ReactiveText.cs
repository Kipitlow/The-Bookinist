using TMPro;
using UnityEngine;

public enum DataDisplay
{
    PlayerName,
    PlayerLevel,
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

            case DataDisplay.PlayerLevel:
                textVal.text = SaveSystem.instance.profile.playerLevel.ToString();
                break;

            case DataDisplay.PlayerXP:
                textVal.text = SaveSystem.instance.profile.playerXP.ToString() + " / " + (SaveSystem.instance.profile.playerXP + 1).ToString();
                break;

            case DataDisplay.PlayerBooksUnlocked:
                textVal.text = "Books Unlocked : " + SaveSystem.instance.profile.playerBooksUnlocked.ToString();
                break;
        }  
    }
}
