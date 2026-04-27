using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SO_SceneManager", menuName = "Scriptable Objects/SO_SceneManager")]
public class SO_SceneManager : ScriptableObject
{
    public void LoadScene(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void LoadSceneAditive(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
    }

    public void UnloadScene(string sceneToUnLoad)
    {
        SceneManager.UnloadSceneAsync(sceneToUnLoad);
    }

    public void ExitGame()
    {
        Debug.Log("Exiting game...");
        Application.Quit();
    }
}