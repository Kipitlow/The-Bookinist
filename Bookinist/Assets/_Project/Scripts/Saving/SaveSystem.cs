using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;

    [SerializeField] private SaveManager _saveManager;

    public PlayerProfile profile;
    public PlayerSettings settings;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        Load();
    }

    public void Save()
    {
        SaveData data = new SaveData
        {
            profile = profile,
            settings = settings
        };

        _saveManager.Write("saveData.json", data);
    }

    public void Load()
    {
        SaveData data = _saveManager.Read<SaveData>("saveData.json");

        if (data == null)
        {
            Debug.LogWarning("Failed to load saveData.json");
            return;
        }

        profile = data.profile;
        settings = data.settings;
    }
}