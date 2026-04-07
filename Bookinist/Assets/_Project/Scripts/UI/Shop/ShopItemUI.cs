using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemUI : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private Button _buyButton;

    private ShopItemData _data;

    public void Setup(ShopItemData data)
    {
        _data = data;
        _icon.sprite = data.icon;
        _nameText.text = data.itemName;
        _priceText.text = $"{data.price} €";

        _buyButton.onClick.AddListener(OnBuyClicked);
    }

    private void OnBuyClicked()
    {
        if (CurrencyManager.Instance.GetSoftCurrency() >= _data.price)
        {
            Debug.Log($"Achat : {_data.itemName} pour {_data.price}€");
            CurrencyManager.Instance.SpendSoftCurrency(_data.price);
        }
    }

    private void OnDestroy()
    {
        _buyButton.onClick.RemoveListener(OnBuyClicked);
    }
}