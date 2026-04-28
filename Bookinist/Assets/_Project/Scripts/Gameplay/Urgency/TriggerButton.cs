using UnityEngine;
using UnityEngine.UI;

public class TriggerButton : MonoBehaviour
{
    [SerializeField] Button _button;

    public void ClickButton()
    {
        GameManager.Instance.bookFinish = true;
        GameManager.Instance.bookStarted = true;
        _button.onClick.Invoke();
    }
}
