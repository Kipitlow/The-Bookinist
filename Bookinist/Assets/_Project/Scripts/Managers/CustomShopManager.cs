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

    // Instances 3D actives par view
    private GameObject[,] _currentObjects;

    // Item actuellement placé par view, pour la sauvegarde
    private ShopItemData[] _activeItemByView;

    private bool _isAlreadySeeCustomShop;
    private int _previousViewIndex;

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
        _currentObjects = new GameObject[VIEW_COUNT, 2];
        _activeItemByView = new ShopItemData[VIEW_COUNT];

        for (int i = 0; i < VIEW_COUNT; i++)
        {
            _inventoryByView[i] = new List<ShopItemData>();
            _buttonsByView[i] = new List<GameObject>();

            _horizontalPanels[i] = Instantiate(_horizontalPanelPrefab, _horizontalPanelParent.transform);
            _horizontalPanels[i].SetActive(false);
        }

        _horizontalPanels[0].SetActive(true);

        // Les default items sont ajoutés à l'inventaire ET placés en scène immédiatement
        if (_defaultItems != null)
            foreach (ShopItemData item in _defaultItems)
                AddObject(item, placeImmediately: true);
    }

    private void OnViewChanged(int index, int offset)
    {
        if (_isAlreadySeeCustomShop)
            _horizontalPanels[_previousViewIndex].SetActive(false);

        _horizontalPanels[index].SetActive(true);
        _previousViewIndex = index;
        _isAlreadySeeCustomShop = true;
    }

    /// <summary>
    /// Ajoute un item à l'inventaire et crée son bouton.
    /// Si placeImmediately est true, l'instancie aussi en scène sur sa view
    /// (sans écraser un item déjà placé sur cette view).
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

        // Place en scène uniquement si aucun item n'occupe déjà cette view
        if (placeImmediately && _activeItemByView[targetView] == null)
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

    /// <summary>Appelé par le bouton UI : remplace ce qui est en scène par le nouvel item.</summary>
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

    /// <summary>Détruit les instances actuelles et instancie le nouvel item.</summary>
    private void PlaceObject(int viewIndex, ShopItemData data)
    {
        // Détruit les instances précédentes
        if (_currentObjects[viewIndex, 0] != null) Destroy(_currentObjects[viewIndex, 0]);
        if (_currentObjects[viewIndex, 1] != null) Destroy(_currentObjects[viewIndex, 1]);

        _currentObjects[viewIndex, 0] = Instantiate(data.prefab, _furnitureParent);
        _currentObjects[viewIndex, 1] = data.additionalPrefab != null
            ? Instantiate(data.additionalPrefab, _furnitureParent)
            : null;

        _activeItemByView[viewIndex] = data;
    }

    /// <summary>Retourne l'item actif pour une view — utile pour le SaveSystem.</summary>
    public ShopItemData GetActiveItemForView(int viewIndex)
    {
        if (viewIndex < 0 || viewIndex >= VIEW_COUNT) return null;
        return _activeItemByView[viewIndex];
    }

    /// <summary>Retourne tous les items actifs pour sauvegarder l'état complet.</summary>
    public ShopItemData[] GetAllActiveItems() => _activeItemByView;

    /// <summary>
    /// Charge un état sauvegardé.
    /// Les default items ont déjà été placés dans Start —
    /// on écrase uniquement les views qui avaient un item sauvegardé différent.
    /// </summary>
    public void LoadActiveItems(ShopItemData[] savedItems)
    {
        if (savedItems == null) return;

        for (int i = 0; i < Mathf.Min(savedItems.Length, VIEW_COUNT); i++)
        {
            if (savedItems[i] == null) continue;

            if (!_inventoryByView[i].Contains(savedItems[i]))
                AddObject(savedItems[i]);

            PlaceObject(i, savedItems[i]);
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