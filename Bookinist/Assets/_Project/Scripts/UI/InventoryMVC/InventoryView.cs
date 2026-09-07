using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class InventoryView : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject _inventoryParent;
    [SerializeField] private GameObject _inventorySignPanel;
    [SerializeField] private GameObject _itemBase;
    //[SerializeField] private EventManager _eventManager;

    #endregion

    #region Unity Methods
    [Header("Flash Colors")]
    [SerializeField] private Color _addedItemColor;
    [SerializeField] private Color _fullInvColor;
    [SerializeField] private float _flashDuration = 1f;

    #endregion
    private Image _inventoryImage;
    private Image _inventorySignImage;
    private Color _originalColor;
    private Coroutine _flashRoutine;

    #region Methods
    private void Awake()
    {
        _inventoryImage = _inventoryParent.GetComponent<Image>();
        _inventorySignImage = _inventorySignPanel.GetComponent<Image>();
        _originalColor = _inventoryImage.color;
    }

    public void UpdateInventory(List<Item> objectList, bool isInventoryFull, bool isRemovingItem)
    {
        if (!isInventoryFull && !isRemovingItem)
        {
            FlashInventoryBackground(_addedItemColor);
        }
        if(isInventoryFull && !isRemovingItem)
        {
            FlashInventoryBackground(_fullInvColor);
        }

        for (int i = _inventoryParent.transform.childCount - 1; i >= 0; i--)
            Destroy(_inventoryParent.transform.GetChild(i).gameObject);

        for (int i = 0; i < objectList.Count; i++)
        {
            GameObject newObj = Instantiate(_itemBase, _inventoryParent.transform, false);

            ItemModel objItemModel = newObj.GetComponent<ItemModel>();
            objItemModel.SetScriptable(objectList[i]);

            ItemController controller = newObj.GetComponent<ItemController>();

            Button button = newObj.GetComponent<Button>();
            button.onClick.AddListener(controller.OnItemClick);

            controller.UpdateItem();
        }
    }

    private void FlashInventoryBackground(Color flashColor)
    {
        if (_flashRoutine != null)
            StopCoroutine(_flashRoutine);

        _flashRoutine = StartCoroutine(FlashRoutine(flashColor));
    }

    private IEnumerator FlashRoutine(Color flashColor)
    {
        _inventoryImage.color = flashColor;
        _inventorySignImage.color = flashColor;
        yield return new WaitForSeconds(_flashDuration);
        _inventoryImage.color = _originalColor;
        _inventorySignImage.color = _originalColor;
        _flashRoutine = null;
    }
    #endregion
}