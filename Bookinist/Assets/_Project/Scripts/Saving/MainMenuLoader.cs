using System.IO;
using UnityEngine;

public class MainMenuLoader : MonoBehaviour
{
    [SerializeField] private GameObject _registerUI;

    [SerializeField] private GameObject _loadUI;

    [SerializeField] private UIAnimator _uiAnimator;

    public void ValidateSaveSlot()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "saveData.json");

        if (File.Exists(filePath))
        {
            _uiAnimator.Hide();
            _loadUI.SetActive(true);
            return;
        }
        _uiAnimator.Show();
        _registerUI.SetActive(true);
    }
}
