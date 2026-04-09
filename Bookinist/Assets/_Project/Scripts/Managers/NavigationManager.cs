using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Chargement de scènes / gestion de navigation.
/// </summary>
public class NavigationManager : MonoBehaviour
{
    #region Methods

    public void LoadScene(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    #endregion
}
