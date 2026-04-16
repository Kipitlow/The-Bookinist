using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemUI : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private Button _buyButton;
    [SerializeField] private TextMeshProUGUI _soldText;

    private CustomShopManager _customManager;
    private ShopItemData _data;

    public void Setup(ShopItemData data)
    {
        _data = data;
        _icon.sprite = data.icon;
        // _nameText.text = data.itemName;
        _priceText.text = $"{data.price} €";

        _customManager = CustomShopManager.Instance;

        if (_customManager == null)
            Debug.LogWarning("[ShopItem] CustomShopManager.Instance est null");

        _buyButton.onClick.AddListener(OnBuyClicked);
    }

    private void OnBuyClicked()
    {
        if (CurrencyManager.Instance.GetSoftCurrency() < _data.price)
        {
            Debug.Log("[ShopItem] Fonds insuffisants.");
            return;
        }

        CurrencyManager.Instance.SpendSoftCurrency(_data.price);

        if (_customManager != null)
        {
            _customManager.AddObject(_data);
            SetSoldState();
        }
        else
            Debug.LogWarning("[ShopItem] Impossible d'ajouter le meuble : CustomShopManager est null.");
    }

    public void SetSoldState()
    {
        _buyButton.interactable = false;
        _soldText.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        _buyButton.onClick.RemoveListener(OnBuyClicked);
    }
}