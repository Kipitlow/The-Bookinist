using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;

public class ShopPreviewPanel : MonoBehaviour
{
    public static ShopPreviewPanel Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private RawImage _previewImage;
    [SerializeField] private TextMeshProUGUI _itemNameText;
    [SerializeField] private TextMeshProUGUI _itemPriceText;
    [SerializeField] private Button _confirmBuyButton;
    [SerializeField] private TextMeshProUGUI _confirmBuyText;
    [SerializeField] private GameObject _emptyStateHint;

    [Header("3D Preview")]
    [SerializeField] private Camera _previewCamera;

    // _previewRoot est le pivot de rotation placé devant la caméra preview.
    // Le prefab sera instancié en enfant de ce transform, centré via ses bounds.
    [SerializeField] private Transform _previewRoot;
    [SerializeField] private float _rotationSpeed = 45f;

    [Header("Render Texture")]
    [SerializeField] private RenderTexture _previewRenderTexture;

    [SerializeField] private BookShopOnboardingManager _bookShopOnboardingManager;

    private ShopItemData _currentItem;
    private ShopItemUI _currentItemUI;
    private GameObject _currentInstance;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        if (_previewImage != null && _previewRenderTexture != null)
            _previewImage.texture = _previewRenderTexture;

        _confirmBuyButton.onClick.AddListener(OnConfirmBuy);

        ClearPreview();
    }

    public void ShowPreview(ShopItemData data, ShopItemUI sourceUI)
    {
        if (sourceUI.GetIcon().GetComponent<Image>().sprite.name == "FauteuilBleu")
            _bookShopOnboardingManager.CheckOnboarding(4);

        _currentItem = data;
        _currentItemUI = sourceUI;

        _itemNameText.text = data.itemName;
        _itemPriceText.text = $"{data.price} €";

        if (_emptyStateHint != null) _emptyStateHint.SetActive(false);

        bool alreadyOwned = CustomShopManager.Instance != null && CustomShopManager.Instance.HasItem(data);
        RefreshBuyButton(alreadyOwned);

        SpawnPreview(data.previewPrefab);
    }

    private void SpawnPreview(GameObject prefab)
    {
        if (_currentInstance != null)
        {
            Destroy(_currentInstance);
            _currentInstance = null;
        }

        if (prefab == null) return;

        // Instancie le prefab en enfant du pivot de rotation
        _currentInstance = Instantiate(prefab, _previewRoot);
        _currentInstance.transform.localPosition = Vector3.zero;
        _currentInstance.transform.localRotation = Quaternion.identity;

        CenterOnBounds(_currentInstance);

        // Isole le prefab dans le layer Preview pour la caméra dédiée
        SetLayerRecursive(_currentInstance, LayerMask.NameToLayer("Preview"));
    }

    /// <summary>
    /// Décale le mesh pour que son centre géométrique (Bounds) coïncide
    /// avec la position locale (0,0,0) du pivot, garantissant une rotation centrée.
    /// </summary>
    private void CenterOnBounds(GameObject go)
    {
        Renderer[] renderers = go.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return;

        // Calcule les bounds globales de tous les renderers
        Bounds bounds = renderers[0].bounds;
        foreach (Renderer r in renderers)
            bounds.Encapsulate(r.bounds);

        // Décale l'instance pour centrer ses bounds sur le pivot
        Vector3 offset = go.transform.position - bounds.center;
        go.transform.position += offset;
    }

    private void Update()
    {
        if (_currentInstance != null)
            _previewRoot.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime, Space.Self);
    }

    private void RefreshBuyButton(bool alreadyOwned)
    {
        _confirmBuyButton.interactable = !alreadyOwned;
        if (_confirmBuyText != null)
            _confirmBuyText.text = alreadyOwned ? "Déjà acheté" : "Acheter";
    }

    private void OnConfirmBuy()
    {
        if (_currentItem == null) return;

        if (CurrencyManager.Instance.GetSoftCurrency() < _currentItem.price)
        {
            Debug.Log("[ShopPreview] Fonds insuffisants.");
            return;
        }

        CurrencyManager.Instance.SpendSoftCurrency(_currentItem.price);

        if (CustomShopManager.Instance != null)
            CustomShopManager.Instance.AddObject(_currentItem);

        if (_currentItemUI != null)
            _currentItemUI.SetSoldState();

        RefreshBuyButton(true);

        if (_currentItemUI.GetIcon().GetComponent<Image>().sprite.name == "FauteuilBleu")
            _bookShopOnboardingManager.CheckOnboarding(5);
    }

    public void ClearPreview()
    {
        _currentItem = null;
        _currentItemUI = null;

        if (_currentInstance != null) { Destroy(_currentInstance); _currentInstance = null; }

        if (_itemNameText != null) _itemNameText.text = "";
        if (_itemPriceText != null) _itemPriceText.text = "";

        RefreshBuyButton(true);

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