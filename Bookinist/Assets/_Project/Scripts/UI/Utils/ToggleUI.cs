using System.Collections.Generic;
using UnityEngine;

public class ToggleUI : MonoBehaviour
{
    [SerializeField] private GameObject _enableUI;
    [SerializeField] private List<GameObject> _disableUI;

    public void ToggleAllMenus()
    {
        foreach (GameObject go in _disableUI)
        {
            go.SetActive(false);
        }
        _enableUI.SetActive(!_enableUI.activeSelf);
    }
}
