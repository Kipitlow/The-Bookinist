using UnityEngine;
using UnityEngine.UI;

public class ItemView : MonoBehaviour
{
    #region Variables

    [SerializeField] private Image _itemSprite;

    #endregion

    #region Unity Methods

    #endregion

    #region Methods

    public void UpdateSprite(Sprite newSprite)
    {
        _itemSprite.sprite = newSprite;
    }

    #endregion
}
