using UnityEditor;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    #region Variables
    [SerializeField] private InventoryModel _inventoryModel;
    [SerializeField] private InventoryView _inventoryView;
    [SerializeField] private Item _emptySlot;
    public Item activeItem;

    #endregion

    private static InventoryController _instance;

    public static InventoryController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<InventoryController>();

                if (_instance == null)
                {
                    GameObject obj = new GameObject(nameof(InventoryController));
                    _instance = obj.AddComponent<InventoryController>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    #region Unity Methods
    private void OnEnable()
    {
        ItemController.onItemClicked += HandleItemClicked;
    }

    private void OnDisable()
    {
        ItemController.onItemClicked -= HandleItemClicked;
    }

    #endregion

    #region Methods

    public void AddInventoryItem(Item ItemToAdd)
    {
        _inventoryModel.AddItem(ItemToAdd);
        if (_inventoryModel.GetInventoryContent().Count <=5)
        {
            UpdateInventory(false, false);
        }
        else
        {
            UpdateInventory(true, false);
        }
    }
    public void RemoveInventoryItem(Item ItemToAdd)
    {
        _inventoryModel.RemoveItem(ItemToAdd);
        UpdateInventory(true, true);
        activeItem = _emptySlot;
    }

    public void UpdateInventory(bool isInventoryFull, bool isRemovingItem)
    {
        _inventoryView.UpdateInventory(_inventoryModel.GetInventoryContent(), isInventoryFull, isRemovingItem);
    }

    private void HandleItemClicked(Item item)
    {
        //Debug.Log($"Clicked: {item.name}");
        activeItem = item;
    }

    public bool IsInventoryHasPlace()
    {
        return _inventoryModel.IsInventoryHasAPlace();
    }

    #endregion
}
