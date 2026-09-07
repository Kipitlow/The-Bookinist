using System.Collections.Generic;
using UnityEngine;

public class ToggleMenus : MonoBehaviour
{
    [SerializeField] private List<GameObject> _disabledUI;
    [SerializeField] private List<GameObject> _enabledUI;

    public void ToggleAllMenus()
    {
        foreach (GameObject go in _disabledUI)
        {
            go.SetActive(!go.activeSelf);
        }

        foreach (GameObject go in _enabledUI)
        {
            go.SetActive(!go.activeSelf);
        }
    }
}
