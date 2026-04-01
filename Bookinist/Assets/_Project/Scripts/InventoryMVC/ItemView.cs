using UnityEngine;
using UnityEngine.UI;

public class ItemView : MonoBehaviour
{
    #region Variables

    [SerializeField] Image itemSprite;

    #endregion

    #region Unity Methods

    #endregion

    #region Methods

    public void UpdateSprite(Sprite newSprite)
    {
        itemSprite.sprite = newSprite;
    }

    #endregion
}
