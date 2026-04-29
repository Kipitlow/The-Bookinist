using UnityEngine;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{
    [SerializeField] private GameObject _currentPfp;
    [SerializeField] private GameObject _currentMiniPfp;
    [SerializeField] private GameObject _customWindow;

    private void Start()
    {
        _currentPfp.GetComponent<Button>().onClick.RemoveAllListeners();
        _currentPfp.GetComponent<Button>().onClick.AddListener(() => OpenCustomPfp());
    }

    public void ChangePfp(Image newPfp)
    {
        _currentPfp.GetComponent<Image>().sprite = newPfp.sprite;
        _currentMiniPfp.GetComponent<Image>().sprite = newPfp.sprite;
    }

    public void OpenCustomPfp()
    {
        _customWindow.SetActive(true);
    }
}
