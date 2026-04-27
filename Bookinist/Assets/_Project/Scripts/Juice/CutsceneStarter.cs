using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutsceneStarter : MonoBehaviour
{
    [SerializeField] private PlayableDirector _sceneStarter;

    public void StartIntroCutscene()
    {
        _sceneStarter.Play();
    }
}
