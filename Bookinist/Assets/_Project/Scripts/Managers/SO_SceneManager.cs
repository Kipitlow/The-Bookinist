using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SO_SceneManager", menuName = "Scriptable Objects/SO_SceneManager")]
public class SO_SceneManager : ScriptableObject
{
    public void LoadScene(string sceneToLoad)
    {
        Debug.Log("Load scene...");
        SceneManager.LoadScene(sceneToLoad);
    }

    public void ExitGame()
    {
        Debug.Log("Exiting game...");
        Application.Quit();
    }

    public void LoadSceneDuringOnboardingShop(string sceneToLoad)
    {
        if (GameManager.Instance._isFirstCustomerFinishDialog)
            SceneManager.LoadScene(sceneToLoad);
    }

    public void LoadSceneAfterFirstBook(string sceneToLoad)
    {
        GameManager.Instance.bookFinish = true;
        SceneManager.LoadScene(sceneToLoad);
    }
}