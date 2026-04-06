using System;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    #region Variables

    [SerializeField] private ItemModel _itemModel;
    [SerializeField] private ItemView _itemView;
    [SerializeField] private bool _itemIsSelected;
    public static Action<Item> OnItemClicked;

    #endregion

    #region Methods

    public void UpdateItem()
    {
        _itemView.UpdateSprite(_itemModel.itemSprite);
    }

    public void OnItemClick()
    {
        Debug.Log("Event called OnClick, with item " + gameObject);
        OnItemClicked?.Invoke(_itemModel.itemScriptable);
    }

    #endregion
}
