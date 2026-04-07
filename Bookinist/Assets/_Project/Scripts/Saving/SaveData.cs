using UnityEngine;

[System.Serializable]
public class SaveData : MonoBehaviour
{
    public PlayerProfile player;

    public static SaveData instance;

    [SerializeField] private SaveManager _saveManager;

    private void Awake()
    {
        instance = this;
        Load();
    }

    public void Load()
    {
        PlayerProfile loaded = _saveManager.Read<PlayerProfile>("settings.json");

        if (loaded == null)
        {
            Debug.LogWarning("Impossible de charger settings.json — valeurs par défaut appliquées.");
        }
        else
        {
            Debug.Log("Paramètres chargés depuis settings.json !");
            player.playerName = loaded.playerName;
            player.playerLevel = loaded.playerLevel;
            player.playerXP = loaded.playerXP;
            player.playerBooksUnlocked = loaded.playerBooksUnlocked;
        }
    }

    public void Save()
    {
        _saveManager.Write("settings.json", player);
    }
}