using UnityEngine;

[System.Serializable]
public class PlayerProfile
{
    public string playerName = "Bookinist";
    public int playerLevel = 1;
    public int playerXP = 0;
    public int playerBooksUnlocked = 0;
    public ScriptableObject playerDisplayedBook;

    public void ChangePlayerName(string newName)
    {
        playerName = newName;
    }
    
    public int LevelToMaxExp(int level)
    {
        return level * 200;
    }

    public void ModifyXP(int xpChange)
    {
        if (xpChange < 0) 
            return;

        playerXP += xpChange;
        if (playerXP >= LevelToMaxExp(playerLevel))
        {
            playerXP = 0;
            playerLevel++;
        }

    }
}
