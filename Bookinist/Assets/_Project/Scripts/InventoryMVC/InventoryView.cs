using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class InventoryView : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject _inventoryParent;
    [SerializeField] private GameObject _itemBase;
    //[SerializeField] private EventManager _eventManager;

    #endregion

    #region Unity Methods

    #endregion

    #region Methods

    public void UpdateInventory(List<Item> objectList)
    {
        for (int i = _inventoryParent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(_inventoryParent.transform.GetChild(i).gameObject);
        }
        foreach (Item obj in objectList)
        {
            GameObject newObj = Instantiate(_itemBase, _inventoryParent.transform, false);
            ItemModel objItemModel = newObj.GetComponent<ItemModel>();
            objItemModel.SetScriptable(obj);

            ItemController controller = newObj.GetComponent<ItemController>();

            UnityEngine.UI.Button button = newObj.GetComponent<UnityEngine.UI.Button>();
            button.onClick.AddListener(controller.OnItemClick);
            button.onClick.AddListener(() => Debug.Log("BUTTON CLICKED"));

            //objItemModel.GetEventManager(_eventManager);

            newObj.GetComponent<ItemController>().UpdateItem();
        }
    }

    #endregion
}
