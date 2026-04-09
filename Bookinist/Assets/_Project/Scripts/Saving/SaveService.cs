using UnityEngine;

public class SaveService : MonoBehaviour
{
    public static SaveService instance;

    private PlayerProfile profile => SaveSystem.instance.profile;

    public void ChangeName(string newName)
    {
        profile.ChangePlayerName(newName);

        SaveSystem.instance.Save();
    }
}
