using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Vue de l'inventaire : instancie les éléments UI pour chaque Item.
/// </summary>
public class InventoryView : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject _inventoryParent;
    [SerializeField] private GameObject _itemBase;

    #endregion

    #region Methods

    public void UpdateInventory(List<Item> objectList)
    {
        for (int i = _inventoryParent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(_inventoryParent.transform.GetChild(i).gameObject);
        }

        Debug.Log(objectList.Count);
        for (int i = 0; i < objectList.Count; i++)
        {
            GameObject newObj = Instantiate(_itemBase, _inventoryParent.transform, false);
            ItemModel objItemModel = newObj.GetComponent<ItemModel>();
            objItemModel.SetScriptable(objectList[i]);

            ItemController controller = newObj.GetComponent<ItemController>();

            Button button = newObj.GetComponent<Button>();
            button.onClick.AddListener(controller.OnItemClick);
            button.onClick.AddListener(() => Debug.Log("BUTTON CLICKED"));

            controller.UpdateItem();
        }
    }

    #endregion
}
