using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ScriptableObject pour chargement / sortie de scènes via l'éditeur.
/// </summary>
[CreateAssetMenu(fileName = "SO_SceneManager", menuName = "Scriptable Objects/SO_SceneManager")]
public class SO_SceneManager : ScriptableObject
{
    #region Methods

    public void LoadScene(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void ExitGame()
    {
        Debug.Log("Exiting game...");
        Application.Quit();
    }

    #endregion
}