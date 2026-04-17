using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum ShopTabs
{
    Furniture,
    Special,
    Energy,
    Money
}

public class ShopHandler : MonoBehaviour
{
    [SerializeField] private GameObject _uiToDisable;
    [SerializeField] private GameObject _shopMenu;
    [SerializeField] private List<GameObject> _shopCategories;

    [Header("Shop Items")]
    [SerializeField] private ShopItemUI _shopItemPrefab;
    [SerializeField] private List<ShopItemData> _allItems;

    private ShopTabs _currentTab;

    public void OpenShop()
    {
        _uiToDisable.SetActive(false);
        _shopMenu.SetActive(true);
        NavigateShop(ShopTabs.Furniture);
    }

    public void CloseShop()
    {
        _uiToDisable.SetActive(true);
        _shopMenu.SetActive(false);

        // Nettoie la preview 3D quand on ferme la boutique
        if (ShopPreviewPanel.Instance != null)
            ShopPreviewPanel.Instance.ClearPreview();
    }

    public void NavigateShop(ShopTabs newTab)
    {
        foreach (GameObject go in _shopCategories)
            go.SetActive(false);

        GameObject activeCategory = _shopCategories[(int)newTab];
        activeCategory.SetActive(true);

        // Reset la preview lors du changement d'onglet
        if (ShopPreviewPanel.Instance != null)
            ShopPreviewPanel.Instance.ClearPreview();

        if (newTab == ShopTabs.Energy)
            return;

        PopulateCategory(activeCategory.transform, newTab);
    }

    private void PopulateCategory(Transform container, ShopTabs tab)
    {
        Transform realContainer = container.GetChild(0);
        foreach (Transform child in realContainer)
            Destroy(child.gameObject);

        foreach (ShopItemData item in _allItems)
        {
            bool belongs = tab == ShopTabs.Furniture ? item.isFurniture : !item.isFurniture;
            if (!belongs) continue;

            ShopItemUI ui = Instantiate(_shopItemPrefab, realContainer);
            ui.Setup(item);

            if (CustomShopManager.Instance.HasItem(item))
                ui.SetSoldState();
        }
    }

    public void OpenFurnitureTab() => NavigateShop(ShopTabs.Furniture);
    public void OpenSpecialTab() => NavigateShop(ShopTabs.Special);
    public void OpenEnergyTab() => NavigateShop(ShopTabs.Energy);
    public void OpenMoneyTab() => NavigateShop(ShopTabs.Money);
}