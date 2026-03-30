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
    }

    public void Init(InventoryModel inventoryModel)
    {
        _inventoryModel = inventoryModel;
        inventoryView.InitSlots(inventoryModel.Slots);

    }

    public InventoryView GetInventoryView()
    {
        return inventoryView;
    }
}
