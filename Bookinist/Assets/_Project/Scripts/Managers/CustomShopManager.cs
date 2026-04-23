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

        if (_defaultItems != null)
            foreach (ShopItemData item in _defaultItems)
                AddObject(item, placeImmediately: true);
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

        _inventoryByView[targetView].Add(newObject);
        CreateButton(targetView, _inventoryByView[targetView].Count - 1, newObject);

        // Place en scène uniquement si ce slot de type est encore libre
        if (placeImmediately && !_activeItemByView[targetView].ContainsKey(newObject.furnitureType))
            PlaceObject(targetView, newObject);
    }

    private void CreateButton(int viewIndex, int furnitureIndex, ShopItemData data)
    {
        int capturedIndex = furnitureIndex;

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
    }

    /// <summary>Retourne l'item actif pour un type donné dans une view.</summary>
    public ShopItemData GetActiveItemForView(int viewIndex, FurnitureType type)
    {
        if (viewIndex < 0 || viewIndex >= VIEW_COUNT) return null;
        _activeItemByView[viewIndex].TryGetValue(type, out ShopItemData item);
        return item;
    }

    /// <summary>
    /// Retourne tous les items actifs (toutes views, tous types).
    /// </summary>
    public Dictionary<FurnitureType, ShopItemData>[] GetAllActiveItems() => _activeItemByView;

    /// <summary>
    /// Charge un état sauvegardé.
    /// </summary>
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
}