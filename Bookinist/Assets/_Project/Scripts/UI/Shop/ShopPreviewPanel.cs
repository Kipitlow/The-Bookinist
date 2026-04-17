using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Gère la fenêtre de prévisualisation du meuble sélectionné.
/// 
/// Setup dans Unity :
/// 1. Attache ce script sur le GameObject parent de la fenêtre preview (la grande zone grise).
/// 2. Crée un RenderTexture (ex. 512x512) et assigne-la à _previewRenderTexture.
/// 3. Crée une caméra dédiée (hors scène principale) pointant vers un layer isolé (ex. "Preview").
///    - Assigne cette caméra à _previewCamera.
///    - Coche "Culling Mask = Preview" uniquement.
///    - Règle "Target Texture" = ton RenderTexture.
/// 4. L'Image UI (_previewImage) doit avoir un RawImage component (pas Image),
///    avec sa texture = le même RenderTexture.
/// 5. Le GameObject _previewRoot est un Transform vide dans le layer "Preview",
///    là où le mesh 3D sera instancié et affiché par la caméra dédiée.
/// </summary>
public class ShopPreviewPanel : MonoBehaviour
{
    public static ShopPreviewPanel Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private RawImage _previewImage;          // L'image qui affiche le RenderTexture
    [SerializeField] private TextMeshProUGUI _itemNameText;   // Nom du meuble
    [SerializeField] private TextMeshProUGUI _itemPriceText;  // Prix
    [SerializeField] private Button _confirmBuyButton;        // Bouton "Acheter"
    [SerializeField] private GameObject _emptyStateHint;      // (optionnel) texte "Sélectionne un meuble"

    [Header("3D Preview")]
    [SerializeField] private Camera _previewCamera;           // Caméra dédiée au preview
    [SerializeField] private Transform _previewRoot;          // Pivot où spawn le mesh
    [SerializeField] private float _rotationSpeed = 45f;      // Degrés/seconde

    [Header("Render Texture")]
    [SerializeField] private RenderTexture _previewRenderTexture;

    private ShopItemData _currentItem;
    private GameObject _currentMeshInstance;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        // Branche le RenderTexture sur l'image UI
        if (_previewImage != null && _previewRenderTexture != null)
            _previewImage.texture = _previewRenderTexture;

        _confirmBuyButton.onClick.AddListener(OnConfirmBuy);

        ClearPreview();
    }

    /// <summary>
    /// Appelé par ShopItemUI quand l'utilisateur clique sur une carte.
    /// </summary>
    public void ShowPreview(ShopItemData data)
    {
        _currentItem = data;

        // Met à jour les textes
        _itemNameText.text = data.itemName;
        _itemPriceText.text = $"{data.price} €";

        // Désactive le hint vide
        if (_emptyStateHint != null) _emptyStateHint.SetActive(false);
        _previewImage.gameObject.SetActive(true);
        _itemNameText.gameObject.SetActive(true);
        _itemPriceText.gameObject.SetActive(true);

        // Gère l'état du bouton (déjà acheté ?)
        bool alreadyOwned = CustomShopManager.Instance != null && CustomShopManager.Instance.HasItem(data);
        _confirmBuyButton.interactable = !alreadyOwned;
        _confirmBuyButton.GetComponentInChildren<TextMeshProUGUI>().text = alreadyOwned ? "Déjà acheté" : "Acheter";

        SpawnPreviewMesh(data.mesh);
    }

    private void SpawnPreviewMesh(GameObject meshPrefab)
    {
        // Détruit l'ancien mesh prévisualisé
        if (_currentMeshInstance != null)
            Destroy(_currentMeshInstance);

        if (meshPrefab == null) return;

        _currentMeshInstance = Instantiate(meshPrefab, _previewRoot);
        _currentMeshInstance.transform.localPosition = Vector3.zero;
        _currentMeshInstance.transform.localRotation = Quaternion.identity;

        // Place tous les renderers dans le layer "Preview" pour isoler la caméra dédiée
        SetLayerRecursive(_currentMeshInstance, LayerMask.NameToLayer("Preview"));
    }

    private void Update()
    {
        // Rotation continue du mesh affiché
        if (_currentMeshInstance != null)
            _currentMeshInstance.transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime, Space.World);
    }

    private void OnConfirmBuy()
    {
        if (_currentItem == null) return;

        if (CurrencyManager.Instance.GetSoftCurrency() < _currentItem.price)
        {
            Debug.Log("[ShopPreview] Fonds insuffisants.");
            // Feedback -> Todod
            return;
        }

        CurrencyManager.Instance.SpendSoftCurrency(_currentItem.price);

        if (CustomShopManager.Instance != null)
        {
            CustomShopManager.Instance.AddObject(_currentItem);
            _confirmBuyButton.interactable = false;
            _confirmBuyButton.GetComponentInChildren<TextMeshProUGUI>().text = "Déjà acheté";
        }
    }

    /// <summary>
    /// Remet la preview à son état vide (appelé à la fermeture de la boutique, par exemple).
    /// </summary>
    public void ClearPreview()
    {
        _currentItem = null;

        if (_currentMeshInstance != null) { Destroy(_currentMeshInstance); _currentMeshInstance = null; }

        _itemNameText.text = "";
        _itemPriceText.text = "";
        _confirmBuyButton.interactable = false;

        if (_emptyStateHint != null) _emptyStateHint.SetActive(true);
    }

    private static void SetLayerRecursive(GameObject go, int layer)
    {
        go.layer = layer;
        foreach (Transform child in go.transform)
            SetLayerRecursive(child.gameObject, layer);
    }

    private void OnDestroy()
    {
        _confirmBuyButton.onClick.RemoveListener(OnConfirmBuy);
    }
}