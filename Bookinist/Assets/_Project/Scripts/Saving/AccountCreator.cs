using UnityEngine;
using UnityEngine.UI;

public class AccountCreator : MonoBehaviour
{
    //[SerializeField] private InputField _usernameInput;

    public void CreateAccount(string username)
    {
        SaveSystem.instance.Save();
        SaveService.instance.ChangeName(username);
        SaveSystem.instance.Save();
    }

    public void DeleteAccount()
    {
        SaveSystem.instance.Delete();
    }
}
