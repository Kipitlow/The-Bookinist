using UnityEngine;
using UnityEngine.UI;

public class ButtonColor : MonoBehaviour
{
    #region Variables

    public Image buttonImage;
    public Color selectedColor;
    public Color unselectedColor;

    #endregion

    #region Methods

    public void SelectedButtonChangeColor()
    {
        if (buttonImage != null)
            buttonImage.color = selectedColor;
    }

    public void UnselectedButtonChangeColor()
    {
        if (buttonImage != null)
            buttonImage.color = unselectedColor;
    }

    #endregion
}
