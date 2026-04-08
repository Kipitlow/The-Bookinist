using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Vue d'un item : affiche le sprite.
/// </summary>
public class ItemView : MonoBehaviour
{
    #region Variables

    [SerializeField] private Image _itemSprite;

    #endregion

    #region Methods

    public void UpdateSprite(Sprite newSprite)
    {
        if (_itemSprite != null)
            _itemSprite.sprite = newSprite;
    }

    #endregion
}
