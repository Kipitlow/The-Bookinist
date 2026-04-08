using UnityEngine;
using UnityEngine.UI;

public class ButtonColor : MonoBehaviour
{
    public Image _buttonImage;
    public Color _selectedColor;
    public Color _unselectedColor;

    public void SelectedButtonChangeColor()
    {
        _buttonImage.color = _selectedColor;
    }

    public void UnselectedButtonChangeColor()
    {
        _buttonImage.color = _unselectedColor;
    }
}
