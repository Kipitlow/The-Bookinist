using System.IO;
using UnityEngine;

public class MainMenuLoader : MonoBehaviour
{
    [SerializeField] private GameObject _registerUI;

    [SerializeField] private GameObject _loadUI;

    public void ValidateSaveSlot()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "saveData.json");

        if (File.Exists(filePath))
        {
            _loadUI.SetActive(true);
            return;
        }
        _registerUI.SetActive(true);
    }
}
