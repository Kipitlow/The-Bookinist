using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomShopManager : MonoBehaviour
{
    #region Variables

    public static CustomShopManager Instance { get; private set; }

    private const int VIEW_COUNT = 6;

    [SerializeField] private CamManager _camManager;
    [SerializeField] private Transform _furnitureParent;

    [SerializeField] private GameObject _horizontalPanelPrefab;
    [SerializeField] private GameObject _horizontalPanelParent;
    [SerializeField] private GameObject _buttonPrefab;
    [SerializeField] private BookShopOnboardingManager _onboardingManager;

    [Header("Inventaire de départ - placés en scène automatiquement au load")]
    [SerializeField] private ShopItemData[] _defaultItems;

    private List<ShopItemData>[] _inventoryByView;
    private List<GameObject>[] _buttonsByView;
    private GameObject[] _horizontalPanels;

    // Instances 3D actives par view et par type : [viewIndex][FurnitureType] = { principal, additional }
    private Dictionary<FurnitureType, GameObject[]>[] _currentObjects;

    // Item actif par view et par type, pour la sauvegarde
    private Dictionary<FurnitureType, ShopItemData>[] _activeItemByView;

    private bool _isAlreadySeeCustomShop;
    private int _previousViewIndex = 0;

    #endregion

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        _camManager.OnViewChanged += OnViewChanged;
    }

    private void OnDestroy()
    {
        if (_camManager != null)
            _camManager.OnViewChanged -= OnViewChanged;
    }

    private void Start()
    {
        _inventoryByView = new List<ShopItemData>[VIEW_COUNT];
        _buttonsByView = new List<GameObject>[VIEW_COUNT];
        _horizontalPanels = new GameObject[VIEW_COUNT];
        _currentObjects = new Dictionary<FurnitureType, GameObject[]>[VIEW_COUNT];
        _activeItemByView = new Dictionary<FurnitureType, ShopItemData>[VIEW_COUNT];

        for (int i = 0; i < VIEW_COUNT; i++)
        {
            _inventoryByView[i] = new List<ShopItemData>();
            _buttonsByView[i] = new List<GameObject>();
            _currentObjects[i] = new Dictionary<FurnitureType, GameObject[]>();
            _activeItemByView[i] = new Dictionary<FurnitureType, ShopItemData>();

            _horizontalPanels[i] = Instantiate(_horizontalPanelPrefab, _horizontalPanelParent.transform);
            _horizontalPanels[i].SetActive(false);
        }

        _horizontalPanels[0].SetActive(true);

        // 1. Place les items par défaut (sans écraser ce qui sera chargé)
        if (_defaultItems != null)
            foreach (ShopItemData item in _defaultItems)
                AddObjectSilent(item, placeImmediately: true);

        // 2. Charge l'état sauvegardé par-dessus
        LoadFromSave();
    }

    private void OnViewChanged(int index, int offset)
    {
        _horizontalPanels[_previousViewIndex].SetActive(false);
        _horizontalPanels[index].SetActive(true);

        _previousViewIndex = index;
        _isAlreadySeeCustomShop = true;
    }

    /// <summary>
    /// Ajoute un item à l'inventaire et crée son bouton.
    /// Si placeImmediately est true, l'instancie en scène si aucun item
    /// du même type n'occupe déjà cette view.
    /// </summary>
    public void AddObject(ShopItemData newObject, bool placeImmediately = false)
    {
        int targetView = newObject.viewIndex;

        if (targetView < 0 || targetView >= VIEW_COUNT)
        {
            Debug.LogWarning($"[CustomShopManager] viewIndex {targetView} invalide pour '{newObject.itemName}'.");
            return;
        }

        // Evite les doublons dans l'inventaire
        if (!_inventoryByView[targetView].Contains(newObject))
        {
            _inventoryByView[targetView].Add(newObject);
            CreateButton(targetView, _inventoryByView[targetView].Count - 1, newObject);
        }

        // Sauvegarde l'inventaire
        if (!SaveSystem.instance.inventory.ownedItemIDs.Contains(newObject.id))
        {
            SaveSystem.instance.inventory.ownedItemIDs.Add(newObject.id);
            SaveSystem.instance.Save();
        }

        if (placeImmediately && !_activeItemByView[targetView].ContainsKey(newObject.furnitureType))
            PlaceObject(targetView, newObject);
    }

    private void CreateButton(int viewIndex, int furnitureIndex, ShopItemData data)
    {
        int capturedIndex = furnitureIndex;

        //int remapped = RemapViewIndex(viewIndex);

        //GameObject button = Instantiate(_buttonPrefab, _horizontalPanels[remapped].transform);
        GameObject button = Instantiate(_buttonPrefab, _horizontalPanels[viewIndex].transform);

        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null && data.icon != null)
            buttonImage.sprite = data.icon;

        button.GetComponent<Button>().onClick.RemoveAllListeners();
        button.GetComponent<Button>().onClick.AddListener(() => ChangeFurniture(viewIndex, capturedIndex));

        _buttonsByView[viewIndex].Add(button);
    }

    private void ChangeFurniture(int viewIndex, int furnitureIndex)
    {
        List<ShopItemData> list = _inventoryByView[viewIndex];

        if (furnitureIndex < 0 || furnitureIndex >= list.Count)
        {
            Debug.LogWarning($"[CustomShopManager] Index {furnitureIndex} invalide pour la view {viewIndex}.");
            return;
        }

        PlaceObject(viewIndex, list[furnitureIndex]);
    }

    /// <summary>
    /// Remplace uniquement les instances du même FurnitureType dans la view,
    /// laisse les autres types intacts.
    /// </summary>
    private void PlaceObject(int viewIndex, ShopItemData data)
    {
        FurnitureType type = data.furnitureType;

        // Détruit uniquement les instances du même type
        if (_currentObjects[viewIndex].TryGetValue(type, out GameObject[] existing))
        {
            if (existing[0] != null) Destroy(existing[0]);
            if (existing[1] != null) Destroy(existing[1]);
        }

        GameObject[] instances = new GameObject[2];
        instances[0] = Instantiate(data.prefab, _furnitureParent);
        instances[1] = data.additionalPrefab != null
            ? Instantiate(data.additionalPrefab, _furnitureParent)
            : null;

        _currentObjects[viewIndex][type] = instances;
        _activeItemByView[viewIndex][type] = data;

        if (_activeItemByView[viewIndex][type].name == "FauteuilBleu")
        {
            _onboardingManager.CheckOnboarding(8);
        }

        // Sauvegarde l'item placé
        SaveSystem.instance.customShop.SetPlacedItem(viewIndex, type, data.id);
        SaveSystem.instance.Save();
    }

    // ─── Save / Load ───────────────────────────────────────────────────────────

    /// <summary>
    /// Recharge l'inventaire et les items placés depuis la sauvegarde.
    /// Appelé une seule fois dans Start(), après les defaultItems.
    /// </summary>
    private void LoadFromSave()
    {
        if (SaveSystem.instance == null || ItemDatabase.instance == null) return;

        // --- Inventaire ---
        foreach (string id in SaveSystem.instance.inventory.ownedItemIDs)
        {
            ShopItemData item = ItemDatabase.instance.Get(id);
            if (item == null) continue;

            int v = item.viewIndex;
            if (v < 0 || v >= VIEW_COUNT) continue;

            if (!_inventoryByView[v].Contains(item))
            {
                _inventoryByView[v].Add(item);
                CreateButton(v, _inventoryByView[v].Count - 1, item);
            }
        }

        // --- Items placés (CustomShop) ---
        Dictionary<FurnitureType, string>[] parsed =
            SaveSystem.instance.customShop.ParsePlacedItems(VIEW_COUNT);

        for (int i = 0; i < VIEW_COUNT; i++)
        {
            foreach (KeyValuePair<FurnitureType, string> entry in parsed[i])
            {
                ShopItemData item = ItemDatabase.instance.Get(entry.Value);
                if (item == null) continue;

                // S'assure que l'item est dans l'inventaire avant de le placer
                int v = item.viewIndex;
                if (!_inventoryByView[v].Contains(item))
                {
                    _inventoryByView[v].Add(item);
                    CreateButton(v, _inventoryByView[v].Count - 1, item);
                }

                PlaceObjectSilent(i, item);
            }
        }
    }

    /// <summary>
    /// Place un objet en scène sans déclencher une nouvelle sauvegarde
    /// (utilisé uniquement pendant le chargement).
    /// </summary>
    private void PlaceObjectSilent(int viewIndex, ShopItemData data)
    {
        FurnitureType type = data.furnitureType;

        if (_currentObjects[viewIndex].TryGetValue(type, out GameObject[] existing))
        {
            if (existing[0] != null) Destroy(existing[0]);
            if (existing[1] != null) Destroy(existing[1]);
        }

        GameObject[] instances = new GameObject[2];
        instances[0] = Instantiate(data.prefab, _furnitureParent);
        instances[1] = data.additionalPrefab != null
            ? Instantiate(data.additionalPrefab, _furnitureParent)
            : null;

        _currentObjects[viewIndex][type] = instances;
        _activeItemByView[viewIndex][type] = data;
        // Pas de Save() ici
    }

    // ─── API publique (inchangée) ───────────────────────────────────────────────

    public ShopItemData GetActiveItemForView(int viewIndex, FurnitureType type)
    {
        if (viewIndex < 0 || viewIndex >= VIEW_COUNT) return null;
        _activeItemByView[viewIndex].TryGetValue(type, out ShopItemData item);
        return item;
    }

    public Dictionary<FurnitureType, ShopItemData>[] GetAllActiveItems() => _activeItemByView;

    public void LoadActiveItems(Dictionary<FurnitureType, ShopItemData>[] savedItems)
    {
        if (savedItems == null) return;

        for (int i = 0; i < Mathf.Min(savedItems.Length, VIEW_COUNT); i++)
        {
            if (savedItems[i] == null) continue;

            foreach (KeyValuePair<FurnitureType, ShopItemData> entry in savedItems[i])
            {
                ShopItemData item = entry.Value;
                if (item == null) continue;

                if (!_inventoryByView[i].Contains(item))
                    AddObject(item);

                PlaceObject(i, item);
            }
        }
    }

    public List<ShopItemData> GetInventoryForView(int viewIndex) => _inventoryByView[viewIndex];

    public void LoadInventoryForView(int viewIndex, List<ShopItemData> items)
    {
        if (viewIndex < 0 || viewIndex >= VIEW_COUNT) return;

        foreach (ShopItemData item in items)
            AddObject(item);
    }

    public bool HasItem(ShopItemData item)
    {
        int view = item.viewIndex;
        if (view < 0 || view >= _inventoryByView.Length) return false;
        return _inventoryByView[view].Contains(item);
    }

    private void AddObjectSilent(ShopItemData newObject, bool placeImmediately = false)
    {
        int targetView = newObject.viewIndex;

        if (targetView < 0 || targetView >= VIEW_COUNT)
        {
            Debug.LogWarning($"[CustomShopManager] viewIndex {targetView} invalide pour '{newObject.itemName}'.");
            return;
        }

        if (!_inventoryByView[targetView].Contains(newObject))
        {
            _inventoryByView[targetView].Add(newObject);
            CreateButton(targetView, _inventoryByView[targetView].Count - 1, newObject);
        }

        if (placeImmediately && !_activeItemByView[targetView].ContainsKey(newObject.furnitureType))
            PlaceObjectSilent(targetView, newObject);
    }
}