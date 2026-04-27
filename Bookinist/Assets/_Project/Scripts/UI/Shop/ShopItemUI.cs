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

    private ShopItemData _data;

    public void Setup(ShopItemData data)
    {
        _data = data;
        _icon.sprite = data.icon;
        _priceText.text = $"{data.price} €";

        _buyButton.onClick.AddListener(OnItemClicked);
    }

    private void OnItemClicked()
    {
        // On délègue à ShopPreviewPanel plutôt que d'acheter immédiatement
        ShopPreviewPanel.Instance.ShowPreview(_data, this);
    }

    public void SetSoldState()
    {
        _buyButton.interactable = false;
        if (_soldText != null) _soldText.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        _buyButton.onClick.RemoveListener(OnItemClicked);
    }

    public Image GetIcon()
    {
        return _icon; 
    }
}