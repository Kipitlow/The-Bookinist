using UnityEngine;

public class TransitionCaller : MonoBehaviour
{
    [SerializeField] private string _sceneToLoad;
    [SerializeField] private string _sceneToUnload;

    public void CallNuages()
    {
        CloudTransitionController.Instance.PlayTransition(_sceneToLoad, _sceneToUnload);
    }
}
