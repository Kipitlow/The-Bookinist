using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Gestionnaire global des devises (singleton).
/// Fournit accès et modification aux monnaies "soft" et "hard" avec des événements de notification.
/// </summary>
public class CurrencyManager : MonoBehaviour
{
    // Instance singleton accessible globalement
    public static CurrencyManager Instance { get; private set; }

    [Header("Valeurs initiales")]
    [SerializeField] private int _softCurrency = 0;
    [SerializeField] private int _hardCurrency = 0;

    // Événements Unity déclenchés quand une monnaie change (envoie la nouvelle valeur)
    public UnityEvent<int> OnSoftCurrencyChanged;
    public UnityEvent<int> OnHardCurrencyChanged;

    private void OnEnable()
    {
        SaveSystem.instance.OnDataUpdate += SetupCurrency;
    }

    private void OnDisable()
    {
        if (SaveSystem.instance != null)
            SaveSystem.instance.OnDataUpdate -= SetupCurrency;
    }

    private void Awake()
    {
        // Mise en place du singleton : détruit les doublons et conserve l'instance unique
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Ne pas détruire lors du chargement de scènes
    }

    // Accesseurs en lecture seule pour d'autres classes
    public int SoftCurrency => _softCurrency;
    public int HardCurrency => _hardCurrency;

    /// <summary>
    /// Ajoute une quantité à la monnaie soft et notifie les abonnés.
    /// </summary>
    public void AddSoftCurrency(int amount)
    {
        _softCurrency += amount;
        OnSoftCurrencyChanged?.Invoke(_softCurrency);
    }

    /// <summary>
    /// Tente de dépenser une quantité de monnaie soft.
    /// Retourne false si fonds insuffisants, true sinon (et notifie).
    /// </summary>
    public bool SpendSoftCurrency(int amount)
    {
        if (_softCurrency < amount) return false;
        _softCurrency -= amount;
        OnSoftCurrencyChanged?.Invoke(_softCurrency);
        return true;
    }

    /// <summary>
    /// Set la soft currency a une quantité specifique, puis notifie les abonnés.
    /// </summary>
    public void SetSoftCurrency(int amount)
    {
        _softCurrency = amount;
        //OnSoftCurrencyChanged?.Invoke(_softCurrency);
    }

    /// <summary>
    /// Ajoute une quantité à la monnaie hard et notifie les abonnés.
    /// </summary>
    public void AddHardCurrency(int amount)
    {
        _hardCurrency += amount;
        OnHardCurrencyChanged?.Invoke(_hardCurrency);
    }

    /// <summary>
    /// Tente de dépenser une quantité de monnaie hard.
    /// Retourne false si fonds insuffisants, true sinon (et notifie).
    /// </summary>
    public bool SpendHardCurrency(int amount)
    {
        if (_hardCurrency < amount) return false;
        _hardCurrency -= amount;
        OnHardCurrencyChanged?.Invoke(_hardCurrency);
        return true;
    }

    /// <summary>
    /// Set la hard currency a une quantité specifique, puis notifie les abonnés.
    /// </summary>
    public void SetHardCurrency(int amount)
    {
        _hardCurrency = amount;
        //OnHardCurrencyChanged?.Invoke(_hardCurrency);
    }

    public int GetSoftCurrency() { return _softCurrency; }

    public int GetHardCurrency() { return _hardCurrency; }

    public void SetupCurrency()
    {
        Debug.Log("SetupCurrency Executed");
        SetSoftCurrency(SaveSystem.instance.currency.playerCurrencySoft);
        SetHardCurrency(SaveSystem.instance.currency.playerCurrencyHard);
    }
}