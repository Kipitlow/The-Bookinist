using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AccountCreator : MonoBehaviour
{
    [SerializeField] private TMP_InputField _usernameInput;

    public void CreateAccount()
    {
        SaveSystem.instance.Create();
        if (_usernameInput.text == "")
            SaveService.instance.ChangeName("Bookinist");
        else
            SaveService.instance.ChangeName(_usernameInput.text);
        SaveSystem.instance.Save();
    }

    public void DeleteAccount()
    {
        SaveSystem.instance.Delete();
    }
}
