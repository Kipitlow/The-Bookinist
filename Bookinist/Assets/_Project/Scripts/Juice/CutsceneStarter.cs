using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutsceneStarter : MonoBehaviour
{
    [SerializeField] private PlayableDirector sceneStarter;

    public void StartIntroCutscene()
    {
        sceneStarter.Play();
    }
}
