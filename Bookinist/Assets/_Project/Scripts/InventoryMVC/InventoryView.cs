using UnityEngine;
using System.Collections.Generic;

public class InventoryView : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject _inventoryParent;
    [SerializeField] private GameObject _itemBase;

    #endregion

    #region Unity Methods

    #endregion

    #region Methods

    public void UpdateInventory(List<Item> objectList)
    {
        Debug.Log("Inv updated");
        for (int i = _inventoryParent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(_inventoryParent.transform.GetChild(i).gameObject);
        }
        foreach (Item obj in objectList)
        {
            GameObject newObj = Instantiate(_itemBase, _inventoryParent.transform, false);
            newObj.GetComponent<ItemModel>().SetScriptable(obj);
            newObj.GetComponent<ItemController>().UpdateItem();
        }
    }

    #endregion
}
