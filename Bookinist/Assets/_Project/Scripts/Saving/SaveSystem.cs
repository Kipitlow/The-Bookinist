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
    public PlayerInventory inventory;
    public PlayerProgression progression;
    public PlayerCustomShop customShop;
    public PlayerGacha gacha;
    public PlayerBP bp;

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
        Load();
    }

    private void Start()
    {
        Load();
    }

    public void Save()
    {
        SaveData data = new SaveData
        {
            profile = profile,
            settings = settings,
            currency = currency,
            inventory = inventory,
            progression = progression,
            customShop = customShop,
            gacha = gacha,
            bp = bp
        };

        _saveManager.Write("saveData.json", data);
    }

    public void Load()
    {
        SaveData data = _saveManager.Read<SaveData>("saveData.json");

        if (data == null)
        {
            Debug.LogWarning("Failed to load saveData.json");
            Create();
            return;
        }

        profile = data.profile;
        settings = data.settings;
        currency = data.currency;
        inventory = data.inventory;
        progression = data.progression;
        customShop = data.customShop;
        gacha = data.gacha;
        bp = data.bp;

        Debug.Log("Invoke Called");
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
        inventory = new PlayerInventory();
        progression = new PlayerProgression();
        customShop = new PlayerCustomShop();
        gacha = new PlayerGacha();
        bp = new PlayerBP();

        Save();
    }
}