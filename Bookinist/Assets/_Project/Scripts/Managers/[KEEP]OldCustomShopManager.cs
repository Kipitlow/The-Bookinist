using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OldCustomShopManager : MonoBehaviour
{
    #region Variables

    public static OldCustomShopManager Instance { get; private set; }

    private const int VIEW_COUNT = 6;

    [SerializeField] private CamManager _camManager;

    // Rotation à appliquer aux meubles pour chaque view (remplace SO_FurnitureList._furnitureRotationList)
    [SerializeField] private Vector3[] _rotationByView = new Vector3[VIEW_COUNT];

    // Points de spawn pour chaque view
    [SerializeField] private GameObject[] _spawnPointByView = new GameObject[VIEW_COUNT];

    [SerializeField] private GameObject _horizontalPanelPrefab;
    [SerializeField] private GameObject _horizontalPanelParent;
    [SerializeField] private GameObject _buttonPrefab;

    // Inventaire runtime : une liste de ShopItemData par view
    private List<ShopItemData>[] _inventoryByView;

    // Listes de boutons UI par view
    private List<GameObject>[] _buttonsByView;

    // Panels horizontaux par view
    private GameObject[] _horizontalPanels;

    // Objet 3D actuellement affiché par view
    private GameObject[] _currentObjects;

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
        // Initialisation des tableaux runtime
        _inventoryByView = new List<ShopItemData>[VIEW_COUNT];
        _buttonsByView = new List<GameObject>[VIEW_COUNT];
        _horizontalPanels = new GameObject[VIEW_COUNT];
        _currentObjects = new GameObject[VIEW_COUNT];

        for (int i = 0; i < VIEW_COUNT; i++)
        {
            _inventoryByView[i] = new List<ShopItemData>();
            _buttonsByView[i] = new List<GameObject>();
            _currentObjects[i] = null;

            // Création du panel horizontal pour cette view
            _horizontalPanels[i] = Instantiate(_horizontalPanelPrefab, _horizontalPanelParent.transform);
            _horizontalPanels[i].SetActive(false);
        }

        // La view 0 est active au départ
        _horizontalPanels[0].SetActive(true);
    }

    /// <summary>
    /// Appelé quand la caméra change de view.
    /// </summary>
    private void OnViewChanged(int index, int offset)
    {
        if (_isAlreadySeeCustomShop)
            _horizontalPanels[_previousViewIndex].SetActive(false);

        _horizontalPanels[index].SetActive(true);
        _previousViewIndex = index;
        _isAlreadySeeCustomShop = true;
    }

    /// <summary>
    /// Appelé par ShopItemUI à l'achat d'un meuble.
    /// Ajoute le ShopItemData dans l'inventaire de la bonne view et crée son bouton.
    /// </summary>
    public void AddObject(ShopItemData newObject)
    {
        int targetView = newObject.viewIndex;

        if (targetView < 0 || targetView >= VIEW_COUNT)
        {
            Debug.LogWarning($"[CustomShopManager] viewIndex {targetView} invalide pour '{newObject.itemName}'.");
            return;
        }

        _inventoryByView[targetView].Add(newObject);

        int newIndex = _inventoryByView[targetView].Count - 1;
        CreateButton(targetView, newIndex, newObject);
    }

    /// <summary>
    /// Crée un bouton UI pour un meuble, avec son icône et son listener.
    /// </summary>
    private void CreateButton(int viewIndex, int furnitureIndex, ShopItemData data)
    {
        int capturedIndex = furnitureIndex;

        GameObject button = Instantiate(_buttonPrefab, _horizontalPanels[viewIndex].transform);

        // Affectation de l'icône du meuble sur le bouton
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null && data.icon != null)
            buttonImage.sprite = data.icon;

        button.GetComponent<Button>().onClick.RemoveAllListeners();
        button.GetComponent<Button>().onClick.AddListener(() => ChangeFurniture(viewIndex, capturedIndex));

        _buttonsByView[viewIndex].Add(button);
    }

    /// <summary>
    /// Instancie le mesh du meuble sélectionné dans la view correspondante.
    /// </summary>
    private void ChangeFurniture(int viewIndex, int furnitureIndex)
    {
        List<ShopItemData> list = _inventoryByView[viewIndex];

        if (furnitureIndex < 0 || furnitureIndex >= list.Count)
        {
            Debug.LogWarning($"[CustomShopManager] Index {furnitureIndex} invalide pour la view {viewIndex}.");
            return;
        }

        if (_currentObjects[viewIndex] != null)
            Destroy(_currentObjects[viewIndex]);

        ShopItemData data = list[furnitureIndex];
        Quaternion rotation = Quaternion.Euler(_rotationByView[viewIndex]);
        Vector3 position = _spawnPointByView[viewIndex].transform.position;

        _currentObjects[viewIndex] = Instantiate(data.prefab, position, rotation);
    }

    /// <summary>
    /// Retourne l'inventaire d'une view - utile pour la sauvegarde.
    /// </summary>
    public List<ShopItemData> GetInventoryForView(int viewIndex) => _inventoryByView[viewIndex];

    /// <summary>
    /// Charge un inventaire sauvegardé pour une view (appelé par le SaveSystem).
    /// </summary>
    public void LoadInventoryForView(int viewIndex, List<ShopItemData> items)
    {
        if (viewIndex < 0 || viewIndex >= VIEW_COUNT) return;

        foreach (ShopItemData item in items)
            AddObject(item);
    }

    /// <summary>
    /// Retourne un booléen si l'item du shop se trouve dans l'inventaire du joueur.
    /// </summary>
    public bool HasItem(ShopItemData item)
    {
        int view = item.viewIndex;

        if (view < 0 || view >= _inventoryByView.Length)
            return false;

        return _inventoryByView[view].Contains(item);
    }
}