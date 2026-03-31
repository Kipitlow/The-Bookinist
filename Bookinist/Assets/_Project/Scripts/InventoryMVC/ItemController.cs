using UnityEngine;

public class ItemController : MonoBehaviour
{
    #region Variables

    [SerializeField] private ItemModel _itemModel;
    [SerializeField] private ItemView _itemView;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        _itemView.UpdateSprite(_itemModel.itemSprite);
    }

    #endregion

    #region Methods

    public void UpdateItem()
    {
        _itemView.UpdateSprite(_itemModel.itemSprite);
    }

        #endregion
    }
