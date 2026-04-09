using UnityEngine;

[System.Serializable]
public class PlayerProfile
{
    public string playerName;
    public int playerLevel;
    public int playerXP;
    public int playerBooksUnlocked;
    public ScriptableObject playerDisplayedBook;

    public void ChangePlayerName(string newName)
    {
        playerName = newName;
    }
    
    public int LevelToMaxExp(int level)
    {
        return level * 200;
    }
}
