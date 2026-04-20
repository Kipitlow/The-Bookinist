using System;
using UnityEngine;
using UnityEngine.Events;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;

    [SerializeField] private SaveManager _saveManager;

    public PlayerProfile profile;
    public PlayerSettings settings;
    public PlayerCurrency currency;
    public BattlePassData battlePass;

    public Action OnDataUpdate;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Load();
    }

    public void Save()
    {
        if (ProgressionBar.instance != null)
        {
            battlePass = ProgressionBar.instance.GetDataForSave();
        }

        SaveData data = new SaveData
        {
            profile = profile,
            settings = settings,
            currency = currency,
            battlePass = battlePass 
        };

        _saveManager.Write("saveData.json", data);
    }

    public void Load()
    {
        SaveData data = _saveManager.Read<SaveData>("saveData.json");

        if (data == null)
        {
            Create();
            return;
        }

        profile = data.profile;
        settings = data.settings;
        currency = data.currency;

        battlePass = data.battlePass;

        OnDataUpdate?.Invoke();
    }

    public void Delete()
    {
        _saveManager.Delete("saveData.json");
    }

    public void Create()
    {
        profile = new PlayerProfile();
        settings = new PlayerSettings();
        currency = new PlayerCurrency();
        battlePass = new BattlePassData();

        Save();
    }
}