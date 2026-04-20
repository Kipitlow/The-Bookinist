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
    [SerializeField] private Transform _previewRoot;
    [SerializeField] private float _rotationSpeed = 45f;

    [Header("Render Texture")]
    [SerializeField] private RenderTexture _previewRenderTexture;

    private ShopItemData _currentItem;
    private ShopItemUI _currentItemUI;
    private GameObject _currentMeshInstance;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        if (_previewImage != null && _previewRenderTexture != null)
            _previewImage.texture = _previewRenderTexture;

        _confirmBuyButton.onClick.AddListener(OnConfirmBuy);

        ClearPreview();
    }

    /// <summary>Appelé par ShopItemUI au clic sur une carte.</summary>
    public void ShowPreview(ShopItemData data, ShopItemUI sourceUI)
    {
        _currentItem = data;
        _currentItemUI = sourceUI;

        _itemNameText.text = data.itemName;
        _itemPriceText.text = $"{data.price} €";

        if (_emptyStateHint != null) _emptyStateHint.SetActive(false);

        bool alreadyOwned = CustomShopManager.Instance != null && CustomShopManager.Instance.HasItem(data);
        RefreshBuyButton(alreadyOwned);

        SpawnPreviewMesh(data.mesh);
    }

    private void RefreshBuyButton(bool alreadyOwned)
    {
        _confirmBuyButton.interactable = !alreadyOwned;
        if (_confirmBuyText != null)
            _confirmBuyButton.gameObject.SetActive(true);
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

        // Feedback uniforme : la card passe en Sold Out immédiatement
        if (_currentItemUI != null)
            _currentItemUI.SetSoldState();

        // La preview reste visible mais le bouton est désactivé
        RefreshBuyButton(true);
    }

    public void ClearPreview()
    {
        _currentItem = null;
        _currentItemUI = null;

        if (_currentMeshInstance != null) { Destroy(_currentMeshInstance); _currentMeshInstance = null; }

        if (_itemNameText != null) _itemNameText.text = "";
        if (_itemPriceText != null) _itemPriceText.text = "";

        RefreshBuyButton(true); // désactivé par défaut quand rien n'est sélectionné

        if (_emptyStateHint != null) _emptyStateHint.SetActive(true); _confirmBuyButton.gameObject.SetActive(false);
            
    }

    private void SpawnPreviewMesh(GameObject meshPrefab)
    {
        if (_currentMeshInstance != null) Destroy(_currentMeshInstance);
        if (meshPrefab == null) return;

        _currentMeshInstance = Instantiate(meshPrefab, _previewRoot);
        _currentMeshInstance.transform.localPosition = Vector3.zero;
        _currentMeshInstance.transform.localRotation = Quaternion.identity;

        SetLayerRecursive(_currentMeshInstance, LayerMask.NameToLayer("Preview"));
    }

    private void Update()
    {
        if (_currentMeshInstance != null)
            _currentMeshInstance.transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime, Space.World);
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