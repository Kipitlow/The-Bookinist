using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private InventoryView inventoryView;
    [SerializeField] private GameController gameController;
    // Source de données pour les cartes : ScriptableObjects CardData
    [Header("Cards Data")]
    [Tooltip("Liste des CardData utilisées pour générer les mains. Si vide, on essayera de charger automatiquement depuis Resources.")]
    [SerializeField] private List<ItemData> _cardDataList = new List<ItemData>();
    private InventoryModel _inventoryModel;

    public List<ItemData> CardDataList => _cardDataList;

    private void Awake()
    {
        inventoryView.OnCardClicked += gameController.OnCardSelected;

        if (_cardDataList == null || _cardDataList.Count == 0)
        {
            Debug.LogWarning($"{name} : aucune CardData assignée dans l’inspecteur !");
        }
    }

    public void Init(InventoryModel inventoryModel)
    {
        _inventoryModel = inventoryModel;
        inventoryView.InitSlots(inventoryModel.SlotList);
        inventoryView.InitHand(inventoryModel.SlotList);

        _inventoryModel.OnCreateNewSlot += inventoryView.AddSlot;
    }

    public void UpdateInventoryView(InventoryModel inventoryModel)
    {
        inventoryView.InitHand(inventoryModel.SlotList);
    }

    public void DrawCard(InventoryModel inventoryModel)
    {
        inventoryView.InitHand(inventoryModel.SlotList);
    }

    public void SetInteractable(bool isInteractable)
    {
        inventoryView.SetInteractable(isInteractable);
    }

    public void RemoveCardView(ItemModel cardModel)
    {
        if (cardModel == null) return;
        inventoryView.RemoveCard(cardModel);
    }

    // Nettoie le highlight dans la main
    public void ClearSelection()
    {
        inventoryView.ClearSelection();
    }
    public void SelectCard(ItemModel cardModel)
    {
        inventoryView.SelectCardView(cardModel);
    }

    private void OnDestroy()
    {
        inventoryView.OnCardClicked -= gameController.OnCardSelected;
    }

    public InventoryView GetInventoryView()
    {
        return inventoryView;
    }
}
